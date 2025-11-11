using Conectus.Members.Domain.Entity;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Conectus.Members.Domain.SeedWork;

namespace Conectus.Members.Domain.Repository
{
    public interface IMemberRepository 
        : IGenericRepository<Member>,
        ISearchableRepository<Member>
    {
    }
}
