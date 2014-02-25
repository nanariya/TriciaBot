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
            catch(Exception e )
            {
                throw new Exception("エラー", e);
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
    }
}
