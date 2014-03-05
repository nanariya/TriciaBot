using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriciaBot
{
    public static class Proc
    {
        //定時動作
        public static void Start(String[] args)
        {
            if(args[0].Substring(0,1).Equals("/"))
            {
                switch (args[0].Substring(1,1))
                {
                    case "h" :
                        break;
                    case "m" :
                        switch(args[1])
                        {
                            case "PostPanoramio" :

                                break;
                            default :
                                break;
                        }
                        break;
                    default :
                        break;
                }
            }
        }

        private static void PostPanoramio()
        {

        }


        private static void LoopHour()
        {

        }
        private static void LoopDay()
        {

        }
    }
}
