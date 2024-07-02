using AutoMapper;
using CommentService.DTO;
using CommentService.Entities;

namespace CommentService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCommentDTO, ProductComment>().ReverseMap();
        }
    }
}
