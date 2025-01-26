namespace LibraryApi.Domain.Models
{
    public class OrderModels
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public IList<BookOrderModels> BookOrders { get; set; } = new List<BookOrderModels>();
    }
}