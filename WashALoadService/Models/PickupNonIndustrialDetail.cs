﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PickupNonIndustrialDetail
    {
        public int so_reference { get; set; }
        public int item_code { get; set; }
        public int item_count { get; set; }
        public string description { get; set; }
    }
}
