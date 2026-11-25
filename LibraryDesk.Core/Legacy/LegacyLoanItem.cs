namespace LibraryDesk.Core.Legacy;

public class LegacyLoanItem
{
    public string InventoryNumber { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
