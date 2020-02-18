using Microsoft.EntityFrameworkCore;

namespace Baskid.Core.Models
{
    public class BaskidContext : DbContext
    {
        private readonly string connectionString;

        public BaskidContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public BaskidContext(DbContextOptions<BaskidContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QueryEntry>().HasKey(bc => new { bc.QueryId, bc.EntryId });
            modelBuilder.Entity<QueryEntry>().HasOne(qe => qe.Query).WithMany(e => e.Entries).HasForeignKey(qe => qe.QueryId);
            modelBuilder.Entity<QueryEntry>().HasOne(qe => qe.Entry).WithMany(e => e.Queries).HasForeignKey(qe => qe.EntryId);
        }

        public DbSet<Query> SearchQueries { get; set; }
        public DbSet<Entry> SearchEntries { get; set; }
    }
}