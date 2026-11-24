namespace LibraryDesk.Core;

using LibraryDesk.Core.Domain;

/// <summary>
/// Формуляр видачі книг читачеві.
/// </summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    public Loan(int id, Reader reader)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentNullException.ThrowIfNull(reader);

        Id = id;
        Reader = reader;
        CreatedAt = DateTimeOffset.Now;
        Status = LoanStatus.New;
    }

    public int Id { get; }

    public Reader Reader { get; }

    public DateTimeOffset CreatedAt { get; }

    public LoanStatus Status { get; private set; }

    public IReadOnlyList<LoanItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Додає позицію до нового формуляра.
    /// </summary>
    /// <param name="item">Позиція формуляра, яку потрібно додати.</param>
    public void AddItem(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Status != LoanStatus.New)
        {
            throw new InvalidOperationException(
                "Додавати позиції можна лише до нового документа.");
        }

        foreach (LoanItem existingItem in _items)
        {
            if (existingItem.Book.InventoryNumber == item.Book.InventoryNumber)
            {
                throw new InvalidOperationException(
                    "Цей примірник уже є у формулярі.");
            }
        }

        _items.Add(item);
    }

    /// <summary>
    /// Активує формуляр видачі.
    /// </summary>
    public void Activate()
    {
        if (Status != LoanStatus.New)
        {
            throw new InvalidOperationException(
                "Неможливо активувати документ із поточного стану.");
        }

        if (_items.Count == 0)
        {
            throw new InvalidOperationException(
                "Не можна активувати порожній документ.");
        }

        if (!Reader.IsActive)
        {
            throw new InvalidOperationException(
                "Неактивний читач не може отримати книги.");
        }

        foreach (LoanItem item in _items)
        {
            if (!item.Book.IsAvailable)
            {
                throw new InvalidOperationException(
                    "Один або декілька примірників недоступні.");
            }
        }

        foreach (LoanItem item in _items)
        {
            item.Book.IsAvailable = false;
        }

        ChangeStatus(LoanStatus.Active);
    }

    /// <summary>
    /// Скасовує новий формуляр.
    /// </summary>
    public void Cancel()
    {
        ChangeStatus(LoanStatus.Cancelled);
    }

    /// <summary>
    /// Реєструє повернення примірника.
    /// </summary>
    /// <param name="inventoryNumber">Інвентарний номер примірника, який повертається.</param>
    public void ReturnItem(string inventoryNumber)
    {
        if (Status != LoanStatus.Active &&
            Status != LoanStatus.PartiallyReturned)
        {
            throw new InvalidOperationException(
                "Повернення можливе лише для активного документа.");
        }

        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            throw new ArgumentException(
                "Інвентарний номер не може бути порожнім.",
                nameof(inventoryNumber));
        }

        int itemIndex = _items.FindIndex(
            item => item.Book.InventoryNumber == inventoryNumber);

        if (itemIndex < 0)
        {
            throw new ArgumentException(
                "Позицію не знайдено у формулярі.",
                nameof(inventoryNumber));
        }

        LoanItem currentItem = _items[itemIndex];

        if (currentItem.IsReturned)
        {
            throw new InvalidOperationException(
                "Цей примірник уже повернуто.");
        }

        _items[itemIndex] = currentItem with { IsReturned = true };
        currentItem.Book.IsAvailable = true;

        bool allReturned = true;

        foreach (LoanItem item in _items)
        {
            if (!item.IsReturned)
            {
                allReturned = false;
                break;
            }
        }

        ChangeStatus(
            allReturned
                ? LoanStatus.Returned
                : LoanStatus.PartiallyReturned);
    }

    /// <summary>
    /// Перевіряє та виконує дозволений перехід стану.
    /// </summary>
    private void ChangeStatus(LoanStatus newStatus)
    {
        bool isAllowed =
            (Status == LoanStatus.New &&
                (newStatus == LoanStatus.Active ||
                 newStatus == LoanStatus.Cancelled))
            ||
            (Status == LoanStatus.Active &&
                (newStatus == LoanStatus.PartiallyReturned ||
                 newStatus == LoanStatus.Returned))
            ||
            (Status == LoanStatus.PartiallyReturned &&
                newStatus == LoanStatus.Returned);

        if (!isAllowed)
        {
            throw new InvalidOperationException(
                "Недозволений перехід стану документа.");
        }

        Status = newStatus;
    }
}
