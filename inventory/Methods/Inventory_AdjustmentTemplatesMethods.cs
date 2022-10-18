using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_AdjustmentTemplatesMethods
    {
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_AdjustmentTemplatesMethods(AppDb db)
        {
            gDb = db;
        }

 public async Task<ServiceResponse> GetInventory_AdjustmentTemplates()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT  `template_id`, 
	                                            `description`, 
	                                            `add_to_quantity`, 
	                                            `require_destination_and_source`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                    FROM 
	                                        `inventory`.`items_adjustments_templates`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_AdjustmentTemplatesModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_AdjustmentTemplatesModel();
                        hasValue = true;

                        oEntity.template_id = oresult.GetInt32("template_id");
                        oEntity.description = oresult.GetString("description");
                        oEntity.add_to_quantity = oresult.GetInt32("add_to_quantity");
                        oEntity.require_destination_and_source = oresult.GetInt32("require_destination_and_source");
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


        public async Task<ServiceResponse> Save(Inventory_AdjustmentTemplatesModel template)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`items_adjustments_templates` 
	                                        (
                                                `description`, 
	                                            `add_to_quantity`, 
	                                            `require_destination_and_source`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @description, 
	                                            @add_to_quantity, 
	                                            @require_destination_and_source, 
	                                            @user_id, 
	                                            NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, template.description);
                commonFunctions.BindParameter(oCommand, "@add_to_quantity", System.Data.DbType.Int32, template.add_to_quantity);
                commonFunctions.BindParameter(oCommand, "@require_destination_and_source", System.Data.DbType.Int32, template.require_destination_and_source);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, template.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    template.template_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(template);

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
                oCommand.CommandText = @"SELECT  `template_id`, 
	                                            `description`, 
	                                            `add_to_quantity`, 
	                                            `require_destination_and_source`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`items_adjustments_templates` 
                                        WHERE description LIKE @description";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, "%" + description + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_AdjustmentTemplatesModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_AdjustmentTemplatesModel();
                        hasValue = true;

                        oEntity.template_id = oresult.GetInt32("template_id");
                        oEntity.description = oresult.GetString("description");
                        oEntity.add_to_quantity = oresult.GetInt32("add_to_quantity");
                        oEntity.require_destination_and_source = oresult.GetInt32("require_destination_and_source");
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
                oCommand.CommandText = @"SELECT  `template_id`, 
	                                            `description`, 
	                                            `add_to_quantity`, 
	                                            `require_destination_and_source`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`items_adjustments_templates` 
                                        WHERE template_id = @template_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_id", System.Data.DbType.Int32, id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_AdjustmentTemplatesModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_AdjustmentTemplatesModel();
                        hasValue = true;

                        oEntity.template_id = oresult.GetInt32("template_id");
                        oEntity.description = oresult.GetString("description");
                        oEntity.add_to_quantity = oresult.GetInt32("add_to_quantity");
                        oEntity.require_destination_and_source = oresult.GetInt32("require_destination_and_source");
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

        public async Task<ServiceResponse> Update(Inventory_AdjustmentTemplatesModel template)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`items_adjustments_templates` 
	                                       SET
                                                `description` = @description, 
	                                            `add_to_quantity` =  @add_to_quantity, 
	                                            `require_destination_and_source` =  @require_destination_and_source, 
	                                            `user_id` =  @user_id, 
	                                            `entry_date` = NOW()
	                                        WHERE template_id = @template_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_id", System.Data.DbType.String, template.template_id); 
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, template.description);
                commonFunctions.BindParameter(oCommand, "@add_to_quantity", System.Data.DbType.Int32, template.add_to_quantity);
                commonFunctions.BindParameter(oCommand, "@require_destination_and_source", System.Data.DbType.Int32, template.require_destination_and_source);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, template.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(template);

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
                oCommand.CommandText = @"DELETE FROM `inventory`.`items_adjustments_templates` 
	                                       WHERE template_id = @template_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_id", System.Data.DbType.String, id);
               
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
