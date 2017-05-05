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

using System;
using System.ComponentModel;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows;

namespace Dapplo.CaliburnMicro.Diagnostics.ViewModels
{
    /// <summary>
    /// This view model shows the error that occurred
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ErrorViewModel : Screen
    {
        [Import(AllowDefault = true)]
        public IVersionProvider VersionProvider { get; set; }

        public ErrorViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            // Design mode code
            Stacktrace =
                "ConsoleApplication1.MyCustomException: some message .... ---> System.Exception: Oh noes!\r\n   at ConsoleApplication1.SomeObject.OtherMethod() in C:\\ConsoleApplication1\\SomeObject.cs:line 24\r\n   at ConsoleApplication1.SomeObject..ctor() in C:\\ConsoleApplication1\\SomeObject.cs:line 14\r\n   --- End of inner exception stack trace ---\r\n   at ConsoleApplication1.SomeObject..ctor() in C:\\ConsoleApplication1\\SomeObject.cs:line 18\r\n   at ConsoleApplication1.Program.DoSomething() in C:\\ConsoleApplication1\\Program.cs:line 23\r\n   at ConsoleApplication1.Program.Main(String[] args) in C:\\ConsoleApplication1\\Program.cs:line 13";
#if DEBUG
            VersionProvider = new Dapplo.CaliburnMicro.Diagnostics.Designtime.SimpleVersionProvider();
#endif
            Message = "Oh noes!";
        }

        /// <summary>
        /// Checks if the current version is the latest
        /// </summary>
        public bool IsMostRecent => VersionProvider.Current.Equals(VersionProvider.Latest);

        /// <summary>
        /// Set the exception to display
        /// </summary>
        public void SetExceptionToDisplay(Exception exception)
        {
            Stacktrace = exception.StackTrace;
            Message = exception.Message;
        }

        /// <summary>
        /// The stacktrace to display
        /// </summary>
        public string Stacktrace { get; set; }

        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }
    }
}
