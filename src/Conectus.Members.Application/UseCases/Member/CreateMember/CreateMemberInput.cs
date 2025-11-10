using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Enum;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.CreateMember
{
    public record CreateMemberInput(
        IdentifierDocumentDto Document,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        Gender Gender,
        string PhoneNumber,
        AddressDto Address,
        Guid? ResponsibleId) : IRequest<MemberModelOutput>;
}