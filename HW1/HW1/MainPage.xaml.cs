using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace HW1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class Good
    {
        public string Name { get; set; }
        public int Count { get; set; }
        [JsonIgnore]
        public string PathToPic { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        public static SortedDictionary<string, Good> goods = new SortedDictionary<string, Good>();
        public static bool bOpened = false;
		
        public void UpdateListView()
        {
            var buf = goods.Select((a) => { return a.Value; }).ToList();
            goods_list.ItemsSource = buf;
        }
		
        public MainPage()
        {
            InitializeComponent();
            UpdateListView();
        }
		
        private void Btn_add_Clicked(object sender, EventArgs e)
        {   
            if (!bOpened)
            {
                bOpened = true;
                var cart = new AddPage();
                Navigation.PushAsync(cart);
                cart.Disappearing += (send, ev) =>
                {
                    bOpened = false;
                    UpdateListView();
                };
            }
        }
		
        async private void btn_order_Clicked(object sender, EventArgs e)
        {
            if (goods.Count > 0 && await DisplayAlert("Order", "Are you sure?", "Yes", "No"))
            {
                var json = JsonConvert.SerializeObject(goods.Values);
                goods.Clear();
                UpdateListView();
                await DisplayAlert("Order", json, "Ok");
                await Navigation.PopAsync();
            }
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Remove", "Are you sure?", "Yes", "No"))
            {
                var btn = (Xamarin.Forms.Button)sender;
                goods.Remove(btn.CommandParameter.ToString());
                UpdateListView();
            }
        }
    }
}
