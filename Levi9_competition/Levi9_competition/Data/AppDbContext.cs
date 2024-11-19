using Levi9_competition.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9_competition.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
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

            //modelBuilder.Entity<Player>()
            //    .HasOne(p => p.Team) // Jedan igrač ima jedan tim
            //    .WithMany(t => t.Players) // Tim ima mnogo igrača
            //    .HasForeignKey(p => p.TeamId) // Strani ključ u tabeli Player
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
