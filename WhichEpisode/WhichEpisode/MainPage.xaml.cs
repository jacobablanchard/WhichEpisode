using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WhichEpisode.Controllers;
using WhichEpisode.Models;

namespace WhichEpisode
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        List<TVSearchResult> TVSearchResults;
        public MainPage() {
            InitializeComponent();
            TVSearchResults = new List<TVSearchResult>();
            Task<bool> temp = MovieDBAPIController.Initialize();
            if (!temp.Result)
                DisplayAlert("Error","Error authenticating","OK");
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            TVSearchResults tVSearch = await MovieDBAPIController.SearchForShow(NametoSearch.Text);
            TVSearchResults = tVSearch.data.ToList();
            TVShowsList.ItemsSource = TVSearchResults;
        }
    }
}
