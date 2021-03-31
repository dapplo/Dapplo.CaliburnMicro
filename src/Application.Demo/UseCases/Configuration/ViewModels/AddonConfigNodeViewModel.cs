// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Application.Demo.Shared;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;

namespace Application.Demo.UseCases.Configuration.ViewModels
{
    /// <summary>
    ///     This is just a placeholder, doesn't have a view
    /// </summary>
    public sealed class AddonConfigNodeViewModel : ConfigNode
    {   
        public AddonConfigNodeViewModel(IDemoConfigTranslations configTranslations)
        {
            // automatically update the DisplayName
            configTranslations.CreateDisplayNameBinding(this, nameof(IDemoConfigTranslations.Addons));
            CanActivate = false;
            Id = nameof(ConfigIds.Addons);
        }
    }
}