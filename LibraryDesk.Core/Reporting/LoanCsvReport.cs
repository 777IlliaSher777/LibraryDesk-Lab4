namespace LibraryDesk.Core.Reporting;

using System.Globalization;
using System.Text;
using LibraryDesk.Core.Legacy;

/// <summary>
/// Формує CSV-звіти про старі формуляри.
/// </summary>
public static class LoanCsvReport
{
    /// <summary>
    /// Створює CSV-звіт зі списку формулярів.
    /// </summary>
    /// <param name="loans">Список формулярів.</param>
    /// <returns>Текст CSV-звіту.</returns>
    public static string BuildCsv(IReadOnlyList<LegacyLoan> loans)
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

        return text.ToString();
    }
}
