// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

namespace Application.Demo.UseCases.Configuration.ViewModels
{
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class LanguageConfigViewModel : ConfigScreen, IDisposable
    {
        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        private readonly LanguageContainer _languageContainer;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Used from the View
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public IDictionary<string, string> AvailableLanguages => _languageContainer.AvailableLanguages;

        /// <summary>
        ///     Can the login button be pressed?
        /// </summary>
        public bool CanChangeLanguage => !string.IsNullOrWhiteSpace(DemoConfiguration.Language) && DemoConfiguration.Language != _languageContainer.CurrentLanguage;

        public ICoreTranslations CoreTranslations { get; }

        public IDemoConfiguration DemoConfiguration { get;}

        public LanguageConfigViewModel(
            LanguageContainer languageContainer,
            ICoreTranslations coreTranslations,
            IDemoConfiguration demoConfiguration,
            IEventAggregator eventAggregator)
        {
            _languageContainer = languageContainer;
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
            Execute.OnUIThread(async () =>
            {
                await _languageContainer.ChangeLanguageAsync(DemoConfiguration.Language).ConfigureAwait(false);
            });
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