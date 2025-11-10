using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Enum;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.UpdateMember
{
    public class UpdateMemberInput : IRequest<MemberModelOutput>
    {
        public Guid Id { get; set; }
        public IdentifierDocumentDto Document { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
        public Guid? ResponsibleId { get; set; }
        public bool IsActive { get; set; }

        public UpdateMemberInput(
            Guid id,
            IdentifierDocumentDto document,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            string phoneNumber,
            AddressDto address,
            Guid? responsibleId,
            bool isActive)
        {
            Id = id;
            Document = document;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Address = address;
            ResponsibleId = responsibleId;
            IsActive = isActive;
        }
    }
}
