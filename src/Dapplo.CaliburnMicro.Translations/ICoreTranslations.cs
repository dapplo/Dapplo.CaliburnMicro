// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Dapplo.Config.Interfaces;

namespace Dapplo.CaliburnMicro.Translations
{
    /// <summary>
    ///     These are translations used throughout the project
    /// </summary>
    public interface ICoreTranslations : IDefaultValue
    {
        /// <summary>
        ///     Used everywhere where cancel is used
        /// </summary>
        [DefaultValue("Cancel")]
        string Cancel { get; }

        /// <summary>
        ///     Used everywhere where ok is used
        /// </summary>
        [DefaultValue("Ok")]
        string Ok { get; }
    }
}