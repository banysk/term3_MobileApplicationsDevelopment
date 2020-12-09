using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using HWG.Models;
using System.Net.Http;
using Newtonsoft.Json;
using HWG.Views;
using Xamarin.Essentials;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace HWG.ViewModels
{
    class WeatherItem
    {
        public string Day_ { get; set; }
        public string Date_ { get; set; }
        public string Icon_ { get; set; }
        public string Temp_ { get; set; }
        public WeatherItem()
        {
            Day_ = "";
            Date_ = "";
            Icon_ = "";
            Temp_ = "";
        }
    }

    class WeatherPageViewModel : BaseViewModel
    {
        public WeatherPageViewModel(string city)
        {
            isShared = false;
            Share = new Command(Share_);
            data = new ObservableCollection<WeatherItem>();

            var time = TimeSpan.FromMinutes(5);
            Device.StartTimer(time, () =>
            {
                GetWeatherInfo(city);
                return true;
            });
            GetWeatherInfo(city);
            GetForecast(city);
        }
        public ObservableCollection<WeatherItem> data { get; set; }
        public ICommand Share { get; set; }

        WeatherModel Weather = new WeatherModel();
        public string City
        {
            get => Weather.City;
            set
            {
                if (Weather.City != value)
                {
                    Weather.City = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Temperature
        {
            get => Weather.Temperature;
            set
            {
                if (Weather.Temperature != value)
                {
                    Weather.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Main
        {
            get => Weather.Main;
            set
            {
                if (Weather.Main != value)
                {
                    Weather.Main = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string MainSource
        {
            get => Weather.MainSource;
            set
            {
                if (Weather.MainSource != value)
                {
                    Weather.MainSource = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Date
        {
            get => Weather.Date;
            set
            {
                if (Weather.Date != value)
                {
                    Weather.Date = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Wind
        {
            get => Weather.Wind;
            set
            {
                if (Weather.Wind != value)
                {
                    Weather.Wind = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Humidity
        {
            get => Weather.Humidity;
            set
            {
                if (Weather.Humidity != value)
                {
                    Weather.Humidity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        async private void GetWeatherInfo(string city)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&APPID=23d6fed02b2ea14fcdcdab93be3632fa&lang=ru&units=metric";
                HttpClient client = new HttpClient();
                var responseMessage = await client.GetAsync(url);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", "", "ok");
                }
                else
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(json);
                    City = weatherInfo.name.ToUpper();
                    Temperature = weatherInfo.main.temp.ToString() + "°C";
                    Main = weatherInfo.weather[0].description.ToString().ToUpper();
                    MainSource = $"https://openweathermap.org/img/wn/{weatherInfo.weather[0].icon}@2x.png";
                    var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime().AddSeconds(weatherInfo.dt);
                    Date = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Local).ToString("dddd, MMM dd").ToUpper();
                    Wind = weatherInfo.wind.deg.ToString() + "° | " + weatherInfo.wind.speed.ToString();
                    Humidity = weatherInfo.main.humidity.ToString() + "%";
                }
            }
            catch (Exception _ex)
            {
                await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", _ex.Message.ToString(), "ОК");
            }
        }

        async private void GetForecast(string city)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&APPID=23d6fed02b2ea14fcdcdab93be3632fa&lang=ru&units=metric";
                HttpClient client = new HttpClient();
                var responseMessage = await client.GetAsync(url);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", "", "ok");
                }
                else
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    var forecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(json);
                    List<List> allList = new List<List>();
                    foreach (var list in forecastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);
                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    foreach (var item in allList)
                    {
                        data.Add(new WeatherItem() { 
                            Day_ = DateTime.Parse(item.dt_txt).ToString("dddd"),
                            Date_ = DateTime.Parse(item.dt_txt).ToString("dd MMM"),
                            Icon_ = $"https://openweathermap.org/img/wn/{item.weather[0].icon}@2x.png",
                            Temp_ = item.main.temp.ToString("0") + "°C"
                        });
                    }
                }
            }
            catch (Exception _ex)
            {
                await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", _ex.Message.ToString(), "ОК");
            }
        }

        bool isShared;

        async private void Share_()
        {
            if (!isShared)
            {
                isShared = true;
                string title = $"Погода в городе {City}";
                string data = $"На данный момент температура в городе {City} равна {Temperature}.\n";
                data += $"Влажность {Humidity}.\n";
                data += $"Ветер {Wind}.\n";
                data += $"Состояние погоды {Main}.\n";
                await (Application.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert(title, data, "ok");
                isShared = false;
            }
        }
    }
}
