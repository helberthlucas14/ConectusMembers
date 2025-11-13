using MediatR;

namespace Conectus.Members.Application.UseCases.Member.DeleteMember
{
    public class DeleteMemberInput : IRequest<Unit>
    {
        public Guid Id { get; }
        public DeleteMemberInput(Guid id)
        {
            Id = id;
        }
    }
}
