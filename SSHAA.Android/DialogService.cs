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
using SSHAA.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SSHAA.Droid.DialogService))]
namespace SSHAA.Droid
{


    public class DialogService : IDialogService
    {
        public void ShowDialog(string title, string message)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
            using (var alertDialog = new AlertDialog.Builder(MainActivity.Insatance))
            {
                alertDialog.SetMessage(message);
                alertDialog.SetTitle(title);
                    alertDialog.SetPositiveButton("Ok", (s, a) => { });

                    alertDialog.Show();
            } });
        }
        public bool ShowDialog(string title, string message, string confirmButton, string rejectButton)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(()=> { });
            bool result = false;
            using (var alertDialog = new AlertDialog.Builder(MainActivity.Insatance))
            {
                alertDialog.SetMessage(message);
                alertDialog.SetPositiveButton(confirmButton, (s, a) => { result = true; });
                alertDialog.SetNegativeButton(rejectButton, (s, a) => { result = false; });
                alertDialog.SetTitle(title);
                alertDialog.Show();
                return result;
            }
        }
    }
}