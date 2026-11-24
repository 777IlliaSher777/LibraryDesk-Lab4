namespace LibraryDesk.Core.Reporting
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Формує текстовий звіт про видачу книжок.
    /// </summary>
    public static class DocumentReport
    {
        /// <summary>
        /// Створює текст звіту на основі запиту та результату розрахунку.
        /// </summary>
        /// <param name="request">Дані запиту на розрахунок.</param>
        /// <param name="result">Результат розрахунку.</param>
        /// <returns>Сформований текст звіту.</returns>
        public static string TextOf(TotalRequest request, TotalResult result)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(result);

            StringBuilder text = new();

            text.AppendLine(
                CultureInfo.InvariantCulture,
                $"Документ № {request.DocumentId}");

            text.AppendLine(
                CultureInfo.InvariantCulture,
                $"Клієнт: {request.Client.Name}");

            foreach (Legacy.BookRow line in request.Lines)
            {
                text.AppendLine(
                    CultureInfo.InvariantCulture,
                    $"{line.Sku} x {line.Quantity}");
            }

            text.AppendLine(
                CultureInfo.InvariantCulture,
                $"Разом: {result.Total:F2}");

            text.AppendLine(
                CultureInfo.InvariantCulture,
                $"Стан: {result.Status}");

            return text.ToString();
        }
    }
}
