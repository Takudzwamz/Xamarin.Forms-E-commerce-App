using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class OrderByUser
    {
       // public string fullName { get; set; }
       // public string address { get; set; }
       // public string phone { get; set; }
       //// public double orderTotal { get; set; }
       // public bool isOrderCompleted { get; set; }
       // public int userId { get; set; }


        // New
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
