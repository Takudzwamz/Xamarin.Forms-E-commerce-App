using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }

        // public bool isPopularProduct { get; set; }
        // public int categoryId { get; set; }
        // public object imageArray { get; set; }

        public double TotalPrice => Quantity * Price;
        public double Quantity { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + PictureUrl;

    }
}
