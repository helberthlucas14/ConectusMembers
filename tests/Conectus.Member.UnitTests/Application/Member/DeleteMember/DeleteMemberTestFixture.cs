using Conectus.Members.UnitTests.Application.Member.Common;

namespace Conectus.Members.UnitTests.Application.Member.DeleteMember
{
    public class DeleteMemberTestFixture : MemberUseCaseBaseFixture
    {


    }

    [CollectionDefinition(nameof(DeleteMemberTestFixture))]
    public class DeleteMemberTestFixtureCollection
        : ICollectionFixture<DeleteMemberTestFixture>
    {
    }
}
