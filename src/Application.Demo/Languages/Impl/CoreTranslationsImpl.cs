using System.Diagnostics.CodeAnalysis;
using Application.Demo.Shared;
using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class CoreTranslationsImpl : LanguageBase<ICoreTranslations>, ICoreTranslations
    {
        #region Implementation of ICoreTranslations

        public string Cancel { get; }
        public string Ok { get; }

        #endregion

        #region Implementation of IErrorTranslations

        public string ErrorTitle { get; }
        public string CurrentVersion { get; }
        public string LatestVersion { get; }

        #endregion

        #region Implementation of ICoreTranslations

        public string Language { get; }
        public string Settings { get; }

        #endregion
    }
}
