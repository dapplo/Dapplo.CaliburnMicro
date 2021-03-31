// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Config.Language;

namespace Dapplo.CaliburnMicro.Translations
{
    /// <summary>
    /// Loads the translations at startup
    /// </summary>
    [Service(nameof(CaliburnServices.LanguageService), nameof(CaliburnServices.CaliburnMicroBootstrapper))]
    public class LanguageService : IStartupAsync
    {
        private readonly LanguageContainer _languageContainer;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="languageContainer">LanguageContainer</param>
        public LanguageService(LanguageContainer languageContainer)
        {
            _languageContainer = languageContainer;
        }

        /// <inheritdoc />
        public Task StartupAsync(CancellationToken cancellationToken = default)
        {
            return _languageContainer.ReloadAsync(cancellationToken);
        }
    }
}
