// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

namespace Dapplo.CaliburnMicro.Wizard
{
    /// <summary>
    ///     Base interface for the IWizard
    /// </summary>
    public interface IWizard : INotifyPropertyChanged
    {
        /// <summary>
        ///     Can the current wizard "flow" be cancelled?
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        ///     Test if the wizard can be finished
        /// </summary>
        bool CanFinish { get; }

        /// <summary>
        ///     Can the wizard go to the next wizard screen, where IsEnabled/IsVisible are true, in the list. (e.g. false if we are
        ///     at the last)
        /// </summary>
        bool CanNext { get; }

        /// <summary>
        ///     Can the wizard go to the previous IsEnabled/IsVisible wizard screen? (e.g. false if we are at the first)
        /// </summary>
        bool CanPrevious { get; }

        /// <summary>
        ///     Returns the current wizard screen
        /// </summary>
        IWizardScreen CurrentWizardScreen { get; set; }

        /// <summary>
        ///     Test if we are at the first screen
        /// </summary>
        bool IsFirst { get; }

        /// <summary>
        ///     Test if we are at the last screen
        /// </summary>
        bool IsLast { get; }

        /// <summary>
        ///     The current progress
        /// </summary>
        int Progress { get; }

        /// <summary>
        ///     The TWizardScreen items of the wizard
        /// </summary>
        IEnumerable<IWizardScreen> WizardScreens { get; set; }

        /// <summary>
        ///     Cancel the wizard
        /// </summary>
        void Cancel();

        /// <summary>
        ///     Finish the wizard
        /// </summary>
        void Finish();

        /// <summary>
        ///     Initialize will o.a. initialize the wizard screens
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Go to the next wizard screen, where IsEnabled/IsVisible are true, in the list.
        /// </summary>
        bool Next();

        /// <summary>
        ///     Go to the previous wizard screen, where IsEnabled/IsVisible are true, in the list.
        /// </summary>
        bool Previous();

        /// <summary>
        ///     Cleanup the wizard.
        /// </summary>
        void Terminate();
    }

    /// <summary>
    ///     This is the generic interface for a wizard implementation
    /// </summary>
    public interface IWizard<out TWizardScreen> : IWizard
    {
        /// <summary>
        ///     Returns the current wizard screen
        /// </summary>
        new TWizardScreen CurrentWizardScreen { get; }

        /// <summary>
        ///     The TWizardScreen items of the wizard
        /// </summary>
        new IEnumerable<TWizardScreen> WizardScreens { get; }
    }
}