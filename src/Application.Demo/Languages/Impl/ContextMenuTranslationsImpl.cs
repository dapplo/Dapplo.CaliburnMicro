using System.Diagnostics.CodeAnalysis;
using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class ContextMenuTranslationsImpl : LanguageBase<IContextMenuTranslations>, IContextMenuTranslations
    {
        public string Configure { get; }
        public string JumpToConfigure { get; }
        public string Exit { get; }
        public string CreateError { get; }
        public string One { get; }
        public string SomeWindow { get; }
        public string ActiveCard { get; }
        public string Toast { get; }
        public string Three { get; }
        public string Title { get; }
        public string Two { get; }
        public string WithChildren { get; }
        public string Wizard { get; }
    }
}
