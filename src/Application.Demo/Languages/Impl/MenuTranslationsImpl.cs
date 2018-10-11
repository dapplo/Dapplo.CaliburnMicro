using System.Diagnostics.CodeAnalysis;
using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class MenuTranslationsImpl : LanguageBase<IMenuTranslations>, IMenuTranslations
    {
        public string About { get; }
        public string Edit { get; }
        public string File { get; }
        public string SaveAs { get; }
    }
}
