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

        public List<TwitterResult> HomeTimeLine { get; private set; }

        private String PinCodeStartTag = "<CODE>";
        private Int32 PinCodeOffset = 13;

        //ユーザーストリームのPushを受け取ったときのイベント用
        public delegate void TwitterReceiveStatusEventHandler(TwitterResult result);
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

        /// <summary>
        /// 外部から隠匿するためにTweetSharpのTwitterStatusから最低限だけ抜き出す
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
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
                res.UserId = row.User.Id;
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

        public List<TwitterResult> ListReplyTimeline()
        {
            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions());

            List<TwitterResult> convertResponse = new List<TwitterResult>();

            foreach (TwitterStatus row in response)
            {
                TwitterResult res = ConvertResult(row);
                convertResponse.Add(res);
            }

            return convertResponse;
        }
        public List<TwitterResult> ListReplyTimeline(Int64 LastID)
        {
            ListTweetsMentioningMeOptions option = new ListTweetsMentioningMeOptions();
            option.SinceId = LastID;

            IEnumerable<TwitterStatus> response = _TwitterService.ListTweetsMentioningMe(option);

            List<TwitterResult> convertResponse = new List<TwitterResult>();

            foreach (TwitterStatus row in response)
            {
                TwitterResult res = ConvertResult(row);
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
                    TwitterResult convertedResult =  ConvertResult(status.Status);
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
        
    }
}
