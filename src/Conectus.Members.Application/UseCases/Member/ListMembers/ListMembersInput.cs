using Conectus.Members.Application.Common;
using Conectus.Members.Domain.Repository.SearchableRepository;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.ListMembers
{
    public class ListMembersInput :
        PaginatedListInput, IRequest<ListMembersOutput>
    {
        public ListMembersInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder dir = SearchOrder.Asc,
            FilterBy filterBy = FilterBy.None)
            : base(page, perPage, search, sort, dir, filterBy)
        {
        }

        public ListMembersInput()
            : base(1, 15, "", "", SearchOrder.Asc, FilterBy.None)
        {
        }
    }
}
