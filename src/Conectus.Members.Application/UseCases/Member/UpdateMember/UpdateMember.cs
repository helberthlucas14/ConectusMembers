using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.ValueObject;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.Application.UseCases.Member.UpdateMember
{
    public class UpdateMember : IUpdateMember
    {
        public async Task<MemberModelOutput> Handle(
            UpdateMemberInput input,
            CancellationToken cancellationToken)
        {
            var member = new DomainEntity.Member(
                  input.FirstName,
                  input.LastName,
                  input.DateOfBirth,
                  input.Gender,
                  PhoneNumberToDomain(input.PhoneNumber),
                  IdentifierDocumentDto.ToDomain(input.Document),
                  AddressDto.ToDomain(input.Address),
                  input.ResponsibleId);

            member.Update
                (
                  input.FirstName,
                  input.LastName,
                  input.DateOfBirth,
                  input.Gender,
                  PhoneNumberToDomain(input.PhoneNumber),
                  IdentifierDocumentDto.ToDomain(input.Document),
                  AddressDto.ToDomain(input.Address),
                  input.IsActive,
                  input.ResponsibleId
                );

            return await Task.FromResult(MemberModelOutput.FromMember(member));
        }

        private PhoneNumber PhoneNumberToDomain(string phoneNumber)
            => new PhoneNumber(phoneNumber);
    }
}
