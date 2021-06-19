using System;
using System.Collections.Generic;
using System.Text;

namespace DatCar.Core.Models
{
    public class CarRentalPriceRequest
    {
        public CarRentalPriceRequest(long carId, DateTime startDate, DateTime endDate)
        {
            CarId = carId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public long CarId { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
    }
}
