using Conectus.Members.Application.UseCases.Member.Common;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.GetMember
{
    public interface IGetMember : IRequestHandler<GetMemberInput, MemberModelOutput>
    { }
}
