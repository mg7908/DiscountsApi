namespace Discounts.Api.Db.Entities;

public class DiscountPromotionProduct
{
    public string DiscountPromotionId { get; set; }
    public DiscountPromotion DiscountPromotion { get; set; }

    public string ProductId { get; set; }
    public Product Product { get; set; }
}

