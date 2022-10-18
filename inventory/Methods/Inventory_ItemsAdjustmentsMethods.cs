using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_ItemsAdjustmentsMethods
    {
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_ItemsAdjustmentsMethods(AppDb db)
        {
            gDb = db;
        }

        //===================================================================
        //Function: SaveItemAdjustment
        //Purpose: To save items adjustment
        //Author: Romar Socobos
        //Parameter:
        //  Name             Comment
        //  -----------     --------------------------------
        //  item             object class Inventory_ItemsAdjustmentsModel
        //
        //Result:
        // serviceResponse (ServiceResponse Class)
        //===================================================================
        public async Task<ServiceResponse> SaveItemAdjustment(Inventory_ItemsAdjustmentsModel item)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {

                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`items_adjustments` 
	                                        (
                                                `adjument_date`, 
	                                            `template_id`, 
	                                            `destination_id`, 
	                                            `source_id`, 
	                                            `item_id`, 
	                                            `quantity`, 
	                                            `remarks`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                NOW(), 
	                                            @template_id, 
	                                            @destination_id, 
	                                            @source_id, 
	                                            @item_id, 
	                                            @quantity, 
	                                            @remarks, 
	                                            @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_id", System.Data.DbType.Int64, item.template_id);
                commonFunctions.BindParameter(oCommand, "@destination_id", System.Data.DbType.Int64, item.destination_id);
                commonFunctions.BindParameter(oCommand, "@source_id", System.Data.DbType.Int64, item.source_id);
                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int64, item.item_id);
                commonFunctions.BindParameter(oCommand, "@quantity", System.Data.DbType.Double, item.quantity);
                commonFunctions.BindParameter(oCommand, "@remarks", System.Data.DbType.String, item.remarks);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, item.user_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    item.adjustment_id = oCommand.LastInsertedId;
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


        //===================================================================
        //Function: GetItemAdjustmentByDate
        //Purpose: To get items adjustment baseed on date range
        //Author: Romar Socobos
        //Parameter:
        //  Name                            Comment
        //  -----------                     --------------------------------
        //  adjustment_date_from            start of adjustment date
        //  adjustment_date_to              end of adjustment date
        //
        //Result:
        // serviceResponse (ServiceResponse Class)
        //===================================================================
        public async Task<ServiceResponse> GetItemAdjustmentByDate(string adjustment_date_from, string adjustment_date_to)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {

                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT
                                                `adjustment_id`,
                                                `adjument_date`, 
	                                            `template_id`, 
	                                            `destination_id`, 
	                                            `source_id`, 
	                                            `item_id`, 
	                                            `quantity`, 
	                                            `remarks`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                     FROM `inventory`.`items_adjustments`
	                                     WHERE
                                            `adjument_date` between @date_from AND @date_to
                                            ;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@date_from", System.Data.DbType.String, adjustment_date_from);
                commonFunctions.BindParameter(oCommand, "@date_to", System.Data.DbType.String, adjustment_date_to);


                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_ItemsAdjustmentsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_ItemsAdjustmentsModel();
                        hasValue = true;

                        oEntity.adjustment_id = oresult.GetInt64("adjustment_id"); 
                        oEntity.adjustment_date = oresult.GetDateTime("adjument_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.template_id = oresult.GetInt64("template_id");
                        oEntity.destination_id = oresult.GetInt64("destination_id");
                        oEntity.source_id = oresult.GetInt64("source_id");
                        oEntity.item_id = oresult.GetInt64("item_id");
                        oEntity.quantity = oresult.GetDouble("quantity");
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
        //===================================================================
        //Function: GetOneItemAdjustmentByDate
        //Purpose: To get one adjustment baseed on id
        //Author: Romar Socobos
        //Parameter:
        //  Name                            Comment
        //  -----------                     --------------------------------
        //  adjustment_id                   id if the item adjustment
        //
        //Result:
        // serviceResponse (ServiceResponse Class)
        //===================================================================
        public async Task<ServiceResponse> GetOneItemAdjustment(long adjustment_id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {

                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT
                                                `adjustment_id`,
                                                `adjument_date`, 
	                                            `template_id`, 
	                                            `destination_id`, 
	                                            `source_id`, 
	                                            `item_id`, 
	                                            `quantity`, 
	                                            `remarks`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                     FROM `inventory`.`items_adjustments`
	                                     WHERE
                                            `adjustment_id` = @adjustment_id
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@adjustment_id", System.Data.DbType.Int64, adjustment_id);


                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_ItemsAdjustmentsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_ItemsAdjustmentsModel();
                        hasValue = true;

                        oEntity.adjustment_id = oresult.GetInt64("adjustment_id"); 
                        oEntity.adjustment_date = oresult.GetDateTime("adjument_date").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.template_id = oresult.GetInt64("template_id");
                        oEntity.destination_id = oresult.GetInt64("destination_id");
                        oEntity.source_id = oresult.GetInt64("source_id");
                        oEntity.item_id = oresult.GetInt64("item_id");
                        oEntity.quantity = oresult.GetDouble("quantity");
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


    }
}
