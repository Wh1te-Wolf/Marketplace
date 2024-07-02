using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CommentService.Entities;

namespace CommentService.Repositories.EF
{
    public class CommentServiceContext : DbContext
    {
        public CommentServiceContext(DbContextOptions<CommentServiceContext> options) : base(options)
        {

        }

        public DbSet<ProductComment> ProductComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
