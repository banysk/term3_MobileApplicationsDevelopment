using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HWG.Views;
using HWG.ViewModels;

namespace HWG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageView : MasterDetailPage
    {
        public MasterDetailPageView()
        {
            InitializeComponent();
            Master = new MasterPageView();
            Detail = new NavigationPage(new WeatherPageView()) { BarBackgroundColor = Color.FromHex("#2196f3") };
        }

        public void SetDetailPage(Page page)
        {
            Detail = new NavigationPage(page) { BarBackgroundColor = Color.FromHex("#2196f3") };
        }

        private bool checkConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}