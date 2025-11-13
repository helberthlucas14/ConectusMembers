namespace Conectus.Members.Domain.Repository.SearchableRepository;

public enum FilterBy
{
    None,
    Document,
    FirstName,
    LastName,
}
public class SearchInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search { get; set; }
    public string OrderBy { get; set; }
    public SearchOrder Order { get; set; }
    public FilterBy SearchBy { get; set; } = FilterBy.None;

    public SearchInput(
        int page,
        int perPage,
        string search,
        string orderBy,
        SearchOrder order,
        FilterBy searchBy)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
        SearchBy = searchBy;
    }
}
