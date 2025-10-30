using System.ComponentModel.DataAnnotations;
using Backend.Domain.Enums;

namespace Backend.Application.DTOs
{
  public record QuantityChangeDto(
    [Required] AdjustmentType operation,
    [Required] int amount
  )
  { }
}