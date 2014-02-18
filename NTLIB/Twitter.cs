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

        public String ConsumerKey { get; set; }
        public String ConsumerSecret { get; set; }
        public String AccessToken { get; set; }
        public String AccessSecret { get; set; }

        public Twitter()
        {
            this.isAuthed = false;
        }

        public Uri GetAuthURL()
        {
            _TwitterService = new TwitterService(this.ConsumerKey, this.ConsumerSecret);
            _RequestToken = _TwitterService.GetRequestToken();
            return _TwitterService.GetAuthenticationUrl(_RequestToken);
        }
        public void PinCodeAuth(String pinCode)
        {
            OAuthAccessToken accessToken = _TwitterService.GetAccessToken(_RequestToken, pinCode);
            this.AccessToken = accessToken.Token;
            this.AccessSecret = accessToken.TokenSecret;
        }
    }
}
