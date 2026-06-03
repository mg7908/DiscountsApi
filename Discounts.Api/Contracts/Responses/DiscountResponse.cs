namespace Discounts.Api.Contracts.Responses;

public record DiscountResponse(Guid CustomerId, string LoyaltyCard, string TransactionDate, string TotalAmount, string DiscountApplied, string GrandTotal, string PointsEarned);