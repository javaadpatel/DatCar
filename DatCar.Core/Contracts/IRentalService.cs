using DatCar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.Core.Contracts
{
    public interface IRentalService
    {
        Task<CarRentalPrice> GetRentalPriceAsync(CarRentalPriceRequest carRentalPriceRequest);
    }
}
