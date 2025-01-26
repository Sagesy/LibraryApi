namespace LibraryApi.Domain.Request
{
    public class ModifyBookRequest : BookRequest
    {
        public Guid BookId { get; set; }
    }
}