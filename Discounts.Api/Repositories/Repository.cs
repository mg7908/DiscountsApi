using Discounts.Api.Db;
using Discounts.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discounts.Api.Repositories;

public class Repository(DiscountsDbContext dbContext) : IRepository
{
    public async Task<PointsPromotion> GetPointsPromotionAsOf(DateOnly date)
    {
        return await dbContext.PointsPromotions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StartDate <= date && x.EndDate >= date);
    }

    public async Task<DiscountPromotion> GetDiscountPromotionAsOf(DateOnly date)
    {
        return await dbContext.DiscountPromotions
            .AsNoTracking()
            .Include(x => x.DiscountPromotionProducts)
            .FirstOrDefaultAsync(x => x.StartDate <= date && x.EndDate >= date);
    }

    public async Task<List<Product>> GetProducts(IEnumerable<string> productIds)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync();
    }
}
