using DatCar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Contracts
{
    public interface IPricingEngine
    {
        Task<CarRentalPrice> CalculatePriceAsync(CarRentalPrice carRental);
    }
}
