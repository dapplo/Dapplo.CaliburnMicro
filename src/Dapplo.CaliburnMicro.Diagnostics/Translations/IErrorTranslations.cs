//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System.ComponentModel;

#endregion

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