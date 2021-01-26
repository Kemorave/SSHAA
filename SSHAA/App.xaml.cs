using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SSHAA.Services;
using SSHAA.View;

namespace SSHAA
{
    public partial class App : Application
    {

        public App()
        {
            Kemorave.Threading.MainUIThreadIInvoker = new SSHAA.Services.MainUIThreadIInvoker();
            Device.SetFlags(new string[] { "Brush_Experimental" });
          
            InitializeComponent();
     // DLToolkit.Forms.Controls.FlowListView.Init();
        MainPage = new LoadingView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
