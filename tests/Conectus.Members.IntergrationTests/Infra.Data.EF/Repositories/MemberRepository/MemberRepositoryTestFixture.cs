using Bogus.Extensions.Brazil;
using Conectus.Members.Domain.Entity;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.IntergrationTests.Base;
using System.Text.RegularExpressions;

namespace Conectus.Members.IntergrationTests.Infra.Data.EF.Repositories
{
    [CollectionDefinition(nameof(MemberRepositoryTestFixture))]
    public class MemberRepositoryTestFixtureCollection
    : ICollectionFixture<MemberRepositoryTestFixture>
    { }
    public class MemberRepositoryTestFixture
        : BaseFixture
    {
        public Member GetExampleMember(
            bool isMinor = false,
            Guid? responsibleId = null,
            IdentifierDocument? document = null)
        {
            return new Member(
                  GetValidFirstName(),
                  GetValidLastName(),
                  GetValidDateOfBirth(isMinor),
                  GetValidGender(),
                  GetValidPhoneNumber(),
                  document ?? GetIdentifierDocument(),
                  GetAddress(),
                  isMinor ? responsibleId ?? Guid.NewGuid() : null);
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
         => Enumerable.Range(1, length).Select(_ => GetExampleMember()).ToList();

        public List<Member> GetExampleMembersListWithMinors(List<Guid> adultIds, int length = 10)
        {
            var members = new List<Member>();
            for (int i = 0; i < length; i++)
            {
                var isOdd = i % 2 != 0;
                var member = isOdd ? GetExampleMember(true, adultIds[new Random().Next(0, adultIds.Count)]) : GetExampleMember();
                members.Add(member);
            }

            return members;
        }

        public List<Member> GetExampleMembersListWithIdentifierDocuments(
            List<IdentifierDocument> documents)
         => documents.Select(document =>
         {
             var member = GetExampleMember(document: document);
             return member;
         }).ToList();

        public List<Member> GetExampleMembersListWithFirstNames(List<string> lastNames)
            => lastNames.Select(firstName =>
        {
            var member = GetExampleMember();
            member.Update(firstName: firstName);
            return member;
        }).ToList();

        public List<Member> GetExampleMembersListWithLastNames(List<string> names)
          => names.Select(lastName =>
         {
             var member = GetExampleMember();
             member.Update(lastName: lastName);
             return member;
         }).ToList();

        public List<Member> CloneMembersListOrdered(
            List<Member> membersList,
            string orderBy,
            SearchOrder order)
        {
            var listClone = new List<Member>(membersList);
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
