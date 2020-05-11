using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeeterBeeperApp.Services
{
    [FirestoreData]
    public class LocationModel
    {
        [FirestoreProperty]
        public string DeviceId { get; set; }
        [FirestoreProperty]
        public double Longitude { get; set; }
        [FirestoreProperty]
        public double Latitude { get; set; }

        public int Distance { get; set; }
    }
}
