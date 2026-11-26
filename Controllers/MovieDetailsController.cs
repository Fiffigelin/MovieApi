
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.MovieDetail;
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
  public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieDetails()
  {
    var data = await _context.MovieDetails.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  // GET: api/moviedetails/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<MovieDetailsDto>> GetMovieDetail(Guid id)
  {
    var data = await _context.MovieDetails
        .FirstOrDefaultAsync(m => m.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  // POST: api/moviedetails
  [HttpPost]
  public async Task<ActionResult<MovieDetailsDto>> CreateMovieDetail(Guid movieId, CreateMovieDetailsDto dto)
  {

    var data = MapDtoToEntity(movieId, dto);

    _context.MovieDetails.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetMovieDetail),
        new { id = data.Id },
        data);
  }

  // PUT: api/moviedetails/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateMovieDetail(Guid id, UpdateMovieDetailsDto dto)
  {

    var data = await _context.MovieDetails.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.Synopsis = dto.Synopsis;
    data.Language = dto.Language;
    data.Budget = dto.Budget;

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

  private static MovieDetailsDto MapEntityToDto(MovieDetails movieDetails)
  {
    return new MovieDetailsDto()
    {
      Id = movieDetails.Id,
      Synopsis = movieDetails.Synopsis,
      Language = movieDetails.Language,
      Budget = movieDetails.Budget,
      MovieId = movieDetails.MovieId
    };
  }
  private static MovieDetails MapDtoToEntity(Guid movieId, CreateMovieDetailsDto movieDetails)
  {
    return new MovieDetails()
    {
      Id = Guid.NewGuid(),
      Synopsis = movieDetails.Synopsis,
      Language = movieDetails.Language,
      Budget = movieDetails.Budget,
      MovieId = movieId
    };
  }
}
