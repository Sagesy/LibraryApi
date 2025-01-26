using Microsoft.AspNetCore.Http;

namespace LibraryApi.Domain.Request
{
    public class OrderRequest
    {
        public string OrderName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public IEnumerable<Guid> BookIds { get; set; }
    }
}