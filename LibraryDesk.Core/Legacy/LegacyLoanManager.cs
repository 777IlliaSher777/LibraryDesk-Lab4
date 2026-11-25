namespace LibraryDesk.Core.Legacy;

using System.Globalization;
using System.Text;

public class LegacyLoanManager
{
    private readonly FileLoanStore _store = new();

    private readonly LoanMailer _mailer = new();

    private readonly string _reportFileName = "loan-report.csv";

    public decimal Place(LegacyLoan loan, string readerType)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerType);

        decimal total = 0m;

        foreach (LegacyLoanItem item in loan.Items)
        {
            total += item.UnitPrice * item.Quantity;
        }

        if (readerType == "regular")
        {
            total *= 0.95m;
        }
        else if (readerType == "vip")
        {
            total *= 0.90m;
        }
        else if (readerType == "staff")
        {
            total *= 0.70m;
        }

        if (total > 1000m)
        {
            total -= 50m;
        }

        loan.Total = total;
        loan.Status = 1;

        _store.Save(loan, "loans.json");

        _mailer.Notify(
            loan.Email,
            string.Concat("Формуляр ", loan.Id),
            total.ToString(CultureInfo.InvariantCulture));

        return total;
    }

    public string BuildCsvReport(IReadOnlyList<LegacyLoan> loans)
    {
        ArgumentNullException.ThrowIfNull(loans);

        StringBuilder text = new();

        text.AppendLine("id;total;status");

        foreach (LegacyLoan loan in loans)
        {
            text.Append(loan.Id.ToString(CultureInfo.InvariantCulture));
            text.Append(';');
            text.Append(loan.Total.ToString(CultureInfo.InvariantCulture));
            text.Append(';');
            text.AppendLine(
                loan.Status.ToString(CultureInfo.InvariantCulture));
        }

        string report = text.ToString();

        File.WriteAllText(_reportFileName, report);

        Console.WriteLine("Звіт збережено");

        return report;
    }

    public void Archive(LegacyLoan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        ArchivedLoan archived = new(loan, DateOnly.FromDateTime(DateTime.Now));

        _store.SaveArchive(archived, "archive.json");
    }
}
