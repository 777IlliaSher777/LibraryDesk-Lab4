namespace LibraryDesk.Core.Legacy;

public class LegacyLoan
{
    private readonly List<LegacyLoanItem> _items = new();

    public int Id { get; set; }

    public int Status { get; set; }

    public decimal Total { get; set; }

    public string Email { get; set; } = string.Empty;

    public IReadOnlyList<LegacyLoanItem> Items => _items;

    public virtual void AddItem(LegacyLoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }
}
