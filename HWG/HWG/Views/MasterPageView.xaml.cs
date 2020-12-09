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
    public partial class MasterPageView : ContentPage
    {
        public MasterPageView()
        {
            InitializeComponent();
            BindingContext = new MasterPageViewModel();
        }
    }
}