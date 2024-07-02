using AutoMapper;
using CommentService.DTO;
using CommentService.Entities;
using CommentService.Services.Interfaces;
using Core.Interfaces;
using MicroServiceBase.BaseImplementations;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers
{
    public class ProductCommentController : BaseCRUDController<ProductComment, ProductCommentDTO>
    {
        public ProductCommentController(IProductCommentService baseService, IMapper mapper, ILogger<ProductCommentController> logger) : base(baseService, mapper, logger)
        {

        }
    }
}
