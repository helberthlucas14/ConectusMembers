using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Application.UseCases.Member.ListMembers;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.Infra.Data.EF.Repositories;
using FluentAssertions;
using DomainEntity = Conectus.Members.Domain.Entity;


namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.ListMember
{
    [Collection(nameof(ListMembersTestFixture))]
    public class ListMembersTest
    {
        private readonly ListMembersTestFixture _fixture;
        public ListMembersTest(ListMembersTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
        [Trait("Integration/Application", "ListMembers -  Use Cases")]
        public async Task SearchReturnListAndTotal()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleMembersList = _fixture.GetValidMembersList(10);
            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var repository = new MemberRepository(dbContext);

            var searchInput = new ListMembersInput(1, 20);

            var usecase = new ListMembers(repository);

            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleMembersList.Count);
            output.Items.Should().HaveCount(exampleMembersList.Count);

            foreach (MemberModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleMembersList.Find(
                    Member => Member.Id == outputItem.Id
                );

                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().NotBeEmpty();
                outputItem.FirstName.Should().Be(exampleItem.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.DateOfBirth.Should().Be(exampleItem.DateOfBirth);
                outputItem.Gender.Should().Be(exampleItem.Gender);
                outputItem.PhoneNumber.Should().Be(exampleItem.PhoneNumber.Value);
                outputItem.Document.Should().NotBeNull();
                outputItem.Document.Number.Should().Be(exampleItem.Document.Document);
                outputItem.Document.Type.Should().Be(exampleItem.Document.Type);

                outputItem.Address.Should().NotBeNull();
                outputItem.Address.Street.Should().Be(exampleItem.Address.Street);
                outputItem.Address.Number.Should().Be(exampleItem.Address.Number);
                outputItem.Address.Complement.Should().Be(exampleItem.Address.Complement);
                outputItem.Address.District.Should().Be(exampleItem.Address.District);
                outputItem.Address.City.Should().Be(exampleItem.Address.City);
                outputItem.Address.State.Should().Be(exampleItem.Address.State);
                outputItem.Address.ZipCode.Should().Be(exampleItem.Address.ZipCode);
                outputItem.Address.Latitude.Should().Be(exampleItem.Address.Latitude);
                outputItem.Address.Longitude.Should().Be(exampleItem.Address.Longitude);

                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.IsMinor.Should().Be(exampleItem.IsMinor);
            }
        }

        [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Application", "ListMembers -   Use Cases")]
        public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
        {
            var dbContext = _fixture.CreateDbContext();

            var repository = new MemberRepository(dbContext);
            var searchInput = new ListMembersInput(1, 20, "", "", SearchOrder.Asc);

            var usecase = new ListMembers(repository);
            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

    }
}
