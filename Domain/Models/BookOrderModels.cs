namespace LibraryApi.Domain.Models
{
    public class BookOrderModels
    {
        public Guid BookOrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public OrderModels Order { get; set; }
        public Guid BookId { get; set; }
        public BookModels Book { get; set; }
    }
}