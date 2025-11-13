using MediatR;

namespace Conectus.Members.Application.UseCases.Member.ListMembers
{
    public interface IListMembers :
        IRequestHandler<ListMembersInput, ListMembersOutput>
    {
    }
}
