// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Config.Language;

namespace Application.Demo.Languages
{
    [Language("Wizard")]
    public interface IWizardTranslations : ILanguage
    {
        string Cancel { get; }
        string CompleteStep4 { get; }
        string Finish { get; }
        string Next { get; }
        string Previous { get; }
        string Title { get; }
        string TitleStep1 { get; }
        string TitleStep2 { get; }
        string TitleStep3 { get; }
        string TitleStep4 { get; }

        string TitleStep5 { get; }
    }
}