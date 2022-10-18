using System;

namespace inventory.Models
{
    public class Inventory_InventoryItemsModel
    {
        
        public long item_id { get;set; }
        public int category_id { get;set; }
        public int brand_id { get;set; }
        public string model_description { get;set; }
        public int part_description { get;set; }
        public int part_number { get;set; }
        public int size { get;set; }
        public int valve_type { get;set; } 
        public int ratio { get;set; }
        public int thread_pattern { get;set; }
        public string stocking_unit { get;set; }
        public string retail_unit { get;set; }
        public double rtu_over_stu { get;set; }
        public double wtd_ave_cost { get;set; }
        public double markup_rate { get;set; }
        public double selling_price { get;set; }
        public int user_id { get;set; }
        public string entry_date { get;set; }


    } 
}
