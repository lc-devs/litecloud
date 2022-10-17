using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class NonIndustrialService
    {
        public long id_service { get; set; }
        public string description { get; set; }
        public int manual_costing { get; set; }
        public double unit_cost { get; set; }
    }
}
