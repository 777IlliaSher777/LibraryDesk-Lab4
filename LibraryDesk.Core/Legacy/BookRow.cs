namespace LibraryDesk.Core.Legacy;

/// <summary>
/// Представляє позицію формуляра видачі.
/// </summary>
public sealed class BookRow
{
    public string Sku { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
