using Core.BaseImplementations;
using ProductCartService.Entities;
using ProductCartService.Repositories.Interfaces;
using ProductCartService.Services.Interfaces;

namespace ProductCartService.Services
{
    public class ProductCartService : BaseService<ProductCart>, IProductCartService
    {
        public ProductCartService(IProductCartRepository baseRepository) : base(baseRepository)
        {

        }
    }
}
