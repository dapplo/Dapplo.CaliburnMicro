// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    ///     Binding describing all information needed for binding names
    /// </summary>
    public sealed class DisplayNameBinding : IDisposable
    {
        private readonly CompositeDisposable _disposables;
        /// <summary>
        ///     Create the name binding object
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="notifyPropertyChanged"></param>
        public DisplayNameBinding(IObservable<EventPattern<PropertyChangedEventArgs>> observable, INotifyPropertyChanged notifyPropertyChanged)
        {
            Observable = observable;
            NotifyPropertyChanged = notifyPropertyChanged;
            _disposables = new CompositeDisposable();
        }

        /// <summary>
        ///     All bindings are stored here
        /// </summary>
        internal CompositeDisposable Disposables => _disposables;

        /// <summary>
        ///     The source of the events
        /// </summary>
        public INotifyPropertyChanged NotifyPropertyChanged { get; }

        /// <summary>
        ///     Observable as event provider for the name binding
        /// </summary>
        public IObservable<EventPattern<PropertyChangedEventArgs>> Observable { get; }

        /// <summary>
        ///     Dispose the underlying bindings which are stored in a CompositeDisposable
        /// </summary>
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}