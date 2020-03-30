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