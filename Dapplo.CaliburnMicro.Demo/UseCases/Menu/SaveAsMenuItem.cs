#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Menu
{
	/// <summary>
	/// This will add the File item to menu
	/// </summary>
	[Export("menu", typeof(IMenuItem))]
	public sealed class SaveAsMenuItem : MenuItem
	{
		[Import]
		private IMenuTranslations MenuTranslations { get; set; }

		public override void Initialize()
		{
			Id = "A_SaveAs";
			ParentId = "1_File";
			// automatically update the DisplayName
			this.BindDisplayName(MenuTranslations, nameof(IMenuTranslations.SaveAs));
			Icon = new PackIconMaterial
			{
				Kind = PackIconMaterialKind.ContentSave
			};
		}

		public override void Click(IMenuItem clickedItem)
		{
			
		}
	}
}