namespace Backend.Domain.ValueObjects
{
  public record Price(
      decimal Amount,
      string CurrencySymbol
  )
  { }
}