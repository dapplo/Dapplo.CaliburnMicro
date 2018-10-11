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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Application.Demo.Models;
using Application.Demo.Shared;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Config.Language;

#endregion

namespace Application.Demo.UseCases.Configuration.ViewModels
{
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class LanguageConfigViewModel : ConfigScreen, IDisposable
    {
        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Used from the View
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public IDictionary<string, string> AvailableLanguages { get; } = new Dictionary<string, string>(); // => LanguageLoader.Current.AvailableLanguages;

        /// <summary>
        ///     Can the login button be pressed?
        /// </summary>
        public bool CanChangeLanguage { get; } = false;
            // => !string.IsNullOrWhiteSpace(DemoConfiguration.Language) && DemoConfiguration.Language != LanguageLoader.Current.CurrentLanguage;

        public ICoreTranslations CoreTranslations { get; }

        public IDemoConfiguration DemoConfiguration { get;}

        public LanguageConfigViewModel(
            ICoreTranslations coreTranslations,
            IDemoConfiguration demoConfiguration,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DemoConfiguration = demoConfiguration;
            CoreTranslations = coreTranslations;
        }

        /// <inheritdoc />
        public override void Commit()
        {
            // Manually commit
            DemoConfiguration.CommitTransaction();
            _eventAggregator.PublishOnUIThread($"Changing to language: {DemoConfiguration.Language}");
            // TODO: Fix
            //Execute.OnUIThread(async () => { await LanguageLoader.Current.ChangeLanguageAsync(DemoConfiguration.Language).ConfigureAwait(false); });
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

        /// <inheritdoc />
        public override void Initialize(IConfig config)
        {
            // Prepare disposables
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            // Place this under the Ui parent
            ParentId = nameof(ConfigIds.Ui);

            // Make sure Commit/Rollback is called on the IDemoConfiguration
            config.Register(DemoConfiguration);

            // automatically update the DisplayName
            _disposables.Add(CoreTranslations.CreateDisplayNameBinding(this, nameof(ICoreTranslations.Language)));

            // automatically update the CanChangeLanguage state when a different language is selected
            _disposables.Add(DemoConfiguration.OnPropertyChanged(nameof(IDemoConfiguration.Language)).Subscribe(pcEvent => NotifyOfPropertyChange(nameof(CanChangeLanguage))));

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