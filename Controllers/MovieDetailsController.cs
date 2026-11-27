
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

  /// <summary>
  /// Hämtar alla filmdetaljer som finns registrerade.
  /// </summary>
  /// <returns>En lista med filmdetaljer, inklusive synopsis, språk och budget.</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieDetails()
  {
    var data = await _context.MovieDetails.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar filmdetaljer baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för filmdetaljerna som ska hämtas.</param>
  /// <returns>Filmens synopsis, språk och budget.</returns>
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

  /// <summary>
  /// Skapar detaljerad information för en befintlig film.
  /// </summary>
  /// <param name="movieId">ID för filmen som filmdetaljerna ska kopplas till.</param>
  /// <param name="dto">Filmens synopsis, språk och budget.</param>
  /// <returns>De skapade filmdetaljerna.</returns>
  [HttpPost("~/api/movies/{movieId}/moviedetails")]
  public async Task<ActionResult<MovieDetailsDto>> CreateMovieDetail(Guid movieId, CreateMovieDetailsDto dto)
  {
    var movie = await _context.Movies
        .FirstOrDefaultAsync(m => m.Id == movieId);

    if (movie is null)
    {
      return NotFound();
    }

    if (movie.MovieDetails is not null)
    {
      return Conflict("This movie already has details");
    }

    var data = MapDtoToEntity(movieId, dto);

    _context.MovieDetails.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetMovieDetail),
        new { id = data.Id },
        result);
  }

  /// <summary>
  /// Uppdaterar filmdetaljerna för en befintlig film.
  /// </summary>
  /// <param name="id">ID för filmdetaljerna som ska uppdateras.</param>
  /// <param name="dto">De nya uppgifterna om filmens synopsis, språk och budget.</param>
  /// <returns>204 om uppdateringen lyckades, annars 404 om filmdetaljerna inte hittades.</returns>
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

  /// <summary>
  /// Tar bort filmdetaljer baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för filmdetaljerna som ska tas bort.</param>
  /// <returns>204 om filmdetaljerna togs bort, annars 404 om de inte hittades.</returns>
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
