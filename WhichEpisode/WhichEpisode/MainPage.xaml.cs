using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhichEpisode
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage() {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e) {
            int maxNum;
            bool res = int.TryParse(EnteredNumber.Text,out maxNum);
            if (res && maxNum >= 1) {
                Random random = new Random();
                int a = random.Next(1, maxNum + 1);
                ChosenNumber.Text = a.ToString();
            }
            else {
                DisplayAlert("Invalid Input", "Please only input a number in here", "OK");
            }
        }
    }
}
