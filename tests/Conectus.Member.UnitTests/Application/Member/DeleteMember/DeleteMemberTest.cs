using Conectus.Members.Application.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = Conectus.Members.Application.UseCases.Member.DeleteMember;

namespace Conectus.Members.UnitTests.Application.Member.DeleteMember
{
    [Collection(nameof(DeleteMemberTestFixture))]
    public class DeleteMemberTest
    {

        private readonly DeleteMemberTestFixture _fixture;

        public DeleteMemberTest(DeleteMemberTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteMember))]
        [Trait("Application", "DeleteMember - Use Cases")]
        public async Task DeleteMember()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var MemberExample = _fixture.GetValidMemberExample();
            repositoryMock.Setup(x => x.Get(
                MemberExample.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(MemberExample);

            var input = new UseCase.DeleteMemberInput(MemberExample.Id);
            var useCase = new UseCase.DeleteMember(
                repositoryMock.Object,
                unitOfWorkMock.Object);

            await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(
                MemberExample.Id,
                It.IsAny<CancellationToken>()
            ), Times.Once);
            repositoryMock.Verify(x => x.Delete(
                MemberExample,
                It.IsAny<CancellationToken>()
            ), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }


        [Fact(DisplayName = nameof(ThrowWhenMemberNotFound))]
        [Trait("Application", "DeleteMember - Use Cases")]
        public async Task ThrowWhenMemberNotFound()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleGuid = Guid.NewGuid();
            repositoryMock.Setup(x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>())
            ).ThrowsAsync(
                new NotFoundException($"Member '{exampleGuid}' not found.")
            );
            var input = new UseCase.DeleteMemberInput(exampleGuid);
            var useCase = new UseCase.DeleteMember(
                repositoryMock.Object,
                unitOfWorkMock.Object);

            var task = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<NotFoundException>();

            repositoryMock.Verify(x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
