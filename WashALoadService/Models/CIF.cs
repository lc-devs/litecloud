using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class CIF
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public string address_street { get; set; }
        public string address_city { get; set; }
        public string province { get; set; }
        public string zip { get; set; }
    }
}
