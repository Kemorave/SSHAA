using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SSHAA.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(SSHAA.Droid.DeviceServices))]

namespace SSHAA.Droid
{
    public class DeviceServices : IDeviceServices
    { 
        public DeviceServices()
        { 
            //Environment.
        }
        public string GetDeviceName()
        {
            return Android.OS.Build.Model + " OS "+ DeviceInfo.Platform;
        }
    }
}