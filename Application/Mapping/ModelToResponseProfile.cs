using AutoMapper;
using LibraryApi.Application.Service;
using LibraryApi.Domain.Models;
using LibraryApi.Domain.Response;

namespace LibraryApi.Application.Mapping
{
    public class ModelToResponseProfile : Profile
    {
        public ModelToResponseProfile()
        {
            CreateMap<BookModels, BookResponse>()
                .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => new AttachmentService().GetDataById(src.CoverId).Url));


            CreateMap<BookOrderModels, BookOrderResponse>();

            CreateMap<BookResponse, BookOrderResponse>();

            CreateMap<OrderModels, OrderResponse>();
        }
    }
}