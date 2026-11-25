namespace LibraryDesk.Core.Policies;

using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Legacy;

/// <summary>
/// Визначає політику розрахунку вартості для звичайних читачів.
/// </summary>
public sealed class RegularLoanPricingPolicy : ILegacyPricingPolicy
{
    /// <summary>
    /// Обчислює вартість позиції зі знижкою 5 відсотків.
    /// </summary>
    /// <param name="item">Позиція формуляра.</param>
    /// <returns>Вартість позиції зі знижкою.</returns>
    public decimal PriceOf(LegacyLoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item.UnitPrice * item.Quantity * 0.95m;
    }
}
