using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TwoPhaseCommitUtils.Entities;

namespace TwoPhaseCommitUtils.Repository
{
    public class TwoPhaseCommitContext : DbContext
    {
        public TwoPhaseCommitContext(DbContextOptions<TwoPhaseCommitContext> options) : base(options)
        {

        }

        public TwoPhaseCommitContext()
        {
            
        }

        public DbSet<LockData> LockDatas { get; set; }

        public static string ConnectionString { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LockData>().HasKey(ld => ld.UUID);
            builder.Entity<LockData>().Property(ld => ld.Type).HasMaxLength(128);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}
