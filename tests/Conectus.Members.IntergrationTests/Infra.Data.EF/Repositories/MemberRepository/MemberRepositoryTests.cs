using Conectus.Members.Infra.Data.EF;
using FluentAssertions;
using Repository = Conectus.Members.Infra.Data.EF.Repositories;

namespace Conectus.Members.IntergrationTests.Infra.Data.EF.Repositories
{
    [Collection(nameof(MemberRepositoryTestFixture))]
    public class MemberRepositoryTest
    {
        private readonly MemberRepositoryTestFixture _fixture;

        public MemberRepositoryTest(MemberRepositoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "MemberRepository - Repositories")]
        public async Task Insert()
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
            dbmember.IsActive.Should().Be(exampleMember.IsActive);
            dbmember.CreatedAt.Should().Be(exampleMember.CreatedAt);
        }
    }
}
