namespace LibraryDesk.Core.Abstractions;

using LibraryDesk.Core.Legacy;

/// <summary>
/// Визначає операції збереження старих формулярів.
/// </summary>
public interface ILegacyLoanRepository
{
    /// <summary>
    /// Зберігає формуляр у сховище.
    /// </summary>
    /// <param name="loan">Формуляр для збереження.</param>
    /// <param name="path">Шлях до файлу.</param>
    void Save(LegacyLoan loan, string path);

    /// <summary>
    /// Зберігає архівний формуляр.
    /// </summary>
    /// <param name="archivedLoan">Архівний формуляр.</param>
    /// <param name="path">Шлях до файлу.</param>
    void SaveArchive(ArchivedLoan archivedLoan, string path);
}
