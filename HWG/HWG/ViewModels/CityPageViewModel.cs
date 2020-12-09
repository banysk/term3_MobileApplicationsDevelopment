using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using HWG.ViewModels;
using HWG.Views;
using HWG.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;

namespace HWG.ViewModels
{
    class CityPageViewModel : BaseViewModel
    {
        private string input_;
        public CityPageViewModel()
        {
            Cities = new ObservableCollection<string>();
            getCities();
            Current = getCity();
            Push = new Command(Push_);
            Input = "";
            GetCityFromLocation();
        }
        public ObservableCollection<string> Cities { get; set; }
        public string Current { get; set; }
        public string Input
        {
            get => input_;
            set
            {
                if (input_ != value)
                {
                    input_ = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ICommand Push { get; set; }
        private void getCities()
        {
            if (!App.Current.Properties.ContainsKey("Cities"))
            {
                if (Cities.Count == 0)
                {
                    Cities.Add("МОСКВА");
                }
                App.Current.Properties["Cities"] = JsonConvert.SerializeObject(Cities);
            } else
            {
                Cities = JsonConvert.DeserializeObject<ObservableCollection<string>>(App.Current.Properties["Cities"].ToString());
            }
        }

        private string getCity()
        {
            if (App.Current.Properties.ContainsKey("CurrentCity"))
            {
                return App.Current.Properties["CurrentCity"].ToString();
            }
            else
            {
                App.Current.Properties["CurrentCity"] = "МОСКВА";
                App.Current.SavePropertiesAsync();
                return "МОСКВА";
            }
        }

        async private void Push_()
        {
            try
            {
                if (Input != "" && !Cities.Contains(Input.ToUpper()))
                {
                    // Validate city name
                    var url = $"https://api.openweathermap.org/data/2.5/weather?q={Input}&APPID=23d6fed02b2ea14fcdcdab93be3632fa&lang=ru&units=metric";
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        Cities.Add(Input.ToUpper());
                        NotifyPropertyChanged("Cities");
                        App.Current.Properties["Cities"] = JsonConvert.SerializeObject(Cities);
                        await App.Current.SavePropertiesAsync();
                        // await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Успешно", "Город добавлен", "ОК");
                    }
                    else
                    {
                        await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Ошибка", "Такого города не существует", "ОК");
                    }
                }
                else
                {
                    // await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Ошибка", "Введена пустая строка / город уже есть в списке", "ОК");
                }
                Input = "";
            } catch (Exception _ex)
            {
                await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", _ex.Message.ToString(), "ОК");
            }
        }

        async void GetCityFromLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var url = $"https://api.openweathermap.org/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&APPID=23d6fed02b2ea14fcdcdab93be3632fa&lang=ru&units=metric";
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(json);
                        Input = weatherInfo.name.ToUpper();
                        // await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Успешно", "Получены координаты", "ОК");
                        Push_();
                    }
                    else
                    {
                        // await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Ошибка", "Некорректные координаты", "ОК");
                    }
                }
                else
                {
                    // await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Ошибка", "Координаты не получены", "ОК");
                }
            }
            catch (Exception _ex)
            {
                await (App.Current.MainPage as MasterDetailPageView).Detail.DisplayAlert("Error", _ex.Message.ToString(), "ОК");
            }
        }
    }
}
