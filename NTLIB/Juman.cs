
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
        
        public static String execJumanRaw(String message)
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
            return result;
        }

        public static List<JumanResult> execJuman(String message)
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

            List<JumanResult> jumanResult = ConvertResult(result);

            return jumanResult;

        }

        /// <summary>
        /// Jumanの実行結果を分解する。
        /// </summary>
        /// <param name="jumanResult"></param>
        /// <returns></returns>
        private static List<JumanResult> ConvertResult(String jumanResult)
        {
            List<JumanResult> result = new List<JumanResult>();

            String[] sep = { "\r\n" };
            String[] rows = jumanResult.Split(sep, StringSplitOptions.None);

            foreach (String row in rows)
            {
                JumanResult res = new JumanResult();
                if(row.Length > 16 && row.Substring(row.Length-16,2) == "空白")
                {
                    res.Word = " ";
                    res.Part = "特殊";
                    res.SubPart = "空白";
                    result.Add(res);
                    continue;
                }
                String[] sp = row.Split(' ');
                if (sp.Count() >= 11)
                {
                    res.Word = sp[0];
                    res.Part = sp[3];
                    res.SubPart = sp[5];
                    result.Add(res);
                }
            }

            return result;
        }
    }
}
