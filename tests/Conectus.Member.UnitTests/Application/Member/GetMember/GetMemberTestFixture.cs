using Conectus.Members.UnitTests.Application.Member.Common;
using Conectus.Members.UnitTests.Common;

namespace Conectus.Members.UnitTests.Application.Member.GetMember
{
    public class GetMemberTestFixture : MemberUseCaseBaseFixture
    {

    }

    [CollectionDefinition(nameof(GetMemberTestFixture))]
    public class GetMemberTestFixtureCollection
    : ICollectionFixture<GetMemberTestFixture>
    {
    }
}
