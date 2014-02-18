using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace NTLIB
{
    public static class Tool
    {
        public static void SaveConfig(Object obj)
        {
            try
            {
                if (obj == null) throw new Exception("null");

                String fileName = obj.GetType().Name + ".conf";

                DataContractSerializer srz =
                    new DataContractSerializer(obj.GetType());

                FileStream fs = new FileStream(fileName, FileMode.Create);

                srz.WriteObject(fs, obj);

                fs.Close();
            }
            catch (Exception e)
            {
                throw new Exception("(´A'）書けね" , e);
            }
        }

        public static dynamic LoadConfig(Type objType)
        {

            dynamic obj = null;

            try
            {
                String fileName = objType.Name + ".conf";

                DataContractSerializer srz =
                    new DataContractSerializer(objType);

                FileStream fs = new FileStream(fileName, FileMode.Open);

                obj = srz.ReadObject(fs);

                fs.Close();
            }
            catch (Exception e)
            {
                throw new Exception("(´A'）読めね", e);
            }

            return obj;
        }
    }
}
