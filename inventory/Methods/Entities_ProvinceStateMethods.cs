
using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_ProvinceStateMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_ProvinceStateMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> SaveProvinceState(Entities_ProvinceStateModel provincestate)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `entities`.`provinces_states` 
	                                        (
                                                `province_state_name`,
                                                `country_id`
	                                        )
	                                     VALUES
                                            (
                                                @province_state_name,
                                                @country_id
                                            );";
 
                oCommand.Parameters.Clear();
 
                commonFunctions.BindParameter(oCommand, "@province_state_name", System.Data.DbType.String, provincestate.province_state_name);
                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.String, provincestate.country.country_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    provincestate.province_state_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(provincestate);

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

        public async Task<ServiceResponse> GetProvinceState(int countryId)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `province_state_id`, 
	                                            `province_state_name`,
                                                `country_id`
	 
	                                    FROM 
	                                        `entities`.`provinces_states`

                                        WHERE  country_id = @country_id";

                  oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, countryId);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_ProvinceStateModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_ProvinceStateModel();
                        hasValue = true;

                        oEntity.province_state_id = oresult.GetInt32("province_state_id");
                        oEntity.province_state_name = oresult.GetString("province_state_name");

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
                //serviceResponse.SetValues(500, "Internal Server Error", "");
                serviceResponse.SetValues(500, ex.ToString(), "");
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> GetOneProvinceState(long province_state_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `province_state_id`, 
	                                            `province_state_name`,
                                                `country_id`
	 
	                                    FROM 
	                                        `entities`.`provinces_states`
                                        WHERE
                                            `province_state_id` = @province_state_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, province_state_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_ProvinceStateModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_ProvinceStateModel();
                        hasValue = true;

                        oEntity.country = new Entities_CountryModel();

                        oEntity.province_state_id = oresult.GetInt32("province_state_id");
                        oEntity.province_state_name = oresult.GetString("province_state_name");
                        oEntity.country.country_id = oresult.GetInt32("country_id");

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

        public async Task<ServiceResponse> UpdateProvinceState(Entities_ProvinceStateModel  provincestate)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `entities`.`provinces_states` 
	                                        SET
                                                `province_state_name` =  @province_state_name,
                                                 `country_id` =  @country_id
	                                        WHERE
                                                `province_state_id` = @province_state_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@province_state_name", System.Data.DbType.String, provincestate.province_state_name);
                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, provincestate.province_state_id);
                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, provincestate.country.country_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(provincestate);

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

        public async Task<ServiceResponse> DeleteProvinceState(Int32 province_state_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `entities`.`provinces_states` 
	                                        WHERE
                                                `province_state_id` = @province_state_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, province_state_id);


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
 

