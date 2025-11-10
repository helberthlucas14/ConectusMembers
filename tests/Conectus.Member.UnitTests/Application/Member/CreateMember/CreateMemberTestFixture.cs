using Bogus.Extensions.Brazil;
using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;
using UseCase = Conectus.Members.Application.UseCases.Member.CreateMember;

namespace Conectus.Members.UnitTests.Application.Member.CreateMember
{
    public class CreateMemberTestFixture : BaseFixture
    {
        public UseCase.CreateMemberInput GetExampleInput(bool isMinor = false, Guid? resposibleId = null)
        {
            return new UseCase.CreateMemberInput(
                 IdentifierDocumentDto.FromDomain(GetValidDocument()),
                 GetValidFirstName(),
                 GetValidLastName(),
                 isMinor ? GetValidMinorDateOfBirth() : GetValidAdultDateOfBirth(),
                 GetValidGender(),
                 GetValidPhoneNumber().Value,
                 AddressDto.FromDomain(GetValidAddress()),
                 isMinor ? resposibleId : null
                 );
        }
        public IdentifierDocument GetValidDocument() => new IdentifierDocument(
                        DocumentType.CPF,
                        Faker.Person.Cpf()
         );
        public string GetValidFirstName() => Faker.Name.FirstName();
        public string GetValidLastName() => Faker.Name.LastName();
        public DateTime GetValidAdultDateOfBirth() =>
            Faker.Date.Past(30, DateTime.Now.AddYears(-18));

        public DateTime GetValidMinorDateOfBirth() =>
            Faker.Date.Past(17, DateTime.Now);
        public PhoneNumber GetValidPhoneNumber() =>
           new PhoneNumber(Faker.Phone.PhoneNumber("(##)###-###-###"));

        public Address GetValidAddress() => GetAddress();
        public Gender GetValidGender() => GetRandomBoolean() ? Gender.Male : Gender.Female;
    }


    [CollectionDefinition(nameof(CreateMemberTestFixture))]
    public class CreateMemberTestFixtureCollection
        : ICollectionFixture<CreateMemberTestFixture>
    {
    }
}
