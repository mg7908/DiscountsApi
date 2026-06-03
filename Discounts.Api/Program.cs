using Discounts.Api.Contracts.Requests;
using Discounts.Api.Contracts.Mappers;
using Discounts.Api.Db;
using Discounts.Api.Services;
using Discounts.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var discountsConnectionString = builder.Configuration.GetConnectionString("Discounts") ?? "Data Source=discounts.db";
builder.Services.AddDbContext<DiscountsDbContext>(options =>
    options.UseSqlite(discountsConnectionString));

builder.Services.AddScoped<IDiscountsService, DiscountsService>();
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DiscountsDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapPost("/discounts/calculate", async (IDiscountsService discountsService, DiscountRequest request) =>
{
    var discountInfo = await discountsService.CalculateDiscountInfo(request.ToTransaction());

    return Results.Ok(discountInfo.ToResponse());
});

app.Run();

public partial class Program;
