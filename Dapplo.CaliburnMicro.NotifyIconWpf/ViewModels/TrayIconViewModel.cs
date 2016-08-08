using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Controls;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Tree;
using Dapplo.Log.Facade;
using Hardcodet.Wpf.TaskbarNotification;

namespace Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels
{
	/// <summary>
	/// A default implementation for the ITrayIconViewModel
	/// </summary>
	public class TrayIconViewModel : Screen, ITrayIconViewModel
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		/// The ITrayIconManager, which makes it possible to show and hide the icon
		/// </summary>
		[Import]
		protected ITrayIconManager TrayIconManager { get; set; }

		/// <summary>
		/// The ITrayIcon for the ViewModel
		/// </summary>
		public ITrayIcon TrayIcon => TrayIconManager.GetTrayIconFor(this);

		/// <summary>
		/// These are the Context MenuItems for the system tray
		/// </summary>
		public ObservableCollection<ITreeNode<IMenuItem>> TrayMenuItems { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

		/// <summary>
		/// Show the icon for this ViewModel
		/// </summary>
		public virtual void Show()
		{
			TrayIcon.Show();
		}

		/// <summary>
		/// Hide the icon for this ViewModel
		/// </summary>
		public virtual void Hide()
		{
			TrayIcon.Hide();
		}

		/// <summary>
		/// Call to set the TrayMenuItems
		/// </summary>
		/// <param name="menuItems">IEnumerable with IMenuItem</param>
		protected void ConfigureMenuItems(IEnumerable<IMenuItem> menuItems)
		{
			foreach (var contextMenuItem in menuItems.CreateTree())
			{
				TrayMenuItems.Add(contextMenuItem);
			}
		}

		/// <summary>
		/// Set the Icon to the underlying TrayIcon.Icon
		/// </summary>
		public Icon Icon
		{
			get
			{
				var taskbarIcon = TrayIcon as TaskbarIcon;
				return taskbarIcon?.Icon;
			}
			set
			{
				var taskbarIcon = TrayIcon as TaskbarIcon;
				if (taskbarIcon != null)
				{
					taskbarIcon.Icon = value;
				}
			}
		}

		/// <summary>
		/// Set the Icon to the underlying TrayIcon.Icon, use this to prevent using System.Drawing
		/// </summary>
		public void SetIcon(Control control)
		{
			Icon = control.ToIcon();
		}

		/// <summary>
		/// This is called when someone makes a a left-click on the icon
		/// </summary>
		public virtual void Click()
		{
			// No implementation, unless overridden
			Log.Verbose().WriteLine("Left-click");
		}
	}
}
