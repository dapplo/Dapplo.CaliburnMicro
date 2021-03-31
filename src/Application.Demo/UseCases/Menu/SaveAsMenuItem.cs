// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.Menu
{
    /// <summary>
    ///     This will add the File item to menu
    /// </summary>
    [Menu("menu")]
    public sealed class SaveAsMenuItem : MenuItem
    {
        private readonly IMenuTranslations _menuTranslations;

        public SaveAsMenuItem(IMenuTranslations menuTranslations)
        {
            _menuTranslations = menuTranslations;
        }

        public override void Initialize()
        {
            Id = "A_SaveAs";
            ParentId = "1_File";
            // automatically update the DisplayName
            _menuTranslations.CreateDisplayNameBinding(this, nameof(IMenuTranslations.SaveAs));
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.ContentSave
            };
        }
    }
}