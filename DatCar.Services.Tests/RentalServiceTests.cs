using DatCar.Core.Contracts;
using DatCar.Core.Models;
using DatCar.PricingEngine.Contracts;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.Services.Tests
{
    [TestFixture]
    public class RentalServiceTests
    {
        private IRentalService _sut;
        private ICarRepository _carRepository;
        private IPricingEngine _pricingEngine;

        [SetUp]
        public void Setup()
        {
            _carRepository = Substitute.For<ICarRepository>();
            _pricingEngine = Substitute.For<IPricingEngine>();

            _sut = new RentalService(_carRepository, _pricingEngine);
        }


        [Test]
        public async Task GivenCarRentalRequest_WhenGetRentalPrice_ShouldReturnARentalPrice()
        {
            //arrange
            var carRentalRequest = new CarRentalPriceRequest(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            var car = new Car
            {
                Id = 1,
                DailyBasePrice =100m,
                Name = "Citi Golf"
            };
            var carRentalPrice = new CarRentalPrice();

            _carRepository.GetCarAsync(1).Returns(car);
            _pricingEngine.CalculatePriceAsync(Arg.Any<CarRentalPrice>()).Returns(carRentalPrice);

            //act
            var result = await _sut.GetRentalPriceAsync(carRentalRequest);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(carRentalPrice, result);

            await _carRepository
                .Received(1)
                .GetCarAsync(1);

            await _pricingEngine
             .Received(1)
             .CalculatePriceAsync(Arg.Any<CarRentalPrice>());
        }
    }
}
