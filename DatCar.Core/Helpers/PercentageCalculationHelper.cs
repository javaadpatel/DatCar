using System;
using System.Collections.Generic;
using System.Text;

namespace DatCar.Core.Helpers
{
    public static class PercentageCalculationHelper
    {
        public static decimal CalculatePercentage(this decimal price, decimal percentage)
        {
            return price * (percentage/100.0m);
        }
    }
}
