//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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
