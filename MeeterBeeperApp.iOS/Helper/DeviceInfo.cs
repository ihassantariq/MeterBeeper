using System;
using MeeterBeeperApp.Helper;
using UIKit;

namespace MeeterBeeper.iOS.Helper
{
    public class DeviceInfo : IDeviceInfo
    {

        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.ToString();
        }
    }
}
