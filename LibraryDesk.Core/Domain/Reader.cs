namespace LibraryDesk.Core.Domain;

using System.Net.Mail;

/// <summary>
/// Незмінний запис довідника читачів бібліотеки.
/// </summary>
public sealed record Reader
{
    public Reader(int id, string name, string email, bool isActive = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Ім'я читача не може бути порожнім.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Електронна пошта не може бути порожньою.",
                nameof(email));
        }

        try
        {
            MailAddress address = new(email);

            if (address.Address != email.Trim())
            {
                throw new ArgumentException(
                    "Некоректний формат електронної пошти.",
                    nameof(email));
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException(
                "Некоректний формат електронної пошти.",
                nameof(email));
        }

        Id = id;
        Name = name.Trim();
        Email = email.Trim();
        IsActive = isActive;
    }

    public int Id { get; }

    public string Name { get; }

    public string Email { get; }

    public bool IsActive { get; init; }
}
