using System;
namespace inventory.Models
{
    public class Entities_ProvinceStateModel
    {
        

        public long province_state_id { get; set; }
        public string province_state_name { get; set; }
        public Entities_CountryModel country { get; set; }


    }
}
 