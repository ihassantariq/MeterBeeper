using System;
using MeeterBeeperApp.Data.Models;

namespace MeeterBeeperApp.Helper
{
    public interface IGetLocation
    {
        void StartGetLocation(Action<LocationModel> LocationUpdated);
    }
}
