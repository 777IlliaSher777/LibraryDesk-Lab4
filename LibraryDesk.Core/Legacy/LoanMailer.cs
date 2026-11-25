namespace LibraryDesk.Core.Legacy;

using LibraryDesk.Core.Abstractions;

/// <summary>
/// Забезпечує надсилання повідомлень про формуляри.
/// </summary>
public sealed class LoanMailer : INotifier
{
    private readonly string _channelName = "Консоль";

    /// <summary>
    /// Надсилає повідомлення отримувачу.
    /// </summary>
    /// <param name="recipient">Отримувач повідомлення.</param>
    /// <param name="subject">Тема повідомлення.</param>
    /// <param name="body">Текст повідомлення.</param>
    public void Notify(string recipient, string subject, string body)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentNullException.ThrowIfNull(body);

        Console.WriteLine(
            string.Concat(
                "Канал: ",
                _channelName,
                ". Повідомлення для ",
                recipient,
                ". Тема: ",
                subject,
                ". Текст: ",
                body));
    }
}
