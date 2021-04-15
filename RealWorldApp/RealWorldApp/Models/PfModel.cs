using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class PfModel
    {
        public string merchant_id { get; set; }
        public string merchant_key { get; set; }
        public string return_url { get; set; }
        public string notify_url { get; set; }
        public string m_payment_id { get; set; }
        public string amount { get; set; }
        public string item_name { get; set; }
    }

}
