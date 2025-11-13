using Conectus.Members.Application.Common;
using Conectus.Members.Application.UseCases.Member.Common;

namespace Conectus.Members.Application.UseCases.Member.ListMembers
{
    public class ListMembersOutput :
        PaginatedListOutput<MemberModelOutput>
    {
        public ListMembersOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<MemberModelOutput> items)
            : base(page, perPage, total, items)
        {
        }
    }
}
