namespace LibraryDesk.Core.Abstractions;

using LibraryDesk.Core.Legacy;

/// <summary>
/// Визначає правило розрахунку вартості позиції старого формуляра.
/// </summary>
public interface ILegacyPricingPolicy
{
    /// <summary>
    /// Обчислює вартість позиції з урахуванням політики.
    /// </summary>
    /// <param name="item">Позиція формуляра.</param>
    /// <returns>Розрахована вартість позиції.</returns>
    decimal PriceOf(LegacyLoanItem item);
}
