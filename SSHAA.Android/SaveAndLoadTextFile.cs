using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(SSHAA.Droid.SaveAndLoadTextFile))]
namespace SSHAA.Droid
{
    public class SaveAndLoadTextFile : SSHAA.Services.ISaveAndLoadTextFile
    {
        public void SaveText(string filename, string text)
        {
            MainActivity.Insatance.CheckStoragePermissions();
            var documentsPath = "/storage/emulated/0/";
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            try
            {
                MainActivity.Insatance.CheckStoragePermissions();
                var documentsPath = "/storage/emulated/0/";
                var filePath = Path.Combine(documentsPath, filename);
                return System.IO.File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                return null;
            } }

        public void DeleteText(string filename)
        {
            MainActivity.Insatance.CheckStoragePermissions();
            var documentsPath = "/storage/emulated/0/";
            var filePath = Path.Combine(documentsPath, filename);
             System.IO.File.Delete(filePath);
        }
    }
}