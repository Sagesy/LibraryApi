using AutoMapper;
using LibraryApi.Domain.Models;
using LibraryApi.Domain.Request;

namespace LibraryApi.Application.Mapping
{
    public class RequestToModelProfile : Profile
    {
        public RequestToModelProfile()
        {
            CreateMap<BookRequest, BookModels>();

            CreateMap<ModifyBookRequest, BookModels>();
            CreateMap<ModifyBookRequest, BookRequest>();
            CreateMap<OrderRequest, OrderModels>();
        }
    }
}