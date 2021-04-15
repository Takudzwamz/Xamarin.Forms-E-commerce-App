using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class UserDto
    {
        //Success
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; } 
        
        //Error
        public string[] errors { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }

        public bool IsSuccess { get; set; }
    }
   
}
