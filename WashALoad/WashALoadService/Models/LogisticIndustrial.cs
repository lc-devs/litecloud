using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class LogisticIndustrial
    {
		public int so_reference { get; set; }
		public int customer_id { get; set; }
		public string booking_reference { get; set; }
		public string picked_up_by { get; set; }
		public string picked_up_datetime { get; set; }
		public double weight_in_kg { get; set; }
		public int number_of_bags { get; set; }
		public string received_by_logistics_user { get; set; }
		public string received_from_pickup_datetime { get; set; }
		public string received_by_laundry { get; set; }
		public string received_by_laundry_datetime { get; set; }
		public string completed_by_laundry { get; set; }
		public string completed_by_laundry_datetime { get; set; }
		public int received_from_laundry { get; set; }
		public string received_from_laundry_datetime { get; set; }
		public int for_invoicing { get; set; }
		public int with_invoice { get; set; }
		public int delivered { get; set; }
		public string delivery_datetime { get; set; }
		public List<LogisticIndustrialDetail> industrialDetails { get; set; }
	}
}
