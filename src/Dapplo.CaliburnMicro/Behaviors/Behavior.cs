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
    /// <typeparam name="THost">DependencyObject</typeparam>
    public abstract class Behavior<THost> : IBehavior where THost : DependencyObject
    {
        private readonly WeakReference _hostReference;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="uiElement">DependencyObject</param>
        protected Behavior(THost uiElement)
        {
            Contract.Requires(uiElement != null, "Host is not the expected type");

            _hostReference = new WeakReference(uiElement);
        }

        /// <summary>
        ///     Attach this Behavior to the specified DependencyObject
        /// </summary>
        /// <param name="host">THost which extends DependencyObject</param>
        protected virtual void Attach(THost host)
        {
        }

        /// <summary>
        ///     Remove this Behavior from the specified DependencyObject
        /// </summary>
        /// <param name="host">THost which extends DependencyObject</param>
        protected virtual void Detach(THost host)
        {
        }

        private THost GetHost()
        {
            return (THost) _hostReference.Target;
        }

        /// <summary>
        ///     Returns if the Behavior is applicable for the specified DependencyObject
        /// </summary>
        /// <param name="host">THost which extends DependencyObject</param>
        /// <returns>bool</returns>
        protected virtual bool IsApplicable(THost host)
        {
            return true;
        }

        /// <summary>
        ///     Let the behavior update it's "stuffT for the specified host
        /// </summary>
        /// <param name="updateAction">Action of THost (which extends DependencyObject)</param>
        protected void TryUpdate(Action<THost> updateAction)
        {
            Contract.Requires(updateAction != null);

            var host = GetHost();

            if (host != null)
            {
                updateAction(host);
            }
        }

        /// <summary>
        ///     Let the behavior update it's "stuffT for the specified host
        /// </summary>
        /// <param name="uiElement">THost which extends DependencyObject</param>
        /// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
        protected abstract void Update(THost uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs);

        /// <inheritdoc />
        public bool IsApplicable()
        {
            var host = GetHost();

            return host != null && IsApplicable(host);
        }

        /// <inheritdoc />
        public void Attach()
        {
            var host = GetHost();

            if (host != null)
            {
                Attach(host);
            }
        }

        /// <inheritdoc />
        public void Detach()
        {
            var host = GetHost();

            if (host != null)
            {
                Detach(host);
            }
        }

        /// <inheritdoc />
        public void Update(DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
        {
            var host = GetHost();

            if (host != null)
            {
                Update(host, dependencyPropertyChangedEventArgs);
            }
        }
    }
}