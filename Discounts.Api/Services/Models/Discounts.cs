namespace Discounts.Api.Services.Models;

public record DiscountInfo(Guid CustomerId, string LoyaltyCard, DateOnly TransactionDate, decimal TotalAmount, decimal DiscountApplied, decimal GrandTotal, int PointsEarned);