namespace LibraryDesk.Core.Legacy;

/// <summary>
/// Містить перевірки передумов для формуляра видачі книжок.
/// </summary>
public static class LoanGuards
{
    /// <summary>
    /// Перевіряє основні дані формуляра та читача.
    /// </summary>
    /// <param name="docId">Номер формуляра.</param>
    /// <param name="clientName">Ім'я читача.</param>
    /// <param name="clientEmail">Електронна пошта читача.</param>
    public static void EnsureHeaderValid(
        int docId,
        string? clientName,
        string? clientEmail)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(docId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientName);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientEmail);

        if (!clientEmail.Contains('@'))
        {
            throw new ArgumentException(
                "Електронна пошта має містити символ @.",
                nameof(clientEmail));
        }
    }

    /// <summary>
    /// Перевіряє правильність окремої книжкової позиції.
    /// </summary>
    /// <param name="line">Книжкова позиція формуляра.</param>
    public static void EnsureLineValid(BookRow? line)
    {
        ArgumentNullException.ThrowIfNull(line);
        ArgumentException.ThrowIfNullOrWhiteSpace(line.Sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(line.Quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(line.UnitPrice);
    }

    /// <summary>
    /// Перевіряє список книжкових позицій формуляра.
    /// </summary>
    /// <param name="lines">Список книжкових позицій.</param>
    public static void EnsureLinesValid(
        IReadOnlyList<BookRow>? lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (lines.Count == 0)
        {
            throw new ArgumentException(
                "Формуляр не містить жодної книжкової позиції.",
                nameof(lines));
        }

        foreach (BookRow line in lines)
        {
            EnsureLineValid(line);
        }
    }

    /// <summary>
    /// Перевіряє всі передумови запиту на розрахунок.
    /// </summary>
    /// <param name="request">Запит на розрахунок вартості видачі.</param>
    public static void EnsureRequestValid(TotalRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Client);

        EnsureHeaderValid(
            request.DocumentId,
            request.Client.Name,
            request.Client.Email);

        EnsureLinesValid(request.Lines);

        ArgumentOutOfRangeException.ThrowIfNegative(
            request.DeliveryPrice);
    }
}
