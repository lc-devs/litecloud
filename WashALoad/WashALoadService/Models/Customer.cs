using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class Customer
    {
		public long customer_id { get; set; }
		public string source_id { get; set; }
		public string customer_name { get; set; }
		public string cellular_number { get; set; }
		public string email_address { get; set; }
		public int industrial { get; set; }
		public int non_industrial { get; set; }
		public int active_customer { get; set; }
		public int account_reset { get; set; }
		public string customer_password { get; set; }
		public string street_building_address { get; set; }
		public string barangay_address { get; set; }
		public string town_address { get; set; }
		public string province { get; set; }
		public float longitude { get; set; }
		public float latitude { get; set; }
		public double average_daily_load { get; set; }
		public int weight_per_load { get; set; }
		public string authkey { get; set; }
	}
}
