using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public Address ShipToAddress { get; set; }
    }
}
