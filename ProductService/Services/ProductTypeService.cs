using Core.BaseImplementations;
using Core.Interfaces;
using ProductService.Entities;
using ProductService.Repositories.Interfaces;
using ProductService.Services.Interfaces;

namespace ProductService.Services
{
    public class ProductTypeService : BaseService<ProductType>, IProductTypeService
    {
        public ProductTypeService(IProductTypeRepository baseRepository) : base(baseRepository)
        {

        }
    }
}
