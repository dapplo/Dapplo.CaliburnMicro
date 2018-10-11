using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    internal class ToastTranslationsImpl : LanguageBase<IToastTranslations>, IToastTranslations
    {
        public string StartupNotify { get; }
    }
}
