using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PaymentReference
    {
        public long payment_reference { get; set; }
        public string payment_date { get; set; }
        public int customer_id { get; set; }
        public double amount_paid { get; set; }
        public string payment_mode { get; set; }
        public int float_payment { get; set; }
        public string posted_by { get; set; }
        public string posting_datetime { get; set; }
        public string payment_image { get; set; }
        public long image_entry_id { get; set; }
        public List<PaymentReferenceDetails> paymentReferenceDetails { get; set; }
    }
}
