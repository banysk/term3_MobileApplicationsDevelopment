using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HWG.ViewModels;

namespace HWG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPageView : ContentPage
    {
        public WeatherPageView()
        {
            InitializeComponent();
            BindingContext = new WeatherPageViewModel(getCity());
        }

        private string getCity()
        {
            if (Application.Current.Properties.ContainsKey("CurrentCity"))
            {
                return Application.Current.Properties["CurrentCity"].ToString();
            }
            else
            {
                Application.Current.Properties["CurrentCity"] = "МОСКВА";
                Application.Current.SavePropertiesAsync();
                return "МОСКВА";
            }
        }
    }
}