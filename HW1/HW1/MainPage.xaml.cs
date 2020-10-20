using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HW1
{
    public partial class MainPage : ContentPage
    {

        public class Good
        {
            public string Name { get; set; }
            public string Count { get; set; }
            public string PathToPic { get; set; }
        }

        public static SortedDictionary<string, Good> goods = new SortedDictionary<string, Good>();

        public static bool bOpened = false;
        public MainPage()
        {
            InitializeComponent();
            var buf = goods.Select((a) => { return a.Value; }).ToList();
            goods_list.ItemsSource = buf;
        }

        private void Btn_add_Clicked(object sender, EventArgs e)
        {   
            if (!bOpened)
            {
                bOpened = true;
                var cart = new AddPage();
                Navigation.PushAsync(cart);
                cart.Disappearing += (send, ev) => bOpened = false;
            }
        }
    }
}
