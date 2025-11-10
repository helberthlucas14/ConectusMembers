using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.ValueObject;
using EntityDomain = Conectus.Members.Domain.Entity;

namespace Conectus.Members.Application.UseCases.Member.CreateMember
{
    public class CreateMember : ICreateMember
    {
        public async Task<MemberModelOutput> Handle(CreateMemberInput input, CancellationToken cancellationToken)
        {
            var member = new EntityDomain.Member(
                input.FirstName,
                input.LastName,
                input.DateOfBirth,
                input.Gender,
                PhoneNumberToDomain(input.PhoneNumber),
                IdentifierDocumentDto.ToDomain(input.Document),
                AddressDto.ToDomain(input.Address),
                input.ResponsibleId);

            return await Task.FromResult(MemberModelOutput.FromMember(member));
        }
        private PhoneNumber PhoneNumberToDomain(string phoneNumber) => new PhoneNumber(phoneNumber);
    }
}
