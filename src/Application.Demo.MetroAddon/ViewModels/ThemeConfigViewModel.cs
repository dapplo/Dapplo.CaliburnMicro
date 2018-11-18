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
using Dapplo.Utils.Extensions;

#endregion

namespace Application.Demo.MetroAddon.ViewModels
{
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class ThemeConfigViewModel : ConfigScreen, IDisposable
    {
        private readonly MetroWindowManager _metroWindowManager;

        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        /// <summary>
        ///     The avaible theme accents
        /// </summary>
        public ObservableCollection<Tuple<ThemeAccents, string>> AvailableThemeAccents { get; set; } = new ObservableCollection<Tuple<ThemeAccents, string>>();

        /// <summary>
        ///     The avaible themes
        /// </summary>
        public ObservableCollection<Tuple<Themes, string>> AvailableThemes { get; set; } = new ObservableCollection<Tuple<Themes, string>>();

        public IMetroConfiguration MetroConfiguration { get; }

        public IUiTranslations UiTranslations { get; }


        public ThemeConfigViewModel(
            IMetroConfiguration metroConfiguration,
            IUiTranslations uiTranslations,
            MetroWindowManager metroWindowManager = null
            )
        {
            MetroConfiguration = metroConfiguration;
            UiTranslations = uiTranslations;
            _metroWindowManager = metroWindowManager;
        }

        /// <inheritdoc />
        public override void Rollback()
        {
            // Nothing to do
        }

        /// <inheritdoc />
        public override void Terminate()
        {
            // Nothing to do
        }

        public override void Commit()
        {
            // Manually commit
            MetroConfiguration.CommitTransaction();
            if (_metroWindowManager == null)
            {
                return;
            }
            _metroWindowManager.ChangeTheme(MetroConfiguration.Theme, MetroConfiguration.ThemeAccent);
        }

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

            base.Initialize(config);
        }

        protected override void OnDeactivate(bool close)
        {
            _disposables?.Dispose();
            base.OnDeactivate(close);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}