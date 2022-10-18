using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Inventory_AdjustmentTemplatesModel
    {
		public long template_id { get; set; }
		public string description { get; set; }
		public int add_to_quantity { get; set; }
		public int require_destination_and_source { get; set; }
		public long user_id { get; set; }
		public string entry_date { get; set; }
	}
}
