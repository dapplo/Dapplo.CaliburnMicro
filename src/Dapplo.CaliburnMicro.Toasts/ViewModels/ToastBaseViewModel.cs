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
        private int _id;

        /// <inheritdoc />
        public string GetMessage()
        {
            return DisplayPart.GetMessage();
        }

        /// <summary>
        /// Id of the toast
        /// </summary>
        public virtual int Id {
            get => _id;
            set
            {
                _id = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// This takes care of assigning the new view to the DisplayPart
        /// </summary>
        /// <param name="view"></param>
        /// <param name="context"></param>
        protected override void OnViewAttached(object view, object context)
        {
            var toastView = view as ToastView;
            DisplayPart = toastView ?? throw new NotSupportedException("View doesn't base on ToastView");

            // Specify MessageOptions if any
            if (toastView.Options == null)
            {
                toastView.Options = Options;
            }

            base.OnViewAttached(view, context);
        }

        /// <summary>
        /// Sets the close action for the toast
        /// </summary>
        /// <param name="closeAction">Action which accepts a INotification</param>
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
            base.TryClose(dialogResult);
        }

        /// <summary>
        /// Return the view for this ViewModel, it needs to base upon NotificationDisplayPart 
        /// </summary>
        public virtual NotificationDisplayPart DisplayPart { get; protected set; }

        /// <inheritdoc />
        bool INotification.CanClose { get; set; } = true;

        /// <inheritdoc />
        public MessageOptions Options { get; set; } = new MessageOptions {FreezeOnMouseEnter = true, UnfreezeOnMouseLeave = true};
    }
}
