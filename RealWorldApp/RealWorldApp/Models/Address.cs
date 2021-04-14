using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; } //not mapped
        public string State { get; set; } //not mapped
        public string Zipcode { get; set; } //not mapped
        public string PhoneNumber { get; set; }
    }
}
