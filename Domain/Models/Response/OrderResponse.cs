namespace LibraryApi.Domain.Response
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public IEnumerable<BookOrderResponse> BookOrders { get; set; }
    }
}