using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class NonIndustrialSelfService
    {

		public int so_reference { get; set; }
		public int customer_id { get; set; }
		public string customer_name { get; set; }
		public string cellular_number { get; set; }
		public double weight_in_kg { get; set; }
		public int number_of_loads { get; set; }
		public double amount_due { get; set; }
		public string payment_mode { get; set; }
		public string entry_datetime { get; set; }
		public string posted_by { get; set; }
		public string payment_image { get; set; }
		public string so_qr_image { get; set; }


	}
}
