using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriciaBot
{
    public class AppData
    {
        public String ConsumerKey { get; set; }
        public String ConsumerSecret { get; set; }
        public String AccessToken { get; set; }
        public String AccessSecret { get; set; }

        public String PinCodeStartTag { get; set; }
        public Int32 PinCodeOffset { get; set; }

        public AppData()
        {
            this.PinCodeStartTag = "<code>";
            this.PinCodeOffset = 13;
        }
    }
}
