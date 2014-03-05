using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace NTLIB
{
    public class Panoramio
    {
        public Panoramio()
        {
        }

        public String GetRandomPhoto(Double maxx, Double maxy, Double minx, Double miny)
        {
            Double xdiff = maxx - minx;
            Double ydiff = maxy - miny;
            System.Random random = new Random();
            Double r = random.NextDouble();

            Double xr_min = minx + (xdiff * r);
            Double yr_min = miny + (ydiff * r);

            String jsonResult = "";
            String url = "http://www.panoramio.com/map/get_panoramas.php?order=popularity&set=full&from=0&to=10&minx=" + xr_min.ToString() + "&miny=" + yr_min.ToString() + "&maxx=" + (xr_min+1).ToString() + "&maxy=" + (yr_min+1).ToString() + "&size=medium";
            WebRequest req = WebRequest.Create(url);
            WebResponse res = req.GetResponse();
            using(Stream stm = res.GetResponseStream())
            {
                using(StreamReader sr = new StreamReader(stm, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    jsonResult = sr.ReadToEnd();
                }
            }
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PanoramioResult>(jsonResult);

            return result.photos[random.Next(10)].photo_url;
        }
    }
}
