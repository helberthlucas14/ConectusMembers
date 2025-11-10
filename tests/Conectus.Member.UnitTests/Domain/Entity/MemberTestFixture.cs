using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;
using System.Text.RegularExpressions;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.UnitTests.Domain.Entity
{
    public class MemberTestFixture : BaseFixture
    {
        public DomainEntity.Member GetValidMember(bool isMinor = false)
        {
            return new DomainEntity.Member(
                  GetValidFirstName(),
                  GetValidLastName(),
                  GetValidDateOfBirth(isMinor),
                  GetValidGender(),
                  GetValidPhoneNumber(),
                  GetIdentifierDocument(),
                  GetAddress(),
                  isMinor ? Guid.NewGuid() : null);
        }

        public IdentifierDocument GetIdentifierDocument(DocumentType type = DocumentType.CPF)
            => type switch
            {
                DocumentType.CPF => new IdentifierDocument(type, Faker.Person.Cpf()),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public PhoneNumber GetValidPhoneNumber()
        {
            string pattern = @"^\(\d{2}\)\d{3}-\d{3}-\d{3}$";
            var regex = new Regex(pattern);

            var phone = Faker.Phone.PhoneNumber("(##)###-###-###");

            while (!regex.IsMatch(phone))
                phone = Faker.Phone.PhoneNumber("(##)###-###-###");

            return new PhoneNumber(phone);
        }

        public string GetValidFirstName()
        {
            var validFirstName = "";

            while (validFirstName.Length < 3)
                validFirstName = Faker.Person.FirstName;
            if (validFirstName.Length > 50)
                validFirstName = validFirstName[..50];

            return validFirstName;
        }
        public string GetValidLastName()
        {
            var validLastName = "";

            while (validLastName.Length < 3)
                validLastName = Faker.Person.LastName;
            if (validLastName.Length > 50)
                validLastName = validLastName[..50];
            return validLastName;
        }

        public DateTime GetValidDateOfBirth(bool isMinor = false)
        {
            return isMinor ? Faker.Date.Past(17, DateTime.Now) :
                Faker.Date.Past(30, DateTime.Now.AddYears(-18));
        }

        public Gender GetValidGender() =>
            GetRandomBoolean() ? Gender.Male : Gender.Male;
    }


    [CollectionDefinition(nameof(MemberTestFixture))]
    public class MemberTestFixtureCollection
        : ICollectionFixture<MemberTestFixture>
    {
    }
}
