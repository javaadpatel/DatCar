using DatCar.Core.Contracts;
using DatCar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.Services
{
    /// <summary>
    /// Repository class to fetch cars stored in persistent storage (MongoDB, SQL, etc)
    /// </summary>
    public class CarRepository : ICarRepository
    {
        private readonly List<Car> _cars = new List<Car>
        {
            new Car
            {
                Id = 1,
                Name = "Citi Golf",
                DailyBasePrice = 100,
            },
            new Car
            {
                Id = 2,
                Name = "Mini",
                DailyBasePrice = 150,
            },
            new Car
            {
                Id = 3,
                Name = "Audi A3",
                DailyBasePrice = 200
            }
        };

        public Task<List<Car>> GetAllCarsAsync()
        {
            return Task.FromResult(_cars);
        }

        public Task<Car> GetCarAsync(long id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(car);
        }
    }
}
