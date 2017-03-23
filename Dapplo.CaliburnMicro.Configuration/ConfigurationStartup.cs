using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Ini;
using System;
using System.Linq;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	/// This registers a ServiceProviderExportProvider for providing IIniSection
	/// </summary>
	[StartupAction(StartupOrder = int.MinValue)]
	public class ConfigurationStartup : IAsyncStartupAction
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private IApplicationBootstrapper ApplicationBootstrapper { get; set; }

		public async Task StartAsync(CancellationToken token = new CancellationToken())
		{
			var iniConfig = IniConfig.Current;
			if (iniConfig == null)
			{
				iniConfig = new IniConfig(ApplicationBootstrapper.ApplicationName, ApplicationBootstrapper.ApplicationName);
				await iniConfig.LoadIfNeededAsync(token);
			}
			ApplicationBootstrapper.Export<IServiceProvider>(iniConfig);

			var s = ApplicationBootstrapper.GetExports<IServiceProvider>();
			if (!s.Any())
			{
				throw new Exception();
			}
		}
	}
}
