using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{
    public class IdentifierDocumentTestFixture : BaseFixture
    {
        public IdentifierDocument GetIdentifierDocument(DocumentType type = DocumentType.CPF)
            => type switch
            {
                DocumentType.CPF => new IdentifierDocument(type, Faker.Person.Cpf()),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }

    [CollectionDefinition(nameof(IdentifierDocumentTestFixture))]
    public class IdentifierDocumentTestFixtureCollection
   : ICollectionFixture<IdentifierDocumentTestFixture>
    {
    }
}
