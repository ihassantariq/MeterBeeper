using System;
using CoreLocation;
using MeeterBeeperApp.Data.Models;
using MeeterBeeperApp.Helper;

namespace MeeterBeeper.iOS.Helper
{
    public class GetLocation : IGetLocation
    {
        private Action<LocationModel> _locationUpdated;
        public void StartGetLocation(Action<LocationModel> LocationUpdated)
        {
            var Manager = new LocationManager();
            Manager.StartLocationUpdates();
            Manager.LocationUpdated += Manager_LocationUpdated;
            _locationUpdated = LocationUpdated;
        }

        private void Manager_LocationUpdated(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation location = e.Location;
            _locationUpdated?.Invoke(new LocationModel { Longitude = location.Coordinate.Longitude, Latitude = location.Coordinate.Latitude });
        }
    }
}
