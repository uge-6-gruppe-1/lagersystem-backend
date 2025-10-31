using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record CartDto
  {
    [Required]
    public Guid Id { get; init; }

    public Guid? UserId { get; init; }

    public string? Name { get; init; } = string.Empty;

    [Required]
    public IEnumerable<OrderLineDto> Items { get; init; } = Enumerable.Empty<OrderLineDto>();
  }
}