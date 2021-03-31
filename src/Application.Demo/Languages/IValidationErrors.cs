// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("ValidationErrors")]
    public interface IValidationErrors : ILanguage
    {
        string Name { get; }
    }
}