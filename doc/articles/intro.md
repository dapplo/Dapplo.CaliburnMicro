# Dapplo.CaliburnMicro

Install the following packages via the nuget package manager:

'''
Install-Package Dapplo.CaliburnMicro.Dapp
Install-Package Dapplo.CaliburnMicro.Menu
Install-Package Dapplo.CaliburnMicro.Configuration
Install-Package Dapplo.CaliburnMicro.Translations
Install-Package Dapplo.Log.Loggers
'''

Remove the App.xaml & MainWindow.xaml (if you didn't have logic in it), and create a new class in a file called Startup.cs.

'''
	/// <summary>
    ///     This takes care or starting the Application
    /// </summary>
    public static class Startup
    {
        /// <summary>
        ///     Start the application
        /// </summary>
        [STAThread]
        public static void Main()
        {
#if DEBUG
            // Initialize a debug logger for Dapplo packages
            LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Debug);
#endif

            // Use this to setup the default culture of your UI
            var cultureInfo = CultureInfo.GetCultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var application = new Dapplication("ApplicationName", "Unique GUID")
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };

            // Load additional assemblies
            application.Bootstrapper.FindAndLoadAssemblies("Component.*");
			// Start the application
            application.Run();
        }
    }
'''

Than add packages (directories) ending on ViewModels and Views, and start creating your viewmodels & views in there.
The ViewModel(s) classes marked with the ExportAttribute like this [Export(typeof(IShell))] will be shown automatically.
