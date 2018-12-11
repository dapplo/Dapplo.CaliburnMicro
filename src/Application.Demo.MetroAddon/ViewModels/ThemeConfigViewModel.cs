//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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

#region using

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using Application.Demo.MetroAddon.Configurations;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.CaliburnMicro.Metro.Configuration;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.Config.Intercepting;
using Dapplo.Utils.Extensions;

#endregion

namespace Application.Demo.MetroAddon.ViewModels
{
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class ThemeConfigViewModel : ConfigScreen, IDisposable
    {
        private readonly MetroThemeManager _metroThemeManager;
        private readonly ITrayIconManager _trayIconManager;

        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        /// <summary>
        ///     The available theme accents
        /// </summary>
        public ObservableCollection<Tuple<ThemeAccents, string>> AvailableThemeAccents { get; set; } = new ObservableCollection<Tuple<ThemeAccents, string>>();

        /// <summary>
        ///     The available themes
        /// </summary>
        public ObservableCollection<Tuple<Themes, string>> AvailableThemes { get; set; } = new ObservableCollection<Tuple<Themes, string>>();

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
        /// <param name="trayIconManager">TrayIconManager</param>
        public ThemeConfigViewModel(
            IMetroConfiguration metroConfiguration,
            IUiTranslations uiTranslations,
            MetroThemeManager metroThemeManager,
            ITrayIconManager trayIconManager
            )
        {
            MetroConfiguration = metroConfiguration;
            UiTranslations = uiTranslations;
            _metroThemeManager = metroThemeManager;
            _trayIconManager = trayIconManager;
        }

        /// <inheritdoc />
        public override void Rollback()
        {
            MetroConfiguration.RollbackTransaction();
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeAccent);
        }

        /// <inheritdoc />
        public override void Terminate()
        {
            MetroConfiguration.RollbackTransaction();
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeAccent);
        }

        public override void Commit()
        {
            _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeAccent);
            // Manually commit
            MetroConfiguration.CommitTransaction();
            _trayIconManager.TrayIcons.ForEach(trayIcon =>
            {
                trayIcon.Hide();
                trayIcon.Show();
            });
        }

        /// <inheritdoc />
        public override void Initialize(IConfig config)
        {
            // Prepare disposables
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            AvailableThemeAccents.Clear();
            foreach (var themeAccent in Enum.GetValues(typeof(ThemeAccents)).Cast<ThemeAccents>())
            {
                var translation = themeAccent.EnumValueOf();
                AvailableThemeAccents.Add(new Tuple<ThemeAccents, string>(themeAccent, translation));
            }

            AvailableThemes.Clear();
            foreach (var theme in Enum.GetValues(typeof(Themes)).Cast<Themes>())
            {
                var translation = theme.EnumValueOf();
                AvailableThemes.Add(new Tuple<Themes, string>(theme, translation));
            }

            // Place this under the Ui parent
            ParentId = "Ui";

            // Make sure Commit/Rollback is called on the UiConfiguration
            config.Register(MetroConfiguration);

            // automatically update the DisplayName
            _disposables.Add(UiTranslations.CreateDisplayNameBinding(this, nameof(IUiTranslations.Theme)));

            // Automatically show theme changes
            _disposables.Add(
                MetroConfiguration.OnPropertyChanging(nameof(MetroConfiguration.ThemeAccent)).Subscribe(args =>
                {
                    if (args is PropertyChangingEventArgsEx propertyChangingEventArgsEx)
                    {
                        _metroThemeManager.ChangeTheme(MetroConfiguration.Theme, (ThemeAccents)propertyChangingEventArgsEx.NewValue);
                    }
                })
            );

            // Automatically show theme accent changes
            _disposables.Add(
                MetroConfiguration.OnPropertyChanging(nameof(MetroConfiguration.Theme)).Subscribe(args =>
                {
                    if (args is PropertyChangingEventArgsEx propertyChangingEventArgsEx)
                    {
                        _metroThemeManager.ChangeTheme((Themes)propertyChangingEventArgsEx.NewValue, MetroConfiguration.ThemeAccent);
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