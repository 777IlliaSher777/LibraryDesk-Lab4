namespace LibraryDesk.Core.Abstractions;

using LibraryDesk.Core.Domain;

public interface IPricingPolicy
{
    decimal PriceOf(LoanItem item);
}
