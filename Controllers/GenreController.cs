
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Genre;
using MovieApi.Models;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
  private readonly MovieDbContext _context;

  public GenreController(MovieDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Hämtar alla tillgängliga filmgenrer.
  /// </summary>
  /// <returns>En lista med samtliga genrer.</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
  {
    var data = await _context.Genre.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar en genre baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för genren som ska hämtas.</param>
  /// <returns>Genrens ID och namn.</returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<GenreDto>> GetGenre(Guid id)
  {
    var data = await _context.Genre
        .FirstOrDefaultAsync(a => a.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  /// <summary>
  /// Skapar en ny filmgenre. Genrenamnet måste vara unikt.
  /// </summary>
  /// <param name="dto">Den nya genrens namn.</param>
  /// <returns>Den skapade genren.</returns>
  [HttpPost]
  public async Task<ActionResult<GenreDto>> CreateGenre(CreateGenreDto dto)
  {
    bool isUnique = await IsNameUnique(dto.Name);

    if (isUnique is false)
    {
      return BadRequest(new { message = "A genre with this name already exists." });
    }

    var data = MapDtoToEntity(name: dto.Name);

    _context.Genre.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetGenre),
        new { id = data.Id },
        result);
  }

  /// <summary>
  /// Uppdaterar namnet på en befintlig genre.
  /// </summary>
  /// <param name="id">ID för genren som ska uppdateras.</param>
  /// <param name="dto">Genrens nya namn.</param>
  /// <returns>204 om uppdateringen lyckades, annars 404 om genren inte hittades.</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateGenre(Guid id, UpdateGenreDto dto)
  {

    var data = await _context.Genre.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.Name = dto.Name;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Tar bort en genre baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för genren som ska tas bort.</param>
  /// <returns>204 om genren togs bort, annars 404 om den inte hittades.</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteGenre(Guid id)
  {
    var data = await _context.Genre.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    _context.Genre.Remove(data);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Hämtar alla genrer som är kopplade till en specifik film.
  /// </summary>
  /// <param name="id">ID för filmen vars genrer ska hämtas.</param>
  /// <returns>En lista med filmens genrer.</returns>
  [HttpGet("~/api/movies/{id}/genre")]
  public async Task<ActionResult<GenreDto>> GetGenreByMovie(Guid id)
  {
    var data = await _context.MovieGenre.AsNoTracking().Where(g => g.MovieId == id).Include(g => g.Genre).ToListAsync();

    if (data.Count == 0)
    {
      return NotFound();
    }

    var result = data.Select(d => MapEntityToDto(d.Genre)).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Kopplar en befintlig genre till en film.
  /// En genre kan inte kopplas till samma film flera gånger.
  /// </summary>
  /// <param name="movieId">ID för filmen som genren ska kopplas till.</param>
  /// <param name="genreId">ID för genren som ska kopplas till filmen.</param>
  /// <returns>204 om kopplingen skapades, annars 409 om genren redan är kopplad.</returns>
  [HttpPost("~/api/movie/{movieId}/genre/{genreId}")]
  public async Task<ActionResult<GenreDto>> AddGenreToMovie(Guid movieId, Guid genreId)
  {
    if (await IsAvailableToMovie(movieId, genreId))
    {
      return Conflict("This genre is already connected to the movie");
    }

    // todo later: check if movie exists by Id
    // jag kommer komma på en massa saker att ändra
    // varför satt jag så länge och funderade? XD 
    // hade inte tänkt färdigt ändå XD

    var data = new MovieGenre()
    {
      MovieId = movieId,
      GenreId = genreId
    };

    _context.MovieGenre.Add(data);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private static Genre MapDtoToEntity(string name, Guid? id = null)
  {
    return new Genre()
    {
      Id = id ?? Guid.NewGuid(),
      Name = name
    };
  }

  private static GenreDto MapEntityToDto(Genre genre)
  {
    return new GenreDto()
    {
      Id = genre.Id,
      Name = genre.Name
    };
  }

  private async Task<bool> IsNameUnique(string name)
  {
    return !await _context.Genre.AnyAsync(g => g.Name == name);
  }

  private async Task<bool> IsAvailableToMovie(Guid movieId, Guid genreId)
  {
    return await _context.MovieGenre.AnyAsync(g =>
        g.MovieId == movieId && g.GenreId == genreId);
  }
}
