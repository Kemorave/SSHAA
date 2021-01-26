using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(SSHAA.Droid.ToastService))]
namespace SSHAA.Droid
{
    public class ToastService : SSHAA.Services.IToastService
    {
        public void LongAlert(string message)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { 
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();});
        }

        public void ShortAlert(string message)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { 

            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();});
        }
    }
}