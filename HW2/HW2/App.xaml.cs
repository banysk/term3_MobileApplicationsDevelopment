using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace HW2
{
    public partial class App : Application
    {
        // Variables
        public static int globalID = 0;

        public App()
        {
            InitializeComponent();
            var page = new NavigationPage(new MainPage());
            page.BarBackgroundColor = Color.FromHex("#338af9");
            MainPage = page;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
