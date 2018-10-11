using Dapplo.Config.Language;

namespace Application.Demo.Languages.Impl
{
    internal class ValidationErrorsImpl : LanguageBase<IValidationErrors>, IValidationErrors
    {
        public string Name { get; }
    }
}
