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
    public partial class CityPageView : ContentPage
    {
        public CityPageView()
        {
            InitializeComponent();
            BindingContext = new CityPageViewModel();
        }

        async private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Application.Current.Properties["CurrentCity"] = list.SelectedItem;
            await Application.Current.SavePropertiesAsync();
        }
    }
}