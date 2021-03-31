// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Toasts.ViewModels;

namespace Application.Demo.UseCases.Toast.ViewModels
{
    public class ToastExampleViewModel : ToastBaseViewModel
    {
        public string Message => "Hello World";

        public string Text { get; set; }
    }
}
