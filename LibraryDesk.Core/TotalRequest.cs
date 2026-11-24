namespace LibraryDesk.Core;

/// <summary>
/// Містить усі дані, необхідні для розрахунку вартості видачі.
/// </summary>
/// <param name="DocumentId">Номер формуляра.</param>
/// <param name="Client">Інформація про читача.</param>
/// <param name="Lines">Список книжкових позицій.</param>
/// <param name="CreatedAt">Дата створення формуляра.</param>
/// <param name="CouponCode">Код купона або промокоду.</param>
/// <param name="Status">Поточний статус формуляра.</param>
/// <param name="DeliveryPrice">Вартість доставки.</param>
public sealed record TotalRequest(
    int DocumentId,
    ClientInfo Client,
    IReadOnlyList<Legacy.BookRow> Lines,
    DateOnly CreatedAt,
    string? CouponCode,
    LoanStatus Status,
    decimal DeliveryPrice);
