
using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_TownMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_TownMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> SaveTown(Entities_TownModel towns)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `entities`.`towns` 
	                                        (
                                                `town_name`,
                                                `province_state_id`
	                                        )
	                                     VALUES
                                            (
                                                @town_name,
                                                @province_state_id
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, towns.province_state_id);
                commonFunctions.BindParameter(oCommand, "@town_name", System.Data.DbType.String, towns.town_name);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    towns.town_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(towns);

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

        public async Task<ServiceResponse> GetTown(int province_state_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `town_id`, 
	                                            `town_name`,
                                                `province_state_id`
	 
	                                    FROM 
	                                        `entities`.`towns`
                                         WHERE province_state_id = @province_state_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, province_state_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_TownModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_TownModel();
                        hasValue = true;

                        oEntity.town_id = oresult.GetInt32("town_id");
                        oEntity.town_name = oresult.GetString("town_name");
                        oEntity.province_state_id = oresult.GetInt32("province_state_id");

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

        public async Task<ServiceResponse> GetOneTown(long town_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `town_id`, 
	                                            `town_name`,
                                                `province_state_id`
	 
	                                    FROM 
	                                        `entities`.`towns`
                                        WHERE
                                            `town_id` = @town_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, town_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_TownModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_TownModel();
                        hasValue = true;

                        oEntity.town_id = oresult.GetInt32("town_id");
                        oEntity.town_name = oresult.GetString("town_name");
                        oEntity.province_state_id = oresult.GetInt32("province_state_id");

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

        public async Task<ServiceResponse> UpdateTown(Entities_TownModel  towns)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `entities`.`towns` 
	                                        SET
                                                `town_name` =  @town_name,
                                                 `province_state_id` = @province_state_id
	                                        WHERE
                                                `town_id` = @town_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@town_name", System.Data.DbType.String, towns.town_name);
                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, towns.town_id);
                commonFunctions.BindParameter(oCommand, "@province_state_id", System.Data.DbType.Int32, towns.province_state_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(towns);

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

        public async Task<ServiceResponse> DeleteTown(long town_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `entities`.`towns` 
	                                        WHERE
                                                `town_id` = @town_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, town_id);


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
 

