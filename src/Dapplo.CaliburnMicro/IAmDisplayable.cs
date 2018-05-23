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

#region using



#endregion

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     This interface is implemented by elements that can be displayed
    /// </summary>
    public interface IAmDisplayable
    {
        /// <summary>
        ///     Returns if the element can be selected (visible but not usable)
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        ///     Returns if the element is visible (not visible and not usable)
        /// </summary>
        bool IsVisible { get; }
    }
}