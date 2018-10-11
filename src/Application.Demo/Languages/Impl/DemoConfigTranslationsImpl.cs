using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    internal class DemoConfigTranslationsImpl : LanguageBase<IDemoConfigTranslations>, IDemoConfigTranslations
    {
        public string Addons { get; }
        public string Apply { get; }
        public string RestoreDefaults { get; }
        public string Ui { get; }
        public string Filter { get; }
    }
}
