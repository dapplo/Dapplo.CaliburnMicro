// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry to the context menu which generates an exception, and causes a popup to show.
    /// </summary>
    [Menu("contextmenu")]
    public sealed class CreateErrorMenuItem : ClickableMenuItem
    {
        public CreateErrorMenuItem(IContextMenuTranslations contextMenuTranslations)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.CreateError));
            Id = "X_Error";
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Exclamation,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
            ClickAction = clickedItem => throw new NotSupportedException("This should be shown in an error windows!");
            this.ApplyIconForegroundColor(Brushes.DarkRed);
        }
    }
}