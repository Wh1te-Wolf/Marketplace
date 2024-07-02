using Database.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using ProductCartService.Entities;
using ProductCartService.Repositories.EF;
using ProductCartService.Repositories.Interfaces;

namespace ProductCartService.Repositories
{
    public class BillRepository : BaseRepository<Bill>, IBillRepository
    {
        public BillRepository(ProductCartServiceContext dbContext) : base(dbContext)
        {

        }
    }
}
