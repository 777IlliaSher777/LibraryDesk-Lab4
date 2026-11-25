namespace LibraryDesk.Core.Policies;

using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Legacy;

/// <summary>
/// Визначає політику розрахунку вартості для працівників.
/// </summary>
public sealed class StaffLoanPricingPolicy : ILegacyPricingPolicy
{
    /// <summary>
    /// Обчислює вартість позиції зі знижкою 30 відсотків.
    /// </summary>
    /// <param name="item">Позиція формуляра.</param>
    /// <returns>Вартість позиції зі знижкою.</returns>
    public decimal PriceOf(LegacyLoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item.UnitPrice * item.Quantity * 0.70m;
    }
}
