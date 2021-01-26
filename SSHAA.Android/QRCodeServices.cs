using System;
using System.Threading.Tasks;
using Android.Graphics;
using QRCoder;
using SSHAA.Services;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(SSHAA.Droid.QRCodeServices))]

namespace SSHAA.Droid
{
    public class QRCodeServices : IQRCodeServices
    {
        public QRCodeServices()
        {
        }

        public async Task<bool> SaveImage(string v, string filename)
        {
            try
            {
                if (System.IO.Path.GetExtension(filename).ToLower() != ".png")
                    filename += ".png";
                    MainActivity.Insatance.CheckStoragePermissions();
                var directory = "/storage/emulated/0/SSH";

                System.IO.Stream outputStream = null;

                var renderer = new Xamarin.Forms.Platform.Android.StreamImagesourceHandler();
                Bitmap photo = await renderer.LoadImageAsync(GetImage(v),MainActivity.Insatance);
                var savedImageFilename = System.IO.Path.Combine(directory, filename);

                System.IO.Directory.CreateDirectory(directory);

                bool success = false;
                using (outputStream = new System.IO.FileStream(savedImageFilename, System.IO.FileMode.Create))
                {
                    if (System.IO.Path.GetExtension(filename).ToLower() == ".png")
                        success = await photo.CompressAsync(Bitmap.CompressFormat.Png, 100, outputStream);
                    else
                        success = await photo.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, outputStream);
                }

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        public ImageSource GetImage(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            return ImageSource.FromStream(() => new System.IO.MemoryStream(qrCodeBytes));
        }
    }
}