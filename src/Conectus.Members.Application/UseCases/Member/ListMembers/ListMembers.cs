using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Repository;
using Conectus.Members.Domain.Repository.SearchableRepository;

namespace Conectus.Members.Application.UseCases.Member.ListMembers
{
    public class ListMembers : IListMembers
    {
        private readonly IMemberRepository _repository;

        public ListMembers(IMemberRepository repository)
            => _repository = repository;

        public async Task<ListMembersOutput> Handle(
            ListMembersInput input,
            CancellationToken cancellationToken)
        {
            var searchInput = new SearchInput(
                input.Page,
                input.PerPage,
                input.Search,
                input.Sort,
                input.Dir,
                input.FiterBy);

            var searchOutput = await _repository.Search(
                searchInput,
                cancellationToken);

            return new ListMembersOutput(
                  searchOutput.CurrentPage,
                  searchOutput.PerPage,
                  searchOutput.Total,
                  searchOutput.Items
                  .Select(MemberModelOutput.FromMember)
                  .ToList());
        }
    }
}
