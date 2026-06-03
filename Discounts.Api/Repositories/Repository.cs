using Discounts.Api.Db;
using Discounts.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Discounts.Api.Repositories;

public class Repository(DiscountsDbContext dbContext, IMemoryCache cache) : IRepository
{
    private const string CachePrefixProducts = "Product";
    private const string CachePrefixPointsPromotion = "PointsPromotion";
    private const string CachePrefixDiscountPromotion = "DiscountPromotion";

    public async Task<PointsPromotion> GetPointsPromotionAsOf(DateOnly date)
    {
        return await cache.GetOrCreateAsync($"{CachePrefixPointsPromotion}{date}", async entry =>
            await dbContext.PointsPromotions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StartDate <= date && x.EndDate >= date));
    }

    public async Task<DiscountPromotion> GetDiscountPromotionAsOf(DateOnly date)
    {
        return await cache.GetOrCreateAsync($"{CachePrefixDiscountPromotion}{date}", async entry =>
            await dbContext.DiscountPromotions
            .AsNoTracking()
            .Include(x => x.DiscountPromotionProducts)
            .FirstOrDefaultAsync(x => x.StartDate <= date && x.EndDate >= date));
    }

    public async Task<Dictionary<string, Product>> GetProducts(IEnumerable<string> productIds)
    {
        var result = new Dictionary<string, Product>();
        var productIdList = productIds.ToList();

        // First, find those products that exist in the cache
        for (int i = productIdList.Count - 1; i >= 0; i--)
        {
            if (cache.TryGetValue($"{CachePrefixProducts}{productIdList[i]}", out Product product))
            {
                result.TryAdd(productIdList[i], product);
                productIdList.RemoveAt(i);
            }
        }

        // Retrieve the remaining uncached products from the database and cache them
        if (productIdList.Count > 0)
        {
            foreach (var product in await dbContext.Products
                .AsNoTracking()
                .Where(x => productIdList.Contains(x.Id))
                .ToListAsync())
            {
                cache.Set($"{CachePrefixProducts}{product.Id}", product);
                result.Add(product.Id, product);
            }
        }

        return result;
    }
}
