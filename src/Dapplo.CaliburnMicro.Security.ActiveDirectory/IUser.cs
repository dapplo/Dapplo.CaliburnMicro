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
using Dapplo.ActiveDirectory;
using Dapplo.ActiveDirectory.Entities;
using Dapplo.ActiveDirectory.Enums;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory
{
    /// <summary>
    /// Default user details
    /// </summary>
    public interface IUser : IAdObject
    {
        /// <summary>
        /// IP Telephone number
        /// </summary>
        [AdProperty(UserProperties.IpTelephoneNumber)]
        string IpTelephoneNumber { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [AdProperty(UserProperties.Description)]
        string Description { get; set; }

        /// <summary>
        /// Telephone number
        /// </summary>
        [AdProperty(UserProperties.TelephoneNumber)]
        string TelephoneNumber { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        [AdProperty(UserProperties.EmailAddress)]
        string EmailAddress { get; set; }

        /// <summary>
        /// Department where the users works
        /// </summary>
        [AdProperty(UserProperties.Department)]
        string Department { get; set; }

        /// <summary>
        /// Name to display
        /// </summary>
        [AdProperty(UserProperties.DisplayName)]
        string Displayname { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [AdProperty(UserProperties.GivenName)]
        string Firstname { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [AdProperty(UserProperties.Location)]
        string Location { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [AdProperty(UserProperties.Surname)]
        string Name { get; set; }

        /// <summary>
        /// Given Name
        /// </summary>
        [AdProperty(UserProperties.GivenName)]
        string GivenName { get; set; }

        /// <summary>
        /// Initials
        /// </summary>
        [AdProperty(UserProperties.Initials)]
        string Initials { get; set; }

        /// <summary>
        /// Bytes for the thumbnail image
        /// </summary>
        [AdProperty(UserProperties.Thumbnail)]
        byte[] Thumbnail { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [AdProperty(UserProperties.Username)]
        string Username { get; set; }

        /// <summary>
        /// Groups which are assigned to this user
        /// </summary>
        [AdProperty(UserProperties.MemberOfGroups)]
        IList<DistinguishedName> Groups { get; set; }
    }
}
