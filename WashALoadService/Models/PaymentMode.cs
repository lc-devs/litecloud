using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PaymentMode
    {
		public string payment_code { get; set; }
		public string description { get; set; }
		public int non_cash { get; set; }
		public int accounting_only { get; set; }
		public int Float { get; set; }
		public int require_proof { get; set; }
	}
}
