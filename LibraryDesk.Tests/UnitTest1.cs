namespace LibraryDesk.Tests;

using LibraryDesk.Core;
using LibraryDesk.Core.Domain;

public class UnitTest1
{
    [Fact]
    public void ActivateWithAvailableBookChangesStatusToActive()
    {
        Reader reader = new(1, "Іваненко", "ivan@example.com");
        Book book = new("A-100", "Кобзар", "Тарас Шевченко");

        Loan loan = new(1, reader);

        LoanItem item = new(
            book,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20));

        loan.AddItem(item);
        loan.Activate();

        Assert.Equal(LoanStatus.Active, loan.Status);
        Assert.False(book.IsAvailable);
    }

    [Fact]
    public void AddItemAfterActivationThrowsException()
    {
        Reader reader = new(1, "Іваненко", "ivan@example.com");
        Book book = new("A-100", "Кобзар", "Тарас Шевченко");

        Loan loan = new(1, reader);

        loan.AddItem(new LoanItem(
            book,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20)));

        loan.Activate();

        Book anotherBook = new(
            "A-200",
            "Лісова пісня",
            "Леся Українка");

        LoanItem anotherItem = new(
            anotherBook,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20));

        Assert.Throws<InvalidOperationException>(
            () => loan.AddItem(anotherItem));
    }

    [Fact]
    public void ReturnOneOfTwoBooksChangesStatusToPartiallyReturned()
    {
        Reader reader = new(1, "Іваненко", "ivan@example.com");

        Book firstBook = new(
            "A-100",
            "Кобзар",
            "Тарас Шевченко");

        Book secondBook = new(
            "A-200",
            "Лісова пісня",
            "Леся Українка");

        Loan loan = new(1, reader);

        loan.AddItem(new LoanItem(
            firstBook,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20)));

        loan.AddItem(new LoanItem(
            secondBook,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20)));

        loan.Activate();
        loan.ReturnItem("A-100");

        Assert.Equal(LoanStatus.PartiallyReturned, loan.Status);
        Assert.True(firstBook.IsAvailable);
        Assert.False(secondBook.IsAvailable);
    }

    [Fact]
    public void ReturnAllBooksChangesStatusToReturned()
    {
        Reader reader = new(1, "Іваненко", "ivan@example.com");

        Book book = new(
            "A-100",
            "Кобзар",
            "Тарас Шевченко");

        Loan loan = new(1, reader);

        loan.AddItem(new LoanItem(
            book,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20)));

        loan.Activate();
        loan.ReturnItem("A-100");

        Assert.Equal(LoanStatus.Returned, loan.Status);
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void CancelNewLoanChangesStatusToCancelled()
    {
        Reader reader = new(1, "Іваненко", "ivan@example.com");

        Loan loan = new(1, reader);

        loan.Cancel();

        Assert.Equal(LoanStatus.Cancelled, loan.Status);
    }
}
