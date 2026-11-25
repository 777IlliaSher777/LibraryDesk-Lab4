namespace LibraryDesk.Tests;

using LibraryDesk.Core;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Policies;
using LibraryDesk.Core.Repositories;
using LibraryDesk.Core.Services;

public class DomainInvariantTests
{
    [Fact]
    public void CreateLoanItemWithDueDateBeforeCreatedDateThrowsArgumentOutOfRangeException()
    {
        Book book = new(
            "A-100",
            "Кобзар",
            "Тарас Шевченко");

        DateOnly createdDate = new(2026, 3, 20);
        DateOnly dueDate = new(2026, 3, 10);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new LoanItem(book, createdDate, dueDate));
    }

    [Fact]
    public void CancelActiveLoanThrowsInvalidOperationException()
    {
        Reader reader = new(
            1,
            "Іваненко",
            "ivan@example.com");

        Book book = new(
            "A-100",
            "Кобзар",
            "Тарас Шевченко");

        Loan loan = new(1, reader);

        LoanItem item = new(
            book,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20));

        loan.AddItem(item);
        loan.Activate();

        Assert.Throws<InvalidOperationException>(
            () => loan.Cancel());
    }

    [Fact]
    public void CalculateTotalWithInjectedDiscountPolicyAndInMemoryRepositoryReturnsExpectedTotal()
    {
        Reader reader = new(
            1,
            "Іваненко",
            "ivan@example.com");

        Book book = new(
            "A-100",
            "Кобзар",
            "Тарас Шевченко");

        Loan loan = new(1, reader);

        LoanItem item = new(
            book,
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 3, 20));

        loan.AddItem(item);

        ILoanRepository repository = new InMemoryLoanRepository();
        IPricingPolicy pricingPolicy = new DiscountPricingPolicy();

        LoanService service = new(repository, pricingPolicy);

        service.Place(loan);

        decimal total = service.TotalOf(loan);

        Assert.Equal(95m, total);
        Assert.Same(loan, service.GetById(loan.Id));
    }
}
