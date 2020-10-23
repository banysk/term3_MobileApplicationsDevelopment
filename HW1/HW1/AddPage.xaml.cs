﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HW1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        public AddPage()
        {
            InitializeComponent();
            picker.ItemsSource = items.Select(a => a.Name).ToList();
            picker.SelectedIndex = 0;
        }
		
        public class MyItem{
            public string Name;
            public string PathToPic;
        }

        List<MyItem> items = new List<MyItem>()
        {
            new MyItem()
            {
                Name = "Lipton",
                PathToPic = "lipton.png"
            },
            new MyItem()
            {
                Name = "Mirinda",
                PathToPic = "mirinda.png"
            },
            new MyItem()
            {
                Name = "Pepsi",
                PathToPic = "pepsi.png"
            }
        };

        private void picker_SelectedIndexChanged(object _sender, EventArgs _e)
        {
            amount.Text = "1";
            img.Source = ImageSource.FromFile(items.Where(a => a.Name == picker.SelectedItem?.ToString()).First().PathToPic);
            stepper.Value = 1;
            if (MainPage.goods.ContainsKey(picker.SelectedItem?.ToString()))
            {
                stepper.Value = MainPage.goods[picker.SelectedItem?.ToString()].Count;
                amount.Text = MainPage.goods[picker.SelectedItem?.ToString()].Count.ToString();
            }
        }

        private void stepper_ValueChanged(object _sender, ValueChangedEventArgs _e)
        {
            amount.Text = stepper.Value.ToString();
        }

        private void amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse(amount.Text, out num) && num >= 0 && num <= 100)
            {
                stepper.Value = num;
            }
            else
            {
                amount.Text = stepper.Value.ToString();
            }
        }

        async private void btn_confirm_Clicked(object sender, EventArgs e)
        {
            MainPage.goods[picker.SelectedItem?.ToString()] = new Good()
            {
                Name = picker.SelectedItem?.ToString(),
                Count = int.Parse(amount.Text),
                PathToPic = items.Where(a => a.Name == picker.SelectedItem?.ToString()).First().PathToPic
            };
            await DisplayAlert("Order", "Succesfull", "OK");
        }
    }
}