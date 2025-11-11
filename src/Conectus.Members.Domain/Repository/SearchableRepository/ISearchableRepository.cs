using Conectus.Members.Domain.SeedWork;

namespace Conectus.Members.Domain.Repository.SearchableRepository
{
    public interface ISearchableRepository<TAggregate>
        where TAggregate : AggregateRoot
    {
        Task<SearchOutput<TAggregate>> Search(
            SearchInput input,
            CancellationToken cancellationToken
        );
    }
}
