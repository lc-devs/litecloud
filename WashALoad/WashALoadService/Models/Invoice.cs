using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class Invoice
    {
		public int invoice_reference { get; set; }
		public string invoice_datetime { get; set; }
		public double invoice_amount { get; set; }
		public int paid { get; set; }
		public string payment_reference { get; set; }
		public int billed { get; set; }
		public int billing_reference { get; set; }
		public string invoice_generated_by { get; set; }
	}
}
