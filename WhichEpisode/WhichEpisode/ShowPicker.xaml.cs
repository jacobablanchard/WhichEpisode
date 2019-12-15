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
            Task temp = initEpisodes();
            temp.Wait();
            //List<List<Episode>> task = await MovieDBAPIController.GetEpisodes(TVSeries.id);
            //Seasons = task.Result;
            ShowBanner.Source = ImageSource.FromUri(new Uri(tvseries.FullBannerURL));
            NumberOfSeasons.Text = Seasons.Count.ToString();
            int numEpisodes = 0;
            foreach(List<Episode> Season in Seasons) {
                numEpisodes += Season.Count;
            }
            NumberOfEpisodes.Text = numEpisodes.ToString();
        }

        private async Task initEpisodes() {
            Seasons = await MovieDBAPIController.GetEpisodes(TVSeries.id).ConfigureAwait(false);
        }

        private void Button_Clicked(object sender, EventArgs e) {
            Random generator = new Random();
            int lowerSeasonBound;
            if (IncludeNonStandardEpisodes.IsToggled) {
                if (Seasons[0].Count > 0)
                    lowerSeasonBound = 0;
                else
                    lowerSeasonBound = 1;
            }
            else {
                lowerSeasonBound = 1;
            }
            int chosenSeason = generator.Next(lowerSeasonBound, Seasons.Count);
            int chosenEpisode = generator.Next(0, Seasons[chosenSeason].Count);
            while(Seasons[chosenSeason].Count == 0) {
                chosenSeason = generator.Next(lowerSeasonBound, Seasons.Count);
                chosenEpisode = generator.Next(0, Seasons[chosenSeason].Count);
            }
            EpisodeName.Text = Seasons[chosenSeason][chosenEpisode].episodeName;
            SelectedSeason.Text = (chosenSeason).ToString();
            SelectedEpisode.Text = (chosenEpisode+1).ToString();
            Overview.Text = Seasons[chosenSeason][chosenEpisode].overview;
            EpisodePicture.Source = ImageSource.FromUri(new Uri("https://thetvdb.com/banners/" +$"{Seasons[chosenSeason][chosenEpisode].filename}"));
        }
    }
}