// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Toasts.ViewModels;

namespace Application.Demo.UseCases.Toast.ViewModels
{
    /// <inheritdoc />
    public class StartupReadyToastViewModel : ToastBaseViewModel
    {
        private readonly IToastTranslations _toastTranslations;

        public StartupReadyToastViewModel(IToastTranslations toastTranslations)
        {
            _toastTranslations = toastTranslations;
        }

        /// <summary>
        /// This contains the message for the ViewModel
        /// </summary>
        public string Message => _toastTranslations.StartupNotify;
    }
}
