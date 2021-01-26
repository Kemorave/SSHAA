using System;
using HMSDataRepo.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationsView : ContentPage
    {
        public ReservationsView()
        {
            InitializeComponent();
        }

     

        private void OnReservationTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Reservations re)
            {
                Open(re);
            }
        }
        private void Open(Reservations re)
        {
            if (!re.IsAvailable)
            {
                Services.ServicesInterfaces.GetDialogService().ShowDialog("Reservation not available",
                    "This reservation has expired");
                return;
            }
            Navigation.PushAsync(new FullReservationView(re));
        }
        private void BookARoom(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddReservationView());
        }

        private async void DeleteReservation(object sender, EventArgs e)
        {
            if (await this.DisplayActionSheet("Are you sure ?", "Cancel", "Delete") == "Delete")
            {
                if (((sender) as MenuItem)?.BindingContext is Reservations reservations)
                {
                    await VM.AppData.Instance.DeleteReservation(reservations);
                }
            }
        }

        private void ShowReservation(object sender, EventArgs e)
        {
            if (((sender) as MenuItem)?.BindingContext is Reservations reservations)
            {
                Open(reservations);
            }

        }
    }
}