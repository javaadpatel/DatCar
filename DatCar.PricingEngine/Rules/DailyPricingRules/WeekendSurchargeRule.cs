using DatCar.Core.Helpers;
using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using DatCar.PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Rules.DailyPricingRules
{
    /// <summary>
    /// This rule allows for charging a percentage surcharge when the car is hired over the weekend
    /// </summary>
    public class WeekendSurchargeRule : IPricingRule<DailyPricingRule>
    {
        private readonly decimal _weekendSurcharge = 5; //TODO: pull from configuration settings, injection IConfiguration into constructor
        private readonly string _dailyChargeName = "Weekend Surcharge";

        public Task<CarRentalPrice> ApplyRule(CarRentalPrice carRentalPrice)
        {
            decimal totalDailySurcharge = 0;

            var modifiedPricePerDay = new Dictionary<DateTime, decimal>();
            //update the price per day
            foreach(var dailyPrice in carRentalPrice.PricePerDay)
            {
                decimal dailySurcharge = 0;

                if (IsWeekend(dailyPrice.Key))
                {
                    dailySurcharge = dailyPrice.Value.CalculatePercentage(_weekendSurcharge);
                }

                totalDailySurcharge += dailySurcharge;
                modifiedPricePerDay.Add(dailyPrice.Key, dailyPrice.Value + dailySurcharge);
            }

            //update the price per day
            carRentalPrice.PricePerDay = modifiedPricePerDay;

            //record the total daily charge that was applied
            if (totalDailySurcharge > 0)
                carRentalPrice.DailyCharges.Add(_dailyChargeName, totalDailySurcharge);

            return Task.FromResult(carRentalPrice);
        }

        private bool IsWeekend(DateTime rentalDay)
        {
            return rentalDay.DayOfWeek == DayOfWeek.Saturday || rentalDay.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
