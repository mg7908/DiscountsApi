using Discounts.Api.Contracts.Requests;
using Discounts.Api.Contracts.Responses;
using Discounts.Api.Services.Models;

namespace Discounts.Api.Contracts.Mappers;

public static class DiscountMappingExtensions
{
    public const string DateFormat = "dd-MMM-yyyy";

    public static Transaction ToTransaction(this DiscountRequest request)
    {
        return new Transaction(request.CustomerId,
            request.LoyaltyCard,
            DateOnly.ParseExact(request.TransactionDate, DateFormat),
            request.Basket.Select(x => new Services.Models.BasketItem(x.ProductId, decimal.Parse(x.UnitPrice), decimal.Parse(x.Quantity))).ToList());
    }

    public static DiscountResponse ToResponse(this DiscountInfo discount)
    {
        return new DiscountResponse(discount.CustomerId,
            discount.LoyaltyCard,
            discount.TransactionDate.ToString(DateFormat),
            $"{discount.TotalAmount:F2}",
            $"{discount.DiscountApplied:F2}",
            $"{discount.GrandTotal:F2}",
            $"{discount.PointsEarned:F0}");
    }
}
