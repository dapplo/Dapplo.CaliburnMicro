// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    ///     Extensions for IHaveDisplayName
    /// </summary>
    public static class HaveDisplayNameExtensions
    {
        /// <summary>
        ///     Add a displayname binding to the NameBinding
        /// </summary>
        /// <param name="displayNameBinding">NameBinding to bind to</param>
        /// <param name="haveDisplayName">IHaveDisplayName</param>
        /// <param name="propertyName">Name of the property in the original INotifyPropertyChanged object</param>
        /// <returns>binding</returns>
        public static DisplayNameBinding AddDisplayNameBinding(this DisplayNameBinding displayNameBinding, IHaveDisplayName haveDisplayName, string propertyName)
        {
            if (haveDisplayName == null)
            {
                throw new ArgumentNullException(nameof(haveDisplayName));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            var disposable = displayNameBinding.Observable.Where(args => args.EventArgs.PropertyName == propertyName).Subscribe(haveDisplayName.CopyValue);
            // Update the display name right away
            haveDisplayName.DisplayName = displayNameBinding.NotifyPropertyChanged.GetType().GetProperty(propertyName)?.GetValue(displayNameBinding.NotifyPropertyChanged) as string;
            // If the disposables is passed, add the disposable
            displayNameBinding.Disposables?.Add(disposable);
            return displayNameBinding;
        }

        /// <summary>
        ///     Copy the value specified by the property name from the source to the haveDisplayName
        /// </summary>
        /// <param name="haveDisplayName">IHaveDisplayName</param>
        /// <param name="eventPattern"></param>
        private static void CopyValue(this IHaveDisplayName haveDisplayName, EventPattern<PropertyChangedEventArgs> eventPattern)
        {
            var source = eventPattern.Sender;
            if (source == null)
            {
                return;
            }
            var propertyName = eventPattern.EventArgs.PropertyName;
            var value = source.GetType().GetProperty(propertyName)?.GetValue(source) as string;
            haveDisplayName.DisplayName = value;
        }

        /// <summary>
        ///     Create a binding between the INotifyPropertyChanged and optional IHaveDisplayName objects.
        /// </summary>
        /// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
        /// <param name="haveDisplayName">optional IHaveDisplayName for the first binding</param>
        /// <param name="propertyName">optional property name for the first binding</param>
        /// <returns>NameBinding</returns>
        public static DisplayNameBinding CreateDisplayNameBinding(this INotifyPropertyChanged notifyPropertyChanged, IHaveDisplayName haveDisplayName = null, string propertyName = null)
        {
            var propertyChangedObservable = notifyPropertyChanged.OnPropertyChangedPattern();
            var nameBinding = new DisplayNameBinding(propertyChangedObservable, notifyPropertyChanged);
            if (haveDisplayName != null)
            {
                nameBinding.AddDisplayNameBinding(haveDisplayName, propertyName);
            }
            return nameBinding;
        }
    }
}