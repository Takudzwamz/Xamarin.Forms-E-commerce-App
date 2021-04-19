using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ProductApiData
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public ProductData[] data { get; set; }
    }   
}
