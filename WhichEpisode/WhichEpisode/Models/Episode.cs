using System;
using System.Collections.Generic;
using System.Text;

namespace WhichEpisode.Models
{
    public class Episode
    {
        public int airedEpisodeNumber { get; set; }
        public int airedSeason { get; set; }
        public string episodeName { get; set; }
        public string filename { get; set; }
        public int id { get; set; }
        public string overview { get; set; }

    }
}
