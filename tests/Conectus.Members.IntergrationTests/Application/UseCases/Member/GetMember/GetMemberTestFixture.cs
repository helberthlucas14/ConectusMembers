using Conectus.Members.IntergrationTests.Application.UseCases.Member.Common;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.GetMember
{
    public class GetMemberTestFixture : MemberUseCaseBaseFixture
    {

    }

    [CollectionDefinition(nameof(GetMemberTestFixture))]
    public class GetMemberTestFixtureCollection :
         ICollectionFixture<GetMemberTestFixture>
    {
    }
}
