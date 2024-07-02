using CommentService.Entities;
using CommentService.Repositories;
using CommentService.Repositories.Interfaces;
using CommentService.Services.Interfaces;
using Core.BaseImplementations;
using Core.Interfaces;
using Database.BaseImplementations;

namespace CommentService.Services
{
    public class ProductCommentService : BaseService<ProductComment>, IProductCommentService
    {
        public ProductCommentService(IProductCommentRepository baseRepository) : base(baseRepository)
        {

        }
    }
}
