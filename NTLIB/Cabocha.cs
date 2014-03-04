
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

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
        

        public static CabochaResult execCabocha(String message)
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

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(p.StandardOutput.ReadToEnd());

            Debug.WriteLine(xml.InnerXml);

            CabochaResult result = new CabochaResult();

            foreach(XmlElement xmlWord in xml.DocumentElement)
            {
                String word = "";
                String lword = "";
                List<String> hinsi = new List<String>();
                if (xmlWord.ChildNodes.Count == 1)
                {
                    word = xmlWord.LastChild.InnerText;
                }
                else
                {
                    for (Int32 i = 0; i < xmlWord.ChildNodes.Count - 1; i++)
                    {
                        word += xmlWord.ChildNodes[i].InnerText;
                        hinsi.Add(xmlWord.ChildNodes[i].Attributes["feature"].Value.Split(',')[5]);
                    }
                    lword = xmlWord.LastChild.InnerText;
                    String last_p = xmlWord.LastChild.Attributes["feature"].Value.ToString();
                }
            }

            return result;
        }
    }
}
