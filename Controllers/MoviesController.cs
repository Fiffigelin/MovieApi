
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
  /// Kod nedanför är för dtos
  /// </summary>

  // GET: api/movies
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
  {
    var data = await _context.Movies.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  // GET: api/movies/{id}
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

  // POST: api/movies
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
        data);
  }

  // PUT: api/movies/{id}
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

  // DELETE: api/movies/{id}
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
  /// Kod nedanför är för utökad funktionalitet
  /// </summary>


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
        .AsSplitQuery()
        .ToListAsync();

    if (data is null)
    {
      return NotFound();
    }

    var result = data.Select(MapToMovieReviewModel).ToList();

    return Ok(result);
  }

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
      })]
    };
  }
}
