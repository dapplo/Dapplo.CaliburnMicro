// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Dapplo.Config.Interfaces;

namespace Dapplo.CaliburnMicro.Translations
{
    /// <summary>
    ///     These are the translations used on the ConfigViewModel
    /// </summary>
    public interface IConfigTranslations : IDefaultValue
    {
        /// <summary>
        ///     Used for the label / watermark test where the filter can be specified
        /// </summary>
        [DefaultValue("Filter")]
        string Filter { get; }
    }
}