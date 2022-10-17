using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class CustomerMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal CustomerMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public CustomerMethods() { }

        public async Task<ServiceResponse> CustomerLoginAsync(string userid, string password)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `customer_id`, 
	                                            `customer_name`, 
	                                            `cellular_number`, 
	                                            `email_address`,
                                                `customer_password`,
                                                `active_customer`,
	                                            UUID() as `session_authentication_key`
                                      FROM  `laundry`.`customers` 
                                      WHERE `account_reset` = 0 AND `active_customer`= 1 AND (`cellular_number`= @userid OR `email_address` = @userid);";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@userid", System.Data.DbType.String, userid);

                var userDetails = new LoginDetails();

                var oresult = await oCommand.ExecuteReaderAsync();

                bool hasData = false;

                int active_customer = 1;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        hasData = true; 
                        
                        string pwd = oresult.GetString("customer_password");

                        if (BCrypt.Net.BCrypt.Verify(password, pwd, true))
                        {
                            active_customer = oresult.GetInt32("active_customer");
                            userDetails.customer_id = oresult.GetInt32("customer_id");
                            userDetails.session_authentication_key = oresult.GetString("session_authentication_key");
                            userDetails.customer_name = oresult.GetString("customer_name");
                            userDetails.cellular_number = oresult.GetString("cellular_number");
                            userDetails.email_address = oresult.GetString("email_address");
                        }

                    }
                }

              
                if (hasData == true)
                {
                    if (userDetails.customer_id > 0)
                    {
                        if(active_customer == 0)
                        {
                            serviceResponse.SetValues(401, "Account is not active. Please contact the admin.", "");
                            return serviceResponse;
                        }

                        oCommand.CommandText = @"UPDATE `laundry`.`customers`  
                                             SET 
                                                `session_authentication_key` = @authkey, 
                                                `session_datetime`= NOW()
                                            WHERE `customer_id` = @customer_id;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, userDetails.customer_id);
                        commonFunctions.BindParameter(oCommand, "@authkey", System.Data.DbType.String, userDetails.session_authentication_key);

                        await oCommand.ExecuteNonQueryAsync();

                        string jsonString = JsonSerializer.Serialize(userDetails);

                        serviceResponse.SetValues(200, "Success", jsonString);

                    }
                    else
                    {
                        serviceResponse.SetValues(401, "User and password mismatched.", "");
                    }
                }
                else
                {
                    serviceResponse.SetValues(401, "User could not be indentified or user and password mismatched.", "");
                }
            }
            catch(Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }            

            return serviceResponse;
        }

        public async Task<ServiceResponse> VerifyCustomerKeyAsync(string authKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `customer_id`, 
	                                            `customer_name`, 
	                                            `cellular_number`, 
	                                            `email_address`,
                                                `customer_password`,
	                                            `session_authentication_key`,
                                                TIMESTAMPDIFF(MINUTE, COALESCE(`session_datetime`, NOW()), NOW()) AS session_duration
                                      FROM  `laundry`.`customers` 
                                      WHERE `session_authentication_key`= @session_authentication_key;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@session_authentication_key", System.Data.DbType.String, authKey);

                var userDetails = new LoginDetails();

                var oresult = await oCommand.ExecuteReaderAsync();

                bool hasData = false;

                int session_duration = 0;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        hasData = true;

                        session_duration = oresult.GetInt32("session_duration");

                        userDetails.customer_id = oresult.GetInt32("customer_id");
                        userDetails.session_authentication_key = oresult.GetString("session_authentication_key");
                        userDetails.customer_name = oresult.GetString("customer_name");
                        userDetails.cellular_number = oresult.GetString("cellular_number");
                        userDetails.email_address = oresult.GetString("email_address");

                    }
                }

                if (session_duration > 60)
                {
                    serviceResponse.SetValues(401, "Your session expired.", "");
                    return serviceResponse;
                }

                if (hasData == true)
                {
                    if (userDetails.customer_id > 0)
                    {
                        oCommand.CommandText = @"UPDATE `laundry`.`customers`  
                                             SET 
                                                `session_datetime`= NOW()
                                            WHERE `customer_id` = @customer_id;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, userDetails.customer_id);
                        commonFunctions.BindParameter(oCommand, "@authkey", System.Data.DbType.String, userDetails.session_authentication_key);

                        await oCommand.ExecuteNonQueryAsync();

                        string jsonString = JsonSerializer.Serialize(userDetails);

                        serviceResponse.SetValues(200, "Success", jsonString);

                    }
                    else
                    {
                        serviceResponse.SetValues(401, "User and password mismatched.", "");
                    }
                }
                else
                {
                    serviceResponse.SetValues(401, "User could not be indentified or user and password mismatched.", "");
                }
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> FindCustomerByValueAsync(string value)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `customer_id`, 
	                                            `source_id`,
                                                `customer_name`,
	                                            `cellular_number`, 
	                                            `email_address`, 
	                                            `industrial`, 
	                                            `non_industrial`, 
	                                            `active_customer`, 
	                                            `account_reset`
	 
	                                    FROM 
	                                            `laundry`.`customers`
	                                    WHERE 
	                                            
	                                            customer_name LIKE @value;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@value", System.Data.DbType.String, "%" + value + "%");
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Customer>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Customer();
                        hasValue = true;

                        oEntity.customer_id = oresult.GetInt32("customer_id");
                        oEntity.source_id = oresult.GetString("source_id");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.cellular_number = oresult.GetString("cellular_number");
                        oEntity.email_address = oresult.GetString("email_address");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.non_industrial = oresult.GetInt32("non_industrial");
                        oEntity.active_customer = oresult.GetInt32("active_customer");
                        oEntity.account_reset = oresult.GetInt32("account_reset");

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

        public async Task<ServiceResponse> FindCustomerByEmailOrContactNoAsync(string value)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `customer_id`, 
	                                            `source_id`,
                                                `customer_name`, 
	                                            `cellular_number`, 
	                                            `email_address`, 
	                                            `industrial`, 
	                                            `non_industrial`, 
	                                            `active_customer`, 
	                                            `account_reset`,
                                                COALESCE((SELECT `wkg_per_load` FROM `laundry`.`system_parameters` LIMIT 1), 1) as weight_per_load
	 
	                                    FROM 
	                                            `laundry`.`customers`
	                                    WHERE 
	                                            (cellular_number LIKE @value OR
	                                            email_address LIKE @value);";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@value", System.Data.DbType.String, "%" + value + "%");
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Customer>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Customer();
                        hasValue = true;

                        oEntity.customer_id = oresult.GetInt32("customer_id");
                        oEntity.source_id = oresult.GetString("source_id");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.cellular_number = oresult.GetString("cellular_number");
                        oEntity.email_address = oresult.GetString("email_address");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.non_industrial = oresult.GetInt32("non_industrial");
                        oEntity.active_customer = oresult.GetInt32("active_customer");
                        oEntity.account_reset = oresult.GetInt32("account_reset");
                        oEntity.weight_per_load = oresult.GetInt32("weight_per_load");

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

        public async Task<ServiceResponse> FindCustomerDetailsAsync(int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `customer_id`, 
	                                            `source_id`, 
	                                            `customer_name`, 
	                                            `cellular_number`, 
	                                            `email_address`, 
	                                            `industrial`, 
	                                            `non_industrial`, 
	                                            `active_customer`, 
	                                            `account_reset`, 
	                                            `street_building_address`, 
	                                            `barangay_address`, 
	                                            `town_address`, 
	                                            `province`, 
	                                            COALESCE(`longitude`, 0.0) as longitude, 
	                                            COALESCE(`latitude`, 0.0) as latitude, 
	                                            `session_authentication_key`, 
	                                            `session_datetime`, 
	                                            `connection_id`, 
	                                            `connection_active`,
                                                `average_daily_load`
	 
	                                    FROM 
	                                            `laundry`.`customers`
	                                    WHERE 
	                                            customer_id = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Customer>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Customer();
                        hasValue = true;

                        oEntity.customer_id = oresult.GetInt32("customer_id");
                        oEntity.source_id = oresult.GetString("source_id");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.cellular_number = oresult.GetString("cellular_number");
                        oEntity.email_address = oresult.GetString("email_address");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.non_industrial = oresult.GetInt32("non_industrial");
                        oEntity.active_customer = oresult.GetInt32("active_customer");
                        oEntity.account_reset = oresult.GetInt32("account_reset");
                        oEntity.street_building_address = oresult.GetString("street_building_address");
                        oEntity.barangay_address = oresult.GetString("barangay_address");
                        oEntity.town_address = oresult.GetString("town_address");
                        oEntity.province = oresult.GetString("province");
                        oEntity.longitude = oresult.GetFloat("longitude");
                        oEntity.latitude = oresult.GetFloat("latitude");
                        oEntity.average_daily_load = oresult.GetDouble("average_daily_load");                       

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

        public async Task<ServiceResponse> SaveUserAsync(Customer customer)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`customers`
                                            (
                                                	`source_id`, 
	                                                `customer_name`, 
	                                                `cellular_number`, 
	                                                `email_address`, 
	                                                `industrial`, 
	                                                `non_industrial`, 
	                                                `active_customer`, 
	                                                `account_reset`, 
	                                                `street_building_address`, 
	                                                `barangay_address`, 
	                                                `town_address`, 
	                                                `province`, 
	                                                `customer_password`,
                                                    `average_daily_load`,
                                                    `session_authentication_key`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @source_id,
                                                @customer_name,
                                                @cellular_number,
                                                @email_address,
                                                @industrial,
                                                @non_industrial,
                                                @active_customer,
                                                @account_reset,
                                                @street_building_address,
                                                @barangay_address,
                                                @town_address,
                                                @province,
                                                @customer_password,
                                                @average_daily_load,
                                                @session_authentication_key
                                             );";

                oCommand.Parameters.Clear();

                Guid authkey = Guid.NewGuid();

                commonFunctions.BindParameter(oCommand, "@source_id", System.Data.DbType.String, customer.source_id);
                commonFunctions.BindParameter(oCommand, "@customer_name", System.Data.DbType.String, customer.customer_name);
                commonFunctions.BindParameter(oCommand, "@cellular_number", System.Data.DbType.String, customer.cellular_number);
                commonFunctions.BindParameter(oCommand, "@email_address", System.Data.DbType.String, customer.email_address);
                commonFunctions.BindParameter(oCommand, "@industrial", System.Data.DbType.Int32, customer.industrial);
                commonFunctions.BindParameter(oCommand, "@non_industrial", System.Data.DbType.Int32, customer.non_industrial);
                commonFunctions.BindParameter(oCommand, "@active_customer", System.Data.DbType.Int32, 0);
                commonFunctions.BindParameter(oCommand, "@account_reset", System.Data.DbType.Int32, 0);
                commonFunctions.BindParameter(oCommand, "@street_building_address", System.Data.DbType.String, customer.street_building_address);
                commonFunctions.BindParameter(oCommand, "@barangay_address", System.Data.DbType.String, customer.barangay_address);
                commonFunctions.BindParameter(oCommand, "@town_address", System.Data.DbType.String, customer.town_address);
                commonFunctions.BindParameter(oCommand, "@province", System.Data.DbType.String, customer.province);
                commonFunctions.BindParameter(oCommand, "@customer_password", System.Data.DbType.String, "");
                commonFunctions.BindParameter(oCommand, "@average_daily_load", System.Data.DbType.Double, customer.average_daily_load);
                commonFunctions.BindParameter(oCommand, "@session_authentication_key", System.Data.DbType.String, authkey.ToString());

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    customer.customer_id = oCommand.LastInsertedId;
                    customer.authkey = authkey.ToString();

                    dynamic jsonObject = new ExpandoObject();

                    jsonObject.customer = customer;
                    jsonObject.ToEmail = customer.email_address;
                    jsonObject.Subject = "WASH A LOAD ACTIVATION ACCOUNT";
                    jsonObject.Body = GenerateEmailBodyForActivation(customer.customer_id, customer.customer_name, authkey.ToString());

                    string jsonString = JsonSerializer.Serialize(jsonObject);

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

        public async Task<ServiceResponse> UpdateUserAsync(Customer customer)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`customers`
                                            SET
                                                	`customer_name` = @customer_name, 
	                                                `cellular_number` = @cellular_number, 
	                                                `email_address` = @email_address, 
	                                                `industrial` = @industrial, 
	                                                `non_industrial` = @non_industrial, 	                                               
	                                                `street_building_address` = @street_building_address, 
	                                                `barangay_address` = @barangay_address, 
	                                                `town_address` = @town_address, 
	                                                `province` = @province,
                                                    `average_daily_load` = @average_daily_load
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customer.customer_id);
                commonFunctions.BindParameter(oCommand, "@customer_name", System.Data.DbType.String, customer.customer_name);
                commonFunctions.BindParameter(oCommand, "@cellular_number", System.Data.DbType.String, customer.cellular_number);
                commonFunctions.BindParameter(oCommand, "@email_address", System.Data.DbType.String, customer.email_address);
                commonFunctions.BindParameter(oCommand, "@industrial", System.Data.DbType.Int32, customer.industrial);
                commonFunctions.BindParameter(oCommand, "@non_industrial", System.Data.DbType.Int32, customer.non_industrial);                
                commonFunctions.BindParameter(oCommand, "@street_building_address", System.Data.DbType.String, customer.street_building_address);
                commonFunctions.BindParameter(oCommand, "@barangay_address", System.Data.DbType.String, customer.barangay_address);
                commonFunctions.BindParameter(oCommand, "@town_address", System.Data.DbType.String, customer.town_address);
                commonFunctions.BindParameter(oCommand, "@province", System.Data.DbType.String, customer.province);
                commonFunctions.BindParameter(oCommand, "@average_daily_load", System.Data.DbType.Double, customer.average_daily_load);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(customer);

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

        public async Task<ServiceResponse> ActivateDeactivateUserAsync(int customerID, int isActivate)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`customers`
                                            SET
                                                	`active_customer` = @active_customer 
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                commonFunctions.BindParameter(oCommand, "@active_customer", System.Data.DbType.Int32, isActivate);
             

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

        public async Task<ServiceResponse> OnlineUserActivationAsync(int customerID, double lat, double lng)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`customers`
                                            SET
                                                	`active_customer` = 1,
                                                    `latitude` = @latitude,
                                                    `longitude` = @longitude
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                commonFunctions.BindParameter(oCommand, "@latitude", System.Data.DbType.Double, lat);
                commonFunctions.BindParameter(oCommand, "@longitude", System.Data.DbType.Double, lng);


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

        public async Task<ServiceResponse> ResetUserAsync(Customer customer)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                Guid authkey = Guid.NewGuid();

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`customers`
                                            SET
                                                	`account_reset` = 1,
                                                    `session_authentication_key` = @session_authentication_key,
                                                    `session_datetime` = NULL
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customer.customer_id);
                commonFunctions.BindParameter(oCommand, "@session_authentication_key", System.Data.DbType.String, authkey.ToString());


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    customer.authkey = authkey.ToString();
                    serviceResponse.SetValues(200, "Success", GenerateEmailBodyFor2FA(customer.customer_id, customer.customer_name, customer.authkey));
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

        public async Task<ServiceResponse> ChangeUserPasswordAsync(int customerID, string password)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`customers`
                                            SET
                                                	`customer_password` = @customer_password,
                                                    `account_reset`= 0
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);

                if (password.Equals(""))
                {
                    password = Guid.NewGuid().ToString();
                }

                commonFunctions.BindParameter(oCommand, "@customer_password", System.Data.DbType.String, BCrypt.Net.BCrypt.EnhancedHashPassword(password));


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

        public async Task<ServiceResponse> DeleteUserAsync(int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"DELETE FROM `laundry`.`designated_laundry_items`
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);


                await oCommand.ExecuteNonQueryAsync();

                oCommand.CommandText = @"DELETE FROM `laundry`.`customers`
                                            WHERE
	                                            `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);


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
                serviceResponse.SetValues(500, "Customer could not be deleted due to existing transaction record.", "");
            }

            return serviceResponse;
        }

        private string GenerateEmailBodyForActivation(long custID, string custName, string authKey)
        {
            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["CustomerURL"];

            string html = @"<html xmlns='https://www.w3.org/1999/xhtml'><head>
                            <link href='https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800' rel='stylesheet'>
                            <style type='text/css'>
                            body {
                            margin: 0 !important;
                            padding: 0 !important;
                            -webkit-text-size-adjust: 100% !important;
                            -ms-text-size-adjust: 100% !important;
                            -webkit-font-smoothing: antialiased !important;
                            }
                            img {
                            border: 0 !important;
                            outline: none !important;
                            }
                            p {
                            Margin: 0px !important;
                            Padding: 0px !important;
                            }
                            table {
                            border-collapse: collapse;
                            mso-table-lspace: 0px;
                            mso-table-rspace: 0px;
                            }
                            td, a, span {
                            border-collapse: collapse;
                            mso-line-height-rule: exactly;
                            }
                            .ExternalClass * {
                            line-height: 100%;
                            }
                            .em_defaultlink a {
                            color: inherit !important;
                            text-decoration: none !important;
                            }
                            span.MsoHyperlink {
                            mso-style-priority: 99;
                            color: inherit;
                            }
                            span.MsoHyperlinkFollowed {
                            mso-style-priority: 99;
                            color: inherit;
                            }
                            @media only screen and (min-width:481px) and (max-width:699px) {
                            .em_main_table {
                            width: 100% !important;
                            }
                            .em_wrapper {
                            width: 100% !important;
                            }
                            .em_hide {
                            display: none !important;
                            }
                            .em_img {
                            width: 100% !important;
                            height: auto !important;
                            }
                            .em_h20 {
                            height: 20px !important;
                            }
                            .em_padd {
                            padding: 20px 10px !important;
                            }
                            }
                            @media screen and (max-width: 480px) {
                            .em_main_table {
                            width: 100% !important;
                            }
                            .em_wrapper {
                            width: 100% !important;
                            }
                            .em_hide {
                            display: none !important;
                            }
                            .em_img {
                            width: 100% !important;
                            height: auto !important;
                            }
                            .em_h20 {
                            height: 20px !important;
                            }
                            .em_padd {
                            padding: 20px 10px !important;
                            }
                            .em_text1 {
                            font-size: 16px !important;
                            line-height: 24px !important;
                            }
                            u + .em_body .em_full_wrap {
                            width: 100% !important;
                            width: 100vw !important;
                            }
                            }
                            </style>
                            </head>

                            <body class='em_body' style='margin:0px; padding:0px;' bgcolor='#efefef'>
                            <table class='em_full_wrap' valign='top' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#efefef' align='center'>
                            <tbody><tr>
                            <td valign='top' align='center'><table class='em_main_table' style='width:700px;' width='700' cellspacing='0' cellpadding='0' border='0' align='center'>

                            <!—Banner section—>
                            <tr>
                            <td valign='top' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody style='background-color: #f6f7f8;'><tr>
                            <td valign='top' align='center'><br/><br/><img class='em_img' alt='Wash a Load Laundry' style='display:block; font-family:Arial, sans-serif; font-size:30px; line-height:34px; color:##3498eb; max-width:400px;' src='" + appURL + @"images/icon/logo.png' width='700' border='0' height='100'></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <!—//Banner section–>
                            <!—Content Text Section—>
                            <tr>
                            <td style='padding:35px 70px 30px;' class='em_padd' valign='top' bgcolor='#f6f7f8' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:16px; line-height:30px; color:#696666;' valign='top' align='left'>Hello " + custName + @"!</td>
                            </tr>
                            <tr>
                            <td style='font-size:0px; line-height:0px; height:15px;' height='15'>&nbsp;</td>
                            <!——this is space of 15px to separate two paragraphs ——>
                            </tr>
                            <tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:18px; line-height:22px; color:#696666; letter-spacing:2px; padding-bottom:12px;' valign='top' align='center'>Welcome to Wash A Load online booking! <br/><br/> Please proceed <a href='"+ appURL +  @"online-customer-side-account-activation.html?id=" + custID.ToString() + "&key=" + authKey + @"'>HERE</a> to complete the activation</td>
                            </tr>
                            <tr>
                            <td class='em_h20' style='font-size:0px; line-height:0px; height:25px;' height='25'>&nbsp;</td>
                            <!——this is space of 25px to separate two paragraphs ——>
                            </tr>
                            </tbody></table></td>
                            </tr>

                            <!—//Content Text Section–>
                            <!—Footer Section—>
                            <tr>
                            <td style='padding:38px 30px;' class='em_padd' valign='top' bgcolor='#f6f7f8' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            <td style='padding-bottom:16px;' valign='top' align='center'><table cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:11px; line-height:18px; color:#999999;' valign='top' align='left'>From <br/> Wash A Load Team <br/><br/></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <tr>
                            <td class='em_hide' style='line-height:1px;min-width:700px;background-color:#ffffff;'><img alt='' src='' style='max-height:1px; min-height:1px; display:block; width:700px; min-width:700px;' width='700' border='0' height='1'></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            </tbody></table>
                            <div class='em_hide' style='white-space: nowrap; display: none; font-size:0px; line-height:0px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div>
                            </body></html>";


            return html;
        }

        private string GenerateEmailBodyFor2FA(long custID, string custName, string authKey)
        {
            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["CustomerURL"];

            string html = @"<html xmlns='https://www.w3.org/1999/xhtml'><head>
                            <link href='https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800' rel='stylesheet'>
                            <style type='text/css'>
                            body {
                            margin: 0 !important;
                            padding: 0 !important;
                            -webkit-text-size-adjust: 100% !important;
                            -ms-text-size-adjust: 100% !important;
                            -webkit-font-smoothing: antialiased !important;
                            }
                            img {
                            border: 0 !important;
                            outline: none !important;
                            }
                            p {
                            Margin: 0px !important;
                            Padding: 0px !important;
                            }
                            table {
                            border-collapse: collapse;
                            mso-table-lspace: 0px;
                            mso-table-rspace: 0px;
                            }
                            td, a, span {
                            border-collapse: collapse;
                            mso-line-height-rule: exactly;
                            }
                            .ExternalClass * {
                            line-height: 100%;
                            }
                            .em_defaultlink a {
                            color: inherit !important;
                            text-decoration: none !important;
                            }
                            span.MsoHyperlink {
                            mso-style-priority: 99;
                            color: inherit;
                            }
                            span.MsoHyperlinkFollowed {
                            mso-style-priority: 99;
                            color: inherit;
                            }
                            @media only screen and (min-width:481px) and (max-width:699px) {
                            .em_main_table {
                            width: 100% !important;
                            }
                            .em_wrapper {
                            width: 100% !important;
                            }
                            .em_hide {
                            display: none !important;
                            }
                            .em_img {
                            width: 100% !important;
                            height: auto !important;
                            }
                            .em_h20 {
                            height: 20px !important;
                            }
                            .em_padd {
                            padding: 20px 10px !important;
                            }
                            }
                            @media screen and (max-width: 480px) {
                            .em_main_table {
                            width: 100% !important;
                            }
                            .em_wrapper {
                            width: 100% !important;
                            }
                            .em_hide {
                            display: none !important;
                            }
                            .em_img {
                            width: 100% !important;
                            height: auto !important;
                            }
                            .em_h20 {
                            height: 20px !important;
                            }
                            .em_padd {
                            padding: 20px 10px !important;
                            }
                            .em_text1 {
                            font-size: 16px !important;
                            line-height: 24px !important;
                            }
                            u + .em_body .em_full_wrap {
                            width: 100% !important;
                            width: 100vw !important;
                            }
                            }
                            </style>
                            </head>

                            <body class='em_body' style='margin:0px; padding:0px;' bgcolor='#efefef'>
                            <table class='em_full_wrap' valign='top' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#efefef' align='center'>
                            <tbody><tr>
                            <td valign='top' align='center'><table class='em_main_table' style='width:700px;' width='700' cellspacing='0' cellpadding='0' border='0' align='center'>

                            <!—Banner section—>
                            <tr>
                            <td valign='top' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody style='background-color: #f6f7f8;'><tr>
                            <td valign='top' align='center'><br/><br/><img class='em_img' alt='Wash a Load Laundry' style='display:block; font-family:Arial, sans-serif; font-size:30px; line-height:34px; color:##3498eb; max-width:400px;' src='" + appURL + @"images/icon/logo.png' width='700' border='0' height='100'></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <!—//Banner section–>
                            <!—Content Text Section—>
                            <tr>
                            <td style='padding:35px 70px 30px;' class='em_padd' valign='top' bgcolor='#f6f7f8' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:16px; line-height:30px; color:#696666;' valign='top' align='left'>Hello " + custName + @"!</td>
                            </tr>
                            <tr>
                            <td style='font-size:0px; line-height:0px; height:15px;' height='15'>&nbsp;</td>
                            <!——this is space of 15px to separate two paragraphs ——>
                            </tr>
                            <tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:18px; line-height:22px; color:#696666; letter-spacing:2px; padding-bottom:12px;' valign='top' align='center'>Your account is being due to your request! <br/><br/> Please proceed <a href='" + appURL + @"online-customer-side-account-activation.html?id=" + custID.ToString() + "&key=" + authKey + @"'>HERE</a> to complete account reset.</td>
                            </tr>
                            <tr>
                            <td class='em_h20' style='font-size:0px; line-height:0px; height:25px;' height='25'>&nbsp;</td>
                            <!——this is space of 25px to separate two paragraphs ——>
                            </tr>
                            </tbody></table></td>
                            </tr>

                            <!—//Content Text Section–>
                            <!—Footer Section—>
                            <tr>
                            <td style='padding:38px 30px;' class='em_padd' valign='top' bgcolor='#f6f7f8' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            <td style='padding-bottom:16px;' valign='top' align='center'><table cellspacing='0' cellpadding='0' border='0' align='center'>
                            <tbody><tr>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <tr>
                            <td style='font-family:’Open Sans’, Arial, sans-serif; font-size:11px; line-height:18px; color:#999999;' valign='top' align='left'>From <br/> Wash A Load Team <br/><br/></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            <tr>
                            <td class='em_hide' style='line-height:1px;min-width:700px;background-color:#ffffff;'><img alt='' src='' style='max-height:1px; min-height:1px; display:block; width:700px; min-width:700px;' width='700' border='0' height='1'></td>
                            </tr>
                            </tbody></table></td>
                            </tr>
                            </tbody></table>
                            <div class='em_hide' style='white-space: nowrap; display: none; font-size:0px; line-height:0px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div>
                            </body></html>";


            return html;
        }


    }
}
