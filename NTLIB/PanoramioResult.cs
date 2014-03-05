using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTLIB
{
    public class PanoramioResult
    {
        public int count { get; set; }
        public bool has_more { get; set; }
        public MapLocation map_location { get; set; }
        public List<Photo> photos { get; set; }

        public class MapLocation
        {
            public double lat { get; set; }
            public double lon { get; set; }
            public int panoramio_zoom { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int owner_id { get; set; }
            public string owner_name { get; set; }
            public string owner_url { get; set; }
            public string photo_file_url { get; set; }
            public int photo_id { get; set; }
            public string photo_title { get; set; }
            public string photo_url { get; set; }
            public string upload_date { get; set; }
            public int width { get; set; }
            public string place_id { get; set; }
        }
    }
}
