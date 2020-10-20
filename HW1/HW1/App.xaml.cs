using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HW1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var page = new NavigationPage(new MainPage());
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
