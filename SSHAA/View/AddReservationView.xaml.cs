using System;
using HMSDataRepo.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReservationView : ContentPage
    {
        public AddReservationView()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            await VM.AppData.Instance.RefreshRooms();
        }

        private readonly Reservations _reservation = new Reservations();
        int days;
        private bool CalculatePrice()
        {
            try
            {
                days = int.Parse(DayesBox.Text);
                if (days <= 0)
                {
                    DayesBox.Text = "1";
                    return false;
                }
                if (RoomsList.SelectedItem is Rooms room)
                {
                    TotalPriceLabel.Text = (room.Price * (days)).ToString() + " $";
                    return true;
                }
                else
                {
                    TotalPriceLabel.Text = "No room selected";
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        private async void Reserve(object sender, EventArgs e)
        {
            try
            {

                if (CalculatePrice())
                {
                    Rooms room = (RoomsList.SelectedItem as Rooms);
                    decimal price = (room.Price * (int.Parse(DayesBox.Text)));
                    

                    this._reservation.Days = days;
                    _reservation.StartDate = DateTime.Now;
                    _reservation.TotalPrice = price;
                    _reservation.Room_Id = room.ID;
                    _reservation.Person_Id = VM.AppData.Instance.CurrentLoginInfo.ID;
                    bool res = await VM.AppData.Instance.AddReservation(_reservation);
                    if (res)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Services.ServicesInterfaces.GetToastService().LongAlert("Failed Please try again later");
                    }
                }
            }
            catch (Exception ex)
            {
                Services.ServicesInterfaces.GetDialogService().ShowDialog("Oops", ex.Message);
            }
        }

        private void OnRoomSelectinChanged(object sender, SelectedItemChangedEventArgs e)
        {
            CalculatePrice();
        }

        private void OnDayesChanged(object sender, TextChangedEventArgs e)
        {
            CalculatePrice();
        }

        private void DayesBox_Unfocused(object sender, FocusEventArgs e)
        {
            int.TryParse(DayesBox.Text, out int dayes);
            if (dayes > 100)
            {
                Services.ServicesInterfaces.GetDialogService().ShowDialog("oh sorry about this",
                "Reservations are limited to 100 days you can always book another room on due date");
                DayesBox.Text = "100";
                return;
            }
            if (string.IsNullOrEmpty(DayesBox.Text) || dayes < 0)
            {
                Services.ServicesInterfaces.GetToastService().LongAlert("No days defined");
              //  DayesBox.Text = "1";
                return;
            }

        }
    }
}