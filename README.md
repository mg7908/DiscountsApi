# Discounts.Api
Built using C# Minimal API - it contains one endpoint that calculates discounts using a database containing products, points promotions, discounts promotions and categories.

It uses EF Core and SQLite.  The SQLite database will be created automatically upon first run with sample data inserted.  There is an IMemoryCache for use as a simple cache for anything retrieved from the database.

# Running the API
Clone the repository into Visual Studio or VS Code and run the Discounts.Api project.

This will launch your browser pointing at http://localhost:5037/.  The result will be 404 but that's okay as there are no GET endpoints defined.

# Example request
    POST http://localhost:5037/discounts/calculate
    {
      "CustomerId": "8e4e8991-aaee-495b-9f24-52d5d0e509c5",
      "LoyaltyCard": "CTX0000001",
      "TransactionDate": "03-Jan-2020",
      "Basket": [
        {
          "ProductId": "PRD01",
          "UnitPrice": "1.2",
          "Quantity": "3"
        },
        {
          "ProductId": "PRD02",
          "UnitPrice": "2.0",
          "Quantity": "2"
        },
        {
          "ProductId": "PRD04",
          "UnitPrice": "5.0",
          "Quantity": "1"
        }
      ]
    }

# Example response
    {
      "CustomerId": "8e4e8991-aaee-495b-9f24-52d5d0e509c5",
      "LoyaltyCard": "CTX0000001",
      "TransactionDate": "03-Jan-2020",
      "TotalAmount": "12.60",
      "DiscountApplied": "1.52",
      "GrandTotal": "11.08",
      "PointsEarned": "22"
    }

# Assumptions Made
* The API contract given in the task was fixed and cannot be changed, i.e. amounts and dates are represented as strings as shown.
* The start and end dates of the promotions are inclusive.
* The points per dollar spend are based on the price after any discount, and the points are calculated as the "qualifying spend" ignoring cents multiplied by the "for each dollar spent" value.
* If a Discount Promotion has no products specified, then it applies to all products.
* Given that there is a unit price passed into the request, and a different unit price specified in the product data, the assumption is that the price passed in was the one to use.  This seems to be most likely what the user actually paid for the product.  The unit price in the table I would expect to be "RRP", or "price today".
* The database has keys to ensure duplicate IDs are not entered, however there is no check that dates do not overlap, it is taken as given that only one promo can run at any given time (this would be difficult to enforce at the DB level and would most likely be enforced at the application level).
* It's okay to cache the values retrieved from the DB indefinitely as long as the theoretical process responsible for updating the database also has the ability to invalidate updated entries from the cache.
* A 400 error is returned if a product ID is provided that is not in the database, or if a provided string could not be parsed as a number.