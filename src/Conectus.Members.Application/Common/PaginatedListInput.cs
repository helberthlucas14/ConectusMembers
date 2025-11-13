using Conectus.Members.Domain.Repository.SearchableRepository;

namespace Conectus.Members.Application.Common
{
    public abstract class PaginatedListInput
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public SearchOrder Dir { get; set; }
        public FilterBy FiterBy { get; set; } = FilterBy.None;
        public PaginatedListInput(
            int page,
            int perPage,
            string search,
            string sort,
            SearchOrder dir,
            FilterBy filterBy)
        {
            Page = page;
            PerPage = perPage;
            Search = search;
            Sort = sort;
            Dir = dir;
            FiterBy = filterBy;
        }

        public SearchInput ToSearchInput()
            => new(Page, PerPage, Search, Sort, Dir, FiterBy);
    }
}
