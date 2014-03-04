using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTLIB
{
    public class CabochaResult
    {
        public DateTime When { get; set; }
        /// <summary>
        /// nickname or username
        /// </summary>
        public List<String> Who { get; set; }
        public String Where { get; set; }
        public String What { get; set; }
        public String Why { get; set; }
    }
}
