using System;
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

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            amount.Text = "0";
            img.Source = ImageSource.FromFile(items.Where(a => a.Name == picker.SelectedItem?.ToString()).First().PathToPic);
            stepper.Value = 0;
        }


    }
}