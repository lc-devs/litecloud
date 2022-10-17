using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class IndustrialCategoryMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal IndustrialCategoryMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public IndustrialCategoryMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `id_category`, 
	                                            `description`, 
	                                            `unit`
	 
	                                        FROM 
	                                        `laundry`.`industrial_categories`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<IndustrialCategory>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new IndustrialCategory();
                        hasValue = true;

                        oEntity.id_category = oresult.GetInt32("id_category");
                        oEntity.description = oresult.GetString("description");
                        oEntity.unit = oresult.GetString("unit");
                  
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

        public async Task<ServiceResponse> SaveIndustrialCategoryAsync(IndustrialCategory industrial)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`industrial_categories`
                                                    (description,
                                                     unit
                                                    )                                           
                                                VALUES( 
                                                    @description,
                                                    @unit
                                                );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, industrial.description);
                commonFunctions.BindParameter(oCommand, "@unit", System.Data.DbType.String, industrial.unit);

                int i = await oCommand.ExecuteNonQueryAsync();

                

                if (i > 0)
                {
                    industrial.id_category = oCommand.LastInsertedId;

                    string jsonString = JsonSerializer.Serialize(industrial);

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

        public async Task<ServiceResponse> UpdateIndustrialCategoryAsync(IndustrialCategory industrial)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`industrial_categories`
                                            SET
                                                `description` = @description, 
	                                            `unit` = @unit
                                            WHERE
	                                            `id_category` = @id_category;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_category", System.Data.DbType.Int32, industrial.id_category);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, industrial.description);
                commonFunctions.BindParameter(oCommand, "@unit", System.Data.DbType.String, industrial.unit);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(industrial);
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

        public async Task<ServiceResponse> DeleteIndustrialCategoryAsync(int id_category)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM `laundry`.`industrial_categories`
                                            WHERE
	                                            `id_category` = @id_category
                                                AND `id_category` NOT IN (
                                                                                SELECT 	`category`	 
                                                                                FROM 
	                                                                                `laundry`.`industrial_laundry_items` 
                                                                                WHERE
                                                                                    `category` =  @id_category
                                                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_category", System.Data.DbType.Int32, id_category);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Category because it does not existed or it is being used.", "");
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
