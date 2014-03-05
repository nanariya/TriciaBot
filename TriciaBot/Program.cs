using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TriciaBot
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            if (args.Length > 0)
            {
                if(AttachConsole(System.UInt32.MaxValue) || true)
                {
                    try
                    {
                        System.IO.StreamWriter stdout = new System.IO.StreamWriter(System.Console.OpenStandardOutput(),
                        System.Text.Encoding.GetEncoding("shift-jis"));
                        stdout.AutoFlush = true;
                        System.Console.SetOut(stdout);

                        Proc.Start(args);
                        FreeConsole();
                    }
                    catch(Exception e)
                    {
                        System.IO.File.AppendAllText("debug.txt", e.Message);
                    }
                    finally
                    {
                        //FreeConsole();
                        //Environment.Exit(0);
                    }
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
