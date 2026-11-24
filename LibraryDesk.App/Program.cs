using System.Text;
using LibraryDesk.Core;
using LibraryDesk.Core.Legacy;
using LibraryDesk.Core.Reporting;

Console.OutputEncoding = Encoding.UTF8;

TotalRequest request = new(
    DocumentId: 1,
    Client: new ClientInfo(
        "Іваненко",
        "i@ex.com",
        true),
    Lines: new List<BookRow>
    {
        new()
        {
            Sku = "A1",
            Quantity = 12,
            UnitPrice = 100m
        },
        new()
        {
            Sku = "B2",
            Quantity = 1,
            UnitPrice = 250m
        }
    },
    CreatedAt: new DateOnly(2026, 3, 10),
    CouponCode: null,
    Status: LoanStatus.New,
    DeliveryPrice: 60m);

TotalResult result =
    DocumentTotalCalculator.Calculate(request);

string report =
    DocumentReport.TextOf(request, result);

new DocumentReportPrinter(Console.Out).Print(report);
