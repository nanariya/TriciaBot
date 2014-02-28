using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TweetSharp;

namespace NTLIB
{
    /// <summary>
    /// ε＝└(┐卍՞ةڼ◔)卍
    /// </summary>
    public class Twitter
    {
        private TwitterService _TwitterService = null;
        private OAuthRequestToken _RequestToken = null;

        public Boolean isAuthed { get; private set; }
        public String ConsumerKey { get; private set; }
        public String ConsumerSecret { get; private set; }
        public String AccessToken { get; private set; }
        public String AccessSecret { get; private set; }

        public List<TwitterStatusLight> HomeTimeLine { get; private set; }

        private String PinCodeStartTag = "<CODE>";
        private Int32 PinCodeOffset = 13;

        //ユーザーストリームのPushを受け取ったときのイベント用
        public delegate void TwitterReceiveStatusEventHandler(TwitterStatusLight result);
        public event TwitterReceiveStatusEventHandler TwitterReceiveStatusEvent;

        public Twitter(String consumerKey, String consumerSecret)
        {
            this.isAuthed = false;
            _TwitterService = new TwitterService(consumerKey, consumerSecret);
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

        /// <summary>
        /// オレが└(՞ةڼ◔)」コンストラクタだ
        /// </summary>
        /// <param name="consumerKey">Sweet Rain♪L( ＾ω＾ )┘♪</param>
        /// <param name="consumerSecret">降りだした雨♪└( ＾ω＾ )」♪</param>
        /// <param name="accessToken">傘なんて♪L( ＾ω＾ )┘♪</param>
        /// <param name="accessSecret">必要♪└( ＾ω＾ )」♪無いから♪L( ＾ω＾ )┘♪</param>
        public Twitter(String consumerKey, String consumerSecret, String accessToken, String accessSecret)
        {
            this.isAuthed = true;
            _TwitterService = new TwitterService(consumerKey, consumerSecret);
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            _TwitterService.AuthenticateWith(this.AccessToken, this.AccessSecret);
            this.AccessToken = accessToken;
            this.AccessSecret = accessSecret;
        }

        private NTLIB.TwitterUser ConvertUser(TweetSharp.TwitterUser suser)
        {
            NTLIB.TwitterUser user = new TwitterUser();
            if (suser == null) return user;
            user.ContributorsEnabled = suser.ContributorsEnabled;
            user.CreatedDate = suser.CreatedDate;
            user.DefaultProfile = suser.IsDefaultProfile;
            user.Description = suser.Description;
            user.FavouritesCount = suser.FavouritesCount;
            user.FollowersCount = suser.FollowersCount;
            user.FollowRequestSent = suser.FollowRequestSent;
            user.FriendsCount = suser.FriendsCount;
            user.Id = suser.Id;
            user.IsGeoEnabled = suser.IsGeoEnabled;
            user.IsProfileBackgroundTiled = suser.IsProfileBackgroundTiled;
            user.IsProtected = suser.IsProtected;
            user.IsTranslator = suser.IsTranslator;
            user.IsVerified = suser.IsVerified;
            user.Language = suser.Language;
            user.ListedCount = suser.ListedCount;
            user.Location = suser.Location;
            user.Name = suser.Name;
            user.ProfileBackgroundColor = suser.ProfileBackgroundColor;
            user.ProfileBackgroundImageUrl = suser.ProfileBackgroundImageUrl;
            user.ProfileBackgroundImageUrlHttps = suser.ProfileBackgroundImageUrlHttps;
            user.ProfileImageUrl = suser.ProfileImageUrl;
            user.ProfileImageUrlHttps = suser.ProfileImageUrlHttps;
            user.ProfileLinkColor = suser.ProfileLinkColor;
            user.ProfileSidebarBorderColor = suser.ProfileSidebarBorderColor;
            user.ProfileSidebarFillColor = suser.ProfileSidebarFillColor;
            user.ProfileTextColor = suser.ProfileTextColor;
            user.ScreenName = suser.ScreenName;
            user.Status = ConvertResult(suser.Status);
            user.StatusesCount = suser.StatusesCount;
            user.TimeZone = suser.TimeZone;
            user.Url = suser.Url;
            user.UtcOffset = suser.UtcOffset;
            return user;
        }
        /// <summary>
        /// 外部から隠匿するためにTweetSharpのTwitterStatusから最低限だけ抜き出す
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private TwitterStatusLight ConvertResult(TwitterStatus row)
        {

            TwitterStatusLight res = new TwitterStatusLight();

            if (row != null)
            {
                res.CreateDate = row.CreatedDate;
                res.ID = row.Id;
                res.IdStr = row.IdStr;
                res.InReplyToScreenName = row.InReplyToScreenName;
                res.InReplyToUserId = row.InReplyToUserId;
                res.IsFavorited = row.IsFavorited;
                res.IsPossiblySensitive = row.IsPossiblySensitive;
                res.IsTruncated = row.IsTruncated;
                if (row.Location != null)
                {
                    res.Latitude = row.Location.Coordinates.Latitude;
                    res.Longitude = row.Location.Coordinates.Longitude;
                }
                res.RetweetCount = row.RetweetCount;
                res.RetweetedStatus = ConvertResult(row.RetweetedStatus);
                res.Source = row.Source;
                res.Text = row.Text;
                if (row.User != null)
                {
                    res.UserName = row.User.Name;
                    res.UserScreenName = row.User.ScreenName;
                    res.UserId = row.User.Id;
                }
            }
            return res;
        }

        public void AuthenticateWith(String accessToken, String accessSecret)
        {
            this.AccessToken = accessToken;
            this.AccessSecret = accessSecret;
            _TwitterService.AuthenticateWith(this.AccessToken, this.AccessSecret);
        }

        public Uri GetAuthURL()
        {
            _RequestToken = _TwitterService.GetRequestToken();
            return _TwitterService.GetAuthenticationUrl(_RequestToken);
        }
        public void PinCodeAuth(String pinCode)
        {
            try
            {
                OAuthAccessToken accessToken = _TwitterService.GetAccessToken(_RequestToken, pinCode);
                this.AccessToken = accessToken.Token;
                this.AccessSecret = accessToken.TokenSecret;
                this.isAuthed = true;
            }
            catch(Exception)
            {
            }
        }
        public String GetPinCodeFromHTML(String html)
        {
            String pinCode = "";

            if (0 < html.IndexOf(this.PinCodeStartTag))
            {
                int loc = html.IndexOf(this.PinCodeStartTag);
                pinCode = html.Substring(loc, this.PinCodeOffset);
                pinCode = pinCode.Replace(this.PinCodeStartTag, "");
            }

            return pinCode;
        }

        public void SendTweet(String message)
        {
            TwitterStatus result = _TwitterService.SendTweet(new SendTweetOptions { Status = message });
        }
        public void SendTweetToReply(String message, Int64 toReplyId)
        {
            TwitterStatus result = _TwitterService.SendTweet(new SendTweetOptions { Status = message, InReplyToStatusId = toReplyId });
        }


        public List<TwitterStatusLight> ListHomeTimeline()
        {
            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());

            List<TwitterStatusLight> convertResponse = new List<TwitterStatusLight>();

            foreach (TwitterStatus row in response)
            {
                TwitterStatusLight res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }

        public List<TwitterStatusLight> ListHomeTimeline(Int64 LastID)
        {
            ListTweetsOnHomeTimelineOptions option = new ListTweetsOnHomeTimelineOptions();
            option.SinceId = LastID;

            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsOnHomeTimeline(option);

            List<TwitterStatusLight> convertResponse = new List<TwitterStatusLight>();

            foreach(TwitterStatus row in response)
            {
                TwitterStatusLight res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }

        public List<TwitterStatusLight> ListReplyTimeline()
        {
            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions());

            List<TwitterStatusLight> convertResponse = new List<TwitterStatusLight>();

            foreach (TwitterStatus row in response)
            {
                TwitterStatusLight res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }
        public List<TwitterStatusLight> ListReplyTimeline(Int64 LastID)
        {
            ListTweetsMentioningMeOptions option = new ListTweetsMentioningMeOptions();
            option.SinceId = LastID;

            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsMentioningMe(option);

            List<TwitterStatusLight> convertResponse = new List<TwitterStatusLight>();

            foreach (TwitterStatus row in response)
            {
                TwitterStatusLight res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }

        public void ListHomeTimelineLoop()
        {
            Int32 maxStreamEvents = 5;
            var block = new AutoResetEvent(false);
            Int32 count = 0;

            _TwitterService.StreamUser((streamEvent, res) =>
            {
                if (streamEvent is TwitterUserStreamEnd)
                {
                    block.Set();
                }

                if(streamEvent is TwitterUserStreamStatus)
                {
                    TwitterUserStreamStatus status = (TwitterUserStreamStatus)streamEvent;
                    TwitterStatusLight convertedResult =  ConvertResult(status.Status);
                    TwitterReceiveStatusEvent(convertedResult);
                }
                count++;
                if (count == maxStreamEvents)
                {
                    block.Set();
                }
            });
            //block.WaitOne();
            _TwitterService.CancelStreaming();
        }

        public List<NTLIB.TwitterUser> ListFollower()
        {
            IEnumerable<TweetSharp.TwitterUser> response = _TwitterService.ListFollowers(new ListFollowersOptions());
            List<NTLIB.TwitterUser> convertUser = new List<NTLIB.TwitterUser>();
            foreach (TweetSharp.TwitterUser suser in response)
            {
                NTLIB.TwitterUser user = ConvertUser(suser);
                convertUser.Add(user);
            }
            return convertUser;
        }

        public List<NTLIB.TwitterUser> ListFriends()
        {
            IEnumerable<TweetSharp.TwitterUser> response = _TwitterService.ListFriends(new ListFriendsOptions());
            List<NTLIB.TwitterUser> convertUser = new List<NTLIB.TwitterUser>();
            foreach (TweetSharp.TwitterUser suser in response)
            {
                NTLIB.TwitterUser user = ConvertUser(suser);
                convertUser.Add(user);
            }
            return convertUser;
        }

        public NTLIB.TwitterUser FollowUser(Int64 id)
        {
           TweetSharp.TwitterUser response = _TwitterService.FollowUser(new FollowUserOptions{UserId = id});
            return ConvertUser(response);
        }
        public NTLIB.TwitterUser UnFollowUser(Int64 id)
        {
            TweetSharp.TwitterUser response = _TwitterService.UnfollowUser(new UnfollowUserOptions { UserId = id });
            return ConvertUser(response);
        }

        /// <summary>
        /// フォローしてない人をフォローして、フォロー解除されてたら解除する
        /// </summary>
        public void BaranceFollow()
        {
            List<NTLIB.TwitterUser> followers = ListFollower();
            List<NTLIB.TwitterUser> friends = ListFriends();

            List<Int64> followers_arr = new List<Int64>();
            List<Int64> friends_arr = new List<Int64>();

            followers.ForEach((e) => { followers_arr.Add(e.Id); });
            friends.ForEach((e) => { friends_arr.Add(e.Id); });

            List<Int64> notFollow = new List<Int64>(); // フォローするリスト
            notFollow = followers_arr.Except(friends_arr).ToList();
            List<Int64> notFollower = new List<Int64>(); //フォロー解除するリスト
            notFollower = friends_arr.Except(followers_arr).ToList();

            notFollow.ForEach((e) => { FollowUser(e); });
            notFollower.ForEach((e) => { UnFollowUser(e); });

        }
    }
}
