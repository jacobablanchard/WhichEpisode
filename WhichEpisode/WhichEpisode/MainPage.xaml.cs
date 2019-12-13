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
        private bool doNavigation = true;

        List<SeriesSearchResult> TVSearchResults;
        public MainPage() {
            InitializeComponent();
            TVSearchResults = new List<SeriesSearchResult>();
            Task<bool> temp = MovieDBAPIController.Initialize();
            if (!temp.Result)
                DisplayAlert("Error","Error authenticating","OK");
            NametoSearch.SearchButtonPressed += Button_Clicked;
            this.Title = "Which Episode?";
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            SeriesSearchResults tVSearch = await MovieDBAPIController.SearchForShow(NametoSearch.Text);
            TVSearchResults = tVSearch.data.ToList();
            TVShowsList.ItemsSource = TVSearchResults;
        }

        private void ContentPage_Appearing(object sender, EventArgs e) {
            doNavigation = false;
            if (TVShowsList.SelectedItem != null)
                TVShowsList.SelectedItem = null;
            doNavigation = true;
        }

        private void TVShowsList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            if(doNavigation)
                Navigation.PushAsync(new ShowPicker((SeriesSearchResult)TVShowsList.SelectedItem));
        }
    }
}
