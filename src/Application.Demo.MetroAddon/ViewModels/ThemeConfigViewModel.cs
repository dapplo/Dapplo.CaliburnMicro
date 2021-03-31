// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Application.Demo.MetroAddon.Configurations;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.Config.Intercepting;

namespace Application.Demo.MetroAddon.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the theme configuration
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class ThemeConfigViewModel : ConfigScreen, IDisposable
    {
        private readonly MetroThemeManager _metroThemeManager;

        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        /// <summary>
        ///     The available themes
        /// </summary>
        public ObservableCollection<string> AvailableThemes { get; set; } = new();

        /// <summary>
        ///     The available theme colors
        /// </summary>
        public ObservableCollection<string> AvailableThemeColors { get; set; } = new();

        /// <summary>
        /// Used in the UI
        /// </summary>
        public IMetroConfiguration MetroConfiguration { get; }

        /// <summary>
        /// Used in the UI
        /// </summary>
        public IUiTranslations UiTranslations { get; }

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="metroConfiguration">IMetroConfiguration</param>
        /// <param name="uiTranslations">IUiTranslations</param>
        /// <param name="metroThemeManager">MetroThemeManager</param>
        public ThemeConfigViewModel(
            IMetroConfiguration metroConfiguration,
            IUiTranslations uiTranslations,
            MetroThemeManager metroThemeManager
            )
        {
            MetroConfiguration = metroConfiguration;
            UiTranslations = uiTranslations;
            _metroThemeManager = metroThemeManager;
        }

        /// <inheritdoc />
        public override void Rollback()
        {
            MetroConfiguration.RollbackTransaction();
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeColor);
        }

        /// <inheritdoc />
        public override void Terminate()
        {
            MetroConfiguration.RollbackTransaction();
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeColor);
        }

        public override void Commit()
        {
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeColor);
            // Manually commit
            MetroConfiguration.CommitTransaction();
        }

        /// <inheritdoc />
        public override void Initialize(IConfig config)
        {
            // Prepare disposables
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            AvailableThemes.Clear();
            foreach (var theme in MetroThemeManager.AvailableThemes)
            {
                AvailableThemes.Add(theme);
            }
            foreach (var themeColor in MetroThemeManager.AvailableThemeColors)
            {
                AvailableThemeColors.Add(themeColor);
            }

            // Place this under the Ui parent
            ParentId = "Ui";

            // Make sure Commit/Rollback is called on the UiConfiguration
            config.Register(MetroConfiguration);

            // automatically update the DisplayName
            _disposables.Add(UiTranslations.CreateDisplayNameBinding(this, nameof(IUiTranslations.Theme)));

            // Automatically show theme changes
            _disposables.Add(
                MetroConfiguration.OnPropertyChanging(nameof(MetroConfiguration.Theme)).Subscribe(args =>
                {
                    if (args is PropertyChangingEventArgsEx propertyChangingEventArgsEx)
                    {
                        _metroThemeManager.ChangeTheme(propertyChangingEventArgsEx.NewValue as string, null);
                    }
                })
            );
            _disposables.Add(
                MetroConfiguration.OnPropertyChanging(nameof(MetroConfiguration.ThemeColor)).Subscribe(args =>
                {
                    if (args is PropertyChangingEventArgsEx propertyChangingEventArgsEx)
                    {
                        _metroThemeManager.ChangeTheme(null, propertyChangingEventArgsEx.NewValue as string);
                    }
                })
            );
            base.Initialize(config);
        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            _disposables?.Dispose();
            base.OnDeactivate(close);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}