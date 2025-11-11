using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Entity;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.IntergrationTests.Base;
using System.Text.RegularExpressions;

namespace Conectus.Members.IntergrationTests.Infra.Data.EF.UnitOfWork
{
    [CollectionDefinition(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
    { }
    public class UnitOfWorkTestFixture
        : BaseFixture
    {
        public Member GetValidMember(bool isMinor = false)
        {
            return new Member(
                  GetValidFirstName(),
                  GetValidLastName(),
                  GetValidDateOfBirth(isMinor),
                  GetValidGender(),
                  GetValidPhoneNumber(),
                  GetIdentifierDocument(),
                  GetAddress(),
                  isMinor ? Guid.NewGuid() : null);
        }

        public Address GetAddress()
        {
            var number = Faker.Random.Long(1, 9_999_999_999).ToString();
            return new Address(
                Faker.Address.StreetName(),
                number,
                Faker.Address.SecondaryAddress(),
                Faker.Address.County(),
                Faker.Address.City(),
                Faker.Address.StateAbbr(),
                Faker.Address.ZipCode(),
                Faker.Random.Number(-90, 90),
                Faker.Random.Number(-180, 180));
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

        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

        public List<Member> GetExampleMembersList(int length = 10)
          => Enumerable.Range(1, length)
            .Select(_ => GetValidMember()).ToList();
    }
}
