using Conectus.Members.Domain.Enum;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.Application.UseCases.Member.Common
{
    public record MemberModelOutput
    {
        public Guid Id { get; set; }
        public required IdentifierDocumentDto Document { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required Gender Gender { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid? ResponsibleId { get; set; }
        public MemberModelOutput? Responsible { get; set; }
        public required AddressDto Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsMinor { get; set; }

        public static MemberModelOutput FromMember(DomainEntity.Member member)
        {
            return new MemberModelOutput
            {
                Id = member.Id,
                Document = IdentifierDocumentDto.FromDomain(member.Document),
                FirstName = member.FirstName,
                LastName = member.LastName,
                Gender = member.Gender,
                PhoneNumber = member.PhoneNumber.Value,
                DateOfBirth = member.DateOfBirth,
                Address = AddressDto.FromDomain(member.Address),
                IsActive = member.IsActive,
                CreatedAt = member.CreatedAt,
                IsMinor = member.IsMinor,
                ResponsibleId = member.ResponsibleId,
                Responsible = member.Responsible is not null ? FromMember(member.Responsible) : null
            };
        }
    }
}