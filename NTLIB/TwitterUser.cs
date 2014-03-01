using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTLIB
{
    public class TwitterUser
    {
        public string Description {get;set;}
        public int FollowersCount { get; set; }
        public long Id { get; set; }
        public bool? IsProtected { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ScreenName { get; set; }
        public TwitterStatusLight Status { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsGeoEnabled { get; set; }
        public bool IsProfileBackgroundTiled { get; set; }
        public string ProfileBackgroundColor { get; set; }
        public string ProfileBackgroundImageUrl { get; set; }
        public string ProfileLinkColor { get; set; }
        public string ProfileSidebarBorderColor { get; set; }
        public string ProfileSidebarFillColor { get; set; }
        public string ProfileTextColor { get; set; }
        public int StatusesCount { get; set; }
        public int FriendsCount { get; set; }
        public int FavouritesCount { get; set; }
        public int ListedCount { get; set; }
        public string TimeZone { get; set; }
        public string UtcOffset { get; set; }
        public string Language { get; set; }
        public bool? FollowRequestSent { get; set; }
        public bool? IsTranslator { get; set; }
        public bool? ContributorsEnabled { get; set; }
        public bool? DefaultProfile { get; set; }
        public string ProfileBackgroundImageUrlHttps { get; set; }
        public string ProfileImageUrlHttps { get; set; }
    }
}
