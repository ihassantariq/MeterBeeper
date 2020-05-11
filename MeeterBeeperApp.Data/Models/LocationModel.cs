using System;
using System.Collections.Generic;
using System.Text;

namespace MeeterBeeperApp.Data.Models
{
    public class LocationModel
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string DeviceId { get; set; }

        public double Distance { get; set; }
    }
}
