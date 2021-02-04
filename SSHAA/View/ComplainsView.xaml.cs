using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSHAA.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComplainsView : ContentPage
    {
        public ComplainsView()
        {
            InitializeComponent();
        }

        private async void SubmitReport(object sender, EventArgs e)
        {
            if (await VM.AppData.Instance.SendComplaint(this.ComplaintReportBox.Text))
            {
               await Navigation.PopModalAsync();
            }
        }
    }
}