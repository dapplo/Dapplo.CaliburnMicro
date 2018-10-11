using System.Diagnostics.CodeAnalysis;
using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class ToastTranslationsImpl : LanguageBase<IToastTranslations>, IToastTranslations
    {
        public string StartupNotify { get; }
    }
}
