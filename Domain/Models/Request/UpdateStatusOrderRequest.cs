using Microsoft.AspNetCore.Http;

namespace LibraryApi.Domain.Request
{
    public class UpdateStatusOrderRequest
    {
        // public Guid OrderId { get; set; }

        public IEnumerable<Guid> BookIds { get; set; }
    }
}