using System;
using System.Threading.Tasks;
using HMSDataRepo.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullReservationView : ContentPage
    {
        public FullReservationView(Reservations reservation)
        {
            Reservation = reservation;
            QRContent = new HMSDataRepo.Controllers.QRCodeData(Reservation.ID, VM.AppData.Instance.CurrentLoginInfo.ID, VM.AppData.Instance.CurrentLoginInfo.PasswordHash);

            InitializeComponent();
            Title = ImageName;
            BindingContext = reservation;
            GenerateQRCode();
          //  DaysLeftLabel.Text = (reservation.Days - System.Convert.ToInt32(DateTime.Now.Subtract(reservation.StartDate)
            //    .TotalDays)) + " day/s left";
        }

        private readonly HMSDataRepo.Controllers.QRCodeData QRContent;
        private void GenerateQRCode()
        {
            QrLabel.Text = "Generating ... please wait";
            Task.Run(() =>
            {
                try
                {
                    ImageSource image = Services.ServicesInterfaces.GetQRCodeServices().GetImage(QRContent.GetData());
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        QRImage.Source = image;
                        QrLabel.Text = "QR code";
                     });
                }
                catch (Exception e)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        QrLabel.Text = "Unexpected error occured";
                        Services.ServicesInterfaces.GetDialogService().ShowDialog("Error generating QRCode please contact adminstrator", e.Message);
                    });
                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Aind.IsRunning = false;
                    });
                }
            });
        }


        public Reservations Reservation { get; }
        public string ImageName => "Reservation " + Reservation.ID;
        private async void SaveToGallery(object sender, EventArgs e)
        {
            if (await Services.ServicesInterfaces.GetQRCodeServices().SaveImage(QRContent.GetData(), ImageName))
            {
                Services.ServicesInterfaces.GetToastService().LongAlert("Saved to gallery as " + ImageName);
            }
            else
            {
                Services.ServicesInterfaces.GetToastService().LongAlert("Savig failed .... Perrmissions problem");
            }
        }

        private async void ShareImage(object sender, EventArgs ev)
        {
            try
            {
                if (await this.DisplayAlert("Please be carful", "this image will grant access to a room that you booked sharing it should only be done with intrusted parties", "OK", "Cancel"))
                {
                    await Services.ServicesInterfaces.GetQRCodeServices().SaveImage(QRContent.GetData(), ImageName);
                    await Services.ServicesInterfaces.GetShareServices().Show(ImageName,"QRCode sharing","/storage/emulated/0/SSH/" + ImageName + ".png");
                }
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    // QrLabel.Text = "Unexpected error occured";
                    Services.ServicesInterfaces.GetDialogService().ShowDialog("Error occured ", e.Message);
                });
            }
        }
    }
}