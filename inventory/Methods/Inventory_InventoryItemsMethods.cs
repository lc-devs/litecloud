using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
   public class Inventory_InventoryItemsMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_InventoryItemsMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> GetInventory_InventoryItems()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `item_id`,
                                                `category_id`,
                                                `brand_id`,
                                                `model_description`,
                                                `part_description`,
                                                `part_number`,
                                                `size`,
                                                `valve_type`,
                                                `ratio`,
                                                `thread_pattern`,
                                                `stocking_unit`,
                                                `retail_unit`,
                                                `rtu_over_stu`,
                                                `wtd_ave_cost`,
                                                `markup_rate`,
                                                `selling_price`,
                                                `user_id`,
                                                `entry_date`
	                                    FROM 
	                                        `inventory`.`inventory_items`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntity = new List<Inventory_InventoryItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryItemsModel();
                        hasValue = true;

                        oInventory.item_id = oresult.GetInt32("item_id");
                        oInventory.category_id = oresult.GetInt32("category_id");
                        oInventory.brand_id = oresult.GetInt32("brand_id");
                        oInventory.model_description = oresult.GetString("model_description");
                        oInventory.part_number = oresult.GetInt32("part_number");
                        oInventory.size = oresult.GetInt32("size");
                        oInventory.valve_type = oresult.GetInt32("valve_type");
                        oInventory.ratio = oresult.GetInt32("ratio");
                        oInventory.thread_pattern = oresult.GetInt32("thread_pattern");
                        oInventory.stocking_unit = oresult.GetString("stocking_unit");
                        oInventory.retail_unit = oresult.GetString("retail_unit");
                        oInventory.rtu_over_stu = oresult.GetDouble("rtu_over_stu");
                        oInventory.wtd_ave_cost = oresult.GetDouble("wtd_ave_cost");
                        oInventory.markup_rate = oresult.GetDouble("markup_rate");
                        oInventory.selling_price = oresult.GetDouble("selling_price");
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

       public async Task<ServiceResponse> SaveInventory_InventoryItems( Inventory_InventoryItemsModel inventory_items)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`inventory_items` 
	                                        (
                                               `item_id`,
                                                `category_id`,
                                                `brand_id`,
                                                `model_description`,
                                                `part_description`,
                                                `part_number`,
                                                `size`,
                                                `valve_type`,
                                                `ratio`,
                                                `thread_pattern`,
                                                `stocking_unit`,
                                                `retail_unit`,
                                                `rtu_over_stu`,
                                                `wtd_ave_cost`,
                                                `markup_rate`,
                                                `selling_price`,
                                                `user_id`,
                                                `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @item_id,
                                                @category_id,
                                                @brand_id,
                                                @model_description,
                                                @part_description,
                                                @part_number,
                                                @size,
                                                @valve_type,
                                                @ratio,
                                                @thread_pattern,
                                                @stocking_unit,
                                                @retail_unit,
                                                @rtu_over_stu,
                                                @wtd_ave_cost,
                                                @markup_rate,
                                                @selling_price,
                                                @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();
                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int32, inventory_items.item_id);
                commonFunctions.BindParameter(oCommand, "@category_id", System.Data.DbType.Int32, inventory_items.category_id);
                commonFunctions.BindParameter(oCommand, "@brand_id", System.Data.DbType.Int32, inventory_items.brand_id);
                commonFunctions.BindParameter(oCommand, "@model_description", System.Data.DbType.String, inventory_items.model_description);
                
                if(inventory_items.part_description == 0)
                {
                    commonFunctions.BindParameter(oCommand, "@part_description", System.Data.DbType.Int32, DBNull.Value);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@part_description", System.Data.DbType.Int32, inventory_items.part_description);
                }
                if(inventory_items.part_number == 0){
                    commonFunctions.BindParameter(oCommand, "@part_number", System.Data.DbType.Int32, DBNull.Value);
                
                }else{
                    commonFunctions.BindParameter(oCommand, "@part_number", System.Data.DbType.Int32, inventory_items.part_number);
                
                }
                if(inventory_items.size == 0){
                    commonFunctions.BindParameter(oCommand, "@size", System.Data.DbType.Int32, DBNull.Value);
                }else{
                    commonFunctions.BindParameter(oCommand, "@size", System.Data.DbType.Int32, inventory_items.size);
                }

                if(inventory_items.valve_type == 0){
                    commonFunctions.BindParameter(oCommand, "@valve_type", System.Data.DbType.Int32, DBNull.Value);
                }else{
                    commonFunctions.BindParameter(oCommand, "@valve_type", System.Data.DbType.Int32, inventory_items.valve_type);
                }

                if(inventory_items.ratio == 0){
                    commonFunctions.BindParameter(oCommand, "@ratio", System.Data.DbType.Int32, DBNull.Value);
                }else{
                    commonFunctions.BindParameter(oCommand, "@ratio", System.Data.DbType.Int32, inventory_items.ratio);
                }
                
                if(inventory_items.thread_pattern == 0){
                    commonFunctions.BindParameter(oCommand, "@thread_pattern", System.Data.DbType.Int32, DBNull.Value);
                }else{
                    commonFunctions.BindParameter(oCommand, "@thread_pattern", System.Data.DbType.Int32, inventory_items.thread_pattern);
                }
                
                commonFunctions.BindParameter(oCommand, "@stocking_unit", System.Data.DbType.String, inventory_items.stocking_unit);
                commonFunctions.BindParameter(oCommand, "@retail_unit", System.Data.DbType.String, inventory_items.retail_unit);
                commonFunctions.BindParameter(oCommand, "@rtu_over_stu", System.Data.DbType.Double, inventory_items.rtu_over_stu);
                commonFunctions.BindParameter(oCommand, "@wtd_ave_cost", System.Data.DbType.Double, inventory_items.wtd_ave_cost);
                commonFunctions.BindParameter(oCommand, "@markup_rate", System.Data.DbType.Double, inventory_items.markup_rate);
                commonFunctions.BindParameter(oCommand, "@selling_price", System.Data.DbType.Double, inventory_items.selling_price);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, inventory_items.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    inventory_items.item_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(inventory_items);

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

      //get
        public async Task<ServiceResponse> GetByDescription(string model_description)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `item_id`,
                                                `category_id`,
                                                `brand_id`,
                                                `model_description`,
                                                `part_description`,
                                                `part_number`,
                                                `size`,
                                                `valve_type`,
                                                `ratio`,
                                                `thread_pattern`,
                                                `stocking_unit`,
                                                `retail_unit`,
                                                `rtu_over_stu`,
                                                `wtd_ave_cost`,
                                                `markup_rate`,
                                                `selling_price`,
                                                `user_id`,
                                                `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`inventory_items` 
                                        WHERE model_description LIKE @model_description"; 

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@model_description", System.Data.DbType.String, "%" + model_description + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntity = new List<Inventory_InventoryItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryItemsModel();
                        hasValue = true;

                        oInventory.item_id = oresult.GetInt32("item_id");
                        oInventory.category_id = oresult.GetInt32("category_id");
                        oInventory.brand_id = oresult.GetInt32("brand_id");
                        oInventory.model_description = oresult.GetString("model_description");
                        oInventory.part_number = oresult.GetInt32("part_number");
                        oInventory.size = oresult.GetInt32("size");
                        oInventory.valve_type = oresult.GetInt32("valve_type");
                        oInventory.ratio = oresult.GetInt32("ratio");
                        oInventory.thread_pattern = oresult.GetInt32("thread_pattern");
                        oInventory.stocking_unit = oresult.GetString("stocking_unit");
                        oInventory.retail_unit = oresult.GetString("retail_unit");
                        oInventory.rtu_over_stu = oresult.GetDouble("rtu_over_stu");
                        oInventory.wtd_ave_cost = oresult.GetDouble("wtd_ave_cost");
                        oInventory.markup_rate = oresult.GetDouble("markup_rate");
                        oInventory.selling_price = oresult.GetDouble("selling_price");
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

        public async Task<ServiceResponse> GetByCategoryID(int category_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `item_id`,
                                                `category_id`,
                                                `brand_id`,
                                                `model_description`,
                                                `part_description`,
                                                `part_number`,
                                                `size`,
                                                `valve_type`,
                                                `ratio`,
                                                `thread_pattern`,
                                                `stocking_unit`,
                                                `retail_unit`,
                                                `rtu_over_stu`,
                                                `wtd_ave_cost`,
                                                `markup_rate`,
                                                `selling_price`,
                                                `user_id`,
                                                `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`inventory_items` 
                                        WHERE category_id = @category_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@category_id", System.Data.DbType.String, category_id);



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntity = new List<Inventory_InventoryItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryItemsModel();
                        hasValue = true;

                        oInventory.item_id = oresult.GetInt32("item_id");
                        oInventory.category_id = oresult.GetInt32("category_id");
                        oInventory.brand_id = oresult.GetInt32("brand_id");
                        oInventory.model_description = oresult.GetString("model_description");
                        oInventory.part_number = oresult.GetInt32("part_number");
                        oInventory.size = oresult.GetInt32("size");
                        oInventory.valve_type = oresult.GetInt32("valve_type");
                        oInventory.ratio = oresult.GetInt32("ratio");
                        oInventory.thread_pattern = oresult.GetInt32("thread_pattern");
                        oInventory.stocking_unit = oresult.GetString("stocking_unit");
                        oInventory.retail_unit = oresult.GetString("retail_unit");
                        oInventory.rtu_over_stu = oresult.GetDouble("rtu_over_stu");
                        oInventory.wtd_ave_cost = oresult.GetDouble("wtd_ave_cost");
                        oInventory.markup_rate = oresult.GetDouble("markup_rate");
                        oInventory.selling_price = oresult.GetDouble("selling_price");
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

        public async Task<ServiceResponse> GetOneInventory_InventoryItems(long item_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `item_id`,
                                                `category_id`,
                                                `brand_id`,
                                                `model_description`,
                                                COALESCE(`part_description`,0) AS part_description,
                                                COALESCE(`part_number`,0) AS part_number,
                                                COALESCE(`size`,0) AS size,
                                                COALESCE(`valve_type`,0) AS valve_type,
                                                COALESCE(`ratio`,0) AS ratio,
                                                COALESCE(`thread_pattern`,0) AS thread_pattern,
                                                `stocking_unit`,
                                                `retail_unit`,
                                                `rtu_over_stu`,
                                                `wtd_ave_cost`,
                                                `markup_rate`,
                                                `selling_price`,
                                                `user_id`,
                                                `entry_date`
	 
	                                    FROM 
	                                        `inventory`.`inventory_items`
                                        WHERE
                                            `item_id` = @item_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int64, item_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oinventory = new List<Inventory_InventoryItemsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryItemsModel();
                        hasValue = true;

                        oInventory.item_id = oresult.GetInt32("item_id");

                        oinventory.Add(oInventory);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oinventory);

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

        public async Task<ServiceResponse> UpdateInventory_InventoryItems(Inventory_InventoryItemsModel  inventory_items)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`inventory_items` 
	                                        SET
                                                `brand_id` =  @brand_id
	                                        WHERE
                                                `item_id` = @item_id;";

                oCommand.Parameters.Clear();
                commonFunctions.BindParameter(oCommand, "@brand_id", System.Data.DbType.Int32, inventory_items.brand_id);
                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int32, inventory_items.item_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(inventory_items);

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
                oCommand.CommandText = @"DELETE FROM `inventory`.`inventory_items` 
	                                       WHERE item_id = @item_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_id", System.Data.DbType.Int32, id);


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
 

