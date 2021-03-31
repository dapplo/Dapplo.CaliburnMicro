// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("ContextMenu")]
    public interface IContextMenuTranslations : ILanguage
    {
        string Configure { get; }
        string JumpToConfigure { get; }
        string Exit { get; }
        string CreateError { get; }

        string One { get; }
        string SomeWindow { get; }
        string ActiveCard { get; }
        string Toast { get; }
        string Three { get; }

        string Title { get; }
        string Two { get; }
        string WithChildren { get; }
        string Wizard { get; }
    }
}