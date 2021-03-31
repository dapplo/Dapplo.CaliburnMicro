// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;

namespace Application.Demo.MetroAddon.Configurations
{
    [Language("Credentials")]
    public interface ICredentialsTranslations : ILanguage
    {
        string Login { get; }

        string Password { get; }
        string Username { get; }
    }
}