namespace Discounts.Api.Services.Models;

public record Transaction(Guid CustomerId, string LoyaltyCard, DateOnly TransactionDate, List<BasketItem> Basket);