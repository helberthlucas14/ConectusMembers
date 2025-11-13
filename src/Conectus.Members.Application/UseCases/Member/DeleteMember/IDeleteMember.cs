using MediatR;
namespace Conectus.Members.Application.UseCases.Member.DeleteMember
{
    public interface IDeleteMember 
        : IRequestHandler<DeleteMemberInput, Unit>
    {
    }
}
