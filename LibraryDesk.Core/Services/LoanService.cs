namespace LibraryDesk.Core.Services;

using LibraryDesk.Core;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

public sealed class LoanService
{
    private readonly ILoanRepository _repository;
    private readonly IPricingPolicy _pricing;

    public LoanService(
        ILoanRepository repository,
        IPricingPolicy pricing)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(pricing);

        _repository = repository;
        _pricing = pricing;
    }

    public void Place(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        _repository.Add(loan);
    }

    public Loan? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public IReadOnlyList<Loan> GetAll()
    {
        return _repository.GetAll();
    }

    public decimal TotalOf(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        decimal total = 0m;

        foreach (LoanItem item in loan.Items)
        {
            total += _pricing.PriceOf(item);
        }

        return total;
    }
}
