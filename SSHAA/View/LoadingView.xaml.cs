using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingView : ContentPage
    {
        public LoadingView()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CheckRegistration();

        }

        private async void CheckRegistration()
        {
            await Task.Run(async () =>
                       {
                           if (Services.LocalData.HasLoginData())
                           {
                               bool? res = await VM.AppData.Instance.AutoLoginAsync();
                               if (res == null)
                               {
                                   Device.BeginInvokeOnMainThread(() => { App.Current.MainPage = (new LoginView()); });
                               }
                               else
                               if (res == false)
                               {
                                   Device.BeginInvokeOnMainThread(() => { RetryButton.IsVisible = true; });
                               }
                               else
                               if (res == true && VM.AppData.Instance.IsLoggedIn)
                               {
                                   Device.BeginInvokeOnMainThread(() =>
                                   {
                                       RetryButton.IsVisible = false;
                                       App.Current.MainPage = (new Navigation.TabbedMainView());
                                   });
                               }
                           }
                           else
                           {
                               Device.BeginInvokeOnMainThread(() =>
                               {
                                   App.Current.MainPage = (new LoginView());
                               });
                           }
                       });
        }

        private void Retry(object sender, EventArgs e)
        {
            RetryButton.IsVisible = false;
            CheckRegistration();
        }
    }
}