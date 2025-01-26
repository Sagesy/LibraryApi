using Microsoft.AspNetCore.Http;

namespace LibraryApi.Domain.Request
{
    public class BookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public IEnumerable<IFormFile>? fileData { get; set; }
    }
}