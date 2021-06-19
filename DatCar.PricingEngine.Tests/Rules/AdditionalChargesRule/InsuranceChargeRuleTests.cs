using DatCar.Core.Models;
using DatCar.PricingEngine.Rules.AdditionalChargesRules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatCar.PricingEngine.Tests.Rules.AdditionalChargesRule
{
    [TestFixture]
    public class InsuranceChargeRuleTests
    {
        private InsuranceChargeRule _sut;
        private decimal _basePricePerDay;
        private decimal _insuranceCharge;
        private string _ruleName = "Insurance";

        [SetUp]
        public void Setup()
        {
            _basePricePerDay = 100m;
            _insuranceCharge = 10m;
            _sut = new InsuranceChargeRule();
        }

        [Test]
        public async Task GivenPricePerDay_WhenApplyRule_ShouldCalculateInsuranceCostAndAddAdditionalCharge()
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

            var totalDailyCharges = carRental.PricePerDay.Sum(x => x.Value);

            //act
            var result = await _sut.ApplyRule(carRental);

            //assert
            Assert.IsTrue(result.AdditionalCharges.ContainsKey(_ruleName));
            Assert.AreEqual(totalDailyCharges * (_insuranceCharge / 100.0m)  , result.AdditionalCharges[_ruleName]);
        }
    }
}
