# DatCar

A POC of a car rental service backend API.

### Api Endpoints

The Pricing API has one endpoint which returns the rental price for a specified car

> GET /rental?carId=1&startDate=2021-06-18T00:00:00&endDate=2021-06-21T00:00:00

which returns

```
{
    //car object containing relevant car properties (can be expanded further)
    "car": {
        "id": 1,                    //car's internal system id
        "name": "Citi Golf",        //car's name
        "dailyBasePrice": 100.0     //daily base price of car in euro's
    },
    //the price for each day that the car is to be rented (prices differ due to daily pricing rules being applied eg. Weekend Surcharge)
    "pricePerDay": {
        "2021-06-18T00:00:00": 100.0,
        "2021-06-19T00:00:00": 105.00,
        "2021-06-20T00:00:00": 105.00,
        "2021-06-21T00:00:00": 100.0
    },
    //dictionary of all daily charges that were added on the car's rental price
    "dailyCharges": {
        "Weekend Surcharge": 10.00
    },
    //dictionary of all the additional charges that were added on a car's rental price (additional charges apply to the total cost of the car ie. sum of the `pricePerDay` charges)
    "additionalCharges": {
        "Insurance": 41.000,
        "SnappCar": 41.000
    },
    //dictionary of all the discount that were subtracted from the car's rental price (discounts apply to the total cost of the car ie. sum of the `pricePerDay` charges)
    "discounts": {
        "Long Rental": 73.80000
    },
    //the total rental price incl. additional charges and discounts
    "totalPriceForRental": 418.20000
}

```

### Pricing Engine

The car's rental price is calculated using the pricing engine which implements the `Rules Design Pattern`. This pattern allows rules to be easily tested and composed, and splits pricing rules into three general categories (reflected in the code's folder structure)

- Daily Pricing Rules - Rules which affect the base price of a car on specific days. Eg. The weekend surcharge rule increases the car's rental cost on Weekends. Another example could be a public holiday rule which increases the car's cost for days which are public holidays as demand would be higher on these days.
- Additional Charge Pricing Rules - Rules which levy an additional cost on the car's rental price. Eg. Insurance and service provider costs. These operate on the sum of the `pricePerDay` cost of the car.
- Discount Pricing Rules - Rules which discount the car's rental price. Eg. Long rental discount which reduces the cost if the car is rented for a specific period. These operate on the sum on the sum of the `pricePerDay` + any additional charges.

The advantage of this type of engine is that you can easily add new rules and test rules individually. Each rule is simply registered with DI as:

```
//register pricing rules
services
    .AddSingleton<IPricingRule<DailyPricingRule>, WeekendSurchargeRule>()
    .AddSingleton<IPricingRule<AdditionalChargeRule>, InsuranceChargeRule>()
    .AddSingleton<IPricingRule<AdditionalChargeRule>, SnappCarChargeRule>()
    .AddSingleton<IPricingRule<DiscountRule>, LongRentalDiscountRule>();
```

The ordering of the rules within each category is implicit as rules registered first will run first.

The rules execute in the order `Daily Pricing Rules`, `Additional Charge Pricing Rules` and then `Discount Pricing Rules`.

The base engine `PricingEngine.cs` executes all the rules and calculates the total cost of the car's rental. To extend this engine you can implement the `Decorator` pattern and inject `IPricingEngine` and then add additional behaviour onto the engine.
