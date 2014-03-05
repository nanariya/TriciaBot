using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriciaBot
{
    public static class Proc
    {
        //定時動作
        public static void Start(String[] args)
        {
            if(args[0].Substring(0,1).Equals("/"))
            {
                switch (args[0].Substring(1,1))
                {
                    case "h" :
                        break;
                    case "m" :
                        switch(args[1])
                        {
                            case "PostPanoramio" :
                                PostPanoramio();
                                break;
                            case "AutoBalanceFollow" :
                                AutoBalanceFollow();
                                break;
                            default :
                                break;
                        }
                        break;
                    default :
                        break;
                }
            }
        }

        private static void PostPanoramio()
        {
            try
            {
                NTLIB.Panoramio pm = new NTLIB.Panoramio();
                String result = pm.GetRandomPhoto(-3.6d, 58.52d, -5.76d, 57d);

                AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
                if (appData == null) return;
                NTLIB.Twitter tw = new NTLIB.Twitter(appData.ConsumerKey, appData.ConsumerSecret);
                if (Properties.Settings.Default.AccessToken == "" || Properties.Settings.Default.AccessSecret == "") return;
                tw.AuthenticateWith(Properties.Settings.Default.AccessToken, Properties.Settings.Default.AccessSecret);

                tw.SendTweet("ランダムに選んだスコットランド地方の写真₍₍ ᕕ(՞ةڼ◔)ᕗ⁾⁾ " + result);
            }
            catch(Exception)
            {

            }
        }
        private static void AutoBalanceFollow()
        {
            try
            {
               AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
               UserDB db = new UserDB(Properties.Settings.Default.DatabaseFileName);
                if (appData == null) return;
                NTLIB.Twitter tw = new NTLIB.Twitter(appData.ConsumerKey, appData.ConsumerSecret);
                if (Properties.Settings.Default.AccessToken == "" || Properties.Settings.Default.AccessSecret == "") return;
                tw.AuthenticateWith(Properties.Settings.Default.AccessToken, Properties.Settings.Default.AccessSecret);

                List<Int64> whiteListID = db.SelectWhiteList();
                tw.BaranceFollow(whiteListID);
            }
            catch(Exception)
            { }
        }


        private static void LoopHour()
        {

        }
        private static void LoopDay()
        {

        }
    }
}
