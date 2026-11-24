namespace LibraryDesk.Core.Legacy;

/// <summary>
/// Виконує розрахунок загальної вартості формуляра видачі.
/// </summary>
public static class DocumentTotalCalculator
{
    /// <summary>
    /// Обчислює загальну суму та визначає наступний статус формуляра.
    /// </summary>
    /// <param name="request">Запит із даними для розрахунку.</param>
    /// <returns>Результат розрахунку та новий статус.</returns>
    public static TotalResult Calculate(TotalRequest request)
    {
        LoanGuards.EnsureRequestValid(request);

        decimal total = SubtotalOf(request.Lines);

        total -= ClientDiscountOf(
            total,
            request.Client.IsRegular);

        total -= VolumeDiscountOf(total);

        total -= CouponDiscountOf(
            total,
            request.CouponCode);

        total -= WeekendDiscountOf(
            total,
            request.CreatedAt);

        total += DeliveryFeeOf(
            total,
            request.DeliveryPrice);

        total = request.Status == LoanStatus.Cancelled
            ? 0m
            : Math.Round(Math.Max(total, 0m), 2);

        return new TotalResult(
            total,
            NextStatusOf(request.Status, total));
    }

    /// <summary>
    /// Обчислює суму книжкових позицій із урахуванням оптової знижки.
    /// </summary>
    /// <param name="lines">Список книжкових позицій.</param>
    /// <returns>Сума всіх позицій.</returns>
    public static decimal SubtotalOf(
        IReadOnlyList<BookRow> lines)
    {
        decimal subtotal = 0m;

        foreach (BookRow line in lines)
        {
            decimal lineTotal = line.Quantity * line.UnitPrice;

            if (line.Quantity >= PricingRules.BulkQuantity)
            {
                lineTotal *= 1m - PricingRules.BulkRate;
            }

            subtotal += lineTotal;
        }

        return subtotal;
    }

    /// <summary>
    /// Обчислює знижку для постійного читача.
    /// </summary>
    /// <param name="amount">Поточна сума.</param>
    /// <param name="isRegularClient">Ознака постійного читача.</param>
    /// <returns>Розмір знижки.</returns>
    public static decimal ClientDiscountOf(
        decimal amount,
        bool isRegularClient)
    {
        return isRegularClient
            ? amount * PricingRules.RegularClientRate
            : 0m;
    }

    /// <summary>
    /// Обчислює знижку залежно від суми формуляра.
    /// </summary>
    /// <param name="amount">Поточна сума.</param>
    /// <returns>Розмір знижки.</returns>
    public static decimal VolumeDiscountOf(decimal amount)
    {
        if (amount > PricingRules.LargeOrderFrom)
        {
            return amount * PricingRules.LargeOrderRate;
        }

        if (amount > PricingRules.MediumOrderFrom)
        {
            return amount * PricingRules.MediumOrderRate;
        }

        return 0m;
    }

    /// <summary>
    /// Обчислює знижку за купоном.
    /// </summary>
    /// <param name="amount">Поточна сума.</param>
    /// <param name="couponCode">Код купона.</param>
    /// <returns>Розмір знижки.</returns>
    public static decimal CouponDiscountOf(
        decimal amount,
        string? couponCode)
    {
        return couponCode switch
        {
            PricingRules.SaleCoupon =>
                amount * PricingRules.CouponSaleRate,

            PricingRules.FixedCoupon
                when amount > PricingRules.CouponFixedFrom =>
                PricingRules.CouponFixed,

            _ => 0m
        };
    }

    /// <summary>
    /// Обчислює знижку за створення формуляра у вихідний день.
    /// </summary>
    /// <param name="amount">Поточна сума.</param>
    /// <param name="createdAt">Дата створення формуляра.</param>
    /// <returns>Розмір знижки.</returns>
    public static decimal WeekendDiscountOf(
        decimal amount,
        DateOnly createdAt)
    {
        return createdAt.DayOfWeek == DayOfWeek.Sunday
            ? amount * PricingRules.WeekendRate
            : 0m;
    }

    /// <summary>
    /// Визначає вартість доставки.
    /// </summary>
    /// <param name="amount">Поточна сума.</param>
    /// <param name="deliveryPrice">Вартість доставки.</param>
    /// <returns>Сума доставки.</returns>
    public static decimal DeliveryFeeOf(
        decimal amount,
        decimal deliveryPrice)
    {
        return amount < PricingRules.FreeDeliveryFrom
            ? deliveryPrice
            : 0m;
    }

    /// <summary>
    /// Визначає наступний статус формуляра.
    /// </summary>
    /// <param name="current">Поточний статус.</param>
    /// <param name="total">Підсумкова сума.</param>
    /// <returns>Наступний статус.</returns>
    public static LoanStatus NextStatusOf(
        LoanStatus current,
        decimal total)
    {
        if (current == LoanStatus.New && total > 0m)
        {
            return LoanStatus.Active;
        }

        return current;
    }
}
