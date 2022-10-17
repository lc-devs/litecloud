using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class NonIndustrialLaundryItemMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal NonIndustrialLaundryItemMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public NonIndustrialLaundryItemMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `id_item`, 
	                                            `description`	 
                                        FROM 
	                                        `laundry`.`non_industrial_laundry_items`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<NonIndustrialLaundryItem>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new NonIndustrialLaundryItem();
                        hasValue = true;

                        oEntity.id_item = oresult.GetInt32("id_item");
                        oEntity.description = oresult.GetString("description");
                        oEntities.Add(oEntity);
                    }
                }
                var oEntityOthers = new NonIndustrialLaundryItem();
                hasValue = true;

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

        public async Task<ServiceResponse> SaveItemAsync(NonIndustrialLaundryItem item)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`non_industrial_laundry_items`
                                            (
                                                `description`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @description
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, item.description);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    item.id_item = oCommand.LastInsertedId;

                    string jsonString = JsonSerializer.Serialize(item);

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

        public async Task<ServiceResponse> UpdateItemAsync(NonIndustrialLaundryItem item)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`non_industrial_laundry_items`
                                            SET
                                                `description`= @description
                                            WHERE
	                                            `id_item` = @id_item;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_item", System.Data.DbType.Int32, item.id_item);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, item.description);
                
                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    item.id_item = oCommand.LastInsertedId;

                    string jsonString = JsonSerializer.Serialize(item);

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

        public async Task<ServiceResponse> DeleteItemAsync(int itemID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                if(itemID == 99999)
                {
                    serviceResponse.SetValues(400, "99999 Item code is not allowed to be deleted.", "");
                    return serviceResponse;
                }

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `laundry`.`non_industrial_laundry_items`
                                         WHERE
	                                        `id_item` = @id_item
	                                        AND `id_item`NOT IN (
                                                                    SELECT 	`item_code`
                                                                    FROM 
	                                                                    `laundry`.`pickups_non_industrial_items`  
                                                                    WHERE `item_code` = @id_item
                                                                );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_item", System.Data.DbType.Int32, itemID);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Item because it does not existed or it is being used.", "");
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
