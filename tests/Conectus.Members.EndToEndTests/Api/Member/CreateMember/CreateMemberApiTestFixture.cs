using Conectus.Members.EndToEndTests.Api.Member.Common;

namespace Conectus.Members.EndToEndTests.Api.Member.CreateMember
{
    [CollectionDefinition(nameof(CreateMemberApiTestFixture))]
    public class CreateMemberApiTestFixtureCollection
    : ICollectionFixture<CreateMemberApiTestFixture>
    { }
    public class CreateMemberApiTestFixture : MemberBaseFixture
    {
    }
}
