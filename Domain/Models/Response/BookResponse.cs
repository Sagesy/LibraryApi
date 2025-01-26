namespace LibraryApi.Domain.Response
{
    public class BookResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CoverUrl { get; set; } = string.Empty;
    }
}