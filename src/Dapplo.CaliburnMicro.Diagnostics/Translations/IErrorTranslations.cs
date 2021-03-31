// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Dapplo.CaliburnMicro.Diagnostics.Translations
{
    /// <summary>
    ///     These are the translations used on the ErrorView
    /// </summary>
    public interface IErrorTranslations : INotifyPropertyChanged
    {
        /// <summary>
        ///     Used for the title of the error view
        /// </summary>
        [DefaultValue("Something went wrong")]
        string ErrorTitle { get; }

        /// <summary>
        ///     Used for the current version label
        /// </summary>
        [DefaultValue("Current version")]
        string CurrentVersion { get; }

        /// <summary>
        ///     Used for the latest version label
        /// </summary>
        [DefaultValue("Latest version")]
        string LatestVersion { get; }
    }
}