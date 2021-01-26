using System;

namespace SSHAA.Services
{
    public class ServicesInterfaces
    {
        public static IDialogService GetDialogService()
        {
            return Xamarin.Forms.DependencyService.Get<IDialogService>();
        }

        public static IToastService GetToastService()
        {
            return Xamarin.Forms.DependencyService.Get<IToastService>();
        }

        public static ISaveAndLoadTextFile GetSaveAndLoadTextFileService()
        {
            return Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>();
        }
        public static IDeviceServices GetDeviceServices()
        {
            return Xamarin.Forms.DependencyService.Get<IDeviceServices>();
        }
        public static IQRCodeServices GetQRCodeServices()
        {
            return Xamarin.Forms.DependencyService.Get<IQRCodeServices>();
        }
        public static IShare GetShareServices()
        {
            return Xamarin.Forms.DependencyService.Get<IShare>();
        }


    }
}