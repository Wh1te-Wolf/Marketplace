using Core.BaseImplementations;
using Core.Interfaces;
using ProductService.Entities;
using ProductService.Repositories.Interfaces;
using ProductService.Services.Interfaces;

namespace ProductService.Services
{
    public class ProductPriceService : BaseService<ProductPrice>, IProductPriceService
    {
        public ProductPriceService(IProductPriceRepository baseRepository) : base(baseRepository)
        {

        }
    }
}
