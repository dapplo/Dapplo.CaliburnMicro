// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;

namespace Application.Demo.MetroAddon.Services
{
    /// <summary>
    /// Configure some of the CaliburnMicro defaults
    /// </summary>
    [Service(nameof(ConfigureDefaults), nameof(CaliburnServices.CaliburnMicroBootstrapper), TaskSchedulerName = "ui")]
    public class ConfigureDefaults : IStartup
    {
        private readonly ResourceManager _resourceManager;

        public ConfigureDefaults(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        /// <inheritdoc />
        public void Startup()
        {
            // Override the ConfigView with a much nicer looking version
            ViewLocator.NameTransformer.AddRule(@"^Application\.Demo\.UseCases\.Configuration\.ViewModels\.ConfigViewModel$", "Application.Demo.MetroAddon.Views.ConfigView");
            var demoResources = new Uri("pack://application:,,,/Application.Demo;component/DemoResourceDirectory.xaml", UriKind.RelativeOrAbsolute);
            _resourceManager.AddResourceDictionary(demoResources);
        }
    }
}