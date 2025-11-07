using Conectus.Members.Domain.Enum;
using Conectus.Members.Infra.CrossCutting.Commons.Extensions;


namespace Conectus.Members.Domain.ValueObject
{
    public sealed class IdentifierDocument : IEquatable<IdentifierDocument>
    {
        public DocumentType Type { get; private set; }
        public string Document { get; private set; }
        public bool IsValid => Type.Equals(DocumentType.CPF) ? Document.ValidateFederalRegistration() : Document.ValidateRG();

        protected IdentifierDocument() { }

        public IdentifierDocument(DocumentType type, string document)
        {
            Type = type;
            Document = document;
        }

        public override bool Equals(object? obj) => Equals(obj as IdentifierDocument);

        public bool Equals(IdentifierDocument? other)
            => other != null && Document == other.Document && Type == other.Type;

        public override int GetHashCode()
            => HashCode.Combine(Document, Type);

        public override string ToString()
            => $"{Type}: {Document}";
    }
}
