namespace Discounts.Api.Contracts.Requests;

public class BasketItem
{
    public string ProductId { get; set; }
    public string UnitPrice { get; set; }
    public string Quantity { get; set; }
}