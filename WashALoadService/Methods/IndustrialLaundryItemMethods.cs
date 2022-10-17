using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class IndustrialLaundryItemMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal IndustrialLaundryItemMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public IndustrialLaundryItemMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `id_item`, 
	                                            i.`description`, 
	                                            `category`,
	                                            c.`description` AS categery_description, 
	                                            `service`,
	                                            s.`description` AS service_description, 
	                                            `manual_costing`, 
	                                            `unit_cost_adl`, 
	                                            `unit_cost`
	 
                                        FROM 
	                                        `laundry`.`industrial_laundry_items` i
                                        INNER JOIN 
	                                        `laundry`.`industrial_services` s ON s.`id_service` = i.`service`
                                        INNER JOIN
	                                        `laundry`.`industrial_categories` c ON c.`id_category` = i.`category`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<IndustrialLaundryItem>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new IndustrialLaundryItem();
                        hasValue = true;

                        oEntity.id_item = oresult.GetInt32("id_item");
                        oEntity.description = oresult.GetString("description");

                        oEntity.category = new IndustrialCategory();
                        oEntity.category.id_category = oresult.GetInt32("category");
                        oEntity.category.description = oresult.GetString("categery_description");

                        oEntity.service = new IndustrialService();
                        oEntity.service.id_service = oresult.GetInt32("service");
                        oEntity.service.description = oresult.GetString("service_description");

                        oEntity.manual_costing = oresult.GetInt32("manual_costing");
                        oEntity.unit_cost_adl = oresult.GetDouble("unit_cost_adl");
                        oEntity.unit_cost = oresult.GetDouble("unit_cost");

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
        
        public async Task<ServiceResponse> SaveItemyAsync(IndustrialLaundryItem item)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`industrial_laundry_items`
                                            (
                                                `description`, 
	                                            `category`,
	                                            `service`,
	                                            `manual_costing`, 
	                                            `unit_cost_adl`, 
	                                            `unit_cost`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @description,
                                                @category,
                                                @service,
                                                @manual_costing,
                                                @unit_cost_adl,
                                                @unit_cost
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, item.description);
                commonFunctions.BindParameter(oCommand, "@category", System.Data.DbType.Int32, item.category.id_category);
                commonFunctions.BindParameter(oCommand, "@service", System.Data.DbType.Int32, item.service.id_service);
                commonFunctions.BindParameter(oCommand, "@manual_costing", System.Data.DbType.Int32, item.manual_costing);
                if(item.manual_costing != 1)
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, item.unit_cost_adl);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, item.unit_cost);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, 0.00);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, 0.00);
                }
                

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

        public async Task<ServiceResponse> UpdateItemAsync(IndustrialLaundryItem item)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`industrial_laundry_items`
                                            SET
                                                `description`= @description, 
	                                            `category` = @category,
	                                            `service` = @service,
	                                            `manual_costing`= @manual_costing, 
	                                            `unit_cost_adl`= @unit_cost_adl, 
	                                            `unit_cost` = @unit_cost
                                            WHERE
	                                            `id_item` = @id_item;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_item", System.Data.DbType.Int32, item.id_item);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, item.description);
                commonFunctions.BindParameter(oCommand, "@category", System.Data.DbType.Int32, item.category.id_category);
                commonFunctions.BindParameter(oCommand, "@service", System.Data.DbType.Int32, item.service.id_service);
                commonFunctions.BindParameter(oCommand, "@manual_costing", System.Data.DbType.Int32, item.manual_costing);
                if (item.manual_costing != 1)
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, item.unit_cost_adl);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, item.unit_cost);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, 0.00);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, 0.00);
                }


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

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `laundry`.`industrial_laundry_items`
                                         WHERE
	                                        `id_item` = @id_item
	                                        AND `id_item`NOT IN (
                                                                    SELECT 	`item_code`
                                                                    FROM 
	                                                                    `laundry`.`pickups_industrial_details`  
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


        //Designated Items

        public async Task<ServiceResponse> FindAllDesignatedItemsByCustomerAsync(int customerId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT i.`id_item`, 
	                                            i.`description`, 
	                                            `category`,
	                                            c.`description` AS categery_description, 
	                                            `service`,
	                                            s.`description` AS service_description, 
	                                            d.`manual_costing`, 
	                                            d.`unit_cost_adl`, 
	                                            d.`unit_cost`
	 
                                        FROM 
	                                        `laundry`.`industrial_laundry_items` i
                                        INNER JOIN 
                                            `laundry`.`designated_laundry_items` d ON i.`id_item` = d.`laundry_item_id`
                                        INNER JOIN 
	                                        `laundry`.`industrial_services` s ON s.`id_service` = i.`service`
                                        INNER JOIN
	                                        `laundry`.`industrial_categories` c ON c.`id_category` = i.`category`
                                        WHERE
                                               d.`customer_id` = @customer_id
                                        ORDER BY i.`description`;";
                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerId);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<IndustrialLaundryItem>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new IndustrialLaundryItem();
                        hasValue = true;

                        oEntity.id_item = oresult.GetInt32("id_item");
                        oEntity.description = oresult.GetString("description");

                        oEntity.category = new IndustrialCategory();
                        oEntity.category.id_category = oresult.GetInt32("category");
                        oEntity.category.description = oresult.GetString("categery_description");

                        oEntity.service = new IndustrialService();
                        oEntity.service.id_service = oresult.GetInt32("service");
                        oEntity.service.description = oresult.GetString("service_description");

                        oEntity.manual_costing = oresult.GetInt32("manual_costing");
                        oEntity.unit_cost_adl = oresult.GetDouble("unit_cost_adl");
                        oEntity.unit_cost = oresult.GetDouble("unit_cost");

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
        public async Task<ServiceResponse> SaveDesignatedItemyAsync(List<IndustrialLaundryItem> item, int customerId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                

                if (item.Count > 0)
                {
                    using var oCommand = gDb.Connection.CreateCommand();
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`designated_laundry_items`
                                            (
                                                `laundry_item_id`, 
	                                            `customer_id`,
	                                            `manual_costing`, 
	                                            `unit_cost_adl`, 
	                                            `unit_cost`
                                            )
	 
	                                    VALUES 
	                                         ";
                    string values = "";
                    bool hasData = false;

                    for (int i = 0; i < item.Count; i++)
                    {
                        var oDetail = item[i];
                        hasData = true;
                        values = values + "(" +  oDetail.id_item + ", " +  customerId + ", " + oDetail.manual_costing + ", " + oDetail.unit_cost_adl + ", " + oDetail.unit_cost +")";

                        if (i < item.Count - 1)
                        {
                            values = values + ",";
                        }

                    }

                    if (hasData)
                    {

                        oCommand.CommandText = oCommand.CommandText + values;

                        await oCommand.ExecuteNonQueryAsync();

                        serviceResponse.SetValues(200, "Success.", "");
                    }
                    else
                    {
                        serviceResponse.SetValues(400, "Bad request.", "");
                    }
                }
                else
                {
                    serviceResponse.SetValues(400, "Bad request.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateDesignatedItemAsync(IndustrialLaundryItem item, int customerId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`designated_laundry_items`
                                            SET
                                                `manual_costing`= @manual_costing, 
	                                            `unit_cost_adl`= @unit_cost_adl, 
	                                            `unit_cost` = @unit_cost
                                            WHERE
	                                            `laundry_item_id` = @laundry_item_id
                                                AND `customer_id` = @customer_id";
                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@laundry_item_id", System.Data.DbType.Int32, item.id_item);
                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerId);
                commonFunctions.BindParameter(oCommand, "@manual_costing", System.Data.DbType.Int32, item.manual_costing);
                if (item.manual_costing != 1)
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, item.unit_cost_adl);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, item.unit_cost);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost_adl", System.Data.DbType.Double, 0.00);
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, 0.00);
                }


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
        public async Task<ServiceResponse> DeleteDesignatedItemAsync(int itemID, int customerId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `laundry`.`designated_laundry_items`
                                         WHERE
	                                        `laundry_item_id` = @id_item 
                                            AND `customer_id` = @customer_id
	                                        AND `laundry_item_id`NOT IN (
                                                                    SELECT 	`item_code`
                                                                    FROM 
	                                                                    `laundry`.`pickups_industrial_details` d
                                                                    INNER JOIN `laundry`.`pickups_industrial` i ON i.`so_reference` = d.`so_reference`
                                                                    WHERE 
                                                                        `item_code` = @id_item
                                                                        AND `customer_id` = @customer_id
                                                                );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_item", System.Data.DbType.Int32, itemID);
                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerId);

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
