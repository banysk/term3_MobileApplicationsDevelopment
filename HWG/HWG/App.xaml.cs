using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HWG.Views;

namespace HWG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MasterDetailPageView();
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
