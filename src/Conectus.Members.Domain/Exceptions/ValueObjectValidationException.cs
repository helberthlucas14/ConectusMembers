using Conectus.Members.Domain.Validation;

namespace Conectus.Members.Domain.Exceptions
{
    public class ValueObjectValidationException : DomainValidationException
    {
        public IReadOnlyCollection<ValidationError>? Errors { get; }
        public ValueObjectValidationException(
            string? message,
            IReadOnlyCollection<ValidationError>? errors = null
        ) : base(message)
            => Errors = errors;
    }
}
