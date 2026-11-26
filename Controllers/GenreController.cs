
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

  // GET: api/genre
  [HttpGet]
  public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
  {
    var data = await _context.Genre.ToListAsync();

    var result = data.Select(g => MapDtoToEntity(g.Name, g.Id)).ToList();

    return Ok(result);
  }

  // GET: api/genre/{id}
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

  // POST: api/genre
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
        new { id = result.Id },
        result);
  }

  // PUT: api/genre/{id}
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

  // DELETE: api/genre/{id}
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
}
