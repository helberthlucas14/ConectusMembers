using Conectus.Members.IntergrationTests.Application.UseCases.Member.Common;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.DeleteMember
{
    public class DeleteMemberTestFixture : MemberUseCaseBaseFixture
    {
    }

    [CollectionDefinition(nameof(DeleteMemberTestFixture))]
    public class DeleteMemberTestFixtureCollection :
         ICollectionFixture<DeleteMemberTestFixture>
    {
    }
}
