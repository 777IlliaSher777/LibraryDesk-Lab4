namespace LibraryDesk.Core;

/// <summary>
/// Містить результат розрахунку вартості видачі.
/// </summary>
/// <param name="Total">Загальна розрахована сума.</param>
/// <param name="Status">Новий статус формуляра.</param>
public sealed record TotalResult(
    decimal Total,
    LoanStatus Status);
