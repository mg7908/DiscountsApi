using Discounts.Api.Services.Models;

namespace Discounts.Api.Services;

public interface IDiscountsService
{
    Task<DiscountInfo> CalculateDiscountInfo(Transaction transaction);
}
