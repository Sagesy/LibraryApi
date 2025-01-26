namespace LibraryApi.Domain.Response
{
    public class BookOrderResponse : BookResponse
    {
        public Guid BookOrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}