using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTLIB
{
    public class TwitterResult 
    {
        public DateTime CreateDate { get; set; }
        public Int64 ID { get; set; }
        public String IdStr { get; set; }
        public String InReplyToScreenName { get; set; }
        public Int64? InReplyToStatusId { get; set; }
        public Int64? InReplyToUserId { get; set; }
        public Boolean IsFavorited { get; set; }
        public Boolean IsTruncated { get; set; }
        public String Source { get; set; }
        public String Text { get; set; }

        public String UserName { get; set; }
        public String UserScreenName { get; set; }

        public TwitterResult RetweetedStatus { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }


        public Boolean? IsPossiblySensitive { get; set; }
        public Int32 RetweetCount { get; set; }
    }
}
