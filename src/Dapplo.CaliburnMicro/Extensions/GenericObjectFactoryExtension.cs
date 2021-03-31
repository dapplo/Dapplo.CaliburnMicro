// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Markup;

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    /// This MarkupExtension helps to solve the issue with using generic design time classes
    /// </summary>
    public class GenericObjectFactoryExtension : MarkupExtension
    {
        /// <summary>
        /// your generic (viewmodel) type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The argument for the generic type
        /// </summary>
        public Type GenericTypeArgument { get; set; }

        /// <summary>
        /// Privide the value, if an instance is requested
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <returns>object</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var genericType = Type.MakeGenericType(GenericTypeArgument);
            return Activator.CreateInstance(genericType);
        }
    }
}
