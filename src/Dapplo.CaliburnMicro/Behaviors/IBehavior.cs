// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     Interface for the behavior
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        ///     Attach the behavior
        /// </summary>
        void Attach();

        /// <summary>
        ///     AttachDetach the behavior
        /// </summary>
        void Detach();

        /// <summary>
        ///     Can the behavior be used?
        /// </summary>
        /// <returns></returns>
        bool IsApplicable();

        /// <summary>
        ///     Update the behavior
        /// </summary>
        /// <param name="dependencyPropertyChangedEventArgs">optional DependencyPropertyChangedEventArgs</param>
        void Update(DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs);
    }
}