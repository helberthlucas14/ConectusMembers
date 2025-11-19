using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.IntergrationTests.Base;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.Common
{
    public class MemberUseCaseBaseFixture : BaseFixture
    {
        public IdentifierDocument GetValidDocument() => new IdentifierDocument(
        DocumentType.CPF,
        Faker.Person.Cpf());
        public string GetValidFirstName() =>
            Faker.Name.FirstName();
        public string GetValidLastName() =>
            Faker.Name.LastName();
        public DateTime GetValidAdultDateOfBirth() =>
            Faker.Date.Past(30, DateTime.Now.AddYears(-18));

        public DateTime GetValidMinorDateOfBirth() =>
            Faker.Date.Past(17, DateTime.Now);
        public PhoneNumber GetValidPhoneNumber()
        {
            int[] validDDDs = {11, 12, 13, 14, 15, 16, 17, 18, 19,
                       21, 22, 24, 27, 28, 31, 32, 33, 34, 35,
                       37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                       51, 53, 54, 55, 61, 62, 63, 64, 65, 66, 67,
                       68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83,
                       84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95,
                       96, 97, 98, 99};

            int ddd = Faker.PickRandom(validDDDs);

            bool celular = Faker.Random.Bool();

            string number;
            if (celular)
                number = Faker.Random.Number(900000000, 999999999).ToString();
            else
                number = Faker.Random.Number(20000000, 99999999).ToString();

            string phone = $"+55{ddd}{number}";

            return new PhoneNumber(phone);
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

        public Gender GetValidGender() =>
            GetRandomBoolean() ? Gender.Male :
            Gender.Female;
        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
        public DomainEntity.Member GetValidMemberExample(bool isMinor = false)
        {
            return new DomainEntity.Member(
                  GetValidFirstName(),
                  GetValidLastName(),
                  isMinor ? GetValidMinorDateOfBirth() : GetValidAdultDateOfBirth(),
                  (Gender)Faker.Person.Gender,
                  GetValidPhoneNumber(),
                  GetValidDocument(),
                  GetAddress(),
                  isMinor ? Guid.NewGuid() : null);
        }
        public List<DomainEntity.Member> GetValidMembersList(int length = 10)
        {
            var members = new List<DomainEntity.Member>();
            for (int i = 0; i < length; i++)
                members.Add(GetValidMemberExample());

            return members;
        }
        public List<DomainEntity.Member> CloneMembersListOrdered(
                    List<DomainEntity.Member> membersList,
                    string orderBy,
                    SearchOrder order)
        {
            var listClone = new List<DomainEntity.Member>(membersList);
            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("identifierDocument", SearchOrder.Asc) =>
                listClone.OrderBy(x => x.FirstName)
                         .ThenBy(x => x.Id),
                ("identifierDocument", SearchOrder.Desc) =>
                listClone.OrderByDescending(x => x.Document.Document)
                         .ThenBy(x => x.Id),
                ("firstName", SearchOrder.Asc) => listClone
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.Id),
                ("firstName", SearchOrder.Desc) => listClone
                    .OrderByDescending(x => x.FirstName)
                    .ThenByDescending(x => x.Id),
                ("lastName", SearchOrder.Desc) => listClone
                        .OrderByDescending(x => x.LastName)
                        .ThenBy(x => x.Id),
                ("lastName", SearchOrder.Asc) => listClone.OrderBy(x => x.LastName),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.FirstName).ThenBy(x => x.Id),
            };
            return orderedEnumerable.ToList();
        }
    }
}
