namespace LibraryDesk.Core.Legacy;

public interface ILoanStore
{
    void Add(LegacyLoan loan);

    LegacyLoan? GetById(int id);

    void SaveToFile(LegacyLoan loan, string path);

    void SendEmail(string recipient, string body);

    string ExportCsv();

    void Backup(string folder);

    void PrintInvoice(LegacyLoan loan);
}
