using DatCar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Contracts
{
    public interface IPricingRule
    {
        Task<CarRentalPrice> ApplyRule(CarRentalPrice carRentalPrice);
    }

    public interface IPricingRule<T>
    {
        Task<CarRentalPrice> ApplyRule(CarRentalPrice carRentalPrice);
    }
}
