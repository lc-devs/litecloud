using inventory.Common;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Methods
{
    public class Inventory_ItemCategoriesMethods
    {
        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_ItemCategoriesMethods(AppDb db)
        {
            gDb = db;
        }

       public async Task<ServiceResponse> Save(Inventory_ItemCategoriesModel category)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`items_categories` 
	                                        (
                                                `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	                                        )
	                                     VALUES
                                            (
                                                @description, 
	                                            @user_id, 
	                                            now()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, category.description);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, category.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    category.item_category_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(category);

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
                oCommand.CommandText = @"SELECT  `item_category_id`, 
	                                            `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`items_categories` 
                                        WHERE description LIKE @description";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, "%" + description + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_ItemCategoriesModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_ItemCategoriesModel();
                        hasValue = true;

                        oEntity.item_category_id = oresult.GetInt32("item_category_id");
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

        public async Task<ServiceResponse> GetOneBrand(long id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT  `item_category_id`, 
	                                            `description`, 
	                                            `user_id`, 
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`items_categories` 
                                        WHERE item_category_id = @item_category_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_category_id", System.Data.DbType.Int32, id);



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_ItemCategoriesModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_ItemCategoriesModel();
                        hasValue = true;

                        oEntity.item_category_id = oresult.GetInt32("item_category_id");
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

        public async Task<ServiceResponse> Update(Inventory_ItemCategoriesModel category)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`items_categories` 
	                                       SET
                                                `description` = @description,
	                                            `user_id` =  @user_id, 
	                                            `entry_date` = NOW()
	                                        WHERE item_category_id = @item_category_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_category_id", System.Data.DbType.String, category.item_category_id);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, category.description);
                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, category.user_id);



                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(category);

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
                oCommand.CommandText = @"DELETE FROM `inventory`.`items_categories` 
	                                       WHERE item_category_id = @item_category_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@item_category_id", System.Data.DbType.String, id);

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

