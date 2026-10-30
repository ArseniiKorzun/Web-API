using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.Property(a => a.Name).HasMaxLength(100).IsRequired();
            entity.Property(a => a.Country).HasMaxLength(60);
            entity.HasIndex(a => a.Name).IsUnique();
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(g => g.Name).HasMaxLength(50).IsRequired();
            entity.HasIndex(g => g.Name).IsUnique();
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(a => a.Title).HasMaxLength(150).IsRequired();
            entity.Property(a => a.Price).HasConversion<double>();

            entity.HasOne(a => a.Artist)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.Genre)
                .WithMany(g => g.Albums)
                .HasForeignKey(a => a.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => new { a.ArtistId, a.Title }).IsUnique();
        });

        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Rock" },
            new Genre { Id = 2, Name = "Pop" },
            new Genre { Id = 3, Name = "Electronic" },
            new Genre { Id = 4, Name = "Hip-Hop" });

        modelBuilder.Entity<Artist>().HasData(
            new Artist { Id = 1, Name = "Queen", Country = "United Kingdom" },
            new Artist { Id = 2, Name = "Daft Punk", Country = "France" },
            new Artist { Id = 3, Name = "Okean Elzy", Country = "Ukraine" },
            new Artist { Id = 4, Name = "Kendrick Lamar", Country = "USA" });

        modelBuilder.Entity<Album>().HasData(
            new Album { Id = 1, Title = "A Night at the Opera", Year = 1975, Price = 19.99m, ArtistId = 1, GenreId = 1 },
            new Album { Id = 2, Title = "News of the World", Year = 1977, Price = 17.49m, ArtistId = 1, GenreId = 1 },
            new Album { Id = 3, Title = "Discovery", Year = 2001, Price = 15.99m, ArtistId = 2, GenreId = 3 },
            new Album { Id = 4, Title = "Random Access Memories", Year = 2013, Price = 22.50m, ArtistId = 2, GenreId = 3 },
            new Album { Id = 5, Title = "Model", Year = 2001, Price = 12.00m, ArtistId = 3, GenreId = 1 },
            new Album { Id = 6, Title = "DAMN.", Year = 2017, Price = 18.99m, ArtistId = 4, GenreId = 4 });
    }
}
