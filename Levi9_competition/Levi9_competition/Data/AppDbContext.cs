using Levi9_competition.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9_competition.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<Match> Matches { get; set; } = default!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Player>()
                .Property(p => p.Id)
                .HasMaxLength(100);

            modelBuilder.Entity<Team>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Team>()
                .Property(t => t.Id)
                .HasMaxLength(100);

            modelBuilder.Entity<Match>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Match>()
                .Property(m => m.Id)
                .HasMaxLength(100);
        }
    }
}
