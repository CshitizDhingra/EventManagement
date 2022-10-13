using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class eventMVCModel
    {
        public eventMVCModel()
        {
            this.foodOrders = new HashSet<foodOrders>();
        }

        public int eid { get; set; }
        public string etype { get; set; }
        public string edate { get; set; }
        public Nullable<int> gathering { get; set; }
        public Nullable<int> did { get; set; }
        public Nullable<int> d_price { get; set; }
        public Nullable<int> fo_id { get; set; }
        public Nullable<int> f_price { get; set; }
        public Nullable<int> total { get; set; }
        public virtual decorationPlans decorationPlans { get; set; }
        public virtual eventType eventType { get; set; }
        public virtual ICollection<foodOrders> foodOrders { get; set; }
        public virtual List<foodItems> foodItems { get; set; }
    }
}