using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_NonPersonMethods
   {
  
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_NonPersonMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> SaveNonPerson(Entities_NonPersonModel nonperson)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO entities.`non_persons` 
	                                        (
                                                `nonperson_name`,
                                                `contact_person_id1`, 
	                                            `contact_person_id2`, 
	                                            `tax_identification`, 
	                                            `adrs_house_street`, 
	                                            `adrs_barangay`, 
	                                            `adrs_town`, 
	                                            `adrs_province`, 
	                                            `adrs_country`, 
	                                            `zip_code`, 
	                                            `email_address1`, 
	                                            `email_address2`, 
	                                            `email_address3`, 
	                                            `landphone1`, 
	                                            `landphone2`, 
	                                            `mobilephone1`, 
	                                            `mobilephone2`
	                                        )
	                                     VALUES
                                            (
                                                @nonperson_name,
                                                @contact_person_id1, 
	                                            @contact_person_id2, 
	                                            @tax_identification, 
	                                            @adrs_house_street, 
	                                            @adrs_barangay, 
	                                            @adrs_town, 
	                                            @adrs_province, 
	                                            @adrs_country, 
	                                            @zip_code, 
	                                            @email_address1, 
	                                            @email_address2, 
	                                            @email_address3, 
	                                            @landphone1, 
	                                            @landphone2, 
	                                            @mobilephone1, 
	                                            @mobilephone2
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@nonperson_name", System.Data.DbType.String, nonperson.nonperson_name);
                commonFunctions.BindParameter(oCommand, "@contact_person_id1", System.Data.DbType.Int32, nonperson.contact_person1);
                commonFunctions.BindParameter(oCommand, "@contact_person_id2", System.Data.DbType.Int32, nonperson.contact_person2);
                commonFunctions.BindParameter(oCommand, "@tax_identification", System.Data.DbType.String, nonperson.tax_identification);
                commonFunctions.BindParameter(oCommand, "@adrs_house_street", System.Data.DbType.String, nonperson.adrs_house_street);
                commonFunctions.BindParameter(oCommand, "@adrs_barangay", System.Data.DbType.Int32, nonperson.adrs_barangay);
                commonFunctions.BindParameter(oCommand, "@adrs_town", System.Data.DbType.Int32, nonperson.adrs_town);
                commonFunctions.BindParameter(oCommand, "@adrs_province", System.Data.DbType.Int32, nonperson.adrs_province);                
                commonFunctions.BindParameter(oCommand, "@adrs_country", System.Data.DbType.Int32, nonperson.adrs_country);
                commonFunctions.BindParameter(oCommand, "@zip_code", System.Data.DbType.String, nonperson.zip_code);
                commonFunctions.BindParameter(oCommand, "@email_address1", System.Data.DbType.String, nonperson.email_address1);
                commonFunctions.BindParameter(oCommand, "@email_address2", System.Data.DbType.String, nonperson.email_address2);
                commonFunctions.BindParameter(oCommand, "@email_address3", System.Data.DbType.String, nonperson.email_address3);
                commonFunctions.BindParameter(oCommand, "@landphone1", System.Data.DbType.String, nonperson.landphone1);
                commonFunctions.BindParameter(oCommand, "@landphone2", System.Data.DbType.String, nonperson.landphone2);
                commonFunctions.BindParameter(oCommand, "@mobilephone1", System.Data.DbType.String, nonperson.mobilephone1);
                commonFunctions.BindParameter(oCommand, "@mobilephone2", System.Data.DbType.String, nonperson.mobilephone2);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    nonperson.nonperson_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(nonperson);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> GetNonPersonDetails(long nonPersonId)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT nonperson_id,
                                                `nonperson_name`, 
	                                            `contact_person_id1`, 
	                                            `contact_person_id2`, 
	                                            `tax_identification`, 
	                                            `adrs_house_street`, 
	                                            `adrs_barangay`, 
	                                            `adrs_town`, 
	                                            `adrs_province`, 
	                                            `adrs_country`, 
	                                            `zip_code`, 
	                                            `email_address1`, 
	                                            `email_address2`, 
	                                            `email_address3`, 
	                                            `landphone1`, 
	                                            `landphone2`, 
	                                            `mobilephone1`, 
	                                            `mobilephone2`
	 
	                                    FROM 
	                                        entities.`non_persons`

                                        WHERE nonperson_id = @nonperson_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@nonperson_id", System.Data.DbType.Int32, nonPersonId);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_NonPersonModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_NonPersonModel();
                        hasValue = true;

                        oEntity.nonperson_id = oresult.GetInt64("nonperson_id");
                        oEntity.nonperson_name = oresult.GetString("nonperson_name");
                        oEntity.contact_person1 = oresult.GetInt64("contact_person_id1");
                        oEntity.contact_person2 = oresult.GetInt64("contact_person_id2");
                        oEntity.tax_identification = oresult.GetString("tax_identification");
                        oEntity.adrs_house_street = oresult.GetString("adrs_house_street");
                        oEntity.adrs_barangay = oresult.GetInt64("adrs_barangay");
                        oEntity.adrs_town = oresult.GetInt64("adrs_town");
                        oEntity.adrs_province = oresult.GetInt64("adrs_province");
                        oEntity.adrs_country = oresult.GetInt64("adrs_country");
                        oEntity.zip_code = oresult.GetString("zip_code");
                        oEntity.email_address1 = oresult.GetString("email_address1");
                        oEntity.email_address2 = oresult.GetString("email_address2");
                        oEntity.email_address3 = oresult.GetString("email_address3");
                        oEntity.landphone1 = oresult.GetString("landphone1");
                        oEntity.landphone2 = oresult.GetString("landphone2");
                        oEntity.mobilephone1 = oresult.GetString("mobilephone1");
                        oEntity.mobilephone2 = oresult.GetString("mobilephone2");

                        oEntities.Add(oEntity);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oEntities);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }
            return serviceResponse;
        }       

        public async Task<ServiceResponse> UpdateNonPerson(Entities_NonPersonModel  nonperson)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE entities.`non_persons` 
	                                        SET 
                                                `nonperson_name` = @nonperson_name,
                                                `contact_person_id1` = @contact_person_id1, 
	                                            `contact_person_id2` = @contact_person_id2, 
	                                            `tax_identification` = @tax_identification, 
	                                            `adrs_house_street` = @adrs_house_street, 
	                                            `adrs_barangay` = @adrs_barangay, 
	                                            `adrs_town` = @adrs_town, 
	                                            `adrs_province` = @adrs_province, 
	                                            `adrs_country` = @adrs_country, 
	                                            `zip_code` = @zip_code, 
	                                            `email_address1` = @email_address1, 
	                                            `email_address2` = @email_address2, 
	                                            `email_address3` = @email_address3, 
	                                            `landphone1` = @landphone1, 
	                                            `landphone2` = @landphone2, 
	                                            `mobilephone1` = @mobilephone1, 
	                                            `mobilephone2` = @mobilephone2
	                                        WHERE
                                                nonperson_id = @nonperson_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@nonperson_name", System.Data.DbType.String, nonperson.nonperson_name);
                commonFunctions.BindParameter(oCommand, "@contact_person_id1", System.Data.DbType.Int32, nonperson.contact_person1);
                commonFunctions.BindParameter(oCommand, "@contact_person_id2", System.Data.DbType.Int32, nonperson.contact_person2);
                commonFunctions.BindParameter(oCommand, "@tax_identification", System.Data.DbType.String, nonperson.tax_identification);
                commonFunctions.BindParameter(oCommand, "@adrs_house_street", System.Data.DbType.String, nonperson.adrs_house_street);
                commonFunctions.BindParameter(oCommand, "@adrs_barangay", System.Data.DbType.Int32, nonperson.adrs_barangay);
                commonFunctions.BindParameter(oCommand, "@adrs_town", System.Data.DbType.Int32, nonperson.adrs_town);
                commonFunctions.BindParameter(oCommand, "@adrs_province", System.Data.DbType.Int32, nonperson.adrs_province);
                commonFunctions.BindParameter(oCommand, "@adrs_country", System.Data.DbType.Int32, nonperson.adrs_country);
                commonFunctions.BindParameter(oCommand, "@zip_code", System.Data.DbType.String, nonperson.zip_code);
                commonFunctions.BindParameter(oCommand, "@email_address1", System.Data.DbType.String, nonperson.email_address1);
                commonFunctions.BindParameter(oCommand, "@email_address2", System.Data.DbType.String, nonperson.email_address2);
                commonFunctions.BindParameter(oCommand, "@email_address3", System.Data.DbType.String, nonperson.email_address3);
                commonFunctions.BindParameter(oCommand, "@landphone1", System.Data.DbType.String, nonperson.landphone1);
                commonFunctions.BindParameter(oCommand, "@landphone2", System.Data.DbType.String, nonperson.landphone2);
                commonFunctions.BindParameter(oCommand, "@mobilephone1", System.Data.DbType.String, nonperson.mobilephone1);
                commonFunctions.BindParameter(oCommand, "@mobilephone2", System.Data.DbType.String, nonperson.mobilephone2);
                commonFunctions.BindParameter(oCommand, "@nonperson_id", System.Data.DbType.Int64, nonperson.nonperson_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(nonperson);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;

        }

        public async Task<ServiceResponse> DeleteNonPerson(Int32 nonperson_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  entities.`non_persons` 
	                                        WHERE
                                                nonperson_id = @nonperson_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@nonperson_id", System.Data.DbType.Int32, nonperson_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;

        }
    }
}