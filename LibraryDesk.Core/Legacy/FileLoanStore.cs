namespace LibraryDesk.Core.Legacy;

using System.Globalization;
using System.Text;
using System.Text.Json;

public class FileLoanStore : ILoanStore
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    private readonly Dictionary<int, LegacyLoan> _loans = new();

    public void Add(LegacyLoan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        if (!_loans.TryAdd(loan.Id, loan))
        {
            throw new InvalidOperationException(
                "Формуляр із таким номером уже існує.");
        }
    }

    public LegacyLoan? GetById(int id)
    {
        _loans.TryGetValue(id, out LegacyLoan? loan);
        return loan;
    }

    public void SaveToFile(LegacyLoan loan, string path)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string json = JsonSerializer.Serialize(loan, _jsonOptions);

        File.WriteAllText(path, json);
    }

    public void SendEmail(string recipient, string body)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentNullException.ThrowIfNull(body);

        Console.WriteLine(
            string.Concat("Повідомлення для ", recipient, ": ", body));
    }

    public string ExportCsv()
    {
        StringBuilder text = new();

        text.AppendLine("id;total;status");

        foreach (LegacyLoan loan in _loans.Values)
        {
            text.Append(loan.Id.ToString(CultureInfo.InvariantCulture));
            text.Append(';');
            text.Append(loan.Total.ToString(CultureInfo.InvariantCulture));
            text.Append(';');
            text.AppendLine(
                loan.Status.ToString(CultureInfo.InvariantCulture));
        }

        return text.ToString();
    }

    public void Backup(string folder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, "loans-backup.json");

        string json = JsonSerializer.Serialize(_loans.Values, _jsonOptions);

        File.WriteAllText(path, json);
    }

    public void PrintInvoice(LegacyLoan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        Console.WriteLine(
            string.Concat(
                "Формуляр №",
                loan.Id.ToString(CultureInfo.InvariantCulture),
                ". Сума: ",
                loan.Total.ToString(CultureInfo.InvariantCulture)));
    }
}
