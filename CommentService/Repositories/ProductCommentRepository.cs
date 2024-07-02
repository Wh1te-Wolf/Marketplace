using CommentService.Entities;
using CommentService.Repositories.EF;
using CommentService.Repositories.Interfaces;
using Database.BaseImplementations;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Repositories
{
    public class ProductCommentRepository : BaseRepository<ProductComment>, IProductCommentRepository
    {
        public ProductCommentRepository(CommentServiceContext dbContext) : base(dbContext)
        {

        }
    }
}
