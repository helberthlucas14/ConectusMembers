using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Application.UseCases.Member.UpdateMember;
using Conectus.Members.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = Conectus.Members.Domain.Entity;
using UseCase = Conectus.Members.Application.UseCases.Member.UpdateMember;

namespace Conectus.Members.UnitTests.Application.Member.UpdateMember
{
    [Collection(nameof(UpdateMemberTestFixture))]
    public class UpdateMemberTest
    {
        private readonly UpdateMemberTestFixture _fixture;
        public UpdateMemberTest(UpdateMemberTestFixture fixture)
            => _fixture = fixture;

        [Theory(DisplayName = nameof(UpdateMember))]
        [Trait("Application", "UpdateMember - Use Cases")]
        [MemberData(
            nameof(UpdateMemberTestDataGenerator.GetMembersToUpdate),
            parameters: 12,
            MemberType = typeof(UpdateMemberTestDataGenerator)
        )]
        public async Task UpdateMember(
            DomainEntity.Member exampleMember,
            UpdateMemberInput input
        )
        {
            var useCase = new UseCase.UpdateMember();

            input.Id = exampleMember.Id;

            MemberModelOutput output = await
                useCase.Handle(input, CancellationToken.None);

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
            output.IsActive.Should().Be(input.IsActive);
            output.ResponsibleId.Should().Be(input.ResponsibleId);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantUpdateMember))]
        [Trait("Application", "UpdateMember - Use Cases")]
        [MemberData(nameof(UpdateMemberTestDataGenerator.GetInvalidUpdateMemberInputs),
            parameters: 12,
            MemberType = typeof(UpdateMemberTestDataGenerator)
        )]
        public async Task ThrowWhenCantUpdateMember(
           UpdateMemberInput input,
           string expectedExceptionMessage
        )
        {
            var exampleMember = _fixture.GetValidMemberExample();
            input.Id = exampleMember.Id;
            
            var useCase = new UseCase.UpdateMember();

            var task = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<DomainValidationException>()
                .WithMessage(expectedExceptionMessage);
        }
    }
}
