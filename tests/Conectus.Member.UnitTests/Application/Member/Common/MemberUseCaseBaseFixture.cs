using Bogus.Extensions.Brazil;
using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;
using System.Text.RegularExpressions;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.UnitTests.Application.Member.Common
{
    public class MemberUseCaseBaseFixture
        : BaseFixture
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
            string pattern = @"^\(\d{2}\)\d{3}-\d{3}-\d{3}$";
            var regex = new Regex(pattern);

            var phone = Faker.Phone.PhoneNumber("(##)###-###-###");

            while (!regex.IsMatch(phone))
                phone = Faker.Phone.PhoneNumber("(##)###-###-###");

            return new PhoneNumber(phone);
        }

        public Address GetValidAddress() =>
            GetAddress();
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
