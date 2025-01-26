using Microsoft.AspNetCore.Http;

namespace LibraryApi.Domain.Request
{
    public class ModifyOrderRequest : OrderRequest
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}