using Conectus.Members.Application.UseCases.Member.Common;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.UpdateMember
{
    public interface IUpdateMember 
        : IRequestHandler<UpdateMemberInput, MemberModelOutput>
    {
    }
}
