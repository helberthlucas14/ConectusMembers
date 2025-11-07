
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Exceptions;
using Conectus.Members.Domain.ValueObject;
using FluentAssertions;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{
    [Collection(nameof(IdentifierDocumentTestFixture))]
    public class IdentifierDocumentTest
    {
        private readonly IdentifierDocumentTestFixture _fixture;
        public IdentifierDocumentTest(IdentifierDocumentTestFixture fixture) => _fixture = fixture;


        [Theory(DisplayName = nameof(InstantiateIdentifierDocumentSuccessfully))]
        [Trait("Domain", "IdentifierDocument ValueObject")]
        [InlineData(DocumentType.CPF)]
        public void InstantiateIdentifierDocumentSuccessfully(DocumentType documentType)
        {
            var validIdentifierDocument = _fixture.GetIdentifierDocument(documentType);

            var identifierDocument = new IdentifierDocument(
                validIdentifierDocument.Type,
                validIdentifierDocument.Document
                );

            identifierDocument.Should().NotBeNull();
            identifierDocument.Document.Should().Be(validIdentifierDocument.Document);
            identifierDocument.Type.Should().Be(validIdentifierDocument.Type);
            identifierDocument.IsValid.Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenInvalidDocumentNumber))]
        [Trait("Domain", "IdentifierDocument ValueObject")]
        [MemberData(nameof(GetInvalidDocument), parameters: 10)]
        public void InstantiateErrorWhenInvalidDocumentNumber(string document, DocumentType type)
        {
            Action action = () => new IdentifierDocument(
                   type,
                   document
                   );

            action.Should()
                .Throw<ValueObjectValidationException>()
                .WithMessage("Document is invalid.");
        }
        public static IEnumerable<object[]> GetInvalidDocument(int numberOfTests)
        {
            var fixture = new IdentifierDocumentTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                var document = fixture.GetIdentifierDocument(DocumentType.CPF);
                yield return new object[]
                {
                   document.Document[..(isOdd ? 10: 12) ], document.Type
                };
            }
        }
    }
}
