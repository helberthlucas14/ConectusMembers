using Conectus.Members.Application.Interfaces;
using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Repository;
using Conectus.Members.Domain.ValueObject;
using EntityDomain = Conectus.Members.Domain.Entity;

namespace Conectus.Members.Application.UseCases.Member.CreateMember
{
    public class CreateMember : ICreateMember
    {
        private readonly IMemberRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMember(
            IMemberRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

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

            await _repository.Insert(member, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return await Task.FromResult(MemberModelOutput.FromMember(member));
        }
        private PhoneNumber PhoneNumberToDomain(string phoneNumber) 
            => new PhoneNumber(phoneNumber);
    }
}
