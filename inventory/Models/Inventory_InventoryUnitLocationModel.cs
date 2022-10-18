using System;

namespace inventory.Models
{
    public class Inventory_InventoryUnitLocationModel
    {
        
        public long unit_location_id { get;set; }
        public string description { get;set; }
        public int person_incharge { get;set; }
        public int warehouse { get;set; } 
        public string bldg_street_address { get;set; }
        public int barangay_id { get;set; }
        public int town_id { get;set; }
        public int province_id { get;set; }
        public int country_id { get;set; }
        public string email_address { get;set; }
        public string landline_nos1 { get;set; }
        public string landline_nos2 { get;set; }
        public string mobile_nos1 { get;set; }
        public string mobile_nos2 { get;set; }
        public int user_id { get;set; }
        public string entry_date { get;set; }


    }
}
