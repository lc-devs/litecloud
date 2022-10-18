using System;

namespace inventory.Models
{
    public class Entities_BarangayDistrictsModel
    {
        
       public long barangay_district_id { get; set; }
       public string barangay_district_name { get; set; }
       public Entities_TownModel town { get; set; }

    }
} 