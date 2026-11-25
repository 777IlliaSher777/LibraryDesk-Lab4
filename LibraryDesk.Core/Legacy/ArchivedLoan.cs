namespace LibraryDesk.Core.Legacy;

public class ArchivedLoan : LegacyLoan
{
    public ArchivedLoan(int id)
    {
        Id = id;
    }

    public override void AddItem(LegacyLoanItem item)
    {
        throw new NotSupportedException(
            "Архівний формуляр не можна змінювати.");
    }
}
