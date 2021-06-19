using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatCar.Core.Models
{
    public class CarRentalPrice
    {
        public Car Car { get; set; }

        /// <summary>
        /// These represents the price per day of the car. Specific charges might make the daily price fluctuate
        /// eg. (weekends costing more than week days)
        /// </summary>
        public Dictionary<DateTime, decimal> PricePerDay { get; set; } = new Dictionary<DateTime, decimal>();
        public decimal TotalDailyPrice () => PricePerDay.Sum(x => x.Value);


        /// <summary>
        /// These represent the total additional daily charges and allow you to easily identify what the total 
        /// effect of each daily charge would be
        /// </summary>
        public Dictionary<string, decimal> DailyCharges { get; set; } = new Dictionary<string, decimal>();
        public decimal TotalDailyCharges() => DailyCharges.Sum(x => x.Value);

        /// <summary>
        /// These represent the additional charges that apply to the total price of the car eg. (Insurance, SnappCar)
        /// </summary>
        public Dictionary<string, decimal> AdditionalCharges { get; set; } = new Dictionary<string, decimal>();
        public decimal TotalAdditionalCharges() => AdditionalCharges.Sum(x => x.Value);

        /// <summary>
        /// These represent the discounts that apply to the total price of the car eg. (Long term rental)
        /// </summary>
        public Dictionary<string, decimal> Discounts { get; set; } = new Dictionary<string, decimal>();
        public decimal TotalDiscounts() => Discounts.Sum(x => x.Value);

        /// <summary>
        /// This represents the total price the customer will pay for the full rental duration.
        /// Including base price, daily charges and additional charges less discounts.
        /// </summary>
        public decimal TotalPriceForRental { get; set; }
    }
}
