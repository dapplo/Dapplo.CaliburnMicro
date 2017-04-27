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

#region using

using System;
using System.Reactive.Linq;
using Dapplo.Ini;

#endregion

namespace Dapplo.CaliburnMicro.Configuration.Extensions
{
    /// <summary>
    ///     Supply an extension to simplify the usage of IIniSection.OnLoaded / IIniSection.OnSaved / IIniSection.OnSaving /
    ///     IIniSection.Reset
    /// </summary>
    public static class IniSectionExtensions
    {
        /// <summary>
        ///     Automatically call the update action when the Loaded fires
        ///     If the is called on a DI object, make sure it's available.
        /// </summary>
        /// <param name="iniSection">IIniSection</param>
        /// <param name="eventAction">Action to call on events, argument is the IniSectionEventArgs</param>
        /// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
        public static IDisposable OnLoaded(this IIniSection iniSection, Action<IniSectionEventArgs> eventAction)
        {
            if (iniSection == null)
            {
                throw new ArgumentNullException(nameof(iniSection));
            }
            if (eventAction == null)
            {
                throw new ArgumentNullException(nameof(eventAction));
            }
            var observable = Observable.FromEventPattern<EventHandler<IniSectionEventArgs>, IniSectionEventArgs>(h => iniSection.Loaded += h, h => iniSection.Loaded -= h);
            return observable.Subscribe(pce => eventAction(pce.EventArgs));
        }

        /// <summary>
        ///     Automatically call the update action when the Reset fires
        ///     If the is called on a DI object, make sure it's available.
        /// </summary>
        /// <param name="iniSection">IIniSection</param>
        /// <param name="eventAction">Action to call on events, argument is the IniSectionEventArgs</param>
        /// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
        public static IDisposable OnReset(this IIniSection iniSection, Action<IniSectionEventArgs> eventAction)
        {
            if (iniSection == null)
            {
                throw new ArgumentNullException(nameof(iniSection));
            }
            if (eventAction == null)
            {
                throw new ArgumentNullException(nameof(eventAction));
            }
            var observable = Observable.FromEventPattern<EventHandler<IniSectionEventArgs>, IniSectionEventArgs>(h => iniSection.Reset += h, h => iniSection.Reset -= h);
            return observable.Subscribe(pce => eventAction(pce.EventArgs));
        }

        /// <summary>
        ///     Automatically call the update action when the Saved fires
        ///     If the is called on a DI object, make sure it's available.
        /// </summary>
        /// <param name="iniSection">IIniSection</param>
        /// <param name="eventAction">Action to call on events, argument is the IniSectionEventArgs</param>
        /// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
        public static IDisposable OnSaved(this IIniSection iniSection, Action<IniSectionEventArgs> eventAction)
        {
            if (iniSection == null)
            {
                throw new ArgumentNullException(nameof(iniSection));
            }
            if (eventAction == null)
            {
                throw new ArgumentNullException(nameof(eventAction));
            }
            var observable = Observable.FromEventPattern<EventHandler<IniSectionEventArgs>, IniSectionEventArgs>(h => iniSection.Saved += h, h => iniSection.Saved -= h);
            return observable.Subscribe(pce => eventAction(pce.EventArgs));
        }

        /// <summary>
        ///     Automatically call the update action when the Saving fires
        ///     If the is called on a DI object, make sure it's available.
        /// </summary>
        /// <param name="iniSection">IIniSection</param>
        /// <param name="eventAction">Action to call on events, argument is the IniSectionEventArgs</param>
        /// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
        public static IDisposable OnSaving(this IIniSection iniSection, Action<IniSectionEventArgs> eventAction)
        {
            if (iniSection == null)
            {
                throw new ArgumentNullException(nameof(iniSection));
            }
            if (eventAction == null)
            {
                throw new ArgumentNullException(nameof(eventAction));
            }
            var observable = Observable.FromEventPattern<EventHandler<IniSectionEventArgs>, IniSectionEventArgs>(h => iniSection.Saving += h, h => iniSection.Saving -= h);
            return observable.Subscribe(pce => eventAction(pce.EventArgs));
        }
    }
}