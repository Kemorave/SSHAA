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
    public partial class AccountDeleteView : ContentPage
    {
        public AccountDeleteView()
        {
            InitializeComponent();
        }

        private async void TryDelete(object sender, EventArgs e)
        {
            if (await VM.AppData.Instance.DeleteAccount(password.Text))
            {
                await Navigation.PopAsync();
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = (new LoginView());
                });  // App.Current.MainPage = ;
            }
        }

    } 
}