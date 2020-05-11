using MeeterBeeperApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeeterBeeperApp.APIClients.APIClients.Interfaces
{
    public interface IDeviceLocationApiClient
    {
        Task<bool> SaveLocation(LocationModel location);
        Task<List<LocationModel>> GetNearByDevices(LocationModel location);
    }
}
