// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("Toasts")]
    public interface IToastTranslations : ILanguage
    {
        [DefaultValue("Startup finished")]
        string StartupNotify { get; }
    }
}