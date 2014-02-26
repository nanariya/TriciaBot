using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace NTLIB
{
    public class Twitter
    {
        private TwitterService _TwitterService = null;
        private OAuthRequestToken _RequestToken = null;

        public Boolean isAuthed { get; private set; }
        public String ConsumerKey { get; private set; }
        public String ConsumerSecret { get; private set; }
        public String AccessToken { get; private set; }
        public String AccessSecret { get; private set; }

        public List<TwitterResult> HomeTimeLine { get; private set; }

        private String PinCodeStartTag = "<CODE>";
        private Int32 PinCodeOffset = 13;


        public Twitter(String consumerKey, String consumerSecret)
        {
            this.isAuthed = false;
            _TwitterService = new TwitterService(consumerKey, consumerSecret);
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

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

        private TwitterResult ConvertResult(TwitterStatus row)
        {

            TwitterResult res = new TwitterResult();

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
                res.UserName = row.User.Name;
                res.UserScreenName = row.User.ScreenName;
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
        public List<TwitterResult> ListHomeTimeline()
        {
            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());

            List<TwitterResult> convertResponse = new List<TwitterResult>();

            foreach (TwitterStatus row in response)
            {
                TwitterResult res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }

        public List<TwitterResult> ListHomeTimeline(Int64 LastID)
        {
            ListTweetsOnHomeTimelineOptions option = new ListTweetsOnHomeTimelineOptions();
            option.SinceId = LastID;

            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsOnHomeTimeline(option);

            List<TwitterResult> convertResponse = new List<TwitterResult>();

            foreach(TwitterStatus row in response)
            {
                TwitterResult res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }
    }
}
