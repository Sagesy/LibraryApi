namespace LibraryApi.Domain.Models
{
    public class BookModels
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string CoverId { get; set; } = string.Empty;

        public IList<BookOrderModels> BookOrders { get; set; } = new List<BookOrderModels>();
    }
}