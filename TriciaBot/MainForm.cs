using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace TriciaBot
{
    public partial class MainForm : Form
    {

        NTLIB.Twitter _Twitter = null;

        public MainForm()
        {
            InitializeComponent();

            AppData appData = null;

            if (!File.Exists(NTLIB.Tool.ConfigFilename(typeof(AppData))))
            {
                appData = new AppData();
                NTLIB.Tool.SaveConfig(appData);
            }

            try
            {
                appData = NTLIB.Tool.LoadConfig(typeof(AppData));
            }
            catch(Exception)
            {
                
            }

            _Twitter = new NTLIB.Twitter(appData.ConsumerKey, appData.ConsumerSecret);

            if(Properties.Settings.Default.AccessToken != "" && Properties.Settings.Default.AccessSecret != "")
            {
                _Twitter.AuthenticateWith(Properties.Settings.Default.AccessToken, Properties.Settings.Default.AccessSecret);
                label1.Text = "認証済";
            }

        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {//ブラウザロード完了
            textBox1.Text = _Twitter.GetPinCodeFromHTML(webBrowser1.DocumentText);
        }

        private void button2_Click(object sender, EventArgs e)
        {//認証画面呼出ボタン
            if(_Twitter != null)
            {
                webBrowser1.Url = _Twitter.GetAuthURL();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//PIN認証ボタン
            if (_Twitter != null)
            {
                _Twitter.PinCodeAuth(textBox1.Text);
            }
            //アクセストークンを保存
            Properties.Settings.Default.AccessToken = _Twitter.AccessToken;
            Properties.Settings.Default.AccessSecret = _Twitter.AccessSecret;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.AccessToken != "" && Properties.Settings.Default.AccessSecret != "")
            {
                _Twitter.AuthenticateWith(Properties.Settings.Default.AccessToken, Properties.Settings.Default.AccessSecret);
                label1.Text = "認証済";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
             if(_Twitter != null && _Twitter.isAuthed)
             {
                 UserDB db = new UserDB(Properties.Settings.Default.DatabaseFileName);
                 if (!File.Exists(Properties.Settings.Default.DatabaseFileName))
                 {
                     db.CreateUserDB();
                 }

                 /*
                 List<NTLIB.TwitterStatusLight> res = _Twitter.ListReplyTimeline();
                 foreach (NTLIB.TwitterStatusLight row in res)
                 {
                     db.AddUserData(row.UserId, row.UserName, row.UserScreenName);
                 }

                 List<Int64> whiteListID = db.SelectWhiteList();
                 _Twitter.BaranceFollow(whiteListID);
                  */
             }
        }

        private void checkBoxTwitter_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxTwitter.Checked == true)
            {
            }
            else
            {
            }
        }

        delegate void TextAddDelegate(String text);
        delegate void SendTweetReplyDelegate(String text, Int64 toReplyId);

        private void button4_Click(object sender, EventArgs e)
        {
            if (_Twitter != null && _Twitter.isAuthed)
            {
                _Twitter.TwitterReceiveStatusEvent += _Twitter_TwitterReceiveStatusEvent;
                _Twitter.ListHomeTimelineLoop();
            }
        }

        void _Twitter_TwitterReceiveStatusEvent(NTLIB.TwitterStatusLight result)
        {
            //ここの条件にフォロワー限定を入れる　今は仮で開放
            /*
            if (result.InReplyToUserId == Properties.Settings.Default.MyUserID)
            {
                UserDB db = new UserDB(Properties.Settings.Default.DatabaseFileName);
                String nickName = db.SelectUserNickname(result.UserId);
                if (nickName != "") nickName += "、";

                Object[] param = { new Object(), new Object() };

                Int32 posNickName = result.Text.IndexOf("Just call me ");

                if (posNickName > 0)
                {
                    String preNickName = result.Text.Substring(posNickName + 13, result.Text.Length - posNickName - 13);// 気をつけて
                    db.ChangeUserNickname(result.UserId, preNickName);
                    param[0] = "@" + result.UserScreenName + " 今度から" + preNickName + "って呼ぶね";
                }
                else
                {
                    param[0] = "@" + result.UserScreenName + " " + nickName + "今は" + DateTime.Now.ToString("tth時m分よ");
                    List<NTLIB.JumanResult> jumanResult = NTLIB.Juman.execJuman(result.Text);
                    foreach (NTLIB.JumanResult word in jumanResult)
                    {
                        if (word.Word == "眠い" && word.Part == "形容詞") param[0] = "@" + result.UserScreenName + " " + nickName + "早く寝なさい₍₍ ᕕ(՞ةڼ◔)ᕗ⁾⁾";
                    }
                }

                param[1] = result.ID;
                System.Threading.Thread.Sleep(5000);
                Invoke(new SendTweetReplyDelegate(_Twitter.SendTweetToReply), param);
            }
            */
            /*
            List<NTLIB.JumanResult> jumanResult = NTLIB.Juman.execJuman(result.Text);
            jumanResult.ForEach((e) =>
            {
                Invoke(new TextAddDelegate(richTextBox1.AppendText), result.UserName + ":" + e.Word + "," + e.Part + Environment.NewLine);
            });
             */
            //NTLIB.CabochaResult cabo = NTLIB.Cabocha.execCabocha(result.Text);

            //Invoke(new TextAddDelegate(richTextBox1.AppendText), result.UserName + ":" + result.Text + Environment.NewLine);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
            NTLIB.Gmail mail = new NTLIB.Gmail(appData.GmailAddress, appData.GmailID, appData.GmailPass);
            mail.SentMail("sieben.riya@gmail.com", "ねむい", "そんなことはない");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
            NTLIB.GCal cal = new NTLIB.GCal(appData.GmailID, appData.GmailPass);
            List<String> guests = new List<String>();
            guests.Add("sieben.riya@gmail.com");
            cal.WriteSchedule("ミーティング", "ご飯を食べるだけ", "秋葉原", guests, DateTime.Now, DateTime.Now);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
            NTLIB.GCal cal = new NTLIB.GCal(appData.GmailID, appData.GmailPass);
            List<NTLIB.GCalItem> items = cal.ReadSchedule(new DateTime(2014, 1, 1), new DateTime(2014, 12, 1), true).ToList();
            foreach(NTLIB.GCalItem item in items)
            {
                item.GuestsEmail.ForEach((mail) =>
                {
                    richTextBox1.AppendText(mail + Environment.NewLine);
                });
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NTLIB.TwitterStatusLight status = new NTLIB.TwitterStatusLight();
            status.Text = textBox2.Text;
            //textBox2.Text = "";
            richTextBox1.Text = "";
            _Twitter_TwitterReceiveStatusEvent(status);
        }

    }
}
