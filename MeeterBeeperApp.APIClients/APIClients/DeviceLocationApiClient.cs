using MeeterBeeperApp.APIClients.APIClients.Interfaces;
using MeeterBeeperApp.APIClients.Helpers;
using MeeterBeeperApp.Data.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeeterBeeperApp.APIClients.APIClients
{
    public class DeviceLocationApiClient : BaseApiClient, IDeviceLocationApiClient
    {
        #region Interfaces

        internal interface IDeviceApi
        {
            [Post("/api/DeviceLocation/SaveLocation")]
            Task<HttpResponseMessage> SaveLocation([Body]LocationModel location);

            [Post("/api/DeviceLocation/GetNearByDevices")]
            Task<HttpResponseMessage> GetNearByDevices([Body]LocationModel location);
        }

        #endregion

        #region Constructors

        public DeviceLocationApiClient(HttpClientProvider httpClientProvider) : base(httpClientProvider)
        { }

        #endregion

        #region Public Implementation

        public async Task<bool> SaveLocation(LocationModel location)
        {
            try
            {
                if (!await IsConnectionAvailable())
                {
                    return false;
                }
                var response = await RestService
                    .For<IDeviceApi>(await GetHttpClient())
                    .SaveLocation(location);
                return await HttpResponsHelper.GetObjectFor<bool>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<LocationModel>> GetNearByDevices(LocationModel location)
        {
            try
            {
                if (!await IsConnectionAvailable())
                {
                    return new List<LocationModel>();
                }
                var response = await RestService
                    .For<IDeviceApi>(await GetHttpClient())
                    .GetNearByDevices(location);
                return await HttpResponsHelper.GetObjectFor<List<LocationModel>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<LocationModel>();
            }
        }
        #endregion
    }
}
