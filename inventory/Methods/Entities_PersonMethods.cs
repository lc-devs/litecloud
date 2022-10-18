using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_PersonMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_PersonMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> GetAllPersons()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `person_id`, 
	                                            `lastname`, 
	                                            `firstname`, 
	                                            `middlename`, 
	                                            `sex`, 
	                                            `birthdate`, 
	                                            `civilstatus`, 
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
	                                        entities.`persons`";


                oCommand.Parameters.Clear();

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_PersonModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_PersonModel();
                        hasValue = true;

                        oEntity.person_id = oresult.GetInt64("person_id");

                        oEntity.lastname = oresult.GetString("lastname");
                        oEntity.firstname = oresult.GetString("firstname");
                        oEntity.middlename = oresult.GetString("middlename");
                        oEntity.sex = oresult.GetInt32("sex");
                        oEntity.birthdate = oresult.GetDateTime("birthdate").ToString("yyyy-MM-dd");
                        oEntity.civilstatus = oresult.GetInt32("civilstatus");
                        oEntity.tax_identification = oresult.GetString("tax_identification");
                        oEntity.adrs_house_street = oresult.GetString("adrs_house_street");
                        oEntity.adrs_barangay = oresult.GetInt32("adrs_barangay");
                        oEntity.adrs_town = oresult.GetInt32("adrs_town");
                        oEntity.adrs_province = oresult.GetInt32("adrs_province");
                        oEntity.adrs_country = oresult.GetInt32("adrs_country");
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
        public async Task<ServiceResponse> GetPersonbyName(string name)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `person_id`, 
	                                            `lastname`, 
	                                            `firstname`, 
	                                            `middlename`, 
	                                            `sex`, 
	                                            `birthdate`, 
	                                            `civilstatus`, 
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
	                                        entities.`persons`

                                        WHERE `lastname` = @lastname OR `firstname` = @firstname";


                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@lastname", System.Data.DbType.String, name);
                commonFunctions.BindParameter(oCommand, "@firstname", System.Data.DbType.String, name);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_PersonModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_PersonModel();
                        hasValue = true;

                        oEntity.person_id = oresult.GetInt64("person_id");
                        oEntity.lastname = oresult.GetString("lastname");
                        oEntity.firstname = oresult.GetString("firstname");
                        oEntity.middlename = oresult.GetString("middlename");
                        oEntity.sex = oresult.GetInt32("sex");
                        oEntity.birthdate = oresult.GetDateTime("birthdate").ToString("yyyy-MM-dd");
                        oEntity.civilstatus = oresult.GetInt32("civilstatus");
                        oEntity.tax_identification = oresult.GetString("tax_identification");
                        oEntity.adrs_house_street = oresult.GetString("adrs_house_street");
                        oEntity.adrs_barangay = oresult.GetInt32("adrs_barangay");
                        oEntity.adrs_town = oresult.GetInt32("adrs_town");
                        oEntity.adrs_province = oresult.GetInt32("adrs_province");
                        oEntity.adrs_country = oresult.GetInt32("adrs_country");
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
        public async Task<ServiceResponse> SavePerson(Entities_PersonModel person)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO entities.`persons` 
	                                        (
                                                `lastname`,
                                                `firstname`,
                                                `middlename`,
                                                `sex`,
                                                `birthdate`,
                                                `civilstatus`,
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
                                                @lastname,
                                                @firstname,
                                                @middlename,
                                                @sex,
                                                @birthdate,
                                                @civilstatus,
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

                commonFunctions.BindParameter(oCommand, "@lastname", System.Data.DbType.String, person.lastname);
                commonFunctions.BindParameter(oCommand, "@firstname", System.Data.DbType.String, person.firstname);
                commonFunctions.BindParameter(oCommand, "@middlename", System.Data.DbType.String, person.middlename);
                commonFunctions.BindParameter(oCommand, "@sex", System.Data.DbType.Int32, person.sex);
                commonFunctions.BindParameter(oCommand, "@birthdate", System.Data.DbType.String, person.birthdate);
                commonFunctions.BindParameter(oCommand, "@civilstatus", System.Data.DbType.Int32, person.civilstatus);
                commonFunctions.BindParameter(oCommand, "@tax_identification", System.Data.DbType.String, person.tax_identification);
                commonFunctions.BindParameter(oCommand, "@adrs_house_street", System.Data.DbType.String, person.adrs_house_street);
                commonFunctions.BindParameter(oCommand, "@adrs_barangay", System.Data.DbType.Int64, person.adrs_barangay);
                commonFunctions.BindParameter(oCommand, "@adrs_town", System.Data.DbType.Int64, person.adrs_town);
                commonFunctions.BindParameter(oCommand, "@adrs_province", System.Data.DbType.Int64, person.adrs_province);
                commonFunctions.BindParameter(oCommand, "@adrs_country", System.Data.DbType.Int64, person.adrs_country);
                commonFunctions.BindParameter(oCommand, "@zip_code", System.Data.DbType.String, person.zip_code);
                commonFunctions.BindParameter(oCommand, "@email_address1", System.Data.DbType.String, person.email_address1);
                commonFunctions.BindParameter(oCommand, "@email_address2", System.Data.DbType.String, person.email_address2);
                commonFunctions.BindParameter(oCommand, "@email_address3", System.Data.DbType.String, person.email_address3);
                commonFunctions.BindParameter(oCommand, "@landphone1", System.Data.DbType.String, person.landphone1);
                commonFunctions.BindParameter(oCommand, "@landphone2", System.Data.DbType.String, person.landphone2);
                commonFunctions.BindParameter(oCommand, "@mobilephone1", System.Data.DbType.String, person.mobilephone1);
                commonFunctions.BindParameter(oCommand, "@mobilephone2", System.Data.DbType.String, person.mobilephone2);
 

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    person.person_id = i;
                    string jsonString = JsonSerializer.Serialize(person);

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

        public async Task<ServiceResponse> GetPersonDetails(long personId)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `person_id`, 
	                                            `lastname`, 
	                                            `firstname`, 
	                                            `middlename`, 
	                                            `sex`, 
	                                            `birthdate`, 
	                                            `civilstatus`, 
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
	                                        entities.`persons`

                                        WHERE person_id = @person_id";


                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@person_id", System.Data.DbType.Int32, personId);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_PersonModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_PersonModel();
                        hasValue = true;

                        oEntity.person_id = oresult.GetInt64("person_id");
                        oEntity.lastname = oresult.GetString("lastname");
                        oEntity.firstname = oresult.GetString("firstname");
                        oEntity.middlename = oresult.GetString("middlename");
                        oEntity.sex = oresult.GetInt32("sex");
                        oEntity.birthdate = oresult.GetDateTime("birthdate").ToString("yyyy-MM-dd");
                        oEntity.civilstatus = oresult.GetInt32("civilstatus");
                        oEntity.tax_identification = oresult.GetString("tax_identification");
                        oEntity.adrs_house_street = oresult.GetString("adrs_house_street");
                        oEntity.adrs_barangay = oresult.GetInt32("adrs_barangay");
                        oEntity.adrs_town = oresult.GetInt32("adrs_town");
                        oEntity.adrs_province = oresult.GetInt32("adrs_province");
                        oEntity.adrs_country = oresult.GetInt32("adrs_country");
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

        
        public async Task<ServiceResponse> UpdatePerson(Entities_PersonModel  person)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE entities.`persons` 
	                                        SET
                                                `lastname` = @lastname,
                                                `firstname` = @firstname,
                                                `sex` = @sex,
                                                `birthdate` = @birthdate,
                                                `civilstatus` = @civilstatus,
                                                `tax_identification` = @civilstatus,
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
                                                person_id = @person_id;";

                oCommand.Parameters.Clear();
               
                commonFunctions.BindParameter(oCommand, "@person_id", System.Data.DbType.Int32, person.person_id);
                commonFunctions.BindParameter(oCommand, "@lastname", System.Data.DbType.String, person.lastname);
                commonFunctions.BindParameter(oCommand, "@firstname", System.Data.DbType.String, person.firstname);
                commonFunctions.BindParameter(oCommand, "@sex", System.Data.DbType.Int32, person.sex);
                commonFunctions.BindParameter(oCommand, "@birthdate", System.Data.DbType.String, person.birthdate);
                commonFunctions.BindParameter(oCommand, "@civilstatus", System.Data.DbType.Int32, person.civilstatus);
                commonFunctions.BindParameter(oCommand, "@tax_identification", System.Data.DbType.String, person.tax_identification);
                commonFunctions.BindParameter(oCommand, "@adrs_house_street", System.Data.DbType.String, person.adrs_house_street);
                commonFunctions.BindParameter(oCommand, "@adrs_barangay", System.Data.DbType.Int64, person.adrs_barangay);
                commonFunctions.BindParameter(oCommand, "@adrs_town", System.Data.DbType.Int64, person.adrs_town);
                commonFunctions.BindParameter(oCommand, "@adrs_province", System.Data.DbType.Int64, person.adrs_province);
                commonFunctions.BindParameter(oCommand, "@adrs_country", System.Data.DbType.Int64, person.adrs_country);
                commonFunctions.BindParameter(oCommand, "@zip_code", System.Data.DbType.String, person.zip_code);
                commonFunctions.BindParameter(oCommand, "@email_address1", System.Data.DbType.String, person.email_address1);
                commonFunctions.BindParameter(oCommand, "@email_address2", System.Data.DbType.String, person.email_address2);
                commonFunctions.BindParameter(oCommand, "@email_address3", System.Data.DbType.String, person.email_address3);
                commonFunctions.BindParameter(oCommand, "@landphone1", System.Data.DbType.String, person.landphone1);
                commonFunctions.BindParameter(oCommand, "@landphone2", System.Data.DbType.String, person.landphone2);
                commonFunctions.BindParameter(oCommand, "@mobilephone1", System.Data.DbType.String, person.mobilephone1);
                commonFunctions.BindParameter(oCommand, "@mobilephone2", System.Data.DbType.String, person.mobilephone2);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(person);

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

        public async Task<ServiceResponse> DeletePerson(Int32 person_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  entities.`persons` 
	                                        WHERE
                                                person_id = @person_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@person_id", System.Data.DbType.Int32, person_id);


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