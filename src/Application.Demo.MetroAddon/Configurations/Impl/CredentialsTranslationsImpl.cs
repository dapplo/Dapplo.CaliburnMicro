using Dapplo.Config.Language;

namespace Application.Demo.MetroAddon.Configurations.Impl
{
    internal class CredentialsTranslationsImpl : LanguageBase<ICredentialsTranslations>, ICredentialsTranslations
    {
        public string Login { get; }
        public string Password { get; }
        public string Username { get; }
    }
}
