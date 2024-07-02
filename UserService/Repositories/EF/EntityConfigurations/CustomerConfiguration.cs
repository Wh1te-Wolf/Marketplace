using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Repositories.EF.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();

            builder.Property(n => n.Name)
                .HasMaxLength(128);

            builder.Property(s => s.Surname)
                .HasMaxLength(128);

            builder.Property(e => e.Email)
                .HasMaxLength(128);

            builder.Property(pn => pn.PhoneNumber)
                .HasMaxLength(128);
        }
    }
}
