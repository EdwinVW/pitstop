using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.VehicleManagement.Model
{
    public class Vehicle
    {
        public string LicenseNumber { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string OwnerId { get; set; }
    }
}
