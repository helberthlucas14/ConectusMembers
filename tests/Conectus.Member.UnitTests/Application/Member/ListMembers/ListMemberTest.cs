using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Repository.SearchableRepository;
using FluentAssertions;
using Moq;
using DomainEntity = Conectus.Members.Domain.Entity;
using UseCase = Conectus.Members.Application.UseCases.Member.ListMembers;

namespace Conectus.Members.UnitTests.Application.Member.ListMembers
{
    [Collection(nameof(ListMembersTestFixture))]
    public class ListMemberTest
    {
        private readonly ListMembersTestFixture _fixture;
        public ListMemberTest(ListMembersTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(List))]
        [Trait("Application", "ListMembers - Use Cases")]
        public async Task List()
        {
            var MembersExampleList = _fixture.GetValidMembersList();
            var repositoryMock = _fixture.GetRepositoryMock();
            var input = _fixture.GetExampleInput();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Member>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<DomainEntity.Member>)MembersExampleList,
                total: new Random().Next(50, 200)
            );
            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);
            var useCase = new UseCase.ListMembers(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<MemberModelOutput>)output.Items).ForEach(outputItem =>
            {
                var repositoryMember = outputRepositorySearch.Items
                    .FirstOrDefault(x => x.Id == outputItem.Id);

                repositoryMember.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().NotBeEmpty();
                outputItem.FirstName.Should().Be(repositoryMember.FirstName);
                outputItem.LastName.Should().Be(repositoryMember.LastName);
                outputItem.DateOfBirth.Should().Be(repositoryMember.DateOfBirth);
                outputItem.Gender.Should().Be(repositoryMember.Gender);
                outputItem.PhoneNumber.Should().Be(repositoryMember.PhoneNumber.Value);
                outputItem.Document.Should().NotBeNull();
                outputItem.Document.Number.Should().Be(repositoryMember.Document.Document);
                outputItem.Document.Type.Should().Be(repositoryMember.Document.Type);

                outputItem.Address.Should().NotBeNull();
                outputItem.Address.Street.Should().Be(repositoryMember.Address.Street);
                outputItem.Address.Number.Should().Be(repositoryMember.Address.Number);
                outputItem.Address.Complement.Should().Be(repositoryMember.Address.Complement);
                outputItem.Address.District.Should().Be(repositoryMember.Address.District);
                outputItem.Address.City.Should().Be(repositoryMember.Address.City);
                outputItem.Address.State.Should().Be(repositoryMember.Address.State);
                outputItem.Address.ZipCode.Should().Be(repositoryMember.Address.ZipCode);
                outputItem.Address.Latitude.Should().Be(repositoryMember.Address.Latitude);
                outputItem.Address.Longitude.Should().Be(repositoryMember.Address.Longitude);

                outputItem.IsActive.Should().Be(repositoryMember.IsActive);
                outputItem.IsMinor.Should().Be(repositoryMember.IsMinor);

            });
            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact(DisplayName = nameof(ListOkWhenEmpty))]
        [Trait("Application", "ListMembers - Use Cases")]
        public async Task ListOkWhenEmpty()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var input = _fixture.GetExampleInput();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Member>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: new List<DomainEntity.Member>().AsReadOnly(),
                total: 0
            );
            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);
            var useCase = new UseCase.ListMembers(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);

            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
        [Trait("Application", "ListMembers - Use Cases")]
        [MemberData(
            nameof(ListMembersTestDataGenerator.GetInputsWithoutAllParameter),
            parameters: 14,
            MemberType = typeof(ListMembersTestDataGenerator)
        )]
        public async Task ListInputWithoutAllParameters(
            UseCase.ListMembersInput input
        )
        {
            var MembersExampleList = _fixture.GetValidMembersList();
            var repositoryMock = _fixture.GetRepositoryMock();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Member>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<DomainEntity.Member>)MembersExampleList,
                total: new Random().Next(50, 200)
            );
            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);
            var useCase = new UseCase.ListMembers(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<MemberModelOutput>)output.Items).ForEach(outputItem =>
            {
                var repositoryMember = outputRepositorySearch.Items
                    .FirstOrDefault(x => x.Id == outputItem.Id);

                outputItem.Should().NotBeNull();
                repositoryMember.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().NotBeEmpty();
                outputItem.FirstName.Should().Be(repositoryMember.FirstName);
                outputItem.LastName.Should().Be(repositoryMember.LastName);
                outputItem.DateOfBirth.Should().Be(repositoryMember.DateOfBirth);
                outputItem.Gender.Should().Be(repositoryMember.Gender);
                outputItem.PhoneNumber.Should().Be(repositoryMember.PhoneNumber.Value);
                outputItem.Document.Should().NotBeNull();
                outputItem.Document.Number.Should().Be(repositoryMember.Document.Document);
                outputItem.Document.Type.Should().Be(repositoryMember.Document.Type);

                outputItem.Address.Should().NotBeNull();
                outputItem.Address.Street.Should().Be(repositoryMember.Address.Street);
                outputItem.Address.Number.Should().Be(repositoryMember.Address.Number);
                outputItem.Address.Complement.Should().Be(repositoryMember.Address.Complement);
                outputItem.Address.District.Should().Be(repositoryMember.Address.District);
                outputItem.Address.City.Should().Be(repositoryMember.Address.City);
                outputItem.Address.State.Should().Be(repositoryMember.Address.State);
                outputItem.Address.ZipCode.Should().Be(repositoryMember.Address.ZipCode);
                outputItem.Address.Latitude.Should().Be(repositoryMember.Address.Latitude);
                outputItem.Address.Longitude.Should().Be(repositoryMember.Address.Longitude);

                outputItem.IsActive.Should().Be(repositoryMember.IsActive);
                outputItem.IsMinor.Should().Be(repositoryMember.IsMinor);
            });
            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
