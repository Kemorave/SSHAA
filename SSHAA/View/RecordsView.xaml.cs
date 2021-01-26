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
    public partial class RecordsView : ContentPage
    {
        public RecordsView()
        {
            InitializeComponent();
        }

        private void OnRecordTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void OnRecordSelectinChanged(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}