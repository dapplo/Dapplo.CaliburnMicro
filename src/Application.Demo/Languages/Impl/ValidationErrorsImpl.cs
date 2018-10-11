using System.Diagnostics.CodeAnalysis;
using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class ValidationErrorsImpl : LanguageBase<IValidationErrors>, IValidationErrors
    {
        public string Name { get; }
    }
}
