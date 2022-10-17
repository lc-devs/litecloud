using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PickupIndustrial
    {
		public int so_reference { get; set; }
		public int customer_id { get; set; }
		public string booking_reference { get; set; }
		public string picked_up_by { get; set; }
		public string picked_up_datetime { get; set; }
		public decimal weight_in_kg { get; set; }
		public int number_of_bags { get; set; }
		public string received_by_logistics { get; set; }
		public string so_reference_QR_Image { get; set; }

		public List<PickupIndustrialDetail> pickupIndustrialDetail { get; set; }
	}
}
