using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class foodItemModel
    {
        public bool Checked { get; set; }
        public int fid { get; set; }
        public string fname { get; set; }
        public int price { get; set; }
        public string type { get; set; }
        public string f_desc { get; set; }
    }
}