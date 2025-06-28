namespace RestaurationAPI.Models
{
    public class RestaurantQuery
    {
        public string SearchPhraze { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }

    }
}
