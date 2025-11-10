using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;

namespace Conectus.Members.Application.UseCases.Member.Common
{
    public record IdentifierDocumentDto(DocumentType Type, string Number)
    {
        public static IdentifierDocumentDto FromDomain(IdentifierDocument document) =>
            new IdentifierDocumentDto(document.Type, document.Document);

        public static IdentifierDocument ToDomain(IdentifierDocumentDto dto) =>
            new IdentifierDocument(dto.Type, dto.Number);
    }
}