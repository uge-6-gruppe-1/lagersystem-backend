using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record OrderLineDto
  {
    [Required]
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
  }
}