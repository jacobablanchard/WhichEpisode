using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using WhichEpisode.Models;

namespace WhichEpisode.Controllers
{
    public static class FileController
    {

        public static bool DoesTokenFileExist() {
            //Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt"));
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            return File.Exists(fileName);
        }
        public static void SaveToken(string token) {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            DateTime now = DateTime.Now;
            File.WriteAllText(fileName, now.ToString() + "\n" + token);
        }

        public static DateTime GetTokenDate() {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            string contents = File.ReadAllText(fileName);
            string[] split = contents.Split('\n');
            return DateTime.Parse(split[0]);
        }

        public static string GetToken() {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            string contents = File.ReadAllText(fileName);
            string[] split = contents.Split('\n');
            return split[1];
        }

        public static void DeleteFavorite(SeriesSearchResult tVSeries) {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.json");
            List<SeriesSearchResult> favoriteSeries;
            if (!File.Exists(fileName))
                return;
            using (StreamReader file = File.OpenText(fileName)) {
                JsonSerializer serializer = new JsonSerializer();
                favoriteSeries = (List<SeriesSearchResult>)serializer.Deserialize(file, typeof(List<SeriesSearchResult>));
            }
            if (favoriteSeries == null)
                return;
            if (favoriteSeries.Count == 0 || favoriteSeries.FindIndex((x) => x.id == tVSeries.id) == -1) {
                return;
            }
            else {
                favoriteSeries.RemoveAt(favoriteSeries.FindIndex((x) => x.id == tVSeries.id));
                string output = JsonConvert.SerializeObject(favoriteSeries);
                File.WriteAllText(fileName, output);
                return;
            }
        }

        /// <summary>
        /// Adds the new favorite to the user's list of favorites. Returns true if it was added or was already there, and false if it wasn't added
        /// </summary>
        /// <param name="newFavorite">The new favorite to be added</param>
        public static bool SaveFavorite(SeriesSearchResult newFavorite) {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.json");
            List<SeriesSearchResult> favoriteSeries;
            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();
            using(StreamReader file = File.OpenText(fileName)) {
                JsonSerializer serializer = new JsonSerializer();
                favoriteSeries = (List<SeriesSearchResult>)serializer.Deserialize(file, typeof(List<SeriesSearchResult>));
            }
            if (favoriteSeries == null)
                favoriteSeries = new List<SeriesSearchResult>();
            if (favoriteSeries.Count == 0 || favoriteSeries.FindIndex((x) => x.id == newFavorite.id) == -1) {
                favoriteSeries.Add(newFavorite);
                string output = JsonConvert.SerializeObject(favoriteSeries);
                File.WriteAllText(fileName, output);
                return true;
            }
            else
                return false;
        }

        public static List<SeriesSearchResult> GetFavorites() {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.json");
            List<SeriesSearchResult> favoriteSeries;
            if (!File.Exists(fileName))
                return null;
            using (StreamReader file = File.OpenText(fileName)) {
                JsonSerializer serializer = new JsonSerializer();
                favoriteSeries = (List<SeriesSearchResult>)serializer.Deserialize(file, typeof(List<SeriesSearchResult>));
            }
            return favoriteSeries;
        }
    }
}
