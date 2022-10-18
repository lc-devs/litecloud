
using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Inventory_BrandsMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Inventory_BrandsMethods(AppDb db)
        {
            gDb = db;
        }


  public async Task<ServiceResponse> SaveInventory_Brands(Inventory_BrandsModel brands)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `inventory`.`brands` 
	                                        (
                                                `brand_name`,
                                                `user_id`,
                                                `entry_date`

	                                        )
	                                     VALUES
                                            (
                                                @brand_name,
                                                @user_id,
                                                NOW()
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@brand_name", System.Data.DbType.String, brands.brand_name);
                 commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.Int32, brands.user_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    brands.brand_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(brands);

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



        public async Task<ServiceResponse> GetByDescription(string brand_name)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `brand_id`,
                                                `brand_name`,
                                                `user_id`,
	                                            `entry_date`
	 
	                                            FROM 
	                                            `inventory`.`brands` 
                                        WHERE brand_name LIKE @brand_name";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@brand_name", System.Data.DbType.String, "%" + brand_name + "%");



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_BrandsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_BrandsModel();
                        hasValue = true;

                      
                        oEntity.brand_id = oresult.GetInt32("brand_id");
                        oEntity.brand_name = oresult.GetString("brand_name");
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


         public async Task<ServiceResponse> GetInventory_Brands()
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            { 
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `brand_id`, 
	                                            `brand_name`
	 
	                                    FROM 
	                                        `inventory`.`brands`";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Inventory_BrandsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Inventory_BrandsModel();
                        hasValue = true;

                        oEntity.brand_id = oresult.GetInt32("brand_id");
                        oEntity.brand_name = oresult.GetString("brand_name");

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

        public async Task<ServiceResponse> GetOneInventory_Brands(long brand_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `brand_id`,
                                                `brand_name`,
                                                `user_id`,
                                                `entry_date`
	 
	                                    FROM 
	                                        `inventory`.`brands`
                                        WHERE
                                            `brand_id` = @brand_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@brand_id", System.Data.DbType.Int64, brand_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oinventory = new List<Inventory_BrandsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oInventory = new Inventory_BrandsModel();
                        hasValue = true;

                        oInventory.brand_id = oresult.GetInt32("brand_id");

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

        public async Task<ServiceResponse> UpdateInventory_Brands(Inventory_BrandsModel  brands)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `inventory`.`brands` 
	                                        SET
                                                `brand_name` =  @brand_name
	                                        WHERE
                                                `brand_id` = @brand_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@brand_name", System.Data.DbType.String, brands.brand_name);
                commonFunctions.BindParameter(oCommand, "@brand_id", System.Data.DbType.Int32, brands.brand_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(brands);

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

        public async Task<ServiceResponse> DeleteInventory_Brands(long brand_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `inventory`.`brands` 
	                                        WHERE
                                                `brand_id` = @brand_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@brand_id", System.Data.DbType.Int32, brand_id);


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