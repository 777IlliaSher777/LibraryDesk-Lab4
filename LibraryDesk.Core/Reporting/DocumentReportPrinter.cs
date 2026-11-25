namespace LibraryDesk.Core.Reporting;

/// <summary>
/// Виводить сформований звіт у заданий потік.
/// </summary>
public sealed class DocumentReportPrinter
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentReportPrinter"/> class.
    /// </summary>
    /// <param name="output">Потік для виведення тексту.</param>
    public DocumentReportPrinter(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <summary>
    /// Виводить переданий текст звіту.
    /// </summary>
    /// <param name="report">Текст звіту.</param>
    public void Print(string report)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(report);
        _output.Write(report);
    }
}
