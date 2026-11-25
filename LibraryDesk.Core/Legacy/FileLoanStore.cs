namespace LibraryDesk.Core.Legacy;

using System.Text.Json;
using LibraryDesk.Core.Abstractions;

/// <summary>
/// Забезпечує збереження формулярів у файлах.
/// </summary>
public sealed class FileLoanStore : ILegacyLoanRepository
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    /// <summary>
    /// Зберігає звичайний формуляр у файл.
    /// </summary>
    /// <param name="loan">Формуляр для збереження.</param>
    /// <param name="path">Шлях до файлу.</param>
    public void Save(LegacyLoan loan, string path)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string json = JsonSerializer.Serialize(loan, _jsonOptions);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Зберігає архівний формуляр у файл.
    /// </summary>
    /// <param name="archivedLoan">Архівний формуляр.</param>
    /// <param name="path">Шлях до файлу.</param>
    public void SaveArchive(ArchivedLoan archivedLoan, string path)
    {
        ArgumentNullException.ThrowIfNull(archivedLoan);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string json = JsonSerializer.Serialize(archivedLoan, _jsonOptions);
        File.WriteAllText(path, json);
    }
}
