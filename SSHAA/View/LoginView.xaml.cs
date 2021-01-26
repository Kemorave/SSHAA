using System;
using HMSDataRepo.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void TryLogin(object sender, EventArgs e)
        {
            try
            {
                bool? res = await VM.AppData.Instance.Login(EmailBox.Text, People.HashString(PasswordBox.Text));
                if (res==true)
                {
                    App.Current.MainPage = (new  Navigation.TabbedMainView());
                   // await VM.AppData.Instance.RefreshReservations();
                   // await VM.AppData.Instance.RefreshRooms();
                }
            }
            catch (Exception ex)
            {
                Services.ServicesInterfaces.GetToastService().LongAlert(ex.Message);
            }
        }

        private async void Register(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new RegisterView(), true);
        }

        private void Exit(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}