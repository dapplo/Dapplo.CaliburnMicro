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

using System;
using System.Windows.Media;
using Caliburn.Micro;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace Dapplo.CaliburnMicro.Wizard.ViewModels
{
	/// <summary>
	/// A ViewModel to display the progress of a wizard.
	/// This is based upon the stackoverflow question <a href="http://stackoverflow.com/questions/7691386/implementing-a-wizard-progress-control-in-wpf">here</a>
	/// </summary>
	public class WizardProgressViewModel : PropertyChangedBase
	{
		private const string MapAppsHighlightBrush = "HighlightBrush";
		private const string MapAppsAccentColorBrush4 = "AccentColorBrush4";
		private Brush _progressColorBrush = SystemColors.ControlTextBrush;

		private Brush _disabledColorBrush = Brushes.Gray;

		/// <summary>
		/// Brush for the progress line and dots
		/// </summary>
		public Brush ProgressColorBrush
		{
			get
			{
				return _progressColorBrush;
			}
			set
			{
				NotifyOfPropertyChange(nameof(ProgressColorBrush));
				_progressColorBrush = value;
			}
		}

		/// <summary>
		/// Brush for the disabled wizard text
		/// </summary>
		public Brush DisabledColorBrush
		{
			get
			{
				return _disabledColorBrush;
			}
			set
			{
				NotifyOfPropertyChange(nameof(DisabledColorBrush));
				_disabledColorBrush = value;
			}
		}

		/// <summary>
		/// A sorted view on the WizardScreens
		/// </summary>
		public ICollectionView WizardScreensView { get; set; }

		/// <summary>
		/// The IWizard
		/// </summary>
		public IWizard Wizard { get; set; }

		/// <summary>
		/// Constructor which takes an IWizard, as it's required
		/// </summary>
		/// <param name="wizard"></param>
		public WizardProgressViewModel(IWizard wizard)
		{
			Wizard = wizard;
			if (Wizard.WizardScreens == null)
			{
				throw new ArgumentNullException(nameof(wizard.WizardScreens));
			}
			// Retrieve the values from MapApps, if they can be found
			if (Application.Current.Resources.Contains(MapAppsHighlightBrush))
			{
				_progressColorBrush = Application.Current.Resources[MapAppsHighlightBrush] as SolidColorBrush;
			}
			if (Application.Current.Resources.Contains(MapAppsAccentColorBrush4))
			{
				_disabledColorBrush = Application.Current.Resources[MapAppsAccentColorBrush4] as SolidColorBrush;
			}

			// Make sure the view is created via the dispatcher
			Application.Current.Dispatcher.Invoke(() =>
			{
				WizardScreensView = CollectionViewSource.GetDefaultView(wizard.WizardScreens);
				WizardScreensView.Filter = o =>
				{
					IWizardScreen wizardScreen = o as IWizardScreen;
					return wizardScreen?.IsVisible == true;
				};
			});
		}
	}
}