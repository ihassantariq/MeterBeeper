using Google.Cloud.Firestore;
using MeeterBeeperApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MeeterBeeper.Services
{
    public class DeviceLocationServices
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public DeviceLocationServices()
        {
            string filepath = "MeterBeeper-6073589dc43c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            projectId = "meterbeeper";
            fireStoreDb = FirestoreDb.Create(projectId);
        }
        public async Task<bool> UpdateLocation(LocationModel location)
        {
            try
            {
                if (location != null && !String.IsNullOrWhiteSpace(location.DeviceId))
                {
                    await fireStoreDb.
                        Collection("mobileLocation")
                        .Document(location.DeviceId)
                        .SetAsync(location);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<List<LocationModel>> GetNearByDevices(LocationModel location)
        {
            try
            {
                List<LocationModel> nearbyDevices = new List<LocationModel>();
                if (location != null && !String.IsNullOrWhiteSpace(location.DeviceId))
                {
                    var items = await fireStoreDb.
                         Collection("mobileLocation")
                         .ListDocumentsAsync().ToListAsync();
                   foreach (var item in items)
                   {
                        if (item.Id.Equals(location.DeviceId))
                        {
                            continue;
                        }
                        var currentdocument =await item.GetSnapshotAsync();
                        LocationModel locationModel = currentdocument.ConvertTo<LocationModel>();
                        Location currentUserLocation = new Location(location.Latitude, location.Longitude);
                        Location otherUserLocation = new Location(locationModel.Latitude, locationModel.Longitude);
                        double km = Location.CalculateDistance(currentUserLocation, otherUserLocation, DistanceUnits.Kilometers);
                        int distance = (int) km * 1000;
                        if( distance <= location.Distance)
                        {
                            nearbyDevices.Add(locationModel);
                        }
                        
                    }
                    return nearbyDevices;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
