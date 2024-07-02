using AutoMapper;
using UserService.DTO;
using UserService.Entities;

namespace UserService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDTO, Customer>().ReverseMap();
        }
    }
}
