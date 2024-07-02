using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using System.Reflection;

namespace ProductService.Repositories.EF
{
    public class ProductServiceContext : DbContext
    {
        public ProductServiceContext(DbContextOptions<ProductServiceContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductPrice> ProductPrices { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
