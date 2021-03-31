// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Ini;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Application.Demo.Models
{
    [IniSection("Demo")]
    public interface IDemoConfiguration : IIniSection
    {
        [DataMember(EmitDefaultValue = false)]
        [DefaultValue("en-US")]
        string Language { get; set; }
    }
}