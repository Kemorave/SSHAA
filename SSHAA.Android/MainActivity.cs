using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Java.Interop;
using Android.Support.V4.App;

namespace SSHAA.Droid
{
    [Activity(Label = "Self serving hotel", Icon = "@drawable/ssh", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Insatance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Insatance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //   DLToolkit.Forms.Controls.FlowListView.Init();
            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
       // [Export]
        public bool CheckPermissionGranted(string Permissions)
        {
            // Check if the permission is already available.
            if (ActivityCompat.CheckSelfPermission(this, Permissions) != Permission.Granted)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 
        readonly string[] ExternalStoragePermissions =
        {
                        //TODO add more permissions
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.ReadExternalStorage,
         };
        public void CheckStoragePermissions()
        {
            if (!(CheckPermissionGranted(Manifest.Permission.WriteExternalStorage) &&
            CheckPermissionGranted(Manifest.Permission.ReadExternalStorage)))
            {
                RequestStoragePermission();
            }
        }
        private void RequestStoragePermission()
        {
            ActivityCompat.RequestPermissions(this, ExternalStoragePermissions, 0);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}