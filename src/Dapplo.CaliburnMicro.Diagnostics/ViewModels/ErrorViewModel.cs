// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Caliburn.Micro;
using System.Diagnostics;
using Dapplo.CaliburnMicro.Diagnostics.Translations;
using Dapplo.CaliburnMicro.Extensions;

namespace Dapplo.CaliburnMicro.Diagnostics.ViewModels
{
    /// <summary>
    /// This view model shows the error that occurred
    /// </summary>
    public class ErrorViewModel : Screen
    {
        private IDisposable _disposables;
        /// <summary>
        /// This is the version provider, which makes the screen show a warning when the current != latest
        /// </summary>
        public IVersionProvider VersionProvider { get; }

        /// <summary>
        /// This is used for the translations in the view
        /// </summary>
        public IErrorTranslations ErrorTranslations { get; }

        public ErrorViewModel(IErrorTranslations errorTranslations, IVersionProvider versionProvider)
        {
            ErrorTranslations = errorTranslations;
            VersionProvider = versionProvider;
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            _disposables = ErrorTranslations.CreateDisplayNameBinding(this, nameof(IErrorTranslations.ErrorTitle));
            base.OnActivate();
        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            _disposables?.Dispose();
            _disposables = null;
            base.OnDeactivate(close);
        }

        /// <summary>
        /// Checks if the current version is the latest
        /// </summary>
        public bool IsMostRecent => VersionProvider.CurrentVersion.Equals(VersionProvider.LatestVersion);

        /// <summary>
        /// Set the exception to display
        /// </summary>
        public void SetExceptionToDisplay(Exception exception)
        {
            Stacktrace = exception.ToStringDemystified();
            Message = exception.Message;
        }

        /// <summary>
        /// The stacktrace to display
        /// </summary>
        public string Stacktrace { get; set; }

        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }
    }
}
