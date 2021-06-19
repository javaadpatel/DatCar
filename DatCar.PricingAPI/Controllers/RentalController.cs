using DatCar.Core.Contracts;
using DatCar.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatCar.PricingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalController> _logger;

        public RentalController(IRentalService rentalService, ILogger<RentalController> logger)
        {
            _rentalService = rentalService ?? throw new ArgumentNullException(nameof(rentalService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<CarRentalPrice> Get(long carId, DateTime startDate, DateTime endDate)
        {
            var carRentalRequest = new CarRentalPriceRequest(carId, startDate, endDate);
            
            //TODO: configure validator using FluentValidations

            return await _rentalService.GetRentalPriceAsync(carRentalRequest);
        }
    }
}
