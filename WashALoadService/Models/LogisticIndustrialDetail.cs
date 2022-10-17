using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class LogisticIndustrialDetail
    {
        public int so_reference { get; set; }
        public int item_code { get; set; }
        public int item_count { get; set; }
        public double adl_cost { get; set; }
        public double adl_adjustment { get; set; }
    }
}
