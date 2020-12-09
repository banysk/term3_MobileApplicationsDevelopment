using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using HWG.ViewModels;
using HWG.Views;
using HWG.Models;

namespace HWG.ViewModels
{
    class MasterPageViewModel : BaseViewModel
    {
        public MasterPageViewModel()
        {
            Data = new MasterPageModel();
            ShowWeather = new Command(ShowWeather_);
            ShowCity = new Command(ShowCity_);
            Dynamic = false;
            Animated = false;
            WeatherBorder = Color.FromHex("#2196f3");
            CityBorder = Color.Silver;
            bWeather = true;
            bWeather = false;
        }

        public bool bWeather;
        public bool bCity;
        public MasterPageModel Data;
        public ICommand ShowWeather { get; set; }
        public ICommand ShowCity { get; set; }

        public bool Dynamic
        {
            get => Data.DynamicBackground;
            set
            {
                if (Data.DynamicBackground != value)
                {
                    Data.DynamicBackground = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Animated
        {
            get => Data.Animation;
            set
            {
                if (Data.Animation != value)
                {
                    Data.Animation = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color WeatherBorder
        {
            get => Data.WeatherButtonColor;
            set
            {
                if (Data.WeatherButtonColor != value)
                {
                    Data.WeatherButtonColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color CityBorder
        {
            get => Data.CityButtonColor;
            set
            {
                if (Data.CityButtonColor != value)
                {
                    Data.CityButtonColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void ShowWeather_()
        {
            if (!bWeather)
            {
                bWeather = true;
                bCity = false;
                (Application.Current.MainPage as MasterDetailPageView)?.SetDetailPage(new WeatherPageView());
                WeatherBorder = Color.FromHex("#2196f3");
                CityBorder = Color.Silver;
            }
        }

        private void ShowCity_()
        {
            if (!bCity)
            {
                bWeather = false;
                bCity = true;
                (Application.Current.MainPage as MasterDetailPageView)?.SetDetailPage(new CityPageView() { Title = "Список городов" });
                WeatherBorder = Color.Silver;
                CityBorder = Color.FromHex("#2196f3");
            }
        }
    }
}
