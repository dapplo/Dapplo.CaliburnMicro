// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory
{
    /// <summary>
    /// Some extensions to help with the user
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Retrieve the Thumbnail of the user as a MemoryStream
        /// </summary>
        /// <param name="user">IUser</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream GetThumbnailAsStream(this IUser user)
        {
            return new MemoryStream(user.Thumbnail);
        }
    }
}
