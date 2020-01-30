using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using WhichEpisode.Models;
using WhichEpisode.Controllers;
using Newtonsoft.Json;
using System.Web;

namespace WhichEpisode
{
    public static class MovieDBAPIController
    {
        public static HttpClient Client { get; set; }
        public static string api_key = "81CB9013-2CEF-4F41-9DFC-44FFA2BF02E5";
        public static string user_key = "5DD750053C3102.24389341";
        public static string user_name = "skittlemann";
        private static string token;
        public static async Task<bool> Initialize() {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://api.thetvdb.com");
            Client.Timeout = TimeSpan.FromSeconds(10);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            // Get a token
            if (FileController.DoesTokenFileExist()) {
                TimeSpan res = DateTime.Now - FileController.GetTokenDate();
                if (res.TotalHours < 24) {
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", FileController.GetToken());
                    HttpResponseMessage requestResult = await Client.GetAsync("refresh_token").ConfigureAwait(false);
                    if (requestResult.IsSuccessStatusCode) {
                        var newTokenResponse = await requestResult.Content.ReadAsAsync<Token>();
                        token = newTokenResponse.token;
                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        FileController.SaveToken(token);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            var obj = new {
                apikey = api_key,
                userkey = user_key,
                username = user_name
            };
            var json = JsonConvert.SerializeObject(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("login", data).ConfigureAwait(false);
            if (response.IsSuccessStatusCode) {
                var tokenResponse = await response.Content.ReadAsAsync<Token>();
                token = tokenResponse.token;
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                FileController.SaveToken(token);
                return true;
            }
            else
                return false;

        }

        public static async Task<SeriesSearchResults> SearchForShow(string show) {
            SeriesSearchResults res = null;
            HttpResponseMessage response = await Client.GetAsync($"search/series?name={HttpUtility.UrlEncode(show)}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                res = await response.Content.ReadAsAsync<SeriesSearchResults>();
            }
            return res;
        }

        public static async Task<SeriesEpisodesSummary> GetSeriesEpisodeSummary(int seriesID) {
            HttpResponseMessage response = await Client.GetAsync($"series/{seriesID}/episodes/summary").ConfigureAwait(false);
            SeriesEpisodesSummary summary = new SeriesEpisodesSummary();
            if (response.IsSuccessStatusCode) {
                SeriesEpisodeSummaryQuery res = await response.Content.ReadAsAsync<SeriesEpisodeSummaryQuery>();
                summary = res.data;

            }
            return summary;
        }

        public static async Task<List<List<Episode>>> GetEpisodes(int seriesID) {
            List<List<Episode>> returnResult = new List<List<Episode>>();
            SeriesEpisodesSummary summary = await GetSeriesEpisodeSummary(seriesID).ConfigureAwait(false);
            if (((SeriesEpisodesSummary)summary).airedSeasons.Length == 1) {
                returnResult.Add(new List<Episode>());
            }
            else {
                for (int i = 0; i <= ((SeriesEpisodesSummary)summary).airedSeasons.Length; i++) {
                    returnResult.Add(new List<Episode>());
                }
            }
            HttpResponseMessage response = await Client.GetAsync($"series/{seriesID}/episodes").ConfigureAwait(false);
            SeriesEpisodes res = new SeriesEpisodes();
            if (response.IsSuccessStatusCode) {
                res = await response.Content.ReadAsAsync<SeriesEpisodes>();
            }
            else {
                return returnResult;
            }
            while (true) {
                foreach (Episode e in res.data) {
                    try {
                        returnResult[e.airedSeason].Add(e);
                    }
                    catch {
                        Console.WriteLine();
                    }
                }
                if (!res.links.next.HasValue)
                    break;
                else {
                    response = await Client.GetAsync($"series/{seriesID}/episodes?page={res.links.next}").ConfigureAwait(false);
                    res = new SeriesEpisodes();
                    if (response.IsSuccessStatusCode) {
                        res = await response.Content.ReadAsAsync<SeriesEpisodes>();
                    }
                    else {
                        return returnResult;
                    }
                }
            }

            foreach (List<Episode> season in returnResult)
                season.Sort((a, b) => a.airedEpisodeNumber.CompareTo(b.airedEpisodeNumber));
            return returnResult;
        }
    }
}
