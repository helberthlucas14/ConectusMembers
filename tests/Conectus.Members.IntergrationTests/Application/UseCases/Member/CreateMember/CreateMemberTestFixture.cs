using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.IntergrationTests.Application.UseCases.Member.Common;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.CreateMember
{
    public class CreateMemberTestFixture 
        : MemberUseCaseBaseFixture
    {
        public CreateMemberInput GetExampleInput(bool isMinor = false, Guid? resposibleId = null)
        {
            return new CreateMemberInput(
                 IdentifierDocumentDto.FromDomain(GetValidDocument()),
                 GetValidFirstName(),
                 GetValidLastName(),
                 isMinor ? GetValidMinorDateOfBirth() : GetValidAdultDateOfBirth(),
                 GetValidGender(),
                 GetValidPhoneNumber().Value,
                 AddressDto.FromDomain(GetAddress()),
                 isMinor ? resposibleId : null
                 );
        }
        public CreateMemberInput GetInvalidInputShortFirstName()
        {
            var input = GetExampleInput();
            input.FirstName = Faker.Random.String2(1, 2);
            return input;
        }
        public CreateMemberInput GetInvalidInputLongFirstName()
        {
            var input = GetExampleInput();
            input.FirstName = Faker.Random.String2(51, 60);
            return input;
        }
        public CreateMemberInput GetInvalidInputShortLastName()
        {
            var input = GetExampleInput();
            input.LastName = Faker.Random.String2(1, 2);
            return input;
        }
        public CreateMemberInput GetInvalidInputLongLastName()
        {
            var input = GetExampleInput();
            input.LastName = Faker.Random.String2(51, 60);
            return input;
        }
        public CreateMemberInput GetInvalidDateOfBirth()
        {
            var input = GetExampleInput();
            input.DateOfBirth = DateTime.Now.AddDays(1);
            return input;
        }

        public CreateMemberInput GetInvalidInputShortFirstNameNull()
        {
            var input = GetExampleInput();
            input.FirstName = null!;
            return input;
        }

        public CreateMemberInput GetInvalidInputShortLastNameNull()
        {
            var input = GetExampleInput();
            input.LastName = null!;
            return input;
        }
    }
    
    [CollectionDefinition(nameof(CreateMemberTestFixture))]
    public class CreateMemberTestsCollection 
        : ICollectionFixture<CreateMemberTestFixture>
    {
    }
}
