using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.UnitTests.Domain.Entity
{
    public class MemberTestFixture : BaseFixture
    {
        public DomainEntity.Member GetValidMember()
        {
            return new DomainEntity.Member(
                  Faker.Person.FirstName,
                  Faker.Person.LastName,
                  Faker.Person.DateOfBirth,
                 (Gender)Faker.Person.Gender,
                  GetPhoneNumber(),
                  GetIdentifierDocument(),
                  GetAddress());
        }

        public IdentifierDocument GetIdentifierDocument(DocumentType type = DocumentType.CPF)
            => type switch
            {
                DocumentType.CPF => new IdentifierDocument(type, Faker.Person.Cpf()),
                DocumentType.RG => new IdentifierDocument(type, Faker.Random.Replace("##.###.###-#")),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public PhoneNumber GetPhoneNumber(string? phoneNumber = "")
            => PhoneNumber.Create(
                string.IsNullOrWhiteSpace(phoneNumber) ?
                Faker.Phone.PhoneNumber() :
                phoneNumber);

        public PhoneNumber GetInvalidPhoneNumber(string? phone)
            => PhoneNumber.Create(phone);


    }


    [CollectionDefinition(nameof(MemberTestFixture))]
    public class MemberTestFixtureCollection
        : ICollectionFixture<MemberTestFixture>
    {
    }
}
