using System.Runtime.Serialization;

namespace Conectus.Members.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        protected DomainValidationException(string message) : base(message) { }
    }
}
