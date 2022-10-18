using System;

namespace inventory.Models
{
    public class Inventory_ItemsAdjustmentsModel
    {
        
        public long adjustment_id { get;set; }
        public string adjustment_date { get;set; }
        public long template_id { get;set; }
        public long destination_id { get;set; }
        public long source_id { get;set; }
        public long item_id { get;set; }
        public double quantity { get;set; }
        public string remarks { get;set; }
        public int user_id { get;set; }
        public string entry_date { get;set; }


    }
} 
  