namespace LibraryDesk.Core.Domain;

using LibraryDesk.Core;

/// <summary>
/// Незмінна позиція формуляра видачі книги.
/// </summary>
public sealed record LoanItem
{
    public LoanItem(Book book, DateOnly createdDate, DateOnly dueDate)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (createdDate == default)
        {
            throw new ArgumentException(
                "Дата оформлення не може бути порожньою.",
                nameof(createdDate));
        }

        if (dueDate == default || dueDate < createdDate)
        {
            throw new ArgumentOutOfRangeException(
                nameof(dueDate),
                "Дата повернення не може передувати даті оформлення.");
        }

        Book = book;
        CreatedDate = createdDate;
        DueDate = dueDate;
    }

    public Book Book { get; }

    public DateOnly CreatedDate { get; }

    public DateOnly DueDate { get; }

    public bool IsReturned { get; init; }
}
