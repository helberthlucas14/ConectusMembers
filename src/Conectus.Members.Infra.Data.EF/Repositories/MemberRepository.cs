using Conectus.Members.Application.Exceptions;
using Conectus.Members.Domain.Entity;
using Conectus.Members.Domain.Repository;
using Conectus.Members.Domain.Repository.SearchableRepository;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Conectus.Members.Infra.Data.EF.Repositories;
public class MemberRepository
    : IMemberRepository
{
    private readonly ConectusMemberDbContext _context;
    private DbSet<Member> _members => _context.Set<Member>();
    public MemberRepository(ConectusMemberDbContext context)
        => _context = context;

    public async Task Insert(Member aggregate, CancellationToken cancellationToken)
        => await _members.AddAsync(aggregate, cancellationToken);

    public Task Delete(Member aggregate, CancellationToken _)
        => Task.FromResult(_members.Remove(aggregate));

    public async Task<Member> Get(Guid id, CancellationToken cancellationToken)
    {
        var Member = await _members.AsNoTracking()
            .Include(m => m.Responsible)
            .FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(Member, $"Member '{id}' not found.");
        return Member!;
    }

    public async Task<SearchOutput<Member>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _members.Include(m => m.Responsible).AsNoTracking();
        
        query = AddToQuery(query, input);
        
        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();
        var count = await query.CountAsync();
        return new SearchOutput<Member>(
            input.Page,
            input.PerPage,
            count,
            items.AsReadOnly()
        );
    }

    private IQueryable<Member> AddToQuery(IQueryable<Member> query, SearchInput input)
    {
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (input.SearchBy != FilterBy.None &&
            !string.IsNullOrWhiteSpace(input.Search))
            query = AppendFilterBy(query, input.SearchBy, input.Search);
        else if (!string.IsNullOrWhiteSpace(input.Search))
            query = query.Include(m => m.Responsible)
                         .Where(x => x.FirstName.Contains(input.Search));
        return query;
    }

    private IQueryable<Member> AppendFilterBy(IQueryable<Member> query, FilterBy searchBy, string search)
    {
        var seachByQuery = (searchBy) switch
        {
            FilterBy.FirstName => query.Include(m => m.Responsible)
                    .Where(x => x.FirstName.Contains(search)),
            FilterBy.LastName => query.Include(m => m.Responsible)
                    .Where(x => x.LastName.Contains(search)),
            FilterBy.Document => query.Include(m => m.Responsible)
                    .Where(x => x.Document.Document.Contains(search)),
            FilterBy.None or _ => query.Include(m => m.Responsible),
        };
        return seachByQuery;
    }

    public Task Update(Member aggregate, CancellationToken _)
        => Task.FromResult(_members.Update(aggregate));

    private IQueryable<Member> AddOrderToQuery(
        IQueryable<Member> query,
        string orderProperty,
        SearchOrder order
    )
    {
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("identifierDocument", SearchOrder.Asc) => query.OrderBy(x => x.Document.Document)
                .ThenBy(x => x.Id),
            ("identifierDocument", SearchOrder.Desc) => query.OrderByDescending(x => x.Document.Document)
                .ThenByDescending(x => x.Id),
            ("firstName", SearchOrder.Asc) => query.OrderBy(x => x.FirstName)
                .ThenBy(x => x.Id),
            ("firstName", SearchOrder.Desc) => query.OrderByDescending(x => x.FirstName)
                .ThenByDescending(x => x.Id),
            ("lastName", SearchOrder.Asc) => query.OrderBy(x => x.LastName)
                .ThenBy(x => x.Id),
            ("lastName", SearchOrder.Desc) => query.OrderByDescending(x => x.LastName)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.FirstName)
                .ThenBy(x => x.Id)
        };
        return orderedQuery;
    }

}
