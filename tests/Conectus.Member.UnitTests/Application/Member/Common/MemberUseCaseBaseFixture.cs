using Bogus;
using Bogus.Extensions.Brazil;
using Conectus.Members.Application.Interfaces;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.Repository;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;
using Moq;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.UnitTests.Application.Member.Common
{
    public class MemberUseCaseBaseFixture
        : BaseFixture
    {
        public Mock<IMemberRepository> GetRepositoryMock()
             => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();
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

        public Gender GetValidGender() =>
            GetRandomBoolean() ? Gender.Male :
            Gender.Female;

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
    }
}
