// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels
{
    /// <summary>
    ///     A default implementation for the ITrayIconViewModel
    /// </summary>
    public class TrayIconViewModel : Screen, ITrayIconViewModel
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        /// The ITrayIconManager
        /// </summary>
        protected ITrayIconManager TrayIconManager { get; }

        /// <summary>
        /// 
        /// </summary>
        public TrayIconViewModel(ITrayIconManager trayIconManager)
        {
            TrayIconManager = trayIconManager;
            // Make sure the default DisplayName (class name) is not used on the ToolTipText
            // ReSharper disable once VirtualMemberCallInConstructor, I know what I am doing here...
            DisplayName = "";
        }

        /// <summary>
        ///     The ITrayIcon for the ViewModel
        /// </summary>
        public ITrayIcon TrayIcon => TrayIconManager.GetTrayIconFor(this);

        /// <summary>
        ///     These are the Context MenuItems for the system tray
        /// </summary>
        public ObservableCollection<ITreeNode<IMenuItem>> TrayMenuItems { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

        /// <summary>
        ///     Show the icon for this ViewModel
        /// </summary>
        public virtual void Show()
        {
            TrayIcon.Show();
        }

        /// <summary>
        ///     Hide the icon for this ViewModel
        /// </summary>
        public virtual void Hide()
        {
            TrayIcon.Hide();
        }

        /// <summary>
        ///     Set the Icon to the underlying TrayIcon.Icon
        /// </summary>
        public void SetIcon(FrameworkElement value)
        {
            var taskbarIcon = TrayIcon as FrameworkElement;
            taskbarIcon?.SetCurrentValue(FrameworkElementIcon.ValueProperty, value);
        }

        /// <summary>
        ///     This is called when someone makes a a left-click on the icon
        /// </summary>
        public virtual void Click()
        {
            // No implementation, unless overridden
            Log.Verbose().WriteLine("Left-click");
        }

        /// <summary>
        ///     Call to set the TrayMenuItems, call this from the UI thread!
        /// </summary>
        /// <param name="menuItems">IEnumerable with IMenuItem</param>
        protected void ConfigureMenuItems(IEnumerable<IMenuItem> menuItems)
        {
            var items = menuItems.ToList();
            // Make sure all items are initialized
            foreach (var menuItem in items)
            {
                menuItem.Initialize();
            }

            foreach (var contextMenuItem in items.CreateTree())
            {
                TrayMenuItems.Add(contextMenuItem);
            }
        }
    }
}