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
using Caliburn.Micro;
using ToastNotifications.Core;

namespace Dapplo.CaliburnMicro.Toasts.ViewModels
{
    /// <summary>
    /// Base ViewModel for a Toast
    /// </summary>
    public abstract class ToastBaseViewModel : Screen, IToast
    {
        private Action<INotification> _closeAction;

        /// <inheritdoc />
        protected override void OnActivate()
        {
            var view = ViewLocator.LocateForModel(this, null, null);
            if (view == null)
            {
                throw new NotSupportedException($"Couldn't locate view for {GetType()}");
            }
            var notificationView = view as NotificationDisplayPart;
            if (notificationView == null)
            {
                throw new NotSupportedException("View doesn't base on NotificationDisplayPart");
            }
            // Bind this to the datacontext
            notificationView.DataContext = this;
            ViewModelBinder.Bind(this, view, null);
            DisplayPart = notificationView;
            base.OnActivate();
        }

        /// <summary>
        /// Id of the toast
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Sets the close action for the toast
        /// </summary>
        /// <param name="closeAction"></param>
        public virtual void Bind(Action<INotification> closeAction)
        {
            _closeAction = closeAction;
        }

        /// <summary>
        /// Close the toast, using the internal Caliburn TryClose
        /// </summary>
        public virtual void Close()
        {
            TryClose(true);
        }

        /// <inheritdoc />
        public override void TryClose(bool? dialogResult = null)
        {
            _closeAction(this);
        }

        /// <summary>
        /// Return the view for this ViewModel, it needs to base upon NotificationDisplayPart 
        /// </summary>
        public virtual NotificationDisplayPart DisplayPart { get; private set; }
    }
}
