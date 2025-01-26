namespace LibraryApi.Domain.Models
{
    public class AttachmentModels
    {
        public Guid AttachmentId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string NameFile { get; set; } = string.Empty;
        public string OriginalNameFile { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}