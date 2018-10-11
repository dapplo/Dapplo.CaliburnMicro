using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    internal class WizardTranslationsImpl : LanguageBase<IWizardTranslations>, IWizardTranslations
    {
        public string Cancel { get; }
        public string CompleteStep4 { get; }
        public string Finish { get; }
        public string Next { get; }
        public string Previous { get; }
        public string Title { get; }
        public string TitleStep1 { get; }
        public string TitleStep2 { get; }
        public string TitleStep3 { get; }
        public string TitleStep4 { get; }
        public string TitleStep5 { get; }
    }
}
