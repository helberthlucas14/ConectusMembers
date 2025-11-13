using Conectus.Members.Application.Interfaces;
using Conectus.Members.Domain.Repository;
using MediatR;

namespace Conectus.Members.Application.UseCases.Member.DeleteMember
{
    public class DeleteMember : IDeleteMember
    {
        private readonly IMemberRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMember(
            IMemberRepository repository,
            IUnitOfWork unitOfWork)
            => (_repository, _unitOfWork) = (repository, unitOfWork);

        public async Task<Unit> Handle(DeleteMemberInput request,
            CancellationToken cancellationToken)
        {
            var member = await _repository.Get(request.Id, cancellationToken);
            await _repository.Delete(member, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return Unit.Value;
        }
    }
}
