namespace Discounts.Api.Contracts.Requests;

public class DiscountRequest
{
    public Guid CustomerId { get; set; }
    public string LoyaltyCard { get; set; }
    public string TransactionDate { get; set; }
    public List<BasketItem> Basket { get; set; }
}
