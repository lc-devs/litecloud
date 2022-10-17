using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class LoginDetails
    {
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string session_authentication_key { get; set; }
        public string email_address { get; set; }
        public string cellular_number { get; set; }

    }
}
