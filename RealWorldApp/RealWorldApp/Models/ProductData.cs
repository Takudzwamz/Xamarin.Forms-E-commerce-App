using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ProductData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public string pictureUrl { get; set; }
        public string productType { get; set; }
        public string productBrand { get; set; }
        public ProductPhoto[] photos { get; set; }
    }
}
