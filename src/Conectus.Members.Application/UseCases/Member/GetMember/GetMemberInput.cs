using Conectus.Members.Application.UseCases.Member.Common;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.GetMember
{
    public class GetMemberInput : IRequest<MemberModelOutput>
    {
        public Guid Id { get; set; }
        public GetMemberInput(Guid id)
            => Id = id;
    }
}
