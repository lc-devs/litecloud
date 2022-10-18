using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Inventory_ItemCategoriesModel
    {
		public long item_category_id { get; set; }
		public string description { get; set; }
		public long user_id { get; set; }
		public string entry_date { get; set; }
	}
}
