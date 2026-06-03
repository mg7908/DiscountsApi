namespace Discounts.Api.Db.Entities;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<DiscountPromotionProduct> DiscountPromotionProducts { get; set; }
}
