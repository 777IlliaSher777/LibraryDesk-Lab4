namespace LibraryDesk.Core.Policies;

using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

public sealed class DiscountPricingPolicy : IPricingPolicy
{
    private const decimal _dailyRate = 10m;
    private const decimal _discountRate = 0.05m;

    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        int days = Math.Max(
            1,
            item.DueDate.DayNumber - item.CreatedDate.DayNumber);

        decimal standardPrice = days * _dailyRate;

        return standardPrice * (1m - _discountRate);
    }
}
