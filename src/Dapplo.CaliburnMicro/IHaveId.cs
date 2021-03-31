// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     Implement this interface to speficy that you have an Id
    /// </summary>
    public interface IHaveId
    {
        /// <summary>
        ///     The Id used to identify an element
        /// </summary>
        string Id { get; set; }
    }
}