namespace LibraryDesk.Core;

/// <summary>
/// Містить інформацію про читача, який оформлює видачу книжок.
/// </summary>
/// <param name="Name">Ім'я читача.</param>
/// <param name="Email">Електронна пошта читача.</param>
/// <param name="IsRegular">Ознака постійного читача.</param>
public sealed record ClientInfo(
    string Name,
    string Email,
    bool IsRegular);
