namespace Business.Helpers;

public class CalculatePrice
{
    public decimal CalculateTotalPriceAndHours(decimal totalHours, decimal productPrice)
    {
        return totalHours * productPrice;
    }
}

