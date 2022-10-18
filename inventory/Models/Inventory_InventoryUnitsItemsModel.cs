using System;


namespace inventory.Models
{
    public class Inventory_InventoryUnitsItemsModel
    {
        
        public long unit_item_id { get;set; }
        public int item_id { get;set; }
        public int unit_id { get;set; }
        public string starting_period { get;set; } 
        public string last_entry { get;set; }
        public double starting_quantity { get;set; }
        public double quantity_in { get;set; }
        public double quantity_out { get;set; }
        public double ending_quantity { get;set; }
        public double starting_cost { get;set; }
        public double cost_in { get;set; }
        public double cost_out { get;set; }
        public double ending_cost { get;set; }
        public double unit_cost { get;set; }
        public double last_highest_in_unit_cost { get;set; }
        public int user_id { get;set; }
        public string entry_date { get;set; }


    }
}
