using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCartService.Entities;

namespace ProductCartService.Repositories.EF.EntityConfiguration
{
    public class ProductCartItemConfiguration : IEntityTypeConfiguration<ProductCartItem>
    {
        public void Configure(EntityTypeBuilder<ProductCartItem> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();
        }
    }
}
