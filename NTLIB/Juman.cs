
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NTLIB
{
    public static class Juman
    {
        private static String _JumanPath = @"C:\Program Files\juman\juman.exe";

        public static String JumanPath 
        {
            get { return _JumanPath; }
            set { _JumanPath = value; }
        }
        

        public static String execJuman(String message)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = _JumanPath;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardInput = true;
            psInfo.RedirectStandardOutput = true;

            Process p = Process.Start(psInfo);
            using (StreamWriter sw = p.StandardInput)
            {
                sw.Write(message);
            }

            String result = p.StandardOutput.ReadToEnd();

            Debug.Write(result);

            return result;

        }
    }
}
