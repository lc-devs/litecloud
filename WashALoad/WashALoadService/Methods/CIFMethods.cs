using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class CIFMethods
    {
        internal AppDb_SourceInfo gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal CIFMethods(AppDb_SourceInfo db)
        {
            gDb = db;
        }
        public CIFMethods() { }

        public async Task<ServiceResponse> FindCustomerByContactAsync(string contactNo)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `cust_id`, 
	                                            `cust_name`, 
	                                            `email`, 
	                                             REPLACE(`mobile_number`, '-','') as `mobile_number`, 
	                                            `address_street`, 
	                                            `address_city`, 
	                                            `province`,
                                                `zip`
	 
	                                    FROM 
	                                            `clients_source`
	                                    WHERE 
	                                            REPLACE(`mobile_number`, '-','') = @mobile AND LENGTH(`mobile_number`) > 10;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@mobile", System.Data.DbType.String, contactNo);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<CIF>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new CIF();
                        hasValue = true;

                        oEntity.cust_id = oresult.GetString("cust_id");
                        oEntity.cust_name = oresult.GetString("cust_name");
                        oEntity.email = oresult.GetString("email");
                        oEntity.mobile_number = oresult.GetString("mobile_number");
                        oEntity.address_street = oresult.GetString("address_street");
                        oEntity.address_city = oresult.GetString("address_city");
                        oEntity.province = oresult.GetString("province");
                        oEntity.zip = oresult.GetString("zip");

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

        public async Task<ServiceResponse> FindCustomerByEmailAsync(string email)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `cust_id`, 
	                                            `cust_name`, 
	                                            `email`, 
	                                             REPLACE(`mobile_number`, '-','') as `mobile_number`, 
	                                            `address_street`, 
	                                            `address_city`, 
	                                            `province`,
                                                `zip`	                                           
	 
	                                    FROM 
	                                            `clients_source`
	                                    WHERE 
	                                            `email` = @email 
                                                -- AND `email` REGEXP '^[a-zA-Z0-9][a-zA-Z0-9._-]*[a-zA-Z0-9._-]@[a-zA-Z0-9][a-zA-Z0-9._-]*[a-zA-Z0-9]\\.[a-zA-Z]{2,63}$';";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@email", System.Data.DbType.String, email);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<CIF>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new CIF();
                        hasValue = true;

                        oEntity.cust_id = oresult.GetString("cust_id");
                        oEntity.cust_name = oresult.GetString("cust_name");
                        oEntity.email = oresult.GetString("email");
                        oEntity.mobile_number = oresult.GetString("mobile_number");
                        oEntity.address_street = oresult.GetString("address_street");
                        oEntity.address_city = oresult.GetString("address_city");
                        oEntity.province = oresult.GetString("province");
                        oEntity.zip = oresult.GetString("zip");

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

        public async Task<ServiceResponse> FindCustomerByNameAsync(string name)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `cust_id`, 
	                                            `cust_name`, 
	                                            `email`, 
	                                             REPLACE(`mobile_number`, '-','') as `mobile_number`, 
	                                            `address_street`, 
	                                            `address_city`, 
	                                            `province`,
                                                `zip`
	 
	                                    FROM 
	                                            `clients_source`
	                                    WHERE 
	                                           cust_name LIKE @name;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@name", System.Data.DbType.String, "%" + name + "%");
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<CIF>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new CIF();
                        hasValue = true;

                        oEntity.cust_id = oresult.GetString("cust_id");
                        oEntity.cust_name = oresult.GetString("cust_name");
                        oEntity.email = oresult.GetString("email");
                        oEntity.mobile_number = oresult.GetString("mobile_number");
                        oEntity.address_street = oresult.GetString("address_street");
                        oEntity.address_city = oresult.GetString("address_city");
                        oEntity.province = oresult.GetString("province");
                        oEntity.zip = oresult.GetString("zip");

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
