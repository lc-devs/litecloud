using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class BookingsForPickupDetails
    {
		public string booking_reference { get; set; }
		public string booking { get; set; }
		public string picked_up { get; set; }
		public string received_by_logistics { get; set; }
		public string forwarded_to_laundry { get; set; }
		public string done_laundry { get; set; }
		public string forwarded_to_logistics { get; set; }
		public string delivered { get; set; }
	}
}
