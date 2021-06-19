using DatCar.Core.Contracts;
using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.Services
{
    public class RentalService : IRentalService
    {
        private readonly ICarRepository _carRepository;
        private readonly IPricingEngine _pricingEngine;

        public RentalService(ICarRepository carRepository, IPricingEngine pricingEngine)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _pricingEngine = pricingEngine ?? throw new ArgumentNullException(nameof(pricingEngine));
        }

        public async Task<CarRentalPrice> GetRentalPriceAsync(CarRentalPriceRequest carRentalPriceRequest)
        {
            //get the car
            var car = await _carRepository.GetCarAsync(carRentalPriceRequest.CarId);

            if (car == null)
                throw new ArgumentException($"Car not found"); //TODO: handle this better with an actual error response message

            //calculate the car's price
            var carRentalPrice = new CarRentalPrice
            {
                Car = car,
                PricePerDay = BuildPricePerDay(car, carRentalPriceRequest)
            };

            carRentalPrice = await _pricingEngine.CalculatePriceAsync(carRentalPrice);
            
            return carRentalPrice;
        }

        private Dictionary<DateTime, decimal> BuildPricePerDay(Car car, CarRentalPriceRequest carRentalPriceRequest)
        {
            var days = Enumerable
                .Range(0, (carRentalPriceRequest.EndDate - carRentalPriceRequest.StartDate).Days + 1)
                .Select(d => carRentalPriceRequest.StartDate.AddDays(d));

            var pricePerDay =  days.ToDictionary(x => x, y => car.DailyBasePrice);

            return pricePerDay;
        }

    }
}
