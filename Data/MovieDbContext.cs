using Microsoft.EntityFrameworkCore;

namespace MovieApi.Data;

public partial class MovieDbContext : DbContext
{
  public MovieDbContext()
  { }

  public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
  { }
}
