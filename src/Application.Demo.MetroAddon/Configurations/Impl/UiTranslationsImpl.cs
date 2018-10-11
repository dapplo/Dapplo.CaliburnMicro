using Dapplo.Config.Language;

namespace Application.Demo.MetroAddon.Configurations.Impl
{
    internal class UiTranslationsImpl : LanguageBase<IUiTranslations>, IUiTranslations
    {
        public string Hotkey { get; }
        public string Theme { get; }
    }
}
