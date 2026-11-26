using System;
using MovieApi.DTOs.Actor;

namespace MovieApi.ViewModels;

public class MovieActorViewModel
{
  public Guid Id { get; set; }

  public required ActorDto Actor { get; set; }

  public required string Role { get; set; }
}