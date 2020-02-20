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

        public DbSet<Query> SearchQueries { get; set; }
        public DbSet<Entry> SearchEntries { get; set; }
        public DbSet<EntryReference> EntryReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryReference>().HasKey(bc => new { bc.ParentId, bc.ChildId });
            modelBuilder.Entity<EntryReference>().HasOne(er => er.Parent).WithMany(e => e.References).HasForeignKey(er => er.ParentId);
            modelBuilder.Entity<EntryReference>().HasOne(er => er.Child).WithMany(e => e.References).HasForeignKey(er => er.ChildId);

            modelBuilder.Entity<QueryEntry>().HasKey(bc => new {bc.QueryId, bc.EntryId});
            modelBuilder.Entity<QueryEntry>().HasOne(qe => qe.Query).WithMany(e => e.Entries).HasForeignKey(qe => qe.QueryId);
            modelBuilder.Entity<QueryEntry>().HasOne(qe => qe.Entry).WithMany(e => e.Queries).HasForeignKey(qe => qe.EntryId);
        }
    }
}