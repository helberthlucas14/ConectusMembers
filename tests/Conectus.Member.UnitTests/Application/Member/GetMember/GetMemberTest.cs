using Conectus.Members.Application.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = Conectus.Members.Application.UseCases.Member.GetMember;

namespace Conectus.Members.UnitTests.Application.Member.GetMember
{
    [Collection(nameof(GetMemberTestFixture))]
    public class GetMemberTest
    {
        private readonly GetMemberTestFixture _fixture;
        public GetMemberTest(GetMemberTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetMember))]
        [Trait("Application", "Member - Use Cases")]
        public async Task GetMember()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleMember = _fixture.GetValidMemberExample();

            repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleMember);
            var input = new UseCase.GetMemberInput(exampleMember.Id);
            var useCase = new UseCase.GetMember(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(exampleMember.FirstName);
            output.LastName.Should().Be(exampleMember.LastName);
            output.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            output.Gender.Should().Be(exampleMember.Gender);
            output.PhoneNumber.Should().Be(exampleMember.PhoneNumber.Value);
            output.Document.Should().NotBeNull();
            output.Document.Number.Should().Be(exampleMember.Document.Document);
            output.Document.Type.Should().Be(exampleMember.Document.Type);

            output.Address.Should().NotBeNull();
            output.Address.Street.Should().Be(exampleMember.Address.Street);
            output.Address.Number.Should().Be(exampleMember.Address.Number);
            output.Address.Complement.Should().Be(exampleMember.Address.Complement);
            output.Address.District.Should().Be(exampleMember.Address.District);
            output.Address.City.Should().Be(exampleMember.Address.City);
            output.Address.State.Should().Be(exampleMember.Address.State);
            output.Address.ZipCode.Should().Be(exampleMember.Address.ZipCode);
            output.Address.Latitude.Should().Be(exampleMember.Address.Latitude);
            output.Address.Longitude.Should().Be(exampleMember.Address.Longitude);

            output.IsActive.Should().Be(exampleMember.IsActive);
            output.IsMinor.Should().Be(exampleMember.IsMinor);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenMemberDoesntExist))]
        [Trait("Application", "GetMember - Use Cases")]
        public async Task NotFoundExceptionWhenMemberDoesntExist()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleGuid = Guid.NewGuid();
            repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(
                new NotFoundException($"Member '{exampleGuid}' not found.")
            );
            var input = new UseCase.GetMemberInput(exampleGuid);
            var useCase = new UseCase.GetMember(repositoryMock.Object);

            var task = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            repositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
