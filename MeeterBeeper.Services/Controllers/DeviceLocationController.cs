using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeeterBeeperApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeeterBeeper.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceLocationController : ControllerBase
    {
        private DeviceLocationServices _services;
        public DeviceLocationController(DeviceLocationServices services)
        {
            _services = services;
        }
        // POST api/values
        [HttpPost]
        [Route("SaveLocation")]
        public async Task<bool> SaveLocation(LocationModel location)
        {
            return await _services.UpdateLocation(location);
        }

        // POST api/values
        [HttpPost]
        [Route("GetNearByDevices")]
        public async Task<List<LocationModel>> GetNearByDevices(LocationModel location)
        {
            return await _services.GetNearByDevices(location);
        }
    }
}
