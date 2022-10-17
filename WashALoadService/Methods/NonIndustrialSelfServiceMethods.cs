using MySqlConnector;
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
    public class NonIndustrialSelfServiceMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal NonIndustrialSelfServiceMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public NonIndustrialSelfServiceMethods() { }

        public async Task<ServiceResponse> SelfServiceQueryReportByDate(string dateFrom, string dateTo, int customerID, string postedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `so_reference`, 
	                                            COALESCE(`customer_id`, 0) AS customer_id, 
	                                            COALESCE(`customer_name`, '') AS customer_name, 
	                                            COALESCE(`cellular_number`, '') AS cellular_number,
	                                            `weight_in_kg`, 
	                                            `number_of_loads`, 
	                                            `amount_due`, 
	                                            `payment_mode`, 
	                                            `entry_datetime`, 
	                                            `posted_by`,
	                                            `user_name`,
	                                            `non_cash`
	 
	                                    FROM 
	                                        `laundry`.`non_industrial_self_service` ss
	                                    INNER JOIN `laundry`.`system_users` u ON `user_id` = ss.`posted_by`    
	                                    INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = ss.`payment_mode`
                                        WHERE
                                            DATE(entry_datetime) BETWEEN DATE(@dateFrom) AND (@dateTo) AND `cancelled_entry` = 0";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `customer_id` = @customer_id";
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }

                if (postedBy.Equals(""))
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  posted_by = @posted_by";
                    commonFunctions.BindParameter(oCommand, "@posted_by", System.Data.DbType.String, postedBy);
                }
                                      
                commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObjects = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.so_reference = oresult.GetInt32("so_reference");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.cellular_number = oresult.GetString("cellular_number");
                        jsonObject.posted_by = oresult.GetString("posted_by");
                        jsonObject.user_name = oresult.GetString("user_name");
                        jsonObject.entry_datetime = oresult.GetDateTime("entry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        jsonObject.amount_due = oresult.GetDouble("amount_due");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.non_cash = oresult.GetInt32("non_cash");
                        jsonObject.number_of_loads = oresult.GetInt32("number_of_loads");
                        jsonObjects.Add(jsonObject);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(jsonObjects);

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

        public async Task<ServiceResponse> GetSelfServiceDetail(int referenceNo)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `so_reference`, 
	                                            COALESCE(`customer_id`, 0) AS customer_id, 
	                                            COALESCE(`customer_name`, '') AS customer_name, 
	                                            COALESCE(`cellular_number`, '') AS cellular_number,
	                                            `weight_in_kg`, 
	                                            `number_of_loads`, 
	                                            `amount_due`, 
	                                            `payment_mode`, 
	                                            `entry_datetime`, 
	                                            `posted_by`,
	                                            `user_name`,
                                                COALESCE(CONVERT(pr.`payment_image`, VARCHAR(1000000)),'') AS payment_image
	 
	                                    FROM 
	                                        `laundry`.`non_industrial_self_service` ss
	                                    INNER JOIN `laundry`.`system_users` u ON `user_id` = ss.`posted_by`
                                        LEFT JOIN `images`.`laundry_payment_references` pr ON pr.`entry_id` = ss.`image_entry_id`
                                        WHERE
                                            `so_reference` = @so_reference AND `cancelled_entry` = 0";

                oCommand.Parameters.Clear();

                

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, referenceNo);

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObjects = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.so_reference = oresult.GetInt32("so_reference");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.cellular_number = oresult.GetString("cellular_number");
                        jsonObject.posted_by = oresult.GetString("posted_by");
                        jsonObject.user_name = oresult.GetString("user_name");
                        jsonObject.entry_datetime = oresult.GetDateTime("entry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        jsonObject.amount_due = oresult.GetDouble("amount_due");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.payment_image = oresult.GetString("payment_image");
                        jsonObjects.Add(jsonObject);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(jsonObjects);

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

        public async Task<ServiceResponse> SaveSelfServiceAsync(NonIndustrialSelfService service)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            int series = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.SO);

            transaction = gDb.Connection.BeginTransaction();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.Transaction = transaction;

                //Insert image -----------------------------------------------------------
                oCommand.CommandText = @"INSERT INTO  `images`.`laundry_payment_references` 
                                    (
                                        `payment_image`
                                    )
                                    VALUES
                                    (
                                        @payment_image
                                    )";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_image", System.Data.DbType.String, service.payment_image);

                await oCommand.ExecuteNonQueryAsync();

                long image_id = oCommand.LastInsertedId;

                if (image_id > 0)
                {
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`non_industrial_self_service`
                                            (
                                                	`so_reference`, 
                                                    `customer_id`, 
                                                    `customer_name`, 
                                                    `cellular_number`, 
                                                    `weight_in_kg`, 
                                                    `number_of_loads`, 
                                                    `amount_due`,
                                                    `payment_mode`, 
                                                    `entry_datetime`, 
                                                    `posted_by`,
                                                    `image_entry_id`
                                            )
	 
	                                    VALUES 
	                                         (
                                                    @so_reference,
                                                    @customer_id,
                                                    @customer_name,
                                                    @cellular_number,
                                                    @weight_in_kg,
                                                    @number_of_loads,
                                                    @amount_due,
                                                    @payment_mode,
                                                    NOW(),
                                                    @posted_by,
                                                    @image_entry_id

                                             );";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, series);
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, service.customer_id);
                    commonFunctions.BindParameter(oCommand, "@customer_name", System.Data.DbType.String, service.customer_name);
                    commonFunctions.BindParameter(oCommand, "@cellular_number", System.Data.DbType.String, service.cellular_number);
                    commonFunctions.BindParameter(oCommand, "@weight_in_kg", System.Data.DbType.Double, service.weight_in_kg);
                    commonFunctions.BindParameter(oCommand, "@number_of_loads", System.Data.DbType.Int32, service.number_of_loads);
                    commonFunctions.BindParameter(oCommand, "@amount_due", System.Data.DbType.Double, service.amount_due);
                    commonFunctions.BindParameter(oCommand, "@payment_mode", System.Data.DbType.String, service.payment_mode);
                    commonFunctions.BindParameter(oCommand, "@posted_by", System.Data.DbType.String, service.posted_by);
                    commonFunctions.BindParameter(oCommand, "@image_entry_id", System.Data.DbType.Int32, image_id);

                    int j = await oCommand.ExecuteNonQueryAsync();                    



                    await transaction.CommitAsync();

                    service.so_reference = series;
                    service.so_qr_image = commonFunctions.GenerateQR(Convert.ToString(series));

                    string jsonString = JsonSerializer.Serialize(service);

                    serviceResponse.SetValues(200, "Success", jsonString);

                }
                else
                {
                    await transaction.RollbackAsync();

                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }


            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> CancelSelfServiceAsync(int SOReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.CommandText = @"UPDATE `laundry`.`non_industrial_self_service`
                                            SET
                                                	`cancelled_entry` = 1 
                                            
	 
	                                    WHERE 
	                                        `so_reference` =  @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, SOReference);

                await oCommand.ExecuteNonQueryAsync();

                serviceResponse.SetValues(200, "Success", "");

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
