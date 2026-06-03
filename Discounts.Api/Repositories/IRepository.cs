using Discounts.Api.Db.Entities;

namespace Discounts.Api.Repositories;

public interface IRepository
{
    Task<Dictionary<string, Product>> GetProducts(IEnumerable<string> productIds);
    Task<PointsPromotion> GetPointsPromotionAsOf(DateOnly date);
    Task<DiscountPromotion> GetDiscountPromotionAsOf(DateOnly date);
}
