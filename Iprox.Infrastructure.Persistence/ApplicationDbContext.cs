using Iprox.Domain.Entities;
using Iprox.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iprox.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TvShow> TvShow { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TvShow entity
        modelBuilder.Entity<TvShow>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<TvShow>()
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<TvShow>()
            .Property(t => t.Language)
            .HasMaxLength(50);

        modelBuilder.Entity<TvShow>()
            .HasMany(t => t.Genres)
            .WithMany()
            .UsingEntity(j => j.ToTable("TvShowGenres"));

        modelBuilder.Entity<TvShow>()
            .HasIndex(t => t.TvMazeId)
            .IsUnique();

        modelBuilder.Entity<TvShow>()
            .Property(t => t.TvMazeId)
            .HasDefaultValue(null);

        // Configure Genre entity
        modelBuilder.Entity<Genre>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Genre>()
            .Property(t => t.Id)
            .ValueGeneratedNever(); // Prevent auto-generation of Id

        modelBuilder.Entity<Genre>()
            .Property(t => t.Name)
            .IsRequired();

        // Seed Genre data
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = (int)GenreType.Drama, Name = GenreType.Drama.ToString() },
            new Genre { Id = (int)GenreType.ScienceFiction, Name = GenreType.ScienceFiction.ToString() },
            new Genre { Id = (int)GenreType.Thriller, Name = GenreType.Thriller.ToString() },
            new Genre { Id = (int)GenreType.Action, Name = GenreType.Action.ToString() },
            new Genre { Id = (int)GenreType.Crime, Name = GenreType.Crime.ToString() },
            new Genre { Id = (int)GenreType.Horror, Name = GenreType.Horror.ToString() },
            new Genre { Id = (int)GenreType.Romance, Name = GenreType.Romance.ToString() },
            new Genre { Id = (int)GenreType.Adventure, Name = GenreType.Adventure.ToString() },
            new Genre { Id = (int)GenreType.Other, Name = GenreType.Other.ToString() }
        );
    }

}
