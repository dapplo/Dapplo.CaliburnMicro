// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard
{
    /// <summary>
    ///     Every screen in a wizard should implement this
    ///     Some of the wizard functionality is covered in standard Caliburn interfaces, which are supplied by the interfaces
    ///     which IScreen extends:
    ///     IHaveDisplayName: Covers the name and title of the Wizard screen
    ///     INotifyPropertyChanged: Makes it possible that changes to e.g. the name are directly represented in the view
    ///     IActivate, IDeactivate: to know if the wizard screen is activated or deactivated
    ///     IGuardClose.CanClose: Prevents leaving the wizard screen
    ///     A default implementation is to extend Screen
    /// </summary>
    public interface IWizardScreen : IScreen, IAmDisplayable
    {
        /// <summary>
        ///     Defines if we can leave the current screen, for the next, as it's complete
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        ///     The order in which the IWizardScreen ist shown
        /// </summary>
        int Order { get; }

        /// <summary>
        ///     The parent wizard where this IWizardScreen is used
        /// </summary>
        IWizard ParentWizard { get; set; }

        /// <summary>
        ///     Do some general initialization, if needed
        ///     This is called when the parent wizard is initializing.
        ///     ParentWizard will be set before this is called
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Terminate the wizard screen.
        ///     This is called when the parent wizard is terminated
        /// </summary>
        void Terminate();
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TParentWizard"></typeparam>
    public interface IWizardScreen<TParentWizard> : IWizardScreen where TParentWizard : IWizard
    {
        /// <summary>
        ///     The parent wizard where this IWizardScreen is used
        /// </summary>
        new TParentWizard ParentWizard { get; set; }
    }
}