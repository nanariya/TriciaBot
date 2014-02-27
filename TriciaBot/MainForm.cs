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
             if(_Twitter != null)
             {
                 //List<NTLIB.TwitterResult> res = _Twitter.ListReplyTimeline(Properties.Settings.Default.LastGettedReplyID);
                 /*
                 List<NTLIB.TwitterResult> res = _Twitter.ListReplyTimeline();
                                  
                 foreach(NTLIB.TwitterResult row in res)
                 {
                     richTextBox1.AppendText(
                         row.UserScreenName + ":" + row.Text + Environment.NewLine
                         );

                     foreach(NTLIB.JumanResult s in NTLIB.Juman.execJuman(row.Text))
                     {
                         //形態素解析した後
                     }

                     Properties.Settings.Default.LastGettedReplyID = row.ID;
                 }

                 Properties.Settings.Default.Save();
                 */
                 UserDB db = new UserDB(Properties.Settings.Default.DatabaseFileName);
                 if (!File.Exists(Properties.Settings.Default.DatabaseFileName))
                 {
                     db.CreateUserDB();
                 }

                 List<NTLIB.TwitterResult> res = _Twitter.ListReplyTimeline();
                 foreach (NTLIB.TwitterResult row in res)
                 {
                     db.AddUserData(row.UserId, row.UserName, row.UserScreenName);
                 }
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
            _Twitter.ListHomeTimelineLoop();
            _Twitter.TwitterReceiveStatusEvent += _Twitter_TwitterReceiveStatusEvent;
        }

        void _Twitter_TwitterReceiveStatusEvent(NTLIB.TwitterResult result)
        {
            //ここの条件にフォロワー限定を入れる　今は仮で開放
            if (result.InReplyToUserId == 2354246292L)
            {
                Object[] param = { new Object(), new Object() };
                param[0] = "@" + result.UserScreenName + " 今は" + DateTime.Now.ToString("tth時m分よ");
                List<NTLIB.JumanResult> jumanResult = NTLIB.Juman.execJuman(result.Text);
                foreach (NTLIB.JumanResult word in jumanResult)
                {
                    if (word.Word == "眠い" && word.Part == "形容詞") param[0] = "@" + result.UserScreenName + " 早く寝なさい₍₍ ᕕ(՞ةڼ◔)ᕗ⁾⁾";
                }
                param[1] = result.ID;
                System.Threading.Thread.Sleep(5000);
                Invoke(new SendTweetReplyDelegate(_Twitter.SendTweetToReply), param);
            }
            Invoke(new TextAddDelegate(richTextBox1.AppendText), result.UserName + ":" + result.Text + Environment.NewLine);
        }

    }
}
