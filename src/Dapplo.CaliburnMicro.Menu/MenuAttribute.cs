// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Composition;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     This attribute can be used to specify the menu to use
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MenuAttribute : Attribute
    {
        /// <inheritdoc />
        public MenuAttribute()
        {
        }

        /// <inheritdoc />
        public MenuAttribute(string menu)
        {
            Menu = menu;
        }

        /// <summary>
        ///     The menu to use
        /// </summary>
        public string Menu { get; set; }
    }
}