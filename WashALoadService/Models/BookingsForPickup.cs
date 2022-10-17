using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class BookingsForPickup
    {
		public string booking_reference { get; set; }
		public string booking_datetime { get; set; }
		public int customer_id { get; set; }
		public string scheduled_pickup_datetime { get; set; }
		public string so_reference { get; set; }
		public int industrial { get; set; }
		public string picked_up_by { get; set; }
		public string actual_picked_up_datetime { get; set; }
		public int cancelled_booking { get; set; }
		public int weight_per_load { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
		public string customer_type { get; set; }
		public string customer_name { get; set; }
		public string address { get; set; }
	}
}
