using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Language;

namespace Dapplo.CaliburnMicro.Translations
{
	/// <summary>
	/// This registers a ServiceProviderExportProvider for providing ILanguage
	/// </summary>
	[StartupAction(StartupOrder = int.MinValue)]
	public class LanguageStartup : IAsyncStartupAction
	{
		[Import]
		private IApplicationBootstrapper ApplicationBootstrapper { get; set; }

		public async Task StartAsync(CancellationToken token = new CancellationToken())
		{
			var languageLoader = LanguageLoader.Current;
			if (languageLoader == null)
			{
				languageLoader = LanguageLoader.Current ?? new LanguageLoader(ApplicationBootstrapper.ApplicationName);
				await languageLoader.LoadIfNeededAsync(token);
			}
			ApplicationBootstrapper.Export<IServiceProvider>(languageLoader);

			var s = ApplicationBootstrapper.GetExports<IServiceProvider>();
			if (!s.Any())
			{
				throw new Exception();
			}
		}
	}
}
