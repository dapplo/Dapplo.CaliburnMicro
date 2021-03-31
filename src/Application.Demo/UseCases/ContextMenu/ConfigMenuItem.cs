// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Application.Demo.UseCases.Configuration.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry for the configuration to the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class ConfigMenuItem : ClickableMenuItem
    {
        private static readonly LogSource Log = new();

        private readonly ConfigViewModel _demoConfigViewModel;
        private readonly IWindowManager _windowManager;

        public ConfigMenuItem(
            IWindowManager windowManager,
            IContextMenuTranslations contextMenuTranslations,
            ConfigViewModel demoConfigViewModel)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Configure));

            _demoConfigViewModel = demoConfigViewModel;
            _windowManager = windowManager;
        }

        public override void Click(IMenuItem clickedItem)
        {
            Log.Debug().WriteLine("Configure");
            if (!_demoConfigViewModel.IsActive)
            {
                _windowManager.ShowDialog(_demoConfigViewModel);
            }
        }

        public override void Initialize()
        {
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Cog,
                Spin = true,
                SpinDuration = 3
            };
            HotKeyHint = "Alt+C";
        }
    }
}