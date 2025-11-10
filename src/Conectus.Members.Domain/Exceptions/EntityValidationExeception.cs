using Conectus.Members.Domain.Validation;

namespace Conectus.Members.Domain.Exceptions
{
    public class EntityValidationException : DomainValidationException
    {
        public IReadOnlyCollection<ValidationError>? Errors { get; }
        public EntityValidationException(
            string? message,
            IReadOnlyCollection<ValidationError>? errors = null
        ) : base(message)
            => Errors = errors;
    }
}
