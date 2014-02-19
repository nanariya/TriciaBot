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

        public Boolean isAuthed { get; set; }
        public String ConsumerKey { get; set; }
        public String ConsumerSecret { get; set; }
        public String AccessToken { get; set; }
        public String AccessSecret { get; set; }

        private String PinCodeStartTag = "<CODE>";
        private Int32 PinCodeOffset = 13;
        

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
    }
}
