//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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
