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

        // Loop through each product, and calculate the discount and total qualifying spend for earning points
        foreach (var product in transaction.Basket)
        {            
            decimal discountApplied = 0;
            
            if (discountPromotion is not null)
            {
                discountApplied = discountPromotion.DiscountPromotionProducts.Count == 0 || discountPromotion.DiscountPromotionProducts.Any(x => x.ProductId == product.ProductId)
                    ? product.Quantity * product.UnitPrice * (discountPromotion.DiscountPercent / 100)
                    : 0m;
            }

            if (pointsPromotion is not null)
            {
                totalQualifyingPointsSpend += pointsPromotion.CategoryId is null || dbProducts[product.ProductId].CategoryId == pointsPromotion.CategoryId
                    ? product.Quantity * product.UnitPrice - discountApplied
                    : 0m;
            }

            totalDiscountApplied += discountApplied;
            totalAmount += product.Quantity * product.UnitPrice;
            grandTotal += product.Quantity * product.UnitPrice - discountApplied;
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
}