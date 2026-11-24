namespace LibraryDesk.Core.Abstractions;

using LibraryDesk.Core;

public interface ILoanRepository
{
    void Add(Loan loan);

    Loan? GetById(int id);

    IReadOnlyList<Loan> GetAll();
}
