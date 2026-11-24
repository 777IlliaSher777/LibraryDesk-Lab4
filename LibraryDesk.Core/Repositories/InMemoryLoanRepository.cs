namespace LibraryDesk.Core.Repositories;

using LibraryDesk.Core;
using LibraryDesk.Core.Abstractions;

public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly Dictionary<int, Loan> _loans = new();

    public void Add(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        if (_loans.ContainsKey(loan.Id))
        {
            throw new InvalidOperationException(
                "Формуляр із таким ідентифікатором уже існує.");
        }

        _loans.Add(loan.Id, loan);
    }

    public Loan? GetById(int id)
    {
        _loans.TryGetValue(id, out Loan? loan);

        return loan;
    }

    public IReadOnlyList<Loan> GetAll()
    {
        return _loans.Values.ToList();
    }
}
