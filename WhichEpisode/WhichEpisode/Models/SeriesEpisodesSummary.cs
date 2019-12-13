using System;
using System.Collections.Generic;
using System.Text;

namespace WhichEpisode.Models
{
    public class SeriesEpisodesSummary
    {
        public string airedEpisodes { get; set; }
        public string[] airedSeasons { get; set; }
        public string dvdEpisodes { get; set; }
        public string[] dvdSeasons { get; set; }

    }
}
