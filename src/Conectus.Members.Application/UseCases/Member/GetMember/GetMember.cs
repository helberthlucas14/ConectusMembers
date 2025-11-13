using Conectus.Members.Application.Interfaces;
using Conectus.Members.Application.UseCases.Member.Common;
using Conectus.Members.Domain.Repository;

namespace Conectus.Members.Application.UseCases.Member.GetMember
{
    public class GetMember : IGetMember
    {
        private readonly IMemberRepository _repository;

        public GetMember(IMemberRepository repository) =>
            _repository = repository;

        public async Task<MemberModelOutput> Handle(
            GetMemberInput request,
            CancellationToken cancellationToken
            )
        {
            var member = await _repository.Get(
                request.Id,
                cancellationToken);

            return MemberModelOutput.FromMember(member);
        }
    }
}
