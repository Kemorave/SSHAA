using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMSDataRepo.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
        }
        private void ShowInfoPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HotelNavigationView());
        }
        private void OnReservationTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Reservations re)
            {
                if (!re.IsAvailable)
                {
                    Services.ServicesInterfaces.GetDialogService().ShowDialog("Reservation not available",
                        "This reservation has expired");
                    return;
                }
                Navigation.PushAsync(new FullReservationView(re));
            }
        }
        private void EditAccount(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new EditAccountView(VM.AppData.Instance.CurrentLoginInfo));
        }
        private async void DeleteAccount(object sender, EventArgs e)
        {
         await   Navigation.PushAsync(new AccountDeleteView());
        }
        private void Logout(object sender, EventArgs e)
        {
            try
            {
                VM.AppData.Instance.Logout();
                VM.AppData.RefreshInstanse();

                App.Current.MainPage = new LoginView();
            }
            catch (Exception)
            {

            }
        }

        private void OpenComplains(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ComplainsView());

        }
    }
}