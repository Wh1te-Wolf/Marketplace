using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.Repositories.EF.EntityConfiguration
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();
        }
    }
}
