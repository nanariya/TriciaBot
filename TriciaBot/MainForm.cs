﻿using System;
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
        public MainForm()
        {
            InitializeComponent();

            AppData appData = new AppData();
            appData.ConsumerKey = "aaa";
            NTLIB.Tool.SaveConfig(appData);

            appData = NTLIB.Tool.LoadConfig(typeof(AppData));
        }
    }
}
