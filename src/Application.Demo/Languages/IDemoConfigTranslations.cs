// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Translations;
using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("Config")]
    public interface IDemoConfigTranslations : IConfigTranslations, ILanguage
    {
        string Addons { get; }
        string Apply { get; }
        string RestoreDefaults { get; }
        string Ui { get; }
    }
}