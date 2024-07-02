using Database.BaseImplementations;
using ProductService.Entities;
using ProductService.Repositories.EF;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories
{
    public class ProductPriceRepository : BaseRepository<ProductPrice>, IProductPriceRepository
    {
        public ProductPriceRepository(ProductServiceContext dbContext) : base(dbContext)
        {

        }
    }
}
