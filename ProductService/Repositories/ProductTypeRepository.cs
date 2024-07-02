using Database.BaseImplementations;
using ProductService.Entities;
using ProductService.Repositories.EF;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories
{
    public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(ProductServiceContext dbContext) : base(dbContext)
        {

        }
    }
}
