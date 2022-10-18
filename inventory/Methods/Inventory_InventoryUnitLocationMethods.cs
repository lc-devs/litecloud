using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_InventoryUnitLocationMethods
    {
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_InventoryUnitLocationMethods(AppDb db)
        {
            gDb = db;
        }

         public async Task<ServiceResponse> GetInventory_InventoryUnitLocation()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `unit_location_id`, 
                                                `description`, 
	                                            `person_incharge`, 
	                                            `warehouse`, 
                                                `bldg_street_address`, 
	                                            `barangay_id`, 
	                                            `town_id`,
                                                `province_id`, 
	                                            `country_id`, 
	                                            `email_address`,
                                                `landline_nos1`, 
	                                            `landline_nos2`, 
	                                            `mobile_nos1`,
                                                `mobile_nos2`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                    FROM 
	                                        `inventory`.`units_locations`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntity = new List<Inventory_InventoryUnitLocationModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_InventoryUnitLocationModel();
                        hasValue = true;

                        oInventory.unit_location_id = oresult.GetInt32("unit_location_id");
                        oInventory.description = oresult.GetString("description");
                        oInventory.person_incharge = oresult.GetInt32("person_incharge");
                        oInventory.warehouse = oresult.GetInt32("warehouse");
                        oInventory.bldg_street_address = oresult.GetString("bldg_street_address");
                        oInventory.barangay_id = oresult.GetInt32("barangay_id");
                        oInventory.town_id = oresult.GetInt32("town_id");
                        oInventory.province_id = oresult.GetInt32("province_id");
                        oInventory.country_id = oresult.GetInt32("country_id");
                        oInventory.email_address = oresult.GetString("email_address");
                        oInventory.landline_nos1 = oresult.GetString("landline_nos1");
                        oInventory.landline_nos2 = oresult.GetString("landline_nos2");
                        oInventory.mobile_nos1 = oresult.GetString("mobile_nos1");
                        oInventory.mobile_nos2 = oresult.GetString("mobile_nos2");
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

        public async Task<ServiceResponse> Save(Inventory_InventoryUnitLocationModel unitlocation)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`units_locations` 
	                                        (
                                                `description`, 
	                                            `person_incharge`, 
	                                            `warehouse`, 
                                                `bldg_street_address`, 
	                                            `barangay_id`, 
	                                            `town_id`,
                                                `province_id`, 
	                                            `country_id`, 
	                                            `email_address`,
                                                `landline_nos1`, 
	                                            `landline_nos2`, 
	                                            `mobile_nos1`,
                                                `mobile_nos2`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @description, 
	                                            @person_incharge,
                                                @warehouse, 
	                                            @bldg_street_address, 
                                                @barangay_id, 
	                                            @town_id,
                                                @province_id, 
	                                            @country_id,
                                                @email_address, 
	                                            @landline_nos1,
                                                @landline_nos2, 
	                                            @mobile_nos1,
                                                @mobile_nos2,
	                                            @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, unitlocation.description);
                commonFunctions.BindParameter(oCommand, "@person_incharge", System.Data.DbType.Int32, unitlocation.person_incharge);
                commonFunctions.BindParameter(oCommand, "@warehouse", System.Data.DbType.Int32, unitlocation.warehouse);
                commonFunctions.BindParameter(oCommand, "@bldg_street_address", System.Data.DbType.String, unitlocation.bldg_street_address);
                commonFunctions.BindParameter(oCommand, "@barangay_id", System.Data.DbType.Int32, unitlocation.barangay_id);
                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, unitlocation.town_id);
                commonFunctions.BindParameter(oCommand, "@province_id", System.Data.DbType.Int32, unitlocation.province_id);
                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, unitlocation.country_id);
                commonFunctions.BindParameter(oCommand, "@email_address", System.Data.DbType.String, unitlocation.email_address);
                commonFunctions.BindParameter(oCommand, "@landline_nos1", System.Data.DbType.String, unitlocation.landline_nos1);
                commonFunctions.BindParameter(oCommand, "@landline_nos2", System.Data.DbType.String, unitlocation.landline_nos2);
                commonFunctions.BindParameter(oCommand, "@mobile_nos1", System.Data.DbType.String, unitlocation.mobile_nos1);
                commonFunctions.BindParameter(oCommand, "@mobile_nos2", System.Data.DbType.String, unitlocation.mobile_nos2);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, unitlocation.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    unitlocation.unit_location_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(unitlocation);

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

        public async Task<ServiceResponse> GetByDescription(string description)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `unit_location_id`, 
	                                            `description`, 
	                                            `person_incharge`, 
	                                            `warehouse`, 
	                                            `bldg_street_address`, 
                                                `barangay_id`, 
	                                            `town_id`, 
	                                            `province_id`, 
                                                `country_id`, 
	                                            `email_address`, 
	                                            `landline_nos1`, 
	                                            `landline_nos2`, 
                                                `mobile_nos1`, 
	                                            `mobile_nos2`,  
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`units_locations` 
                                        WHERE description LIKE @description";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, "%" + description + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_InventoryUnitLocationModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_InventoryUnitLocationModel();
                        hasValue = true;

                        oEntity.unit_location_id = oresult.GetInt32("unit_location_id");
                        oEntity.description = oresult.GetString("description");
                        oEntity.person_incharge = oresult.GetInt32("person_incharge");
                        oEntity.warehouse = oresult.GetInt32("warehouse");
                        oEntity.bldg_street_address = oresult.GetString("bldg_street_address");
                        oEntity.barangay_id = oresult.GetInt32("barangay_id");
                        oEntity.town_id = oresult.GetInt32("town_id");
                        oEntity.province_id = oresult.GetInt32("province_id");
                        oEntity.country_id = oresult.GetInt32("country_id");
                        oEntity.email_address = oresult.GetString("email_address");
                        oEntity.landline_nos1 = oresult.GetString("landline_nos1");
                        oEntity.landline_nos2 = oresult.GetString("landline_nos2");
                        oEntity.mobile_nos1 = oresult.GetString("mobile_nos1");
                        oEntity.mobile_nos2 = oresult.GetString("mobile_nos2");
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

        public async Task<ServiceResponse> GetOne(long id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `unit_location_id`, 
	                                            `description`, 
	                                            `person_incharge`, 
	                                            `warehouse`, 
                                                `bldg_street_address`, 
	                                            `barangay_id`, 
	                                            `town_id`, 
	                                            `province_id`, 
                                                `country_id`, 
	                                            `email_address`, 
	                                            `landline_nos1`, 
	                                            `landline_nos2`, 
                                                `mobile_nos1`, 
	                                            `mobile_nos2`,  
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`units_locations` 
                                        WHERE unit_location_id = @unit_location_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_location_id", System.Data.DbType.Int32, id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_InventoryUnitLocationModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_InventoryUnitLocationModel();
                        hasValue = true;

                        oEntity.unit_location_id = oresult.GetInt32("unit_location_id");
                        oEntity.description = oresult.GetString("description");
                        oEntity.person_incharge = oresult.GetInt32("person_incharge");
                        oEntity.warehouse = oresult.GetInt32("warehouse");
                        oEntity.bldg_street_address = oresult.GetString("bldg_street_address");
                        oEntity.barangay_id = oresult.GetInt32("barangay_id");
                        oEntity.town_id = oresult.GetInt32("town_id");
                        oEntity.province_id = oresult.GetInt32("province_id");
                        oEntity.country_id = oresult.GetInt32("country_id");
                        oEntity.email_address = oresult.GetString("email_address");
                        oEntity.landline_nos1 = oresult.GetString("landline_nos1");
                        oEntity.landline_nos2 = oresult.GetString("landline_nos2");
                        oEntity.mobile_nos1 = oresult.GetString("mobile_nos1");
                        oEntity.mobile_nos2 = oresult.GetString("mobile_nos2");
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

        public async Task<ServiceResponse> Update(Inventory_InventoryUnitLocationModel unitlocation)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`units_locations` 
	                                       SET
                                                `description` =  @description, 
	                                            `person_incharge` =  @person_incharge, 
                                                `warehouse` = @warehouse, 
	                                            `bldg_street_address` =  @bldg_street_address, 
	                                            `barangay_id` =  @barangay_id, 
                                                `town_id` = @town_id, 
	                                            `province_id` =  @province_id, 
	                                            `country_id` =  @country_id, 
                                                `email_address` = @email_address, 
	                                            `landline_nos1` =  @landline_nos1, 
	                                            `landline_nos2` =  @landline_nos2, 
                                                `mobile_nos1` = @mobile_nos1, 
	                                            `mobile_nos2` =  @mobile_nos2, 
	                                            `user_id` =  @user_id, 
	                                            `entry_date` = NOW()
	                                        WHERE unit_location_id = @unit_location_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_location_id", System.Data.DbType.Int32, unitlocation.unit_location_id);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, unitlocation.description);
                commonFunctions.BindParameter(oCommand, "@person_incharge", System.Data.DbType.Int32, unitlocation.person_incharge);
                commonFunctions.BindParameter(oCommand, "@warehouse", System.Data.DbType.Int32, unitlocation.warehouse);
                commonFunctions.BindParameter(oCommand, "@bldg_street_address", System.Data.DbType.String, unitlocation.bldg_street_address);
                commonFunctions.BindParameter(oCommand, "@barangay_id", System.Data.DbType.Int32, unitlocation.barangay_id);
                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, unitlocation.town_id);
                commonFunctions.BindParameter(oCommand, "@province_id", System.Data.DbType.Int32, unitlocation.province_id);
                commonFunctions.BindParameter(oCommand, "@country_id", System.Data.DbType.Int32, unitlocation.country_id);
                commonFunctions.BindParameter(oCommand, "@email_address", System.Data.DbType.String, unitlocation.email_address);
                commonFunctions.BindParameter(oCommand, "@landline_nos1", System.Data.DbType.String, unitlocation.landline_nos1);
                commonFunctions.BindParameter(oCommand, "@landline_nos2", System.Data.DbType.String, unitlocation.landline_nos2);
                commonFunctions.BindParameter(oCommand, "@mobile_nos1", System.Data.DbType.String, unitlocation.mobile_nos1);
                commonFunctions.BindParameter(oCommand, "@mobile_nos2", System.Data.DbType.String, unitlocation.mobile_nos2);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, unitlocation.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(unitlocation);

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
                oCommand.CommandText = @"DELETE FROM `inventory`.`units_locations` 
	                                       WHERE unit_location_id = @unit_location_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@unit_location_id", System.Data.DbType.String, id);
               
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
