
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
public class MovieDetailsController : ControllerBase
{
  private readonly MovieDbContext _context;

  public MovieDetailsController(MovieDbContext context)
  {
    _context = context;
  }

  // GET: api/moviedetails
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MovieDetails>>> GetMovieDetails()
  {
    var movieDetails = await _context.MovieDetails.ToListAsync();

    return Ok(movieDetails);
  }

  // GET: api/moviedetails/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<MovieDetails>> GetMovieDetail(Guid id)
  {
    var movieDetail = await _context.MovieDetails
        .FirstOrDefaultAsync(m => m.Id == id);

    if (movieDetail == null)
    {
      return NotFound();
    }

    return Ok(movieDetail);
  }

  // POST: api/moviedetails
  [HttpPost]
  public async Task<ActionResult<MovieDetails>> CreateMovieDetail(MovieDetails movieDetail)
  {
    movieDetail.Id = Guid.NewGuid();

    _context.MovieDetails.Add(movieDetail);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetMovieDetail),
        new { id = movieDetail.Id },
        movieDetail);
  }

  // PUT: api/moviedetails/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateMovieDetail(Guid id, MovieDetails movieDetail)
  {
    if (id != movieDetail.Id)
    {
      return BadRequest();
    }

    var existingMovieDetail = await _context.MovieDetails.FindAsync(id);

    if (existingMovieDetail == null)
    {
      return NotFound();
    }

    existingMovieDetail.Synopsis = movieDetail.Synopsis;
    existingMovieDetail.Language = movieDetail.Language;
    existingMovieDetail.Budget = movieDetail.Budget;
    existingMovieDetail.MovieId = movieDetail.MovieId;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/moviedetails/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteMovieDetail(Guid id)
  {
    var movieDetail = await _context.MovieDetails.FindAsync(id);

    if (movieDetail == null)
    {
      return NotFound();
    }

    _context.MovieDetails.Remove(movieDetail);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
