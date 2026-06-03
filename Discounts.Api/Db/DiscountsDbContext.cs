using Discounts.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discounts.Api.Db;

public class DiscountsDbContext(DbContextOptions<DiscountsDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<DiscountPromotion> DiscountPromotions => Set<DiscountPromotion>();
    public DbSet<DiscountPromotionProduct> DiscountPromotionProducts => Set<DiscountPromotionProduct>();
    public DbSet<PointsPromotion> PointsPromotions => Set<PointsPromotion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(category => category.Id);

            entity.HasData(
                new Category { Id = 1, Name = "Fuel" },
                new Category { Id = 2, Name = "Shop" });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(product => product.Id);

            entity.HasOne(product => product.Category)
                .WithMany()
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Product { Id = "PRD01", Name = "Vortex 95", CategoryId = 1, UnitPrice = 1.2m },
                new Product { Id = "PRD02", Name = "Vortex 98", CategoryId = 1, UnitPrice = 1.3m },
                new Product { Id = "PRD03", Name = "Diesel", CategoryId = 1, UnitPrice = 1.1m },
                new Product { Id = "PRD04", Name = "Twix 55g", CategoryId = 2, UnitPrice = 2.3m },
                new Product { Id = "PRD05", Name = "Mars 72g", CategoryId = 2, UnitPrice = 5.1m },
                new Product { Id = "PRD06", Name = "Snickers 72g", CategoryId = 2, UnitPrice = 3.4m },
                new Product { Id = "PRD07", Name = "Bounty 3 63g", CategoryId = 2, UnitPrice = 6.9m },
                new Product { Id = "PRD08", Name = "Snickers 50g", CategoryId = 2, UnitPrice = 4.0m });
        });

        modelBuilder.Entity<DiscountPromotion>(entity =>
        {
            entity.ToTable("DiscountPromotions");
            entity.HasKey(discountPromotion => discountPromotion.Id);

            entity.HasData(
                new DiscountPromotion { Id = "DP001", Name = "Fuel Discount Promo", StartDate = new DateOnly(2020, 1, 1), EndDate = new DateOnly(2020, 2, 15), DiscountPercent = 20 },
                new DiscountPromotion { Id = "DP002", Name = "Happy Promo", StartDate = new DateOnly(2020, 3, 2), EndDate = new DateOnly(2020, 3, 20), DiscountPercent = 15 });
        });

        modelBuilder.Entity<DiscountPromotionProduct>(entity =>
        {
            entity.ToTable("DiscountPromotionProducts");
            entity.HasKey(discountPromotionProduct => new
            {
                discountPromotionProduct.DiscountPromotionId,
                discountPromotionProduct.ProductId
            });

            entity.HasOne(discountPromotionProduct => discountPromotionProduct.DiscountPromotion)
                .WithMany(discountPromotionProduct => discountPromotionProduct.DiscountPromotionProducts)
                .HasForeignKey(discountPromotionProduct => discountPromotionProduct.DiscountPromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(discountPromotionProduct => discountPromotionProduct.Product)
                .WithMany(discountPromotionProduct => discountPromotionProduct.DiscountPromotionProducts)
                .HasForeignKey(discountPromotionProduct => discountPromotionProduct.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new DiscountPromotionProduct { DiscountPromotionId = "DP001", ProductId = "PRD01" },
                new DiscountPromotionProduct { DiscountPromotionId = "DP001", ProductId = "PRD02" });
        });

        modelBuilder.Entity<PointsPromotion>(entity =>
        {
            entity.ToTable("PointsPromotions");
            entity.HasKey(pointsPromotion => pointsPromotion.Id);

            entity.HasOne(pointsPromotion => pointsPromotion.Category)
                .WithMany()
                .HasForeignKey(pointsPromotion => pointsPromotion.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new PointsPromotion { Id = "PP001", Name = "New Year Promo", StartDate = new DateOnly(2020, 1, 1), EndDate = new DateOnly(2020, 1, 30), CategoryId = null, PointsPerDollarSpent = 2 },
                new PointsPromotion { Id = "PP002", Name = "Fuel Promo", StartDate = new DateOnly(2020, 2, 5), EndDate = new DateOnly(2020, 2, 15), CategoryId = 1, PointsPerDollarSpent = 3 },
                new PointsPromotion { Id = "PP003", Name = "Shop Promo", StartDate = new DateOnly(2020, 3, 1), EndDate = new DateOnly(2020, 3, 20), CategoryId = 2, PointsPerDollarSpent = 4 });
        });
    }
}
