# DiscountsApi
Calculating Discounts Exercise 

# Running the API
Clone the repository into Visual Studio or VS Code and run the Discounts.Api project.

# Assumptions Made
* The API contract given in the task was fixed, i.e. amounts and dates are represented as strings as shown.
* The end dates of the promotions are inclusive.
* The points per dollar spend are based on the price after any discount.
* If a Discount Promotion has no products specified, then it applies to all products.
* Given that there is a unit price passed into the request, and a different unit price specified in the product data, I assumed that the price passed in was the one to use.  This seems to be most likely what the user actually paid for the product.  The unit price in the table I would expect to be "RRP", or "price today", however this would be something I would clarify before starting work.
