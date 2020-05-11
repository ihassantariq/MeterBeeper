using System;
using MeeterBeeperApp.Helper;

namespace MeeterBeeperApp.Droid.Helper
{
    public class DeviceInfo : IDeviceInfo
    {
        public string GetDeviceId()
        {
            return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
    }
}
