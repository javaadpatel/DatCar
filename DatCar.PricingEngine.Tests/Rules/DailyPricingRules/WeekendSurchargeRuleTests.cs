using DatCar.Core.Models;
using DatCar.PricingEngine.Rules.DailyPricingRules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Tests.Rules.DailyPricingRules
{
    [TestFixture]
    public class WeekendSurchargeRuleTests
    {
        private WeekendSurchargeRule _sut;
        private decimal _basePricePerDay;
        private decimal _weekendSurchargePercentage;
        private string _weekendSurgechargeName = "Weekend Surcharge";

        [SetUp]
        public void Setup()
        {
            _basePricePerDay = 100m;
            _weekendSurchargePercentage = 5m;
            _sut = new WeekendSurchargeRule();
        }

        [Test]
        public async Task GivenRentalsOverTheWeekend_WhenApplyRule_ShouldModifyPerDayAndAddDailyCharge()
        {
            //arrange
            var carRental = new CarRentalPrice
            {
                PricePerDay = new Dictionary<DateTime, decimal>
                {
                    {new DateTime(2021, 06, 17), _basePricePerDay },    //Thursday
                    {new DateTime(2021, 06, 18), _basePricePerDay },    //Friday
                    {new DateTime(2021, 06, 19), _basePricePerDay },    //Saturday
                    {new DateTime(2021, 06, 20), _basePricePerDay },    //Sunday
                    {new DateTime(2021, 06, 21), _basePricePerDay }     //Monday
                }
            };

            var expectedPricePerDay = new Dictionary<DateTime, decimal>
            {
                {new DateTime(2021, 06, 17), _basePricePerDay },    //Thursday
                {new DateTime(2021, 06, 18), _basePricePerDay },    //Friday
                {new DateTime(2021, 06, 19), _basePricePerDay + _basePricePerDay * (_weekendSurchargePercentage /100.0m) },    //Saturday
                {new DateTime(2021, 06, 20), _basePricePerDay + _basePricePerDay * (_weekendSurchargePercentage /100.0m)},    //Sunday
                {new DateTime(2021, 06, 21), _basePricePerDay }     //Monday
            };

            //act
            var result = await _sut.ApplyRule(carRental);

            //assert
            Assert.IsTrue(result.DailyCharges.ContainsKey(_weekendSurgechargeName));
            Assert.AreEqual(2 * _weekendSurchargePercentage, result.DailyCharges[_weekendSurgechargeName]);

            CollectionAssert.AreEquivalent(expectedPricePerDay, result.PricePerDay);
        }


        [Test]
        public async Task GivenRentalsNotOverTheWeekend_WhenApplyRule_ShouldNotAddDailyCharge()
        {
            //arrange
            var carRental = new CarRentalPrice
            {
                PricePerDay = new Dictionary<DateTime, decimal>
                {
                    {new DateTime(2021, 06, 17), _basePricePerDay },    //Thursday
                    {new DateTime(2021, 06, 18), _basePricePerDay },    //Friday
                }
            };

            //act
            var result = await _sut.ApplyRule(carRental);

            //assert
            Assert.IsFalse(result.DailyCharges.ContainsKey(_weekendSurgechargeName));

            CollectionAssert.AreEquivalent(result.PricePerDay, result.PricePerDay);
        }
    }
}
