
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NTLIB
{
    public static class Cabocha
    {
        private static String _CabochaPath = @"C:\Program Files (x86)\CaboCha\bin\cabocha.exe";

        public static String CabochaPath 
        {
            get { return _CabochaPath; }
            set { _CabochaPath = value; }
        }
        

        public static String execCabocha(String message)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = _CabochaPath;
            psInfo.Arguments = "-f3 -n2";
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
