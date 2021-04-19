using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class DeliveryMethod
    {
        public string shortName { get; set; }
        public string deliveryTime { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public int id { get; set; }
    }

}
