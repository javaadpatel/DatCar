using DatCar.Core.Helpers;
using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using DatCar.PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Rules.DiscountRules
{
    public class LongRentalDiscountRule : IPricingRule<DiscountRule>
    {
        private readonly string _ruleName = "Long Rental";
        private readonly decimal _percentageDiscount = 15m;

        public Task<CarRentalPrice> ApplyRule(CarRentalPrice carRentalPrice)
        {
            //calculate the total discount
            var discountAmount = (carRentalPrice.TotalDailyPrice() + carRentalPrice.TotalAdditionalCharges())
                .CalculatePercentage(_percentageDiscount);

            //add the discount to discounts
            carRentalPrice.Discounts.Add(_ruleName, discountAmount);

            return Task.FromResult(carRentalPrice);
        }
    }
}
