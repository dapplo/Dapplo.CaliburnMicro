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

using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard
{
    /// <summary>
    ///     A very simple implementation of IWizardScreen
    /// </summary>
    public class WizardScreen<TWizard> : Screen, IWizardScreen<TWizard> where TWizard : class, IWizard
    {
        private bool _isComplete = true;
        private bool _isEnabled = true;
        private bool _isVisible = true;
        private int _order = 1;
        private TWizard _parent;

        /// <summary>
        ///     The order in which the screens are shown
        /// </summary>
        public virtual int Order
        {
            get => _order;
            protected set
            {
                _order = value;
                NotifyOfPropertyChange(nameof(Order));
            }
        }

        /// <summary>
        ///     The parent wizard where this IWizardScreen is used
        /// </summary>
        public TWizard ParentWizard
        {
            get => _parent;
            set
            {
                _parent = value;
                NotifyOfPropertyChange(nameof(ParentWizard));
            }
        }


        /// <summary>
        ///     The parent wizard where this IWizardScreen is used
        /// </summary>
        IWizard IWizardScreen.ParentWizard
        {
            get => ParentWizard;
            set => ParentWizard = value as TWizard;
        }

        /// <summary>
        ///     Do some general initialization, if needed
        ///     This is called when the IWizard is initializing, no matter if the IWizardScreen is shown.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        ///     Cleanup the wizard screen
        ///     This is called when the IWizard terminates, no matter if the IWizardScreen was shown.
        /// </summary>
        public virtual void Terminate()
        {
        }

        /// <summary>
        ///     Returns if the wizard screen can be selected
        /// </summary>
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            protected set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        /// <summary>
        ///     Returns if the wizard screen is visible
        /// </summary>
        public virtual bool IsVisible
        {
            get => _isVisible;
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(nameof(IsVisible));
            }
        }

        /// <summary>
        ///     Returns if the wizard screen is complete (to go to the next)
        /// </summary>
        public virtual bool IsComplete
        {
            get => _isComplete;
            protected set
            {
                _isComplete = value;
                NotifyOfPropertyChange(nameof(IsComplete));
            }
        }
    }
}