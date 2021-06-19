using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using DatCar.PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Engines
{
    public class PricingEngine : IPricingEngine
    {
        private readonly IEnumerable<IPricingRule<DailyPricingRule>> _dailyPricingRules;
        private readonly IEnumerable<IPricingRule<AdditionalChargeRule>> _additonalChargeRules;
        private readonly IEnumerable<IPricingRule<DiscountRule>> _discountRules;

        public PricingEngine(
            IEnumerable<IPricingRule<DailyPricingRule>> dailyPricingRules,
            IEnumerable<IPricingRule<AdditionalChargeRule>> additonalChargeRules,
            IEnumerable<IPricingRule<DiscountRule>> discountRules
        )
        {
            _dailyPricingRules = dailyPricingRules;
            _additonalChargeRules = additonalChargeRules;
            _discountRules = discountRules;
        }

        public async Task<CarRentalPrice> CalculatePriceAsync(CarRentalPrice carRental)
        {
            //apply the daily charge rules first
            foreach (var dailyPricingRule in _dailyPricingRules)
            {
                carRental = await dailyPricingRule.ApplyRule(carRental);
            }

            //apply the additional charge rules
            foreach (var additionalChargeRule in _additonalChargeRules)
            {
                carRental = await additionalChargeRule.ApplyRule(carRental);
            }

            // apply the discount rules
            foreach (var discountRule in _discountRules)
            {
                carRental = await discountRule.ApplyRule(carRental);
            }

            carRental.TotalPriceForRental = carRental.TotalDailyPrice() + carRental.TotalAdditionalCharges() - carRental.TotalDiscounts();

            return carRental;
        }
    }
}
