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

using System.Collections.Generic;
using Dapplo.ActiveDirectory.Entities;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory.Entities
{
    /// <summary>
    /// An entity to store user information
    /// </summary>
    public class UserImpl : IUser
    {
        public string IpTelephoneNumber { get; set; }
        public string Description { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Department { get; set; }
        public string Displayname { get; set; }
        public string Firstname { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public byte[] Thumbnail { get; set; }
        public string Username { get; set; }
        public IList<DistinguishedName> Groups { get; set; }
        public string Id { get; set; }
    }
}
