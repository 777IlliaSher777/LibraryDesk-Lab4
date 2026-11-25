namespace LibraryDesk.Core.Services;

using System.Globalization;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Legacy;

/// <summary>
/// Виконує операції зі старими формулярами.
/// </summary>
public sealed class LegacyLoanService
{
    private readonly ILegacyLoanRepository _repository;
    private readonly ILegacyPricingPolicy _pricingPolicy;
    private readonly INotifier _notifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyLoanService"/> class.
    /// </summary>
    /// <param name="repository">Репозиторій формулярів.</param>
    /// <param name="pricingPolicy">Політика розрахунку вартості.</param>
    /// <param name="notifier">Сервіс повідомлень.</param>
    public LegacyLoanService(
        ILegacyLoanRepository repository,
        ILegacyPricingPolicy pricingPolicy,
        INotifier notifier)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(pricingPolicy);
        ArgumentNullException.ThrowIfNull(notifier);

        _repository = repository;
        _pricingPolicy = pricingPolicy;
        _notifier = notifier;
    }

    /// <summary>
    /// Обробляє формуляр, розраховує його вартість і зберігає результат.
    /// </summary>
    /// <param name="loan">Формуляр для обробки.</param>
    /// <returns>Загальна вартість формуляра.</returns>
    public decimal Place(LegacyLoan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        decimal total = 0m;

        foreach (LegacyLoanItem item in loan.Items)
        {
            total += _pricingPolicy.PriceOf(item);
        }

        if (total > 1000m)
        {
            total -= 50m;
        }

        loan.Total = total;
        loan.Status = 1;

        _repository.Save(loan, "loans.json");

        _notifier.Notify(
            loan.Email,
            string.Concat("Формуляр ", loan.Id),
            total.ToString(CultureInfo.InvariantCulture));

        return total;
    }

    /// <summary>
    /// Створює архівну копію формуляра та зберігає її.
    /// </summary>
    /// <param name="loan">Формуляр для архівування.</param>
    public void Archive(LegacyLoan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        ArchivedLoan archivedLoan = new(
            loan,
            DateOnly.FromDateTime(DateTime.Today));

        _repository.SaveArchive(archivedLoan, "archive.json");
    }
}
