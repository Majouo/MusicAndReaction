using Microsoft.EntityFrameworkCore;

namespace MusicAndReaction.Server.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserReaction> UserReactions { get; set; }

    public DbSet<MusicFile> MusicFiles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MusicFile>()
        .HasIndex(m => m.Hash)
        .IsUnique();
    }
}
