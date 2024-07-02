using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductCartService.Entities;

namespace ProductCartService.Repositories.EF.EntityConfiguration
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();
            builder.HasMany(b => b.Items)
                .WithOne().HasForeignKey(b => b.BillUUID);
        }
    }

}
