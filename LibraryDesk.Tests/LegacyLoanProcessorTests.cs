using System;
using System.IO;
using LibraryDesk.Core;
using LibraryDesk.Core.Legacy;
using LibraryDesk.Core.Reporting;
using Xunit;

namespace LibraryDesk.Tests;

public class LegacyLoanProcessorTests
{
    [Fact]
    public void CalculateRegularClientReturnsExpectedTotal()
    {
        TotalRequest request = BuildSampleRequest();

        TotalResult result = DocumentTotalCalculator.Calculate(request);

        Assert.Equal(1292.70m, result.Total);
        Assert.Equal(LoanStatus.Active, result.Status);
    }

    [Fact]
    public void CalculateWithoutLinesThrowsArgumentException()
    {
        TotalRequest request = BuildSampleRequest() with
        {
            Lines = Array.Empty<BookRow>(),
        };

        Assert.Throws<ArgumentException>(
            () => DocumentTotalCalculator.Calculate(request));
    }

    [Fact]
    public void CalculateZeroQuantityThrowsArgumentOutOfRangeException()
    {
        TotalRequest request = BuildSampleRequest() with
        {
            Lines = new[]
            {
                new BookRow
                {
                    Sku = "BOOK-001",
                    Quantity = 0,
                    UnitPrice = 100m,
                },
            },
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => DocumentTotalCalculator.Calculate(request));
    }

    [Fact]
    public void TextOfContainsTotalLine()
    {
        TotalRequest request = BuildSampleRequest();

        TotalResult result = DocumentTotalCalculator.Calculate(request);

        string text = DocumentReport.TextOf(request, result);

        Assert.Contains("Разом: 1292.70", text);
    }

    [Fact]
    public void PrintWritesReportToOutput()
    {
        using StringWriter writer = new();

        DocumentReportPrinter printer = new(writer);

        printer.Print("Разом: 10.00");

        Assert.Contains("Разом: 10.00", writer.ToString());
    }

    private static TotalRequest BuildSampleRequest()
    {
        return new TotalRequest(
            DocumentId: 1,
            Client: new ClientInfo(
                Name: "Іваненко",
                Email: "i@ex.com",
                IsRegular: true),
            Lines: new[]
            {
                new BookRow
                {
                    Sku = "A1",
                    Quantity = 12,
                    UnitPrice = 100m,
                },
                new BookRow
                {
                    Sku = "B2",
                    Quantity = 1,
                    UnitPrice = 250m,
                },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: null,
            Status: LoanStatus.New,
            DeliveryPrice: 60m);
    }
}
