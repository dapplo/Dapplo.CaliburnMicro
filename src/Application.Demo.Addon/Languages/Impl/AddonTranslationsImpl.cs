using System.Diagnostics.CodeAnalysis;
using Dapplo.Config.Language;

namespace Application.Demo.Addon.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class AddonTranslationsImpl : LanguageBase<IAddonTranslations>, IAddonTranslations
    {
        #region Implementation of IAddonTranslations

        public string Addon { get; }
        public string Admin { get; }
        public string NotSelectableAddon { get; }

        #endregion
    }
}
