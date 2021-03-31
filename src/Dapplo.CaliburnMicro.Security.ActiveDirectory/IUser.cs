// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
