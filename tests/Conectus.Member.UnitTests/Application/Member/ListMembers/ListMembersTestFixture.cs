using Conectus.Members.Application.UseCases.Member.ListMembers;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.UnitTests.Application.Member.Common;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.UnitTests.Application.Member.ListMembers
{
    public class ListMembersTestFixture
        : MemberUseCaseBaseFixture
    {
        public ListMembersInput GetExampleInput(FilterBy filterBy = FilterBy.None)
        {
            var random = new Random();
            return new ListMembersInput(
                page: random.Next(1, 10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                dir: random.Next(0, 10) > 5 ?
                    SearchOrder.Asc : SearchOrder.Desc,
                filterBy: filterBy
            );
        }

        public List<DomainEntity.Member> GetValidMembersList(int length = 10)
        {
            var members = new List<DomainEntity.Member>();
            for (int i = 0; i < length; i++)
                members.Add(GetValidMemberExample());

            return members;
        }
    }

    [CollectionDefinition(nameof(ListMembersTestFixture))]
    public class ListMemberTestFixtureCollection
        : ICollectionFixture<ListMembersTestFixture>
    {
    }
}
