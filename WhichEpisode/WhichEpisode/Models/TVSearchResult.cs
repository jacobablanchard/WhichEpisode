using System;
using System.Collections.Generic;
using System.Text;

namespace WhichEpisode.Models
{
    public class TVSearchResult
    {
        public string[] aliases { get; set; }
        public string banner { get; set; }
        public string firstAired { get; set; }
        public int id { get; set; }
        public string network { get; set; }
        public string overview { get; set; }
        public string seriesName { get; set; }
        public string slug { get; set; }
        public string status { get; set; }

        public string FullBannerURL { get { return $"https://api.thetvdb.com" + banner; } }
    }
}
