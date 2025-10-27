public class Price
{
    string CurrencySymbol { get; set; }
    string FormattedPrice { get; set; }
    double Amount { get; set; }

    public Price(string currencySymbol, double amount)
    {
        CurrencySymbol = currencySymbol;
        Amount = amount;
        FormattedPrice = CurrencySymbol + " " + Amount;
    }
}