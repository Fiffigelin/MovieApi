
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
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
    var movies = await _context.Movies.ToListAsync();

    return Ok(movies);
  }

  // GET: api/movies/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Movie>> GetMovie(Guid id)
  {
    var movie = await _context.Movies
        .FirstOrDefaultAsync(m => m.Id == id);

    if (movie == null)
    {
      return NotFound();
    }

    return Ok(movie);
  }

  // POST: api/movies
  [HttpPost]
  public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
  {
    movie.Id = Guid.NewGuid();

    _context.Movies.Add(movie);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetMovie),
        new { id = movie.Id },
        movie);
  }

  // PUT: api/movies/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateMovie(Guid id, Movie movie)
  {
    if (id != movie.Id)
    {
      return BadRequest();
    }

    var existingMovie = await _context.Movies.FindAsync(id);

    if (existingMovie == null)
    {
      return NotFound();
    }

    existingMovie.Title = movie.Title;
    existingMovie.Year = movie.Year;
    existingMovie.Duration = movie.Duration;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/movies/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteMovie(Guid id)
  {
    var movie = await _context.Movies.FindAsync(id);

    if (movie == null)
    {
      return NotFound();
    }

    _context.Movies.Remove(movie);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
