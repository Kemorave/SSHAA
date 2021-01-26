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
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            Model = new People() {   AccountType="Client"};
            InitializeComponent();
            BindingContext = Model;
        }
        public People Model { get; }
        private async void TryRegister(object sender, EventArgs e)
        {
            if (PasswordBox.Text != PasswordVerificationBox.Text)
            { 
                Services.ServicesInterfaces.GetDialogService()
                    .ShowDialog("Password mismatch", "please repeat Password");
                return;
            }
            if (await VM.AppData.Instance.RegisterUser(Model))
            {

                Services.ServicesInterfaces.GetToastService()
                    .LongAlert("Account created succesfully\n " + EmailBox.Text);
                await this.Navigation.PopModalAsync();
            }
        }
    }
}