// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    public sealed class AttachedBehavior
    {
        private readonly Func<DependencyObject, IBehavior> _behaviorFactory;

        private readonly DependencyProperty _property;

        internal AttachedBehavior(DependencyProperty property, Func<DependencyObject, IBehavior> behaviorFactory)
        {
            _property = property;
            _behaviorFactory = behaviorFactory;
        }

        private static string GetNextPropertyName()
        {
            return "_" + Guid.NewGuid().ToString("N");
        }

        /// <summary>
        ///     Register a behavior to a DependencyObject
        /// </summary>
        /// <param name="behaviorFactory">Func with DependencyObject and IBehavior</param>
        /// <returns>AttachedBehavior</returns>
        public static AttachedBehavior Register(Func<DependencyObject, IBehavior> behaviorFactory)
        {
            Contract.Requires(behaviorFactory != null);

            return new AttachedBehavior(RegisterNextProperty(), behaviorFactory);
        }

        private static DependencyProperty RegisterNextProperty()
        {
            return DependencyProperty.RegisterAttached(GetNextPropertyName(), typeof(IBehavior), typeof(AttachedBehavior));
        }

        private void TryCreateBehavior(DependencyObject host, DependencyPropertyChangedEventArgs propertyChangedEventArgs)
        {
            var behavior = _behaviorFactory(host);

            if (!behavior.IsApplicable())
            {
                return;
            }
            behavior.Attach();
            host.SetValue(_property, behavior);
            behavior.Update(propertyChangedEventArgs);
        }

        /// <summary>
        ///     Update (or create) the Behavior for the DependencyObject
        /// </summary>
        /// <param name="host">DependencyObject</param>
        /// <param name="propertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
        public void Update(DependencyObject host, DependencyPropertyChangedEventArgs propertyChangedEventArgs)
        {
            Contract.Requires(host != null);

            var behavior = (IBehavior) host.GetValue(_property);

            if (behavior == null)
            {
                TryCreateBehavior(host, propertyChangedEventArgs);
            }
            else
            {
                UpdateBehavior(host, behavior, propertyChangedEventArgs);
            }
        }

        private void UpdateBehavior(DependencyObject host, IBehavior behavior, DependencyPropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (behavior.IsApplicable())
            {
                behavior.Update(propertyChangedEventArgs);
            }
            else
            {
                host.ClearValue(_property);

                behavior.Detach();
            }
        }
    }
}