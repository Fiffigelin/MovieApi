using System;

namespace MovieApi.DTOs.Actor;

public class ActorDto
{
  public Guid Id { get; set; }

  public required string Name { get; set; }

  public int? BirthYear { get; set; }
}