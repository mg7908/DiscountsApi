using Discounts.Api.Contracts.Mappers;
using Discounts.Api.Contracts.Requests;
using Discounts.Api.Db;
using Discounts.Api.Repositories;
using Discounts.Api.Services;
using Discounts.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var discountsConnectionString = builder.Configuration.GetConnectionString("Discounts") ?? "Data Source=discounts.db";
builder.Services.AddDbContext<DiscountsDbContext>(options =>
    options.UseSqlite(discountsConnectionString));

builder.Services.AddScoped<IDiscountsService, DiscountsService>();
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddMemoryCache();
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
    Transaction transaction;

    try { transaction = request.ToTransaction(); }
    catch (FormatException) { return Results.BadRequest(new { error = "The input data was not in the correct format." }); }

    var discountInfoResult = await discountsService.CalculateDiscountInfo(transaction);

    return discountInfoResult.Value is null
        ? Results.BadRequest(new { error = discountInfoResult.Error })
        : Results.Ok(discountInfoResult.Value.ToResponse());
});

app.Run();

public partial class Program;
