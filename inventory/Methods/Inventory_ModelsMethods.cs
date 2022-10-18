using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_ModelsMethods
    {
        internal AppDb gDb { get; set; }

        internal string tableName = "";
        internal string idName = "";

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_ModelsMethods(AppDb db, Inventory_Common.Type type)
        {
            gDb = db;
            SetTableName(type);
        }

        private void SetTableName(Inventory_Common.Type type)
        {
            if (type == Inventory_Common.Type.Categories)
            {
                tableName = "`items_categories`";
                idName = "item_category_id";
            }
            else if (type == Inventory_Common.Type.Ratios)
            {
                tableName = "`ratios`";
                idName = "ratio_id";
            }
            else if (type == Inventory_Common.Type.Sizes)
            {
                tableName = "`sizes`";
                idName = "size_id";
            }
            else if (type == Inventory_Common.Type.ThreadPatterns)
            {
                tableName = "`thread_patterns`";
                idName = "pattern_id";
            }
            else if (type == Inventory_Common.Type.ValveTypes)
            {
                tableName = "`valve_types`";
                idName = "valve_id";
            }
            else if (type == Inventory_Common.Type.VehiclePartNumbers)
            {
                tableName = "`vehicle_part_numbers`";
                idName = "part_number_id";
            }
            else if (type == Inventory_Common.Type.VehicleParts)
            {
                tableName = "`vehicle_parts`";
                idName = "part_id";
            }
        }

        public async Task<ServiceResponse> Save(Inventory_Models model)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`." + tableName +@" 
	                                        (
                                                `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @description, 
	                                            @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, model.description);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, model.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    model.id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(model);

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
                oCommand.CommandText = @"SELECT  " + idName +@", 
	                                            `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`." + tableName + @" 
                                        WHERE description LIKE @description";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, "%" + description + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_Models>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_Models();
                        hasValue = true;

                        oEntity.id = oresult.GetInt32(idName);
                        oEntity.description = oresult.GetString("description");
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

        public async Task<ServiceResponse> GetAll()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT  " + idName + @", 
	                                            `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`." + tableName ;

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_Models>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_Models();
                        hasValue = true;

                        oEntity.id = oresult.GetInt32(idName);
                        oEntity.description = oresult.GetString("description");
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
                oCommand.CommandText = @"SELECT  " + idName + @", 
	                                            `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`." + tableName + @" 
                                        WHERE " + idName + @" = @id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id", System.Data.DbType.Int32, id);



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_Models>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_Models();
                        hasValue = true;

                        oEntity.id = oresult.GetInt32(idName);
                        oEntity.description = oresult.GetString("description");
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

        public async Task<ServiceResponse> Update(Inventory_Models model)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`." + tableName + @" 
	                                       SET
                                                `description` = @description, 
	                                            `user_id` =  @user_id, 
	                                            `entry_date` = NOW()
	                                        WHERE " + idName + @" = @id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id", System.Data.DbType.String, model.id);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, model.description);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, model.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(model);

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
                oCommand.CommandText = @"DELETE FROM `inventory`." + tableName + @" 
	                                       WHERE " + idName + @" = @id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id", System.Data.DbType.String, id);

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
