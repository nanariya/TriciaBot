using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriciaBot
{
    public partial class MainForm : Form
    {

        NTLIB.Twitter _Twitter = null;

        public MainForm()
        {
            InitializeComponent();

            AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));

            _Twitter = new NTLIB.Twitter();
            _Twitter.ConsumerKey = appData.ConsumerKey;
            _Twitter.ConsumerSecret = appData.ConsumerSecret;

        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {//ブラウザロード完了
            AppData appData = NTLIB.Tool.LoadConfig(typeof(AppData));
            String html = webBrowser1.DocumentText;
            if(0 < html.IndexOf(appData.PinCodeStartTag))
            {
                webBrowser1.Visible = false;
                int loc = html.IndexOf(appData.PinCodeStartTag);
                String pinCode = html.Substring(loc, appData.PinCodeOffset);
                pinCode = pinCode.Replace(appData.PinCodeStartTag, "");
                textBox1.Text = pinCode;
            }
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
        }
    }
}
