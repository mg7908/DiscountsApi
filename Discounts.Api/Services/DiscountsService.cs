using Discounts.Api.Db.Entities;
using Discounts.Api.Repositories;
using Discounts.Api.Services.Models;

namespace Discounts.Api.Services;

public class DiscountsService(IRepository repository) : IDiscountsService
{
    public async Task<DiscountInfo> CalculateDiscountInfo(Transaction transaction)
    {
        decimal totalAmount = 0, totalDiscountApplied = 0, grandTotal = 0, totalQualifyingPointsSpend = 0;

        // Get the product info for all the products in the transaction
        var dbProducts = (await repository.GetProducts(transaction.Basket.Select(p => p.ProductId))).ToDictionary(x => x.Id);

        // Get the current points promo and discount promo, if any
        var pointsPromotion = await repository.GetPointsPromotionAsOf(transaction.TransactionDate);
        var discountPromotion = await repository.GetDiscountPromotionAsOf(transaction.TransactionDate);

        // Loop through each item in the basket, and calculate the discount and total qualifying spend for earning points
        foreach (var basketItem in transaction.Basket)
        {
            decimal discountApplied = CalculateDiscountApplied(discountPromotion, basketItem);
            decimal qualifyingPointsSpend = CalculateQualifyingPointsSpend(pointsPromotion, dbProducts, basketItem, discountApplied);

            totalDiscountApplied += discountApplied;
            totalAmount += basketItem.Quantity * basketItem.UnitPrice;
            grandTotal += basketItem.Quantity * basketItem.UnitPrice - discountApplied;
            totalQualifyingPointsSpend += qualifyingPointsSpend;
        }

        return new DiscountInfo(transaction.CustomerId,
            transaction.LoyaltyCard,
            transaction.TransactionDate,
            totalAmount,
            totalDiscountApplied,
            grandTotal,
            (int)Math.Floor(totalQualifyingPointsSpend) * (pointsPromotion?.PointsPerDollarSpent ?? 0)
        );
    }

    private static decimal CalculateDiscountApplied(DiscountPromotion discountPromotion, BasketItem basketItem)
    {
        if (discountPromotion is null) return 0;

        return discountPromotion.DiscountPromotionProducts.Count == 0 || discountPromotion.DiscountPromotionProducts.Any(x => x.ProductId == basketItem.ProductId)
            ? basketItem.Quantity * basketItem.UnitPrice * (discountPromotion.DiscountPercent / 100)
            : 0m;
    }

    private static decimal CalculateQualifyingPointsSpend(PointsPromotion pointsPromotion, Dictionary<string, Product> dbProducts, BasketItem basketItem, decimal discountApplied)
    {
        if (pointsPromotion is null) return 0;

        return pointsPromotion.CategoryId is null || dbProducts[basketItem.ProductId].CategoryId == pointsPromotion.CategoryId
            ? basketItem.Quantity * basketItem.UnitPrice - discountApplied
            : 0m;
    }
}