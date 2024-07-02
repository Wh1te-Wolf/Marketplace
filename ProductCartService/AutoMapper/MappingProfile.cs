using AutoMapper;
using ProductCartService.DTO;
using ProductCartService.Entities;

namespace ProductCartService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BillDTO, Bill>().ReverseMap();
            CreateMap<BillItemDTO, BillItem>().ReverseMap();
            CreateMap<ProductCartDTO, ProductCart>().ReverseMap();
            CreateMap<ProductCartItemDTO, ProductCartItem>().ReverseMap();
        }
    }
}
