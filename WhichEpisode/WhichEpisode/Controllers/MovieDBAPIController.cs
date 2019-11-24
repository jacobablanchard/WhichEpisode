using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using WhichEpisode.Models;
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
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            // Get a token
            var obj = new {
                apikey = api_key,
                userkey = user_key,
                username = user_name
            };
            var json = JsonConvert.SerializeObject(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("login", data);
            if (response.IsSuccessStatusCode) {
                var tokenResponse = await response.Content.ReadAsAsync<Token>();
                token = tokenResponse.token;
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            else
                return false;
        }

        public static async Task<TVSearchResults> SearchForShow(string show) {
            TVSearchResults res = null;
            HttpResponseMessage response = await Client.GetAsync($"search/series?name={HttpUtility.UrlEncode(show)}");

            if (response.IsSuccessStatusCode) {
                res = await response.Content.ReadAsAsync<TVSearchResults>();
            }
            return res;
        }


    }
}
