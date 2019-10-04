//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard.ViewModels
{
    /// <summary>
    ///     A ViewModel to display the progress of a wizard.
    ///     This is based upon the stackoverflow question
    ///     <a href="http://stackoverflow.com/questions/7691386/implementing-a-wizard-progress-control-in-wpf">here</a>
    /// </summary>
    public class WizardProgressViewModel : Screen
    {
        private const string MapAppsHighlightBrush = "HighlightBrush";
        private const string MapAppsAccentColorBrush4 = "AccentColorBrush4";

        private Brush _disabledColorBrush;
        private Brush _progressColorBrush;

        /// <summary>
        ///     Design-Mode contructor, will create a design time IWizard
        /// </summary>
        public WizardProgressViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                throw new InvalidOperationException("Should only be used in design mode.");
            }
            // Only for Design mode
            var wizard = new Wizard<IWizardScreen>
            {
                DisplayName = "Test",
                WizardScreens = new[]
                {
                    new WizardScreen<IWizard>
                    {
                        DisplayName = "Step 1",
                        ParentWizard = Wizard
                    },
                    new WizardScreen<IWizard>
                    {
                        DisplayName = "Step 2",
                        ParentWizard = Wizard
                    },
                    new WizardScreen<IWizard>
                    {
                        DisplayName = "Step 3",
                        ParentWizard = Wizard
                    },
                    new WizardScreen<IWizard>
                    {
                        DisplayName = "Step 4",
                        ParentWizard = Wizard
                    },
                    new WizardScreen<IWizard>
                    {
                        DisplayName = "Step 5",
                        ParentWizard = Wizard
                    }
                }
            };
            Initialize(wizard);
            Wizard.Initialize();
            Wizard.CurrentWizardScreen = Wizard.WizardScreens.Skip(2).First();
        }

        /// <summary>
        ///     Constructor which takes an IWizard, as it's required
        /// </summary>
        /// <param name="wizard">IWizard</param>
        public WizardProgressViewModel(IWizard wizard)
        {
            Initialize(wizard);
        }

        /// <summary>
        ///     Brush for the disabled wizard text
        /// </summary>
        public Brush DisabledColorBrush
        {
            get { return _disabledColorBrush; }
            set
            {
                NotifyOfPropertyChange(nameof(DisabledColorBrush));
                _disabledColorBrush = value;
            }
        }

        /// <summary>
        ///     Brush for the progress line and dots
        /// </summary>
        public Brush ProgressColorBrush
        {
            get { return _progressColorBrush; }
            set
            {
                NotifyOfPropertyChange(nameof(ProgressColorBrush));
                _progressColorBrush = value;
            }
        }

        /// <summary>
        ///     The IWizard
        /// </summary>
        public IWizard Wizard { get; set; }

        /// <summary>
        ///     A sorted view on the WizardScreens
        /// </summary>
        public ICollectionView WizardScreensView { get; set; }

        /// <summary>
        ///     Initialize this with the information in the IWizard
        /// </summary>
        /// <param name="wizard">IWizard</param>
        private void Initialize(IWizard wizard)
        {
            Wizard = wizard;
            if (Wizard.WizardScreens == null)
            {
                throw new ArgumentNullException(nameof(wizard.WizardScreens));
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

        /// <summary>
        ///     Makes the clicked item active, if this is allowed
        /// </summary>
        /// <param name="wizardScreen"></param>
        public void JumpTo(IWizardScreen wizardScreen)
        {
            if (wizardScreen.IsEnabled && wizardScreen.IsVisible && !wizardScreen.IsActive)
            {
                Wizard.CurrentWizardScreen = wizardScreen;
            }
        }

        /// <summary>Called when a view is attached.</summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        protected override void OnViewAttached(object view, object context)
        {
            // Retrieve the values from MapApps, if they can be found
            ProgressColorBrush = Application.Current.TryFindResource(MapAppsHighlightBrush) as SolidColorBrush ?? SystemColors.ControlTextBrush;
            DisabledColorBrush = Application.Current.TryFindResource(MapAppsAccentColorBrush4) as SolidColorBrush ?? Brushes.Gray;
        }
    }
}