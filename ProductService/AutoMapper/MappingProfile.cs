using AutoMapper;
using DataModel.DTO;
using ProductService.Entities;

namespace ProductService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<ProductPriceDTO, ProductPrice>().ReverseMap();
        }
    }

}
