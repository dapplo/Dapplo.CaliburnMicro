// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Application.Demo;
using Dapplo.Addons.Bootstrapper.Resolving;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Config.Language;
using Dapplo.Log;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

namespace Dapplo.Caliburn.Tests
{
    public class BasicTests
    {
        public BasicTests(ITestOutputHelper testOutputHelper)
        {
            LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
        }
        [Fact]
        public void Test_Language_Scan()
        {
            var languageConfig = LanguageConfigBuilder.Create()
                .WithSpecificDirectories(
                    ScanLocations.GenerateScanDirectories(
#if NET471
                    "net471",
#else
                    "netcoreapp3.1",
#endif
                        "Application.Demo",
                        "Application.Demo.Addon",
                        "Application.Demo.MetroAddon",
                        "Application.Demo.OverlayAddon").ToArray()
                )
                .BuildLanguageConfig();


            var filePattern = new Regex(languageConfig.FileNamePattern, RegexOptions.Compiled);

            var groupedFiles = FileLocations.Scan(languageConfig.SpecifiedDirectories, filePattern)
                .GroupBy(x => x.Item2.Groups["IETF"].Value)
                .ToDictionary(group => group.Key, group => group.Select(x => x.Item1)
                    .ToList());
            Assert.NotEmpty(groupedFiles);
            var foundFiles = FileLocations.Scan(languageConfig.SpecifiedDirectories, filePattern).Select(tuple => tuple.Item1).Select(Path.GetFileName).ToList();
            Assert.Contains("language_addon1-de-DE.xml", foundFiles);
            Assert.Contains("language_addon1-en-US.xml", foundFiles);
        }

        [Fact]
        public void Test_Tree()
        {
            var item1 = new MenuItem
            {
                Id = "A_1"
            };
            var item11 = new MenuItem
            {
                Id = "A_1_1",
                ParentId = "A_1"
            };
            var item12 = new MenuItem
            {
                Id = "A_1_2",
                ParentId = "A_1"
            };
            var item0 = new MenuItem
            {
                Id = "A_0"
            };
            var items = new List<IMenuItem> {
                item11, item1, item12, item0
            };

            bool wasA0AlreadySeen = false;

            var tree = items.CreateTree();
            foreach (var treeNode in tree)
            {
                if (treeNode.Id == "A_1")
                {
                    Assert.True(treeNode.ChildNodes.Count == 2);
                }
                if (treeNode.Id == "A_1")
                {
                    Assert.True(wasA0AlreadySeen);
                }
                if (treeNode.Id == "A_0")
                {
                    wasA0AlreadySeen = true;
                }
            }
        }
    }
}
