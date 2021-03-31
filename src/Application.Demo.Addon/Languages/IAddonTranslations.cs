// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;
using System.ComponentModel;

namespace Application.Demo.Addon.Languages
{
    [Language("Addon1")]
    public interface IAddonTranslations : ILanguage
    {
        [DefaultValue("Blub")]
        string Addon { get; }

        [DefaultValue("Admin")]
        string Admin { get; }

        [DefaultValue("Can't touch me")]
        string NotSelectableAddon { get; }
    }
}