// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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