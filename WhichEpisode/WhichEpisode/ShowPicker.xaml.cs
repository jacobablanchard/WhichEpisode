using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WhichEpisode.Controllers;
using WhichEpisode.Models;


namespace WhichEpisode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowPicker : ContentPage
    {
        public SeriesSearchResult TVSeries { get; set; }
        public List<List<Episode>> Seasons { get; set; }
        public ShowPicker(SeriesSearchResult tvseries) {
            InitializeComponent();
            TVSeries = tvseries;
            Task<List<List<Episode>>> task = MovieDBAPIController.GetEpisodes(TVSeries.id);
            Seasons = task.Result;
            ShowBanner.Source = ImageSource.FromUri(new Uri(tvseries.FullBannerURL));
            NumberOfSeasons.Text = Seasons.Count.ToString();
            int numEpisodes = 0;
            foreach(List<Episode> Season in Seasons) {
                numEpisodes += Season.Count;
            }
            NumberOfEpisodes.Text = numEpisodes.ToString();

        }

        private void Button_Clicked(object sender, EventArgs e) {
            Random generator = new Random();
            int chosenSeason = generator.Next(1, Seasons.Count);
            int chosenEpisode = generator.Next(1, Seasons[chosenSeason].Count);
            EpisodeName.Text = Seasons[chosenSeason][chosenEpisode].episodeName;
            SelectedSeason.Text = chosenSeason.ToString();
            SelectedEpisode.Text = chosenEpisode.ToString();
            Overview.Text = Seasons[chosenSeason][chosenEpisode].overview;
            EpisodePicture.Source = ImageSource.FromUri(new Uri($"https://https://thetvdb.com/banners/{Seasons[chosenSeason][chosenEpisode].filename}"));

        }
    }
}