//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
        public ObservableCollection<string> AvailableThemes { get; set; } = new ObservableCollection<string>();

        /// <summary>
        ///     The available theme colors
        /// </summary>
        public ObservableCollection<string> AvailableThemeColors { get; set; } = new ObservableCollection<string>();

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