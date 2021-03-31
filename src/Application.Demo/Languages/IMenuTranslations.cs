// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("Menu")]
    public interface IMenuTranslations : ILanguage
    {
        string About { get; }
        string Edit { get; }
        string File { get; }
        string SaveAs { get; }
    }
}