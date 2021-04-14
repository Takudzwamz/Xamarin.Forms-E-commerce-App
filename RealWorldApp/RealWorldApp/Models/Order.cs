using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public double Subtotal { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }
}
