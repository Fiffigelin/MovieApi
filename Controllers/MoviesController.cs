
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Movie;
using MovieApi.Models;

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
}
