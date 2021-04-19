using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ProductPhoto
    {
        public int id { get; set; }
        public string pictureUrl { get; set; }
        public string fileName { get; set; }
        public bool isMain { get; set; }

    }
}
