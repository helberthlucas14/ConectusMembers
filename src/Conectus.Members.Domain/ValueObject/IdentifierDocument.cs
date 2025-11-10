using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Validation;
using Conectus.Members.Infra.CrossCutting.Commons.Extensions;


namespace Conectus.Members.Domain.ValueObject
{
    public sealed class IdentifierDocument : SeedWork.ValueObject
    {
        public DocumentType Type { get; private set; }
        public string Document { get; private set; }
        public bool IsValid => Type.Equals(DocumentType.CPF) ? Document.ValidateFederalRegistration() : false;

        public IdentifierDocument(DocumentType type, string document)
        {
            Type = type;
            Document = document;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNull<IdentifierDocument>(Type, nameof(DocumentType));
            DomainValidation.NotNullOrEmpty<IdentifierDocument>(Document, nameof(Document));

            if (!IsValid)
                DomainValidation.InvalidAtritibute<IdentifierDocument>(nameof(Document));
        }

        public override bool Equals(SeedWork.ValueObject? other)
        {
            if (other is null) return false;
            return other is IdentifierDocument document &&
             Document == document.Document &&
             Type == document.Type;
        }

        protected override int GetCustomHashCode()
            => HashCode.Combine(Document, Type);

        public override string ToString()
            => $"{Type}: {Document}";
    }
}
