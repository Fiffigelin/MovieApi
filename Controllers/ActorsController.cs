
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Actor;
using MovieApi.Models;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
  private readonly MovieDbContext _context;

  public ActorsController(MovieDbContext context)
  {
    _context = context;
  }

  // GET: api/actors
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
  {
    var data = await _context.Actors.ToListAsync();

    var result = data
        .Select(MapEntityToDto)
        .ToList();

    return Ok(result);
  }

  // GET: api/actors/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ActorDto>> GetActor(Guid id)
  {
    var data = await _context.Actors
        .FirstOrDefaultAsync(a => a.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  // POST: api/actors
  [HttpPost]
  public async Task<ActionResult<ActorDto>> CreateActor(CreateActorDto dto)
  {
    var data = MapDtoToEntity(dto);

    _context.Actors.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetActor),
        new { id = result.Id },
        result);
  }

  // PUT: api/actors/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateActor(Guid id, UpdateActorDto dto)
  {

    var data = await _context.Actors.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.Name = dto.Name;
    data.BirthYear = dto.BirthYear;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/actors/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteActor(Guid id)
  {
    var data = await _context.Actors.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    _context.Actors.Remove(data);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private static Actor MapDtoToEntity(CreateActorDto dto)
  {
    return new Actor()
    {
      Id = Guid.NewGuid(),
      Name = dto.Name,
      BirthYear = dto.BirthYear
    };
  }

  private static ActorDto MapEntityToDto(Actor actor)
  {
    return new ActorDto()
    {
      Id = actor.Id,
      Name = actor.Name,
      BirthYear = actor.BirthYear
    };
  }
}
