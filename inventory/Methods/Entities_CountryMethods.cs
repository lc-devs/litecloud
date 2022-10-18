
using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_CountryMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_CountryMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> SaveCountry(Entities_CountryModel country)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `entities`.`countries` 
	                                        (
                                                `country_name`
	                                        )
	                                     VALUES
                                            (
                                                @country_name
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@country_name", System.Data.DbType.String, country.country_name);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    country.country_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(country);

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

        public async Task<ServiceResponse> GetCountries()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `country_id`, 
	                                            `country_name`
	 
	                                    FROM 
	                                        `entities`.`countries`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_CountryModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_CountryModel();
                        hasValue = true;

                        oEntity.country_id = oresult.GetInt32("country_id");
                        oEntity.country_name = oresult.GetString("country_name");

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

        public async Task<ServiceResponse> GetOneCountry(long country_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `country_id`, 
	                                             `country_name`
	 
	                                    FROM 
	                                        `entities`.`countries`
                                        WHERE
                                            `country_id` = @country_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int64, country_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_CountryModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_CountryModel();
                        hasValue = true;

                        oEntity.country_id = oresult.GetInt32("country_id");
                        oEntity.country_name = oresult.GetString("country_name");

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

        public async Task<ServiceResponse> UpdateCountry(Entities_CountryModel  country)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `entities`.`countries` 
	                                        SET
                                                `country_name` =  @country_name
	                                        WHERE
                                                `country_id` = @country_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@country_name", System.Data.DbType.String, country.country_name);
                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, country.country_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(country);

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

        public async Task<ServiceResponse> DeleteCountry(Int32 country_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `entities`.`countries` 
	                                        WHERE
                                                `country_id` = @country_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, country_id);


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
 

