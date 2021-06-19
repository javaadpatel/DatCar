using DatCar.Core.Helpers;
using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using DatCar.PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Rules.AdditionalChargesRules
{
    public class PercentageChargeRule : IPricingRule<AdditionalChargeRule>
    {
        private readonly decimal _percentageCharge;
        private readonly string _ruleName = "SnappCar";

        public PercentageChargeRule(string ruleName, decimal percentageCharge)
        {
            if (string.IsNullOrWhiteSpace(ruleName))
                throw new ArgumentException(nameof(ruleName));

            if (percentageCharge == 0m)
                throw new ArgumentException(nameof(percentageCharge));

            _ruleName = ruleName;
            _percentageCharge = percentageCharge;
        }

        public virtual Task<CarRentalPrice> ApplyRule(CarRentalPrice carRentalPrice)
        {
            //calculate total additional charge
            var additionalChargeAmount = carRentalPrice.TotalDailyPrice().CalculatePercentage(_percentageCharge);

            //add additonal cost to additional charges
            carRentalPrice.AdditionalCharges.Add(_ruleName, additionalChargeAmount);

            return Task.FromResult(carRentalPrice);
        }
    }
}
