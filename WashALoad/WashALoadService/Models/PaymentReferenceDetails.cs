using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PaymentReferenceDetails
    {
        public int payment_reference { get; set; }
        public int invoice_reference { get; set; }
    }
}
