namespace LibraryDesk.Core.Abstractions;

/// <summary>
/// Визначає інтерфейс надсилання повідомлень.
/// </summary>
public interface INotifier
{
    /// <summary>
    /// Надсилає повідомлення отримувачу.
    /// </summary>
    /// <param name="recipient">Отримувач повідомлення.</param>
    /// <param name="subject">Тема повідомлення.</param>
    /// <param name="body">Текст повідомлення.</param>
    void Notify(string recipient, string subject, string body);
}
