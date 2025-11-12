using Conectus.Members.Application.Exceptions;
using Conectus.Members.Domain.Entity;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.Infra.Data.EF;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using Repository = Conectus.Members.Infra.Data.EF.Repositories;

namespace Conectus.Members.IntergrationTests.Infra.Data.EF.Repositories
{
    [Collection(nameof(MemberRepositoryTestFixture))]
    public class MemberRepositoryTest
    {
        private readonly MemberRepositoryTestFixture _fixture;

        public MemberRepositoryTest(MemberRepositoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(InsertAdultMember))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task InsertAdultMember()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMember = _fixture.GetExampleMember();
            var memberRepository = new Repository.MemberRepository(dbContext);

            await memberRepository.Insert(exampleMember, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbmember = await (_fixture.CreateDbContext(true))
                .Members.FindAsync(exampleMember.Id);

            dbmember.Should().NotBeNull();
            dbmember.FirstName.Should().Be(exampleMember.FirstName);
            dbmember.LastName.Should().Be(exampleMember.LastName);
            dbmember.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            dbmember.Gender.Should().Be(exampleMember.Gender);
            dbmember.PhoneNumber.Should().NotBeNull();
            dbmember.PhoneNumber.Value.Should().Be(exampleMember.PhoneNumber.Value);
            dbmember.Document.Should().NotBeNull();
            dbmember.Document.Document.Should().Be(exampleMember.Document.Document);
            dbmember.Responsible.Should().Be(exampleMember.Responsible);
            dbmember.ResponsibleId.Should().Be(exampleMember.ResponsibleId);
            dbmember.IsActive.Should().Be(exampleMember.IsActive);
            dbmember.CreatedAt.Should().Be(exampleMember.CreatedAt);
        }

        [Fact(DisplayName = nameof(InsertMinorMember))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task InsertMinorMember()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();

            var exampleMemberResposible = _fixture.GetExampleMember();
            var exampleMembersList = _fixture.GetExampleMembersList(15);
            exampleMembersList.Add(exampleMemberResposible);

            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var exampleMember = _fixture.GetExampleMember(
                isMinor: true,
                responsibleId: exampleMemberResposible.Id);

            var memberRepository = new Repository.MemberRepository(dbContext);

            await memberRepository.Insert(exampleMember, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbmember = await (_fixture.CreateDbContext(true))
                .Members
                .Include(m => m.Responsible)
                .FirstOrDefaultAsync(m => m.Id == exampleMember.Id);

            dbmember.Should().NotBeNull();
            dbmember.FirstName.Should().Be(exampleMember.FirstName);
            dbmember.LastName.Should().Be(exampleMember.LastName);
            dbmember.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            dbmember.Gender.Should().Be(exampleMember.Gender);
            dbmember.PhoneNumber.Should().NotBeNull();
            dbmember.PhoneNumber.Value.Should().Be(exampleMember.PhoneNumber.Value);
            dbmember.Document.Should().NotBeNull();
            dbmember.Document.Document.Should().Be(exampleMember.Document.Document);
            dbmember.Responsible.Should().NotBeNull();
            dbmember.ResponsibleId.Should().Be(exampleMemberResposible.Id);
            dbmember.IsActive.Should().Be(exampleMember.IsActive);
            dbmember.CreatedAt.Should().Be(exampleMember.CreatedAt);
        }


        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task Get()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMember = _fixture.GetExampleMember();
            var exampleMembersList = _fixture.GetExampleMembersList(15);
            exampleMembersList.Add(exampleMember);
            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var MemberRepository = new Repository.MemberRepository(
                _fixture.CreateDbContext(true)
            );

            var dbMember = await MemberRepository.Get(
                exampleMember.Id,
                CancellationToken.None);

            dbMember.Should().NotBeNull();
            dbMember.FirstName.Should().Be(exampleMember.FirstName);
            dbMember.LastName.Should().Be(exampleMember.LastName);
            dbMember.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            dbMember.Gender.Should().Be(exampleMember.Gender);
            dbMember.PhoneNumber.Should().NotBeNull();
            dbMember.PhoneNumber.Value.Should().Be(exampleMember.PhoneNumber.Value);
            dbMember.Document.Should().NotBeNull();
            dbMember.Document.Document.Should().Be(exampleMember.Document.Document);
            dbMember.Responsible.Should().Be(exampleMember.Responsible);
            dbMember.ResponsibleId.Should().Be(exampleMember.ResponsibleId);
            dbMember.IsActive.Should().Be(exampleMember.IsActive);
            dbMember.CreatedAt.Should().Be(exampleMember.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            await dbContext.AddRangeAsync(_fixture.GetExampleMembersList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var MemberRepository = new Repository.MemberRepository(dbContext);

            var task = async () => await MemberRepository.Get(
                exampleId,
                CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Member '{exampleId}' not found.");
        }

        [Fact(DisplayName = nameof(UpdateAdultMember))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task UpdateAdultMember()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMember = _fixture.GetExampleMember();
            var newMemberValues = _fixture.GetExampleMember();
            var exampleMembersList = _fixture.GetExampleMembersList(15);
            exampleMembersList.Add(exampleMember);
            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var MemberRepository = new Repository.MemberRepository(dbContext);

            exampleMember.Update(
                newMemberValues.FirstName,
                newMemberValues.LastName,
                address: newMemberValues.Address,
                phoneNumber: newMemberValues.PhoneNumber
                );
            await MemberRepository.Update(exampleMember, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbMember = await (_fixture.CreateDbContext(true))
                .Members.FindAsync(exampleMember.Id);
            dbMember.Should().NotBeNull();
            dbMember.FirstName.Should().Be(exampleMember.FirstName);
            dbMember.LastName.Should().Be(exampleMember.LastName);
            dbMember.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            dbMember.Gender.Should().Be(exampleMember.Gender);
            dbMember.Address.Should().NotBeNull();
            dbMember.Address.Should().Be(exampleMember.Address);
            dbMember.PhoneNumber.Should().NotBeNull();
            dbMember.PhoneNumber.Value.Should().Be(exampleMember.PhoneNumber.Value);
            dbMember.Document.Should().NotBeNull();
            dbMember.Document.Document.Should().Be(exampleMember.Document.Document);
            dbMember.Responsible.Should().BeNull();
            dbMember.ResponsibleId.Should().BeNull();
            dbMember.IsActive.Should().Be(exampleMember.IsActive);
            dbMember.CreatedAt.Should().Be(exampleMember.CreatedAt);
        }


        [Fact(DisplayName = nameof(UpdateMinor))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task UpdateMinor()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();

            var exampleMemberResponsible = _fixture.GetExampleMember();

            var exampleMinorMember = _fixture.GetExampleMember(
                isMinor: true,
                responsibleId: exampleMemberResponsible.Id);

            var newMemberValues = _fixture.GetExampleMember();
            var exampleMembersList = _fixture.GetExampleMembersList(15);

            exampleMembersList.Add(exampleMemberResponsible);
            exampleMembersList.Add(exampleMinorMember);

            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var MemberRepository = new Repository.MemberRepository(dbContext);

            exampleMinorMember.Update(
                newMemberValues.FirstName,
                newMemberValues.LastName,
                address: newMemberValues.Address,
                phoneNumber: newMemberValues.PhoneNumber,
                responsibleId: exampleMemberResponsible.Id
                );

            await MemberRepository.Update(exampleMinorMember, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbMember = await (_fixture.CreateDbContext(true))
                 .Members.Include(m => m.Responsible)
                 .FirstOrDefaultAsync(m => m.Id == exampleMinorMember.Id);

            dbMember.Should().NotBeNull();
            dbMember.FirstName.Should().Be(exampleMinorMember.FirstName);
            dbMember.LastName.Should().Be(exampleMinorMember.LastName);
            dbMember.DateOfBirth.Should().Be(exampleMinorMember.DateOfBirth);
            dbMember.Gender.Should().Be(exampleMinorMember.Gender);
            dbMember.Address.Should().NotBeNull();
            dbMember.Address.Should().Be(exampleMinorMember.Address);
            dbMember.PhoneNumber.Should().NotBeNull();
            dbMember.PhoneNumber.Value.Should().Be(exampleMinorMember.PhoneNumber.Value);
            dbMember.Document.Should().NotBeNull();
            dbMember.Document.Document.Should().Be(exampleMinorMember.Document.Document);
            dbMember.Responsible.Should().NotBeNull();
            dbMember.ResponsibleId.Should().Be(exampleMemberResponsible.Id);
            dbMember.IsActive.Should().Be(exampleMinorMember.IsActive);
            dbMember.CreatedAt.Should().Be(exampleMinorMember.CreatedAt);
        }

        [Fact(DisplayName = nameof(SearchRetursListAndTotal))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task SearchRetursListAndTotal()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMembersAdultList = _fixture.GetExampleMembersList(10);

            await dbContext.AddRangeAsync(exampleMembersAdultList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var adultIds = exampleMembersAdultList.ConvertAll(m => m.Id);
            var exampleMembersList = _fixture.GetExampleMembersListWithMinors(adultIds, 10);

            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var MemberRepository = new Repository.MemberRepository(dbContext);


            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await MemberRepository.Search(searchInput, CancellationToken.None);

            var mergedList = exampleMembersAdultList.Concat(exampleMembersList).ToList();

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(mergedList.Count);

            output.Items.Should().HaveCount(mergedList.Count);

            foreach (Member outputItem in output.Items)
            {
                var exampleItem = mergedList.Find(
                    Member => Member.Id == outputItem.Id
                );

                exampleItem.Should().NotBeNull();
                outputItem.FirstName.Should().Be(exampleItem.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.DateOfBirth.Should().Be(exampleItem.DateOfBirth);
                outputItem.Gender.Should().Be(exampleItem.Gender);
                outputItem.Address.Should().NotBeNull();
                outputItem.Address.Should().Be(exampleItem.Address);
                outputItem.PhoneNumber.Should().NotBeNull();
                outputItem.PhoneNumber.Value.Should().Be(exampleItem.PhoneNumber.Value);
                outputItem.Document.Should().NotBeNull();
                outputItem.Document.Document.Should().Be(exampleItem.Document.Document);
                outputItem.ResponsibleId.Should().Be(exampleItem.ResponsibleId);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SearchRetursEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMembersList = _fixture.GetExampleMembersList(10);

            var MemberRepository = new Repository.MemberRepository(dbContext);

            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await MemberRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SearchRetursPaginated))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task SearchRetursPaginated(int quantityMembersToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems)
        {
            ConectusMemberDbContext dbContext = _fixture.CreateDbContext();
            var exampleMembersList = _fixture.GetExampleMembersList(quantityMembersToGenerate);
           
            await dbContext.AddRangeAsync(exampleMembersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            
            var MemberRepository = new Repository.MemberRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);
            
            var output = await MemberRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(quantityMembersToGenerate);
            output.Items.Should().HaveCount(expectedQuantityItems);

            foreach (Member outputItem in output.Items)
            {
                var exampleItem = exampleMembersList.Find(
                    Member => Member.Id == outputItem.Id
                );
                
                exampleItem.Should().NotBeNull();
                outputItem.FirstName.Should().Be(exampleItem.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.DateOfBirth.Should().Be(exampleItem.DateOfBirth);
                outputItem.Gender.Should().Be(exampleItem.Gender);
                outputItem.Address.Should().NotBeNull();
                outputItem.Address.Should().Be(exampleItem.Address);
                outputItem.PhoneNumber.Should().NotBeNull();
                outputItem.PhoneNumber.Value.Should().Be(exampleItem.PhoneNumber.Value);
                outputItem.Document.Should().NotBeNull();
                outputItem.Document.Document.Should().Be(exampleItem.Document.Document);
                outputItem.ResponsibleId.Should().Be(exampleItem.ResponsibleId);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }
    }
}
