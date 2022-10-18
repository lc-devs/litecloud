using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_InventoryUnitsItemsMethods
    {
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_InventoryUnitsItemsMethods(AppDb db)
        {
            gDb = db;
        }


        public async Task<ServiceResponse> GetInventory_InventoryUnitsItems()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `unit_item_id`, 
                                                `item_id`, 
	                                            `unit_id`, 
	                                            `starting_period`, 
	                                            `last_entry`, 
                                                `starting_quantity`, 
	                                            `quantity_in`, 
	                                            `quantity_out`, 
	                                            `ending_quantity`, 
                                                `starting_cost`, 
	                                            `cost_in`, 
	                                            `cost_out`, 
	                                            `ending_cost`, 
                                                `unit_cost`, 
	                                            `last_highest_in_unit_cost`,  
	                                            `user_id`, 
	                                            `entry_date`
	                                    FROM 
	                                        `inventory`.`inventory_units_items`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntity = new List<Inventory_InventoryUnitsItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryUnitsItemsModel();
                        hasValue = true;

                        oInventory.unit_item_id = oresult.GetInt32("unit_item_id");
                        oInventory.item_id = oresult.GetInt32("item_id");
                        oInventory.unit_id = oresult.GetInt32("unit_id");
                        oInventory.starting_period = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oInventory.last_entry =  oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oInventory.starting_quantity = oresult.GetDouble("starting_quantity");
                        oInventory.quantity_in = oresult.GetDouble("quantity_in");
                        oInventory.quantity_out = oresult.GetDouble("quantity_out");
                        oInventory.ending_quantity = oresult.GetDouble("ending_quantity");
                        oInventory.starting_cost = oresult.GetDouble("starting_cost");
                        oInventory.cost_in = oresult.GetDouble("cost_in");
                        oInventory.cost_out = oresult.GetDouble("cost_out");
                        oInventory.ending_cost = oresult.GetDouble("ending_cost");
                        oInventory.unit_cost = oresult.GetDouble("unit_cost");
                        oInventory.last_highest_in_unit_cost = oresult.GetDouble("last_highest_in_unit_cost");
                        oInventory.user_id = oresult.GetInt32("user_id");
                        oInventory.entry_date = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");

                        oEntity.Add(oInventory);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oEntity);

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
     

        public async Task<ServiceResponse> Save(Inventory_InventoryUnitsItemsModel unitsitems)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`inventory_units_items` 
	                                        (
                                                `item_id`, 
	                                            `unit_id`, 
	                                            `starting_period`, 
	                                            `last_entry`, 
                                                `starting_quantity`, 
	                                            `quantity_in`, 
	                                            `quantity_out`, 
	                                            `ending_quantity`, 
                                                `starting_cost`, 
	                                            `cost_in`, 
	                                            `cost_out`, 
	                                            `ending_cost`, 
                                                `unit_cost`, 
	                                            `last_highest_in_unit_cost`,  
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @item_id, 
	                                            @unit_id, 
	                                            @starting_period, 
	                                            @last_entry,
                                                @starting_quantity, 
	                                            @quantity_in, 
	                                            @quantity_out, 
	                                            @ending_quantity,  
                                                @starting_cost, 
	                                            @cost_in, 
	                                            @cost_out, 
	                                            @ending_cost, 
                                                @unit_cost,  
	                                            @last_highest_in_unit_cost, 
	                                            @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int32, unitsitems.item_id);
                commonFunctions.BindParameter(oCommand, "@unit_id", System.Data.DbType.Int32, unitsitems.unit_id);
                commonFunctions.BindParameter(oCommand, "@starting_period", System.Data.DbType.Int32, unitsitems.starting_period);
                commonFunctions.BindParameter(oCommand, "@last_entry", System.Data.DbType.String, unitsitems.last_entry);
                commonFunctions.BindParameter(oCommand, "@starting_quantity", System.Data.DbType.String, unitsitems.starting_quantity);
                commonFunctions.BindParameter(oCommand, "@quantity_in", System.Data.DbType.Double, unitsitems.quantity_in);
                commonFunctions.BindParameter(oCommand, "@quantity_out", System.Data.DbType.Double, unitsitems.quantity_out);
                commonFunctions.BindParameter(oCommand, "@ending_quantity", System.Data.DbType.Double, unitsitems.ending_quantity);
                commonFunctions.BindParameter(oCommand, "@starting_cost", System.Data.DbType.Double, unitsitems.starting_cost);
                commonFunctions.BindParameter(oCommand, "@cost_in", System.Data.DbType.Double, unitsitems.cost_in);
                commonFunctions.BindParameter(oCommand, "@cost_out", System.Data.DbType.Double, unitsitems.cost_out);
                commonFunctions.BindParameter(oCommand, "@ending_cost", System.Data.DbType.Double, unitsitems.ending_cost);
                commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, unitsitems.unit_cost);
                commonFunctions.BindParameter(oCommand, "@last_highest_in_unit_cost", System.Data.DbType.Double, unitsitems.last_highest_in_unit_cost);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, unitsitems.user_id);
                commonFunctions.BindParameter(oCommand, "@entry_date", System.Data.DbType.Int32, unitsitems.entry_date);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    unitsitems.unit_item_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(unitsitems);

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

       
        public async Task<ServiceResponse> GetOne(long id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT  `unit_item_id`, 
                                                  `item_id`,
	                                              `unit_id`, 
	                                              `starting_period`, 
	                                              `last_entry`, 
                                                  `starting_quantity`, 
	                                              `quantity_in`, 
	                                              `quantity_out`, 
	                                              `ending_quantity`, 
                                                  `starting_cost`, 
	                                              `cost_in`, 
	                                              `cost_out`, 
	                                              `ending_cost`, 
                                                  `unit_cost`, 
	                                              `last_highest_in_unit_cost`,  
	                                              `user_id`, 
	                                              `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`inventory_units_items` 
                                        WHERE unit_item_id = @unit_item_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_item_id", System.Data.DbType.Int32, id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_InventoryUnitsItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_InventoryUnitsItemsModel();
                        hasValue = true;

                        oEntity.unit_item_id = oresult.GetInt32("unit_item_id");
                        oEntity.item_id = oresult.GetInt32("item_id");
                        oEntity.unit_id = oresult.GetInt32("unit_id");
                        oEntity.starting_period = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.last_entry = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.starting_quantity = oresult.GetDouble("starting_quantity");
                        oEntity.quantity_in = oresult.GetDouble("quantity_in");
                        oEntity.quantity_out = oresult.GetDouble("quantity_out");
                        oEntity.ending_quantity = oresult.GetDouble("ending_quantity");
                        oEntity.starting_cost = oresult.GetDouble("starting_cost");
                        oEntity.cost_in = oresult.GetDouble("cost_in");
                        oEntity.cost_out = oresult.GetDouble("cost_out");
                        oEntity.ending_cost = oresult.GetDouble("ending_cost");
                        oEntity.unit_cost = oresult.GetDouble("unit_cost");
                        oEntity.last_highest_in_unit_cost = oresult.GetDouble("last_highest_in_unit_cost");
                        oEntity.user_id = oresult.GetInt32("user_id");
                        oEntity.entry_date = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");

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

        public async Task<ServiceResponse> GetItemsByUnitLocation(long unit_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT  `unit_item_id`, 
                                                  `item_id`,
	                                              `unit_id`, 
	                                              `starting_period`, 
	                                              `last_entry`, 
                                                  `starting_quantity`, 
	                                              `quantity_in`, 
	                                              `quantity_out`, 
	                                              `ending_quantity`, 
                                                  `starting_cost`, 
	                                              `cost_in`, 
	                                              `cost_out`, 
	                                              `ending_cost`, 
                                                  `unit_cost`, 
	                                              `last_highest_in_unit_cost`,  
	                                              `user_id`, 
	                                              `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`inventory_units_items` 
                                        WHERE unit_id = @unit_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_id", System.Data.DbType.Int32, unit_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_InventoryUnitsItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_InventoryUnitsItemsModel();
                        hasValue = true;

                        oEntity.unit_item_id = oresult.GetInt32("unit_item_id");
                        oEntity.item_id = oresult.GetInt32("item_id");
                        oEntity.unit_id = oresult.GetInt32("unit_id");
                        oEntity.starting_period = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.last_entry = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.starting_quantity = oresult.GetDouble("starting_quantity");
                        oEntity.quantity_in = oresult.GetDouble("quantity_in");
                        oEntity.quantity_out = oresult.GetDouble("quantity_out");
                        oEntity.ending_quantity = oresult.GetDouble("ending_quantity");
                        oEntity.starting_cost = oresult.GetDouble("starting_cost");
                        oEntity.cost_in = oresult.GetDouble("cost_in");
                        oEntity.cost_out = oresult.GetDouble("cost_out");
                        oEntity.ending_cost = oresult.GetDouble("ending_cost");
                        oEntity.unit_cost = oresult.GetDouble("unit_cost");
                        oEntity.last_highest_in_unit_cost = oresult.GetDouble("last_highest_in_unit_cost");
                        oEntity.user_id = oresult.GetInt32("user_id");
                        oEntity.entry_date = oresult.GetDateTime("entry_date").ToString("yyyy-MM-dd HH:mm:ss");

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

        public async Task<ServiceResponse> Update(Inventory_InventoryUnitsItemsModel unitsitems)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`inventory_units_items` 
	                                       SET
                                                `unit_item_id` = @unit_item_id, 
	                                            `item_id` =  @item_id, 
	                                            `unit_id` =  @unit_id, 
                                                `starting_period` = @starting_period, 
	                                            `last_entry` =  @last_entry, 
	                                            `starting_quantity` =  @starting_quantity, 
                                                `quantity_in` = @quantity_in, 
	                                            `quantity_out` =  @quantity_out, 
	                                            `ending_quantity` =  @ending_quantity, 
                                                `starting_cost` = @starting_cost, 
	                                            `cost_in` =  @cost_in, 
	                                            `cost_out` =  @cost_out, 
                                                `ending_cost` = @ending_cost, 
	                                            `unit_cost` =  @unit_cost, 
	                                            `last_highest_in_unit_cost` =  @last_highest_in_unit_cost, 
	                                            `user_id` =  @user_id, 
	                                            `entry_date` = NOW()
	                                        WHERE unit_item_id = @unit_item_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_item_id", System.Data.DbType.Int32, unitsitems.unit_item_id);
                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int32, unitsitems.item_id);
                commonFunctions.BindParameter(oCommand, "@unit_id", System.Data.DbType.Int32, unitsitems.unit_id);
                commonFunctions.BindParameter(oCommand, "@starting_period", System.Data.DbType.Int32, unitsitems.starting_period);
                commonFunctions.BindParameter(oCommand, "@last_entry", System.Data.DbType.String, unitsitems.last_entry);
                commonFunctions.BindParameter(oCommand, "@starting_quantity", System.Data.DbType.String, unitsitems.starting_quantity);
                commonFunctions.BindParameter(oCommand, "@quantity_in", System.Data.DbType.Double, unitsitems.quantity_in);
                commonFunctions.BindParameter(oCommand, "@quantity_out", System.Data.DbType.Double, unitsitems.quantity_out);
                commonFunctions.BindParameter(oCommand, "@ending_quantity", System.Data.DbType.Double, unitsitems.ending_quantity);
                commonFunctions.BindParameter(oCommand, "@starting_cost", System.Data.DbType.Double, unitsitems.starting_cost);
                commonFunctions.BindParameter(oCommand, "@cost_in", System.Data.DbType.Double, unitsitems.cost_in);
                commonFunctions.BindParameter(oCommand, "@cost_out", System.Data.DbType.Double, unitsitems.cost_out);
                commonFunctions.BindParameter(oCommand, "@ending_cost", System.Data.DbType.Double, unitsitems.ending_cost);
                commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, unitsitems.unit_cost);
                commonFunctions.BindParameter(oCommand, "@unit_id", System.Data.DbType.Double, unitsitems.unit_id);
                commonFunctions.BindParameter(oCommand, "@last_highest_in_unit_cost", System.Data.DbType.Double, unitsitems.last_highest_in_unit_cost);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, unitsitems.user_id);
                commonFunctions.BindParameter(oCommand, "@entry_date", System.Data.DbType.Int32, unitsitems.entry_date);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(unitsitems);

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

        public async Task<ServiceResponse> Delete(long id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM `inventory`.`inventory_units_items` 
	                                       WHERE unit_item_id = @unit_item_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_item_id", System.Data.DbType.String, id);
               
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
