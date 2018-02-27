#region Dapplo 2016-2018 - GNU Lesser General Public License
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
#endregion 

#region using

using System;
using System.ComponentModel.Composition;
using Dapplo.Addons;
#endregion

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     This is the attribute for a IUiShutdownModule module
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UiShutdownActionAttribute : ModuleAttribute, IShutdownMetadata
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public UiShutdownActionAttribute() : base(typeof(IUiShutdownAction))
        {
        }

        /// <summary>
        ///     Constructor with a contract name
        /// </summary>
        /// <param name="contractName"></param>
        public UiShutdownActionAttribute(string contractName) : base(contractName, typeof(IUiShutdownAction))
        {
        }

        /// <summary>
        ///     Order for the shutdowns to be called
        /// </summary>
        public int ShutdownOrder { get; set; }
    }
}
