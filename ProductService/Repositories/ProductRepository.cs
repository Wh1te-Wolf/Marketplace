using Database.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using ProductService.Repositories.EF;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ProductServiceContext _productServiceContext;

        public ProductRepository(ProductServiceContext dbContext) : base(dbContext)
        {
            _productServiceContext = dbContext;
        }

        public override async Task<IReadOnlyCollection<Product>> GetAllAsync(IEnumerable<string>? toInclude = null)
        {
            return await _productServiceContext.Products
                .Include(p => p.Prices).ToListAsync();
        }

        public override async Task<Product?> GetAsync(Guid uuid, IEnumerable<string>? toInclude = null)
        {
            return await _productServiceContext.Products
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.UUID == uuid);
        }

        public override async Task<IReadOnlyCollection<Product>> GetAsync(IEnumerable<Guid>? uuids, IEnumerable<string>? toInclude = null)
        {
            return await _productServiceContext.Products
                .Where(e => uuids.Contains(e.UUID))
                .Include(p => p.Prices)
                .ToListAsync();
        }
    }
}
