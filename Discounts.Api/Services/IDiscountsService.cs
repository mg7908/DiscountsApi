using Discounts.Api.Services.Models;

namespace Discounts.Api.Services;

public interface IDiscountsService
{
    Task<Result<DiscountInfo>> CalculateDiscountInfo(Transaction transaction);
}
