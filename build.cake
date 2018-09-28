#tool "xunit.runner.console"
#tool "OpenCover"
#tool "docfx.console"
#tool "Hub-Nuget"
#tool "PdbGit"
#addin "Cake.DocFx"
#addin "Cake.Http"
#addin "Cake.FileHelpers"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "release");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("NuGetApiKey"));
var solutionFilePath = GetFiles("./**/*.sln").First();
var solutionName = solutionFilePath.GetDirectory().GetDirectoryName();
var repositoryName = EnvironmentVariable("APPVEYOR_REPO_NAME") ?? solutionName;
var repositoryBranch = EnvironmentVariable("APPVEYOR_REPO_BRANCH") ?? "master";
// Used to store the version, which is needed during the build and the packaging
var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "1.0.0";

// Check if we are in a pull request, publishing of packages and coverage should be skipped
var isPullRequest = !string.IsNullOrEmpty(EnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER"));

// Check if the commit is marked as release
var isRelease = (EnvironmentVariable("APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED")?? "").Contains("[release]");

// Branch name
var branch = EnvironmentVariable("APPVEYOR_REPO_BRANCH");

Task("Default")
    .IsDependentOn("Publish");

// Publish the Artifact of the Package Task to the Nexus Pro
Task("Publish")
    .IsDependentOn("Package")
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .WithCriteria(() => !string.IsNullOrEmpty(nugetApiKey))
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRelease)
    .Does(()=>
{
    var settings = new NuGetPushSettings {
        Source = "https://www.nuget.org/api/v2/package",
        ApiKey = nugetApiKey
    };

    var packages = GetFiles("./artifacts/*.nupkg").Where(p => !p.FullPath.Contains("symbols"));
    NuGetPush(packages, settings);
});

// Package the results of the build, if the tests worked, into a NuGet Package
Task("Package")
    .IsDependentOn("Coverage")
    //.IsDependentOn("CoPilot")
    .IsDependentOn("AssemblyVersion")
    .IsDependentOn("Documentation")
	.IsDependentOn("GitLink")
    .Does(()=>
{
    var settings = new NuGetPackSettings 
    {
        OutputDirectory = "./artifacts/",
        Verbosity = NuGetVerbosity.Detailed,
        Symbols = false,
        IncludeReferencedProjects = true,
        Properties = new Dictionary<string, string>
        {
            { "Configuration", configuration },
            { "Platform", "AnyCPU" }
        }
    };

    var projectFilePaths = GetFiles("./**/*.csproj")
		.Where(p => !p.FullPath.ToLower().Contains("test"))
		.Where(p => !p.FullPath.ToLower().Contains("packages"))
		.Where(p => !p.FullPath.ToLower().Contains("tools"))
		.Where(p => !p.FullPath.ToLower().Contains("demo"))
		.Where(p => !p.FullPath.ToLower().Contains("diagnostics"))
		.Where(p => !p.FullPath.ToLower().Contains("power"))
		.Where(p => !p.FullPath.ToLower().Contains("example"));
	foreach(var projectFilePath in projectFilePaths)
	{
		Information("Packaging: " + projectFilePath.FullPath);
		NuGetPack(projectFilePath.FullPath, settings);
	}
});

// Use Blackduck CoPilot, this is all done manually and I hope they will write a cake addon
Task("CoPilot")
    .IsDependentOn("Build")
	.Does(() =>
{
	// Create the Bill Of Materials by using their Hub-Nuget tool
	FilePath buildBomPath = Context.Tools.Resolve("buildBom.exe");
	StartProcess(buildBomPath, new ProcessSettings {
		Arguments = new ProcessArgumentBuilder()
			.Append("--solution_path=" + solutionFilePath.FullPath)
			.Append("--hub_create_merged_bdio=true")
			.Append("--hub_deploy_bdio=false")
		});
		
	// Find files with the BOM
	var copilotFiles = GetFiles("./blackduck/**/*.jsonld").Where(p => !p.FullPath.Contains("Test") && !p.FullPath.Contains("Demo") && !p.FullPath.Contains("Diagnostics") &&!p.FullPath.Contains("packages") &&!p.FullPath.Contains("tools"));

	// and upload the files to blackduck's CoPilot
	var copilotUrl = string.Format(
		"https://copilot.blackducksoftware.com/hub/import?provider=github&repository={0}&branch={1}&pull_request={2}",
		repositoryName,
		repositoryBranch,
		isPullRequest
	);
	
	foreach(var copilotFile in copilotFiles)
	{
		var content = FileReadText(copilotFile.FullPath);
		// curl -v -f -k --data @<filename> -H "Content-Type:application/ld+json" "https://copilot.blackducksoftware.com/hub/import?provider=github&repository=dapplo/Dapplo.CaliburnMicro&branch=master&pull_request=false"
		var response = HttpPost(copilotUrl, settings => { settings.
			SetContentType("application/ld+json").
			SetRequestBody(content);
		});
		Information("CoPilot upload " + copilotFile.FullPath + " answer: " + response);
	}
});
	
// Build the DocFX documentation site
Task("Documentation")
    .Does(() =>
{
    DocFxMetadata("./doc/docfx.json");
    DocFxBuild("./doc/docfx.json");

    CreateDirectory("artifacts");
    // Archive the generated site
    Zip("./doc/_site", "./artifacts/site.zip");
});

// Run the XUnit tests via OpenCover, so be get an coverage.xml report
Task("Coverage")
    .IsDependentOn("Build")
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .Does(() =>
{
    CreateDirectory("artifacts");

    var openCoverSettings = new OpenCoverSettings() {
        // Forces error in build when tests fail
        ReturnTargetCodeOffset = 0
    };

    var projectFiles = GetFiles("./**/*.csproj");
    foreach(var projectFile in projectFiles)
    {
        var projectName = projectFile.GetDirectory().GetDirectoryName();
        if (projectName.Contains("Test")) {
           openCoverSettings.WithFilter("-["+projectName+"]*");
        }
        else {
           openCoverSettings.WithFilter("+["+projectName+"]*");
        }
    }

    // Make XUnit 2 run via the OpenCover process
    OpenCover(
        // The test tool Lamdba
        tool => {
            tool.XUnit2("./**/bin/**/*.Tests.dll",
                new XUnit2Settings {
                    // Add AppVeyor output, this "should" take care of a report inside AppVeyor
                    ArgumentCustomization = args => {
                        if (!BuildSystem.IsLocalBuild) {
                            args.Append("-appveyor");
                        }
                        return args;
                    },
                    ShadowCopy = false,
                    XmlReport = true,
                    HtmlReport = true,
                    ReportName = solutionName,
                    OutputDirectory = "./artifacts",
                    WorkingDirectory = "./src"
                });
            },
        // The output path
        new FilePath("./artifacts/coverage.xml"),
        // Settings
       openCoverSettings
    );
});

// This starts the actual MSBuild
Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("AssemblyVersion")
    .Does(() =>
{
    var settings = new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        ToolVersion = MSBuildToolVersion.VS2017,
        Configuration = configuration,
		Restore = true,
        PlatformTarget = PlatformTarget.MSIL
    };

    MSBuild(solutionFilePath.FullPath, settings);
    
    // Make sure the .dlls in the obj path are not found elsewhere
    CleanDirectories("./**/obj");
});


// Generate Git links in the PDB files
Task("GitLink")
    .IsDependentOn("Build")
    .Does(() =>
{
	FilePath pdbGitPath = Context.Tools.Resolve("PdbGit.exe");
	var pdbFiles = GetFiles("./**/*.pdb")
		.Where(p => !p.FullPath.ToLower().Contains("test"))
		.Where(p => p.FullPath.ToLower().Contains("Caliburn"))
		.Where(p => !p.FullPath.ToLower().Contains("tools"))
		.Where(p => !p.FullPath.ToLower().Contains("packages"))
		.Where(p => !p.FullPath.ToLower().Contains("example"));
    foreach(var pdbFile in pdbFiles)
    {
		Information("Processing: " + pdbFile.FullPath);
		StartProcess(pdbGitPath, new ProcessSettings { Arguments = new ProcessArgumentBuilder().Append(pdbFile.FullPath)});
	}
});

// Version is written to the AssemblyInfo files when !BuildSystem.IsLocalBuild
Task("AssemblyVersion")
    .Does(() =>
{
    foreach(var assemblyInfoFile in  GetFiles("./**/AssemblyInfo.cs").Where(p => p.FullPath.Contains(solutionName))) {
        var assemblyInfo = ParseAssemblyInfo(assemblyInfoFile.FullPath);
        CreateAssemblyInfo(assemblyInfoFile.FullPath, new AssemblyInfoSettings {
            Version = version,
            InformationalVersion = version + (branch == "master" ? "" : "-beta"),
            FileVersion = version,

            CLSCompliant = assemblyInfo.ClsCompliant,
            Company = assemblyInfo.Company,
            ComVisible = assemblyInfo.ComVisible,
            Configuration = assemblyInfo.Configuration,
            Copyright = assemblyInfo.Copyright,
            //CustomAttributes = assemblyInfo.CustomAttributes,
            Description = assemblyInfo.Description,
            //Guid = assemblyInfo.Guid,
            InternalsVisibleTo = assemblyInfo.InternalsVisibleTo,
            Product = assemblyInfo.Product,
            Title = assemblyInfo.Title,
            Trademark = assemblyInfo.Trademark
        });
    }
});


// Clean all unneeded files, so we build on a clean file system
Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/obj");
    CleanDirectories("./**/bin");
    CleanDirectories("./artifacts");	
});

RunTarget(target);