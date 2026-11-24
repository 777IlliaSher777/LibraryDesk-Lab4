namespace LibraryDesk.Core.Policies;

using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

public sealed class StandardPricingPolicy : IPricingPolicy
{
    private const decimal _dailyRate = 10m;

    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        int days = Math.Max(
            1,
            item.DueDate.DayNumber - item.CreatedDate.DayNumber);

        return days * _dailyRate;
    }
}
