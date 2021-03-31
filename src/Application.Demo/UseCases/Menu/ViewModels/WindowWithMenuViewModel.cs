﻿// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Application.Demo.Languages;
using Autofac.Features.AttributeFilters;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;

namespace Application.Demo.UseCases.Menu.ViewModels
{
    /// <summary>
    /// A window with menu
    /// </summary>
    public sealed class WindowWithMenuViewModel : Screen, IDisposable, IMaintainPosition
    {
        private readonly IMenuTranslations _menuTranslations;
        private readonly IContextMenuTranslations _contextMenuTranslations;
        private readonly IEnumerable<Lazy<IMenuItem>> _menuItems;

        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        public WindowWithMenuViewModel(
            IMenuTranslations menuTranslations,
            IContextMenuTranslations contextMenuTranslations,
            [MetadataFilter("Menu", "menu")]IEnumerable<Lazy<IMenuItem>> menuItems
            )
        {
            _menuTranslations = menuTranslations;
            _contextMenuTranslations = contextMenuTranslations;
            _menuItems = menuItems;
        }

        // ReSharper disable once CollectionNeverQueried.Global
        public ObservableCollection<ITreeNode<IMenuItem>> Items { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();
        
        /// <summary>
        /// Used to show a date in the UI
        /// </summary>
        public DateTimeOffset CurrentDate { get; } = DateTimeOffset.Now;

        protected override void OnActivate()
        {
            // Prepare disposables
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            // Remove all items, so we can build them
            Items.Clear();

            var contextMenuNameBinding = _contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.SomeWindow));

            // Make sure the contextMenuNameBinding is disposed when this is no longer active
            _disposables.Add(contextMenuNameBinding);

            var items = _menuItems.Select(x => x.Value).ToList();
            var fileMenuItem = new MenuItem
            {
                Id = "1_File"
            };
            var menuNameBinding = _menuTranslations.CreateDisplayNameBinding(fileMenuItem, nameof(IMenuTranslations.File));
            // Make sure the menuNameBinding is disposed when this is no longer active
            _disposables.Add(menuNameBinding);
            items.Add(fileMenuItem);

            var editMenuItem = new MenuItem
            {
                Id = "2_Edit"
            };
            menuNameBinding.AddDisplayNameBinding(editMenuItem, nameof(IMenuTranslations.Edit));
            items.Add(editMenuItem);

            var aboutMenuItem = new MenuItem
            {
                Id = "3_About"
            };
            menuNameBinding.AddDisplayNameBinding(aboutMenuItem, nameof(IMenuTranslations.About));
            items.Add(aboutMenuItem);

            items.Add(new MenuItem
            {
                Style = MenuItemStyles.Separator,
                Id = "Y_Separator",
                ParentId = "1_File"
            });

            var exitMenuItem = new ClickableMenuItem
            {
                Id = "Z_Exit",
                ParentId = "1_File",
                ClickAction = clickedMenuItem => Dapplication.Current.Shutdown()
            };
            contextMenuNameBinding.AddDisplayNameBinding(exitMenuItem, nameof(IContextMenuTranslations.Exit));
            items.Add(exitMenuItem);

            // Make sure all items are initialized
            foreach (var menuItem in items)
            {
                menuItem.Initialize();
            }
            // Add the root elements to the Items
            foreach (var item in items.CreateTree())
            {
                Items.Add(item);
            }

            base.OnActivate();
        }

        /// <summary>
        ///     Called when deactivating.
        ///     Removes all event subscriptions
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected override void OnDeactivate(bool close)
        {
            _disposables?.Dispose();
            base.OnDeactivate(close);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}