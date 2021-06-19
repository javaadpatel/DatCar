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
    /// <summary>
    /// This rule allows for charging a percentage for insurance
    /// </summary>
    public class InsuranceChargeRule : PercentageChargeRule
    {
        public InsuranceChargeRule() : base("Insurance", 10m)
        {
        }
    }
}
