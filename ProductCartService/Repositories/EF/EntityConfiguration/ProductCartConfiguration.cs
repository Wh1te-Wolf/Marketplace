using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCartService.Entities;

namespace ProductCartService.Repositories.EF.EntityConfiguration
{
    public class ProductCartConfiguration : IEntityTypeConfiguration<ProductCart>
    {
        public void Configure(EntityTypeBuilder<ProductCart> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();
            builder.HasMany(i => i.Items)
                .WithOne()
                .HasForeignKey(i => i.ProductCartUUID);
        }
    }
}
