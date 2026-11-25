namespace LibraryDesk.Core.Legacy;

/// <summary>
/// Представляє архівну копію формуляра.
/// </summary>
public sealed class ArchivedLoan
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArchivedLoan"/> class.
    /// </summary>
    /// <param name="loan">Формуляр для архівування.</param>
    /// <param name="archivedAt">Дата архівування.</param>
    public ArchivedLoan(LegacyLoan loan, DateOnly archivedAt)
    {
        ArgumentNullException.ThrowIfNull(loan);

        Loan = loan;
        ArchivedAt = archivedAt;
    }

    /// <summary>
    /// Gets the archived loan.
    /// </summary>
    public LegacyLoan Loan { get; }

    /// <summary>
    /// Gets the archive date.
    /// </summary>
    public DateOnly ArchivedAt { get; }
}
