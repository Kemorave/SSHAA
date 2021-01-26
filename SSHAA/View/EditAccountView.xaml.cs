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
    public partial class EditAccountView : ContentPage
    {
        public EditAccountView(People people)
        {
            originHash = people.PasswordHash;
            Model = people.Clone() as People;
            Model.PasswordHash = originHash;
            InitializeComponent();
        }

        readonly string originHash;
        public People Model { get; }

        private async void TryChange(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentPasswordVerificationBox.Text))
                {
                    Services.ServicesInterfaces.GetDialogService()
                                        .ShowDialog("Empty Password", "please enter current account password to coninue");
                    return;
                }
                if (!People.HashString(CurrentPasswordVerificationBox.Text).Equals(originHash, StringComparison.Ordinal))
                {
                    Services.ServicesInterfaces.GetDialogService()
                                        .ShowDialog("Incorect Password", "please enter current account password to coninue");
                    return;
                }
                if (!string.IsNullOrEmpty(PasswordBox.Text) || !string.IsNullOrEmpty(PasswordVerificationBox.Text))
                {
                    if (PasswordBox.Text != PasswordVerificationBox.Text)
                    {
                        Services.ServicesInterfaces.GetDialogService()
                            .ShowDialog("New Password mismatch", "please repeat Password on both entries");
                        return;
                    }
                }
                else
                {
                    Model.PasswordHash = originHash; 
                }
                if (await VM.AppData.Instance.EditAccount(Model))
                {
                    await this.Navigation.PopModalAsync();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}