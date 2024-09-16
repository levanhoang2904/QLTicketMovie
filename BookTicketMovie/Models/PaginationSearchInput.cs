namespace BookTicketMovie.Models
{
    public class PaginationSearchInput
    {
        public int Page { get; set; }

        public int PageSize { get; set; } = 0;
        public string searchValue { get; set; } = "";


    }
}
