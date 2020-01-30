using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WhichEpisode.Controllers;
using WhichEpisode.Models;
using System.Diagnostics;

namespace WhichEpisode
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool doNavigation = true;

        List<SeriesSearchResult> TVSearchResults;
        public Command FavoritesButtonCommand { get; set; }
        public MainPage() {
            InitializeComponent();
            TVSearchResults = new List<SeriesSearchResult>();
            Task<bool> temp = MovieDBAPIController.Initialize();
            if (!temp.Result)
                DisplayAlert("Error","Error authenticating","OK");
            NametoSearch.SearchButtonPressed += Button_Clicked;
            ToolbarFavoritesButton.Clicked += ShowFavorites;
            this.Title = "Which Episode?";
            Debug.WriteLine("Saving files to " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
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
        private void ShowFavorites(object sender, EventArgs e) {
            doNavigation = false;
            List<SeriesSearchResult> temp = FileController.GetFavorites();
            if (temp == null)
                DisplayAlert("No Favorites", "You do not have any favorites saved", "OK");
            else {
                TVShowsList.ItemsSource = temp;
            }
            doNavigation = true;
        }

    }
}
