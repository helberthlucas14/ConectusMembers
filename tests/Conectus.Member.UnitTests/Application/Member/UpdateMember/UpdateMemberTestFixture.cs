using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.Application.UseCases.Member.UpdateMember;
using Conectus.Members.UnitTests.Application.Member.Common;

namespace Conectus.Members.UnitTests.Application.Member.UpdateMember
{
    public class UpdateMemberTestFixture : MemberUseCaseBaseFixture
    {
        public UpdateMemberInput GetExampleValidInput(Guid? id = null,
            bool isMinor = false,
            Guid? resposibleId = null)
        => new(
                id ?? Guid.NewGuid(),
                IdentifierDocumentDto.FromDomain(GetValidDocument()),
                GetValidFirstName(),
                GetValidLastName(),
                isMinor ? GetValidMinorDateOfBirth() : GetValidAdultDateOfBirth(),
                GetValidGender(),
                GetValidPhoneNumber().Value,
                AddressDto.FromDomain(GetValidAddress()),
                resposibleId,
                GetRandomBoolean()
                );
        public UpdateMemberInput GetInvalidInputShortFirstName()
        {
            var input = GetExampleValidInput();
            input.FirstName = Faker.Random.String2(1, 2);
            return input;
        }
        public UpdateMemberInput GetInvalidInputLongFirstName()
        {
            var input = GetExampleValidInput();
            input.FirstName = Faker.Random.String2(51, 60);
            return input;
        }
        public UpdateMemberInput GetInvalidInputShortLastName()
        {
            var input = GetExampleValidInput();
            input.LastName = Faker.Random.String2(1, 2);
            return input;
        }
        public UpdateMemberInput GetInvalidInputLongLastName()
        {
            var input = GetExampleValidInput();
            input.LastName = Faker.Random.String2(51, 60);
            return input;
        }
        public UpdateMemberInput GetInvalidDateOfBirth()
        {
            var input = GetExampleValidInput();
            input.DateOfBirth = DateTime.Now.AddDays(1);
            return input;
        }

        public UpdateMemberInput GetInvalidInputShortFirstNameNull()
        {
            var input = GetExampleValidInput();
            input.FirstName = null!;
            return input;
        }

        public UpdateMemberInput GetInvalidInputShortLastNameNull()
        {
            var input = GetExampleValidInput();
            input.LastName = null!;
            return input;
        }
    }


    [CollectionDefinition(nameof(UpdateMemberTestFixture))]
    public class UpdateMemberTestFixtureCollection
        : IClassFixture<UpdateMemberTestFixture>
    { }

}
