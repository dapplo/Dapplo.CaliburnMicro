// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Demo.Languages;
using Application.Demo.Models;
using Application.Demo.Shared;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Config.Language;

namespace Application.Demo.UseCases.Configuration.ViewModels
{
    /// <summary>
    ///     The settings view model is, well... for the settings :)
    ///     It is a conductor where one item is active.
    /// </summary>
    public sealed class ConfigViewModel : Config<IConfigScreen>
    {
        /// <summary>
        /// Is used from View
        /// </summary>
        public IDemoConfigTranslations ConfigTranslations { get; }

        /// <summary>
        /// Is used from View
        /// </summary>
        public ICoreTranslations CoreTranslations { get; }

        public ConfigViewModel(
            LanguageContainer languageContainer,
            IEnumerable<Lazy<IConfigScreen>> configScreens,
            IDemoConfigTranslations configTranslations,
            ICoreTranslations coreTranslations,
            IDemoConfiguration demoConfiguration)
        {
            ConfigScreens = configScreens;
            ConfigTranslations = configTranslations;
            CoreTranslations = coreTranslations;

            // automatically update the DisplayName
            CoreTranslations.CreateDisplayNameBinding(this, nameof(ICoreTranslations.Settings));

            // Set the current language (this should update all registered OnPropertyChanged anyway, so it can run in the background
            var lang = demoConfiguration.Language;
            Task.Run(async () => await languageContainer.ChangeLanguageAsync(lang).ConfigureAwait(false));
        }
    }
}