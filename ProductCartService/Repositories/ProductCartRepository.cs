using Database.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using ProductCartService.Entities;
using ProductCartService.Repositories.EF;
using ProductCartService.Repositories.Interfaces;

namespace ProductCartService.Repositories
{
    public class ProductCartRepository : BaseRepository<ProductCart>, IProductCartRepository
    {
        private readonly ProductCartServiceContext _context;

        public ProductCartRepository(ProductCartServiceContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override async Task<IReadOnlyCollection<ProductCart>> GetAllAsync(IEnumerable<string>? toInclude = null)
        {
            return await _context.ProductCarts.Include(pc => pc.Items).AsNoTracking().ToListAsync();
        }

        public override async Task<ProductCart?> GetAsync(Guid uuid, IEnumerable<string>? toInclude = null)
        {
            return await _context.ProductCarts.Include(pc => pc.Items).AsNoTracking().FirstOrDefaultAsync(pc => pc.UUID == uuid);
        }

        public override async Task<IReadOnlyCollection<ProductCart>> GetAsync(IEnumerable<Guid>? uuids, IEnumerable<string>? toInclude = null)
        {
            return await _context.ProductCarts
               .Where(e => uuids.Contains(e.UUID))
               .Include(p => p.Items).AsNoTracking()
               .ToListAsync();
        }
    }
}
