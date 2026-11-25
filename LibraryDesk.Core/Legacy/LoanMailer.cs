namespace LibraryDesk.Core.Legacy;

public class LoanMailer
{
    private readonly string _channelName = "Консоль";

    public void Send(string recipient, string subject, string body)
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
