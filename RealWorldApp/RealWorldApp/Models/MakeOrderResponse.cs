using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class MakeOrderResponse
    {
        public DateTime orderDate { get; set; }
        public ShipToAddress shipToAddress { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public OrderItemDto[] orderItems { get; set; }
        public double subtotal { get; set; }
        public int status { get; set; }
        public object paymentIntentId { get; set; }
        public int id { get; set; }
        public string PaymentURl { get; set; }
    }
}
