//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System.Collections.Generic;
using Dapplo.ActiveDirectory.Entities;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory.Entities
{
    /// <summary>
    /// An entity to store user information
    /// </summary>
    public class UserImpl : IUser
    {
        /// <inheritdoc />
        public string IpTelephoneNumber { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }
        /// <inheritdoc />
        public string TelephoneNumber { get; set; }
        /// <inheritdoc />
        public string EmailAddress { get; set; }
        /// <inheritdoc />
        public string Department { get; set; }
        /// <inheritdoc />
        public string Displayname { get; set; }
        /// <inheritdoc />
        public string Firstname { get; set; }
        /// <inheritdoc />
        public string Location { get; set; }
        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string GivenName { get; set; }
        /// <inheritdoc />
        public string Initials { get; set; }
        /// <inheritdoc />
        public byte[] Thumbnail { get; set; }
        /// <inheritdoc />
        public string Username { get; set; }
        /// <inheritdoc />
        public IList<DistinguishedName> Groups { get; set; }
        /// <inheritdoc />
        public string Id { get; set; }
    }
}
