using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class IndustrialLaundryItem
    {
		public long id_item { get; set; }
		public string description { get; set; }
		public IndustrialCategory category { get; set; }
		public IndustrialService service { get; set; }
		public int manual_costing { get; set; }
		public double unit_cost_adl { get; set; }
		public double unit_cost { get; set; }
	}
}
