using Microsoft.EntityFrameworkCore;
using ProductCartService.Entities;
using System.Reflection;

namespace ProductCartService.Repositories.EF
{
    public class ProductCartServiceContext : DbContext
    {
        public ProductCartServiceContext(DbContextOptions<ProductCartServiceContext> options) : base(options)
        {

        }

        public DbSet<ProductCart> ProductCarts { get; set; }

        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
