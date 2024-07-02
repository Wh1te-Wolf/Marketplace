using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommentService.Entities;

namespace CommentService.Repositories.EF.EntityConfiguration
{
    public class ProductCommentConfiguration : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.HasKey(id => id.UUID);
            builder.Property(id => id.UUID).ValueGeneratedOnAdd();
        }
    }

}

