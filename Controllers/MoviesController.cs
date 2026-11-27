
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Actor;
using MovieApi.DTOs.Movie;
using MovieApi.DTOs.MovieDetail;
using MovieApi.DTOs.Review;
using MovieApi.Models;
using MovieApi.ViewModels;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
  private readonly MovieDbContext _context;

  public MoviesController(MovieDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Hämtar alla filmer. Filmerna kan filtreras efter genre, årtal och skådespelare.
  /// </summary>
  /// <param name="genre">Filtrera filmer efter genre.</param>
  /// <param name="year">Filtrera filmer efter premiärår.</param>
  /// <param name="actor">Filtrera filmer efter skådespelarens namn.</param>
  /// <returns>En lista med filmer som matchar angivna filter.</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MovieDto>>> GetFilteredMovies(
      [FromQuery] string? genre,
      [FromQuery] int? year,
      [FromQuery] string? actor)
  {
    var query = _context.Movies
        .AsNoTracking()
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(genre))
    {
      query = query.Where(m =>
          m.MovieGenres.Any(mg =>
              mg.Genre.Name == genre));
    }

    if (year.HasValue)
    {
      query = query.Where(m => m.Year == year.Value);
    }

    if (!string.IsNullOrWhiteSpace(actor))
    {
      query = query.Where(m =>
          m.MovieActors.Any(ma =>
              ma.Actor.Name.Contains(actor)));
    }

    var data = await query.ToListAsync();

    var result = data
        .Select(MapEntityToDto)
        .ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar en film baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för filmen som ska hämtas.</param>
  /// <returns>Filmens grundläggande information.</returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<MovieDto>> GetMovie(Guid id)
  {
    var data = await _context.Movies
        .FirstOrDefaultAsync(m => m.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  /// <summary>
  /// Skapar en ny film med grundläggande information.
  /// </summary>
  /// <param name="dto">Filmens titel, premiärår och speltid.</param>
  /// <returns>Den skapade filmen.</returns>
  [HttpPost]
  [HttpPost]
  public async Task<ActionResult<MovieDto>> CreateMovie(CreateMovieDto dto)
  {
    var data = MapDtoToEntity(dto);

    _context.Movies.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetMovie),
        new { id = data.Id },
        result);
  }

  /// <summary>
  /// Uppdaterar informationen för en befintlig film.
  /// </summary>
  /// <param name="id">ID för filmen som ska uppdateras.</param>
  /// <param name="dto">Filmens nya titel, premiärår och speltid.</param>
  /// <returns>204 om uppdateringen lyckades, annars 404 om filmen inte hittades.</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto dto)
  {
    var data = await _context.Movies.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.Title = dto.Title;
    data.Year = dto.Year;
    data.Duration = dto.Duration;

    await _context.SaveChangesAsync();

    return NoContent();
  }


  /// <summary>
  /// Tar bort en film baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för filmen som ska tas bort.</param>
  /// <returns>204 om filmen togs bort, annars 404 om filmen inte hittades.</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteMovie(Guid id)
  {
    var data = await _context.Movies.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    _context.Movies.Remove(data);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Hämtar detaljerad information om en film baserat på filmens ID.
  /// </summary>
  /// <param name="id">ID för filmen vars detaljer ska hämtas.</param>
  /// <returns>Filmens synopsis, språk och budget.</returns>
  [HttpGet("~/api/movies/{id}/moviedetails")]
  public async Task<ActionResult<MovieDetailsDto>> GetMovieDetailByMovie(Guid id)
  {
    var data = await _context.MovieDetails.FirstOrDefaultAsync(md => md.MovieId == id);

    if (data is null)
    {
      return NotFound();
    }

    var result = new MovieDetailsDto()
    {
      Id = data.Id,
      Synopsis = data.Synopsis,
      Language = data.Language,
      Budget = data.Budget
    };

    return Ok(result);
  }

  /// <summary>
  /// Hämtar alla kompletta filmer med filmdetaljer, genrer, skådespelare och recensioner.
  /// Endast filmer med minst en skådespelare och tillhörande filmdetaljer inkluderas.
  /// </summary>
  /// <returns>En lista med kompletta filmer och tillhörande information.</returns>
  [HttpGet("~/api/movies-view")]
  public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetMoviesViewModel()
  {
    var data = await _context.Movies
        .AsNoTracking()
        .Where(m => m.MovieDetails != null)
        .Where(m => m.MovieActors.Any())
        .Include(m => m.MovieDetails)
        .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
        .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
        .Include(m => m.Reviews)
        .AsSplitQuery()
        .ToListAsync();

    if (data.Count == 0)
    {
      return NotFound();
    }

    var result = data.Select(MapToMovieReviewModel).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar en komplett film baserat på dess ID, inklusive filmdetaljer, genrer,
  /// skådespelare och recensioner.
  /// </summary>
  /// <param name="id">ID för filmen som ska hämtas.</param>
  /// <returns>Filmen med tillhörande information.</returns>
  [HttpGet("~/api/movies-view/{id}")]
  public async Task<ActionResult<MovieViewModel>> GetMovieViewModel(Guid id)
  {
    var data = await _context.Movies
        .AsNoTracking()
        .Where(m => m.Id == id)
        .Where(m => m.MovieDetails != null)
        .Where(m => m.MovieActors.Any())
        .Include(m => m.MovieDetails)
        .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
        .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
        .Include(m => m.Reviews)
        .AsSplitQuery()
        .ToListAsync();

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapToMovieReviewModel(data[0]));
  }

  private static MovieDto MapEntityToDto(Movie movie)
  {
    return new MovieDto()
    {
      Id = movie.Id,
      Title = movie.Title,
      Year = movie.Year,
      Duration = movie.Duration
    };
  }

  private static Movie MapDtoToEntity(CreateMovieDto dto)
  {
    return new Movie()
    {
      Id = Guid.NewGuid(),
      Title = dto.Title,
      Year = dto.Year,
      Duration = dto.Duration
    };
  }

  private static MovieViewModel MapToMovieReviewModel(Movie movie)
  {
    return new MovieViewModel()
    {
      Id = movie.Id,
      Title = movie.Title,
      Year = movie.Year,
      Duration = movie.Duration,
      Details = new MovieDetailsDto()
      {
        Synopsis = movie.MovieDetails!.Synopsis,
        Language = movie.MovieDetails.Language,
        Budget = movie.MovieDetails.Budget
      },
      Genres = [.. movie.MovieGenres.Select(g => g.Genre.Name)],
      Actors = [.. movie.MovieActors.Select(ma => new MovieActorDto() {
        Id = ma.Id,
        Name = ma.Actor.Name,
        BirthYear = ma.Actor.BirthYear,
        Role = ma.Role
      })],
      Reviews = [.. movie.Reviews.Select(r => new ReviewDto() {
        Id = r.Id,
        ReviewerName = r.ReviewerName,
        Comment = r.Comment,
        Rating = r.Rating
      })]
    };
  }
}
