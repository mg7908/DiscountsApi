namespace Discounts.Api.Db.Entities;

public class PointsPromotion
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int PointsPerDollarSpent { get; set; }

    public int? CategoryId { get; set; }
    public Category Category { get; set; }
}

