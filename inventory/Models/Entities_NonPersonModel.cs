using System;
 
namespace inventory.Models
{
   public class Entities_NonPersonModel
   {
        public long nonperson_id { get;set; }
        public string nonperson_name {get;set;}
        public long contact_person1 { get;set; }
        public long contact_person2 { get; set; }
        public string tax_identification { get;set; }
        public string adrs_house_street { get;set; }
        public long adrs_barangay { get;set; }
        public long adrs_town { get;set; }
        public long adrs_province { get;set; }
        public long adrs_country { get;set; } 
        public string zip_code { get;set; }
        public string email_address1 { get;set; }
        public string email_address2 { get;set; }
        public string email_address3 { get;set; }
        public string  landphone1 { get;set; }
        public string landphone2 { get;set; }
        public string mobilephone1 { get;set; }
        public string mobilephone2 { get;set; }

 
 
   }
}
