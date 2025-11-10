using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.Domain.Exceptions;
using FluentAssertions;
using UseCase = Conectus.Members.Application.UseCases.Member.CreateMember;

namespace Conectus.Members.UnitTests.Application.Member.CreateMember
{
    [Collection(nameof(CreateMemberTestFixture))]
    public class CreateMemberTest
    {
        private readonly CreateMemberTestFixture _fixture;
        public CreateMemberTest(CreateMemberTestFixture fixture) => _fixture = fixture;


        [Fact(DisplayName = nameof(CreateAdultMemberSuccessfully))]
        [Trait("Application", "CreateMember - Use Cases")]
        public async Task CreateAdultMemberSuccessfully()
        {
            var input = _fixture.GetExampleInput();

            var useCase = new UseCase.CreateMember();

            var datetimeBefore = DateTime.Now;

            var output = await useCase.Handle(
                input,
                CancellationToken.None
                );

            var datetimeAfter = DateTime.Now.AddSeconds(1);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.DateOfBirth.Should().Be(input.DateOfBirth);
            output.Gender.Should().Be(input.Gender);
            output.PhoneNumber.Should().Be(input.PhoneNumber);
            output.Document.Should().NotBeNull();
            output.Document.Should().Be(input.Document);

            output.Address.Should().NotBeNull();
            output.Address.Should().NotBeNull();
            output.IsActive.Should().BeTrue();

            (output.CreatedAt >= datetimeBefore).Should().BeTrue();
            (output.CreatedAt <= datetimeAfter).Should().BeTrue();
        }


        [Fact(DisplayName = nameof(CreateMinorMemberSuccessfully))]
        [Trait("Application", "CreateMember - Use Cases")]
        public async Task CreateMinorMemberSuccessfully()
        {
            var isMinorInput = true;
            var responsibleId = Guid.NewGuid();
            var input = _fixture.GetExampleInput(isMinorInput, responsibleId);

            var useCase = new UseCase.CreateMember();

            var datetimeBefore = DateTime.Now;

            var output = await useCase.Handle(
                input,
                CancellationToken.None
                );

            var datetimeAfter = DateTime.Now.AddSeconds(1);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.DateOfBirth.Should().Be(input.DateOfBirth);
            output.Gender.Should().Be(input.Gender);
            output.PhoneNumber.Should().Be(input.PhoneNumber);
            output.Document.Should().NotBeNull();
            output.Document.Should().Be(input.Document);

            output.Address.Should().NotBeNull();
            output.Address.Should().NotBeNull();
            output.IsActive.Should().BeTrue();
            output.isMinor.Should().BeTrue();
            output.ResponsibleId.Should().NotBeEmpty();
            output.ResponsibleId.Should().Be(responsibleId);

            (output.CreatedAt >= datetimeBefore).Should().BeTrue();
            (output.CreatedAt <= datetimeAfter).Should().BeTrue();
        }

        [Fact(DisplayName = nameof(ThrowWhenMinorWithoutResposibleId))]
        [Trait("Application", "CreateMember - Use Cases")]
        public async Task ThrowWhenMinorWithoutResposibleId()
        {
            var isMinorInput = true;
            var input = _fixture.GetExampleInput(isMinorInput);

            var useCase = new UseCase.CreateMember();

            var action = async () => await useCase.Handle(
                   input,
                   CancellationToken.None
                   );

            await action.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Member is a minor and needs a guardian.");
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateMember))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(
            nameof(CreateMemberTestDataGenerator.GetInvalidCreateMemberInputs),
            parameters: 50,
            MemberType = typeof(CreateMemberTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateMember(
         CreateMemberInput input,
         string exceptionMessage
            )
        {
            var useCase = new UseCase.CreateMember();

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }


    }
}
