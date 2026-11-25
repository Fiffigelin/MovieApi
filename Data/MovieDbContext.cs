using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;

public partial class MovieDbContext : DbContext
{
  public MovieDbContext()
  { }

  public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
  { }

  public DbSet<Movie> Movies => Set<Movie>();
  public DbSet<MovieDetails> MovieDetails => Set<MovieDetails>();
  public DbSet<Actor> Actors => Set<Actor>();
  public DbSet<Genre> Genre => Set<Genre>();
  public DbSet<Review> Reviews => Set<Review>();
  public DbSet<MovieGenre> MovieGenre => Set<MovieGenre>();
  public DbSet<MovieActor> MovieActors => Set<MovieActor>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Movie - MovieDetails 1:1
    modelBuilder.Entity<Movie>()
      .HasOne(m => m.MovieDetails)
      .WithOne(d => d.Movie)
      .HasForeignKey<MovieDetails>(d => d.MovieId);

    // Movie - Review 1:1
    modelBuilder.Entity<Movie>()
      .HasMany(m => m.Reviews)
      .WithOne(r => r.Movie)
      .HasForeignKey(r => r.MovieId);

    // Movie - Actor N:M via kopplingstabellen MovieActors
    modelBuilder.Entity<MovieActor>()
      .HasOne(ma => ma.Movie)
      .WithMany(m => m.MovieActors)
      .HasForeignKey(ma => ma.MovieId);

    modelBuilder.Entity<MovieActor>()
      .HasOne(ma => ma.Actor)
      .WithMany(a => a.MovieActors)
      .HasForeignKey(ma => ma.ActorId);

    // Movie - Genre N:M via kopplingstabellen MovieGenre
    modelBuilder.Entity<MovieGenre>()
        .HasKey(mg => new { mg.MovieId, mg.GenreId });

    modelBuilder.Entity<MovieGenre>()
      .HasOne(mg => mg.Movie)
      .WithMany(m => m.MovieGenres)
      .HasForeignKey(mg => mg.MovieId);

    modelBuilder.Entity<MovieGenre>()
      .HasOne(mg => mg.Genre)
      .WithMany(m => m.MovieGenres)
      .HasForeignKey(mg => mg.GenreId);

    // Restriktioner
    modelBuilder.Entity<Review>()
      .ToTable(t => t.HasCheckConstraint(
        "Ok_Review_Range",
        "[Rating] >= 1 AND [Rating] <= 5"
      ));

    modelBuilder.Entity<Genre>()
      .HasIndex(g => g.Name)
      .IsUnique();
  }
}
