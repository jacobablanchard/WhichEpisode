using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WhichEpisode.Models
{
    public class SeriesSearchResult
    {
        public string[] aliases { get; set; }
        public string banner { get; set; }
        public string firstAired { get; set; }
        public int id { get; set; } = -1;
        public string image { get; set; }
        public string network { get; set; }
        public string overview { get; set; }
        public string poster { get; set; }
        public string seriesName { get; set; }
        public string slug { get; set; }
        public string status { get; set; }

        public string FullPosterURL { get {
                string temp;
                // Precedence order is image, then poster, then banner
                try {
                    if (string.IsNullOrEmpty(image) && string.IsNullOrEmpty(poster) && string.IsNullOrEmpty(banner))
                        temp = $"https://thetvdb.com/banners/images/missing/series.jpg";
                    else {
                        // Guarenteed at least 1 non-empty field
                        if (!string.IsNullOrEmpty(image))
                            temp = image;
                        else if (!string.IsNullOrEmpty(poster))
                            temp = poster;
                        else
                            temp = banner;
                    }
                    if (banner.Contains("https")) {
                        temp = banner;
                    }
                    else {
                        temp = $"https://thetvdb.com" + temp;
                    }
                    if (temp.StartsWith($"/bannershttps")) {
                        temp = banner.Replace($"/banners", "");
                    }
                }
                catch {
                    temp = $"https://thetvdb.com/banners/images/missing/series.jpg";
                    if(id!= -1)
                        Debug.WriteLine($"No image, poster, or banner data found for id {id}. Defaulting to missing");
                }
                return temp;
            } 
        }
    }
}
