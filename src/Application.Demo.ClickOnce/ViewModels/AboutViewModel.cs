//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

#region Usings

using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.ClickOnce;

#endregion

namespace Application.Demo.ClickOnce.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the about
    /// </summary>
    [Export(typeof(IShell))]
    public sealed class AboutViewModel : Screen, IShell
    {
        /// <summary>
        /// Information on the ClickOnce status
        /// </summary>
        public IClickOnceService ClickOnceService { get; }

        /// <summary>
        /// Construct the view model
        /// </summary>
        [ImportingConstructor]
        public AboutViewModel(IClickOnceService clickOnceService)
        {
            ClickOnceService = clickOnceService;
            DisplayName = "ClickOnce Info";
        }

        /// <summary>
        /// This handles the restart when the button in the view is pressed
        /// </summary>
        public void Restart()
        {
            ClickOnceService.Restart();
        }
    }
}