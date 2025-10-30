using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mappers
{
  public static class LocationMapper
  {
    public static LocationDto ToDto(this Location location)
    {
      return new LocationDto(
        location.Id,
        location.Name
      );
    }

    public static Location ToEntity(this LocationDto locationDto)
    {
      return new Location
      {
        Id = locationDto.Id,
        Name = locationDto.Name
      };
    }

    public static LocationDto ToDto(this LocationCreateDto locationCreateDto)
    {
      return new LocationDto(
        Guid.NewGuid(),
        locationCreateDto.Name ?? string.Empty
      );
    }

    public static Location ApplyUpdatesToEntity(this LocationUpdateDto updateDto, Location existingLocation)
    {
      existingLocation.Name = updateDto.Name ?? existingLocation.Name;
      return existingLocation;
    }
  }
}