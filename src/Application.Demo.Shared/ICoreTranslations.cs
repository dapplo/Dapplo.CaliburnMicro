// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Diagnostics.Translations;
using Dapplo.Config.Language;

namespace Application.Demo.Shared
{
    [Language("Core")]
    public interface ICoreTranslations : ILanguage, Dapplo.CaliburnMicro.Translations.ICoreTranslations, IErrorTranslations
    {
        string Language { get; }
        string Settings { get; }
    }
}