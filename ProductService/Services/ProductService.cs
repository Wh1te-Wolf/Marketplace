using Core.BaseImplementations;
using Core.Interfaces;
using ProductService.Entities;
using ProductService.Repositories.Interfaces;
using ProductService.Services.Interfaces;

namespace ProductService.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IProductRepository baseRepository) : base(baseRepository)
        {

        }
    }
}
