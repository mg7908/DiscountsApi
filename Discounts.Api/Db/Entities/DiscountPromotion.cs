namespace Discounts.Api.Db.Entities;

public class DiscountPromotion
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal DiscountPercent { get; set; }

    public ICollection<DiscountPromotionProduct> DiscountPromotionProducts { get; set; }
}

