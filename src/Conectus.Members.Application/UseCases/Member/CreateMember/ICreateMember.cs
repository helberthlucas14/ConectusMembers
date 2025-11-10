using Conectus.Members.Application.UseCases.Member.Common;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.CreateMember
{
    public interface ICreateMember
        : IRequestHandler<CreateMemberInput, MemberModelOutput>
    {
    }
}
