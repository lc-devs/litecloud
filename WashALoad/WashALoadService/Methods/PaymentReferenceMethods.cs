using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;
using static WashALoadService.Common.Common;

namespace WashALoadService.Methods
{
    public class PaymentReferenceMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal PaymentReferenceMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public PaymentReferenceMethods() { }

        public async Task<ServiceResponse> GetPaymentReferenceImage(int paymentReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT pr.`payment_reference`, 
				                                COALESCE(CONVERT(lpr.`payment_image`, VARCHAR(1000000)),'') AS payment_image
	 
	                                    FROM 
	                                    `laundry`.`payment_references` 	 pr
	                                    INNER JOIN `images`.`laundry_payment_references` lpr ON lpr.`entry_id` = pr.`image_entry_id`

                                        WHERE
                                            `payment_reference` = @payment_reference";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference);

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObjects = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.payment_reference = oresult.GetInt32("payment_reference");                        
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
        public async Task<ServiceResponse> QueryReportByBillingReference(string dateFrom, string dateTo, int customerID, string postedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT pr.`payment_reference`, 
	                                            `payment_date`, 
	                                            pr.`customer_id`,  
                                                `customer_name`,
	                                            CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash, 
	                                            CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash,
	                                            `payment_mode`, 
	                                            `float_payment`, 
	                                            `posted_by`, 
	                                            `posting_datetime`,
	                                            `billing_reference`						
	 
	                                    FROM 
	                                    `laundry`.`payment_references` 	 pr
	                                    INNER JOIN `laundry`.`billing_references` br ON br.`payment_reference` = pr.`payment_reference`
	                                    INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                        INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                        WHERE
                                            `float_payment` = 0 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)";

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

                int ttlRecords = 0;
                double ttlPaidCash = 0;
                double ttlPaidNonCash = 0;


                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.payment_reference = oresult.GetInt32("payment_reference");
                        jsonObject.payment_date = oresult.GetDateTime("payment_date").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.paid_cash = oresult.GetDouble("paid_cash").ToString("#,##0.00");
                        jsonObject.paid_noncash = oresult.GetDouble("paid_noncash").ToString("#,##0.00");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.float_payment = oresult.GetInt32("float_payment");
                        jsonObject.posted_by = oresult.GetString("posted_by");
                        jsonObject.posting_datetime = oresult.GetDateTime("posting_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.billing_reference = oresult.GetInt32("billing_reference");
                        jsonObjects.Add(jsonObject);
                        ttlRecords = ttlRecords + 1;
                        ttlPaidCash = ttlPaidCash + oresult.GetDouble("paid_cash");
                        ttlPaidNonCash = ttlPaidNonCash + oresult.GetDouble("paid_noncash");

                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    dynamic jsonTotal = new ExpandoObject();
                    jsonTotal.ttlRecords = ttlRecords;
                    jsonTotal.ttlPaidCash = ttlPaidCash.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCash = ttlPaidNonCash.ToString("#,##0.00");

                    dynamic jsonDetails = new ExpandoObject();
                    jsonDetails.summary = jsonTotal;
                    jsonDetails.records = jsonObjects;

                    string jsonString = JsonSerializer.Serialize(jsonDetails);

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
        public async Task<ServiceResponse> QueryReportByInvoice(string dateFrom, string dateTo, int customerID, string postedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT pr.`payment_reference`, 
	                                            `payment_date`, 
	                                            pr.`customer_id`,  
                                                `customer_name`,
	                                            CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash, 
	                                            CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash,
	                                            `payment_mode`, 
	                                            `float_payment`, 
	                                            `posted_by`, 
	                                            `posting_datetime`,
	                                            `invoice_reference`
						
	 
	                                    FROM 
	                                    `laundry`.`payment_references` 	 pr
	                                    INNER JOIN `laundry`.`invoices` i ON i.`payment_reference` = pr.`payment_reference`
	                                    INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                        INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                        WHERE
                                            `float_payment` = 0 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)";

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

                int ttlRecords = 0;
                double ttlPaidCash = 0;
                double ttlPaidNonCash = 0;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.payment_reference = oresult.GetInt32("payment_reference");
                        jsonObject.payment_date = oresult.GetDateTime("payment_date").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.paid_cash = oresult.GetDouble("paid_cash").ToString("#,##0.00");
                        jsonObject.paid_noncash = oresult.GetDouble("paid_noncash").ToString("#,##0.00");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.float_payment = oresult.GetInt32("float_payment");
                        jsonObject.posted_by = oresult.GetString("posted_by");
                        jsonObject.posting_datetime = oresult.GetDateTime("posting_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.invoice_reference = oresult.GetInt32("invoice_reference");
                        jsonObjects.Add(jsonObject);

                        ttlRecords = ttlRecords + 1;
                        ttlPaidCash = ttlPaidCash + oresult.GetDouble("paid_cash");
                        ttlPaidNonCash = ttlPaidNonCash + oresult.GetDouble("paid_noncash");
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    dynamic jsonTotal = new ExpandoObject();
                    jsonTotal.ttlRecords = ttlRecords;
                    jsonTotal.ttlPaidCash = ttlPaidCash.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCash = ttlPaidNonCash.ToString("#,##0.00");

                    dynamic jsonDetails = new ExpandoObject();
                    jsonDetails.summary = jsonTotal;
                    jsonDetails.records = jsonObjects;

                    string jsonString = JsonSerializer.Serialize(jsonDetails);

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

        public async Task<ServiceResponse> QueryReportByFloats(string dateFrom, string dateTo, int customerID, string postedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT pr.`payment_reference`, 
	                                            `payment_date`, 
	                                            pr.`customer_id`,  
                                                `customer_name`,
	                                            CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash, 
	                                            CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash,
	                                            `payment_mode`, 
	                                            `float_payment`, 
	                                            `posted_by`, 
	                                            `posting_datetime`						
	 
	                                    FROM 
	                                    `laundry`.`payment_references` 	 pr
	                                    INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                        INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                        WHERE
                                            `float_payment` = 1 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)";

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

                int ttlRecords = 0;
                double ttlPaidCash = 0;
                double ttlPaidNonCash = 0;

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;
                        jsonObject.payment_reference = oresult.GetInt32("payment_reference");
                        jsonObject.payment_date = oresult.GetDateTime("payment_date").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.paid_cash = oresult.GetDouble("paid_cash").ToString("#,##0.00");
                        jsonObject.paid_noncash = oresult.GetDouble("paid_noncash").ToString("#,##0.00");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.float_payment = oresult.GetInt32("float_payment");
                        jsonObject.posted_by = oresult.GetString("posted_by");
                        jsonObject.posting_datetime = oresult.GetDateTime("posting_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObjects.Add(jsonObject);

                        ttlRecords = ttlRecords + 1;
                        ttlPaidCash = ttlPaidCash + oresult.GetDouble("paid_cash");
                        ttlPaidNonCash = ttlPaidNonCash + oresult.GetDouble("paid_noncash");
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    dynamic jsonTotal = new ExpandoObject();
                    jsonTotal.ttlRecords = ttlRecords;
                    jsonTotal.ttlPaidCash = ttlPaidCash.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCash = ttlPaidNonCash.ToString("#,##0.00");

                    dynamic jsonDetails = new ExpandoObject();
                    jsonDetails.summary = jsonTotal;
                    jsonDetails.records = jsonObjects;

                    string jsonString = JsonSerializer.Serialize(jsonDetails);

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

        public async Task<ServiceResponse> GetCollectionReport(string dateFrom, string dateTo, int customerID, string postedBy, string logicSite, bool isIndustrial, bool isNonIndustrial)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `payment_reference`,
                                                `payment_date`, 
	                                            `customer_id`,  
                                                `customer_name`,
                                                (CASE WHEN `industrial` = 1 AND `non_industrial` = 1 
	                                            THEN 'Industrial/Non-Idustrial' 
	                                            ELSE 
		                                            CASE WHEN `industrial` = 1 THEN 'Industrial' 
		                                            ELSE
			                                            CASE WHEN `non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                            END
	                                            END)	AS `type`,
                                                SUM(paid_cash_full_billing_payment) AS paid_cash_full_billing_payment,
                                                SUM(paid_noncash_full_billing_payment) AS paid_noncash_full_billing_payment,
                                                SUM(paid_cash_per_invoice_payment) AS paid_cash_per_invoice_payment,
                                                SUM(paid_noncash_per_invoice_payment) AS paid_noncash_per_invoice_payment,
                                                SUM(paid_cash_float_payment) AS paid_cash_float_payment,
                                                SUM(paid_noncash_float_payment) AS paid_noncash_float_payment,
                                                `payment_mode`, 
	                                            `site`,
                                                `code`,
	                                            `posted_by`	
                                         FROM(

                                            SELECT pr.`payment_reference`, 
	                                                `payment_date`, 
	                                                pr.`customer_id`,  
                                                    `customer_name`,
                                                    `industrial`,
                                                    `non_industrial`,
	                                                CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash_full_billing_payment, 
	                                                CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash_full_billing_payment,
	                                                0.0 AS paid_cash_per_invoice_payment, 
	                                                0.0 AS paid_noncash_per_invoice_payment,
	                                                0.0 AS paid_cash_float_payment, 
	                                                0.0 AS paid_noncash_float_payment,
	                                                `payment_mode`, 
	                                                CASE WHEN ls.`site` <> NULL THEN ls.`site`
								                        ELSE CASE WHEN ls1.`site` <> NULL THEN ls1.`site` ELSE 'Admin' END
								                        END AS 'site',
							                        CASE WHEN ls.`code` <> NULL THEN ls.`code`
								                        ELSE CASE WHEN ls1.`code` <> NULL THEN ls1.`code` ELSE 'Admin' END
								                        END AS 'code',
	                                                `posted_by`						
	 
	                                        FROM 
	                                        `laundry`.`payment_references` 	 pr
	                                        INNER JOIN `laundry`.`billing_references` br ON br.`payment_reference` = pr.`payment_reference`
	                                        INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                            INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                            INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                            LEFT JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.`site`
                                            LEFT JOIN `laundry`.`laundry_users`lu1 ON lu1.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`laundry_sites` ls1 ON ls1.`code` = lu1.`site`
                                            WHERE
                                                `float_payment` = 0 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)
                                        
                                            UNION ALL

                                            SELECT pr.`payment_reference`, 
	                                                `payment_date`, 
	                                                pr.`customer_id`,  
                                                    `customer_name`,
                                                    `industrial`,
                                                    `non_industrial`,
	                                                0.0 AS paid_cash_full_billing_payment, 
	                                                0.0 AS paid_noncash_full_billing_payment,
	                                                CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash_per_invoice_payment, 
	                                                CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash_per_invoice_payment,
	                                                0.0 AS paid_cash_float_payment, 
	                                                0.0 AS paid_noncash_float_payment,
	                                                `payment_mode`, 
	                                                CASE WHEN ls.`site` <> NULL THEN ls.`site`
								                        ELSE CASE WHEN ls1.`site` <> NULL THEN ls1.`site` ELSE 'Admin' END
								                        END AS 'site',
							                        CASE WHEN ls.`code` <> NULL THEN ls.`code`
								                        ELSE CASE WHEN ls1.`code` <> NULL THEN ls1.`code` ELSE 'Admin' END
								                        END AS 'code',
	                                                `posted_by`						
	 
	                                        FROM 
	                                        `laundry`.`payment_references` 	 pr
	                                        INNER JOIN `laundry`.`invoices` i ON i.`payment_reference` = pr.`payment_reference`
	                                        INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                            INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                            INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                            LEFT JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.`site`
                                            LEFT JOIN `laundry`.`laundry_users`lu1 ON lu1.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`laundry_sites` ls1 ON ls1.`code` = lu1.`site`
                                            WHERE
                                                `float_payment` = 0 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)
                                                 AND i.`invoice_reference` NOT IN (
                                                                                        SELECT 
                                                                                        rd.`invoice_reference`
                                                                                        FROM `billing_reference_details` rd
                                                                                        INNER JOIN `billing_references` r ON rd.`billing_reference` = r.`billing_reference` AND r.`paid`=1                                                                                    
                                                                                    )
                                        
                                            UNION ALL

                                            SELECT pr.`payment_reference`, 
	                                                `payment_date`, 
	                                                pr.`customer_id`,  
                                                    `customer_name`,
                                                    `industrial`,
                                                    `non_industrial`,
	                                                0.0 AS paid_cash_full_billing_payment, 
	                                                0.0 AS paid_noncash_full_billing_payment,
	                                                0.0 AS paid_cash_per_invoice_payment, 
	                                                0.0 AS paid_noncash_per_invoice_payment,
	                                                CASE WHEN `non_cash` = 0 THEN `amount_paid` ELSE 0 END AS paid_cash_float_payment, 
	                                                CASE WHEN `non_cash` = 1 THEN `amount_paid` ELSE 0 END AS paid_noncash_float_payment,
	                                                `payment_mode`, 
	                                                CASE WHEN ls.`site` <> NULL THEN ls.`site`
								                        ELSE CASE WHEN ls1.`site` <> NULL THEN ls1.`site` ELSE 'Admin' END
								                        END AS 'site',
							                        CASE WHEN ls.`code` <> NULL THEN ls.`code`
								                        ELSE CASE WHEN ls1.`code` <> NULL THEN ls1.`code` ELSE 'Admin' END
								                        END AS 'code',
	                                                `posted_by`						
	 
	                                        FROM 
	                                        `laundry`.`payment_references` 	 pr
	                                        INNER JOIN `laundry`.`system_users` u ON `user_id` = pr.`posted_by`
                                            INNER JOIN `laundry`.`customers` c ON c.`customer_id` = pr.`customer_id`
                                            INNER JOIN `laundry`.`payment_modes` pm ON pm.`payment_code` = pr.`payment_mode`
                                            LEFT JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.`site`
                                            LEFT JOIN `laundry`.`laundry_users`lu1 ON lu1.`user_id` = u.`user_name`
                                            LEFT JOIN `laundry`.`laundry_sites` ls1 ON ls1.`code` = lu1.`site`
                                            WHERE
                                                `float_payment` = 1 AND DATE(payment_date) BETWEEN DATE(@dateFrom) AND (@dateTo)
                                        )xx
                                        WHERE
                                            1=1 ";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `customer_id` = @customer_id";
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }

                if(postedBy != null)
                {
                    if (!postedBy.Equals(""))
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  posted_by = @posted_by";
                        commonFunctions.BindParameter(oCommand, "@posted_by", System.Data.DbType.String, postedBy);
                    }
                }              

                if(logicSite != null)
                {
                    if (!logicSite.Equals(""))
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  code = @code";
                        commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, logicSite);
                    }
                }
               
                if (isIndustrial && isNonIndustrial)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  (industrial = 1 OR non_industrial = 1)";
                }
                else
                {
                    if (isIndustrial)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  industrial = 1";
                    }

                    if (isNonIndustrial)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  non_industrial = 1";
                    }
                }
                
                

                oCommand.CommandText = oCommand.CommandText + @"
                                                                GROUP BY 
                                                                    `payment_reference`,
                                                                    `payment_date`, 
	                                                                `customer_id`,  
                                                                    `customer_name`,
                                                                    `payment_mode`, 
	                                                                `site`,
                                                                    `code`,
	                                                                `posted_by`
                                                               ";

                commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObjects = new List<ExpandoObject>();

                int ttlRecords = 0;
                double ttlPaidCash = 0;
                double ttlPaidNonCash = 0;

                double ttlPaidCashFullPayment = 0;
                double ttlPaidNonCashFullPayment = 0;
                double ttlPaidCashPerInvoice = 0;
                double ttlPaidNonCashPerInvoice = 0;
                double ttlPaidCashFloat = 0;
                double ttlPaidNonCashFloat = 0;

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic jsonObject = new ExpandoObject();
                        hasValue = true;

                        jsonObject.payment_reference = oresult.GetInt32("payment_reference");
                        jsonObject.payment_date = oresult.GetDateTime("payment_date").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.type = oresult.GetString("type");
                        jsonObject.paid_cash_full_billing_payment = oresult.GetDouble("paid_cash_full_billing_payment").ToString("#,##0.00");
                        jsonObject.paid_noncash_full_billing_payment = oresult.GetDouble("paid_noncash_full_billing_payment").ToString("#,##0.00");
                        jsonObject.paid_cash_per_invoice_payment = oresult.GetDouble("paid_cash_per_invoice_payment").ToString("#,##0.00");
                        jsonObject.paid_noncash_per_invoice_payment = oresult.GetDouble("paid_noncash_per_invoice_payment").ToString("#,##0.00");
                        jsonObject.paid_cash_float_payment = oresult.GetDouble("paid_cash_float_payment").ToString("#,##0.00");
                        jsonObject.paid_noncash_float_payment = oresult.GetDouble("paid_noncash_float_payment").ToString("#,##0.00");
                        jsonObject.payment_mode = oresult.GetString("payment_mode");
                        jsonObject.site = oresult.GetString("site");
                        jsonObject.code = oresult.GetString("code");
                        jsonObject.posted_by = oresult.GetString("posted_by");

                        jsonObjects.Add(jsonObject);

                        ttlRecords = ttlRecords + 1;
                        ttlPaidCashFullPayment = ttlPaidCashFullPayment + oresult.GetDouble("paid_cash_full_billing_payment");
                        ttlPaidNonCashFullPayment = ttlPaidNonCashFullPayment + oresult.GetDouble("paid_noncash_full_billing_payment");
                        ttlPaidCashPerInvoice = ttlPaidCashPerInvoice + oresult.GetDouble("paid_cash_per_invoice_payment");
                        ttlPaidNonCashPerInvoice = ttlPaidNonCashPerInvoice + oresult.GetDouble("paid_noncash_per_invoice_payment");
                        ttlPaidCashFloat = ttlPaidCashFloat + oresult.GetDouble("paid_cash_float_payment");
                        ttlPaidNonCashFloat = ttlPaidNonCashFloat + oresult.GetDouble("paid_noncash_float_payment");

                        
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    ttlPaidCash = ttlPaidCash + ttlPaidCashFullPayment + ttlPaidCashPerInvoice + ttlPaidCashFloat;
                    ttlPaidNonCash = ttlPaidNonCash + ttlPaidNonCashFullPayment + ttlPaidNonCashPerInvoice + ttlPaidNonCashFloat;

                    dynamic jsonTotal = new ExpandoObject();
                    jsonTotal.ttlRecords = ttlRecords;
                    jsonTotal.ttlPaidCashFullPayment = ttlPaidCashFullPayment.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCashFullPayment = ttlPaidNonCashFullPayment.ToString("#,##0.00");
                    jsonTotal.ttlPaidCashPerInvoice = ttlPaidCashPerInvoice.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCashPerInvoice = ttlPaidNonCashPerInvoice.ToString("#,##0.00");
                    jsonTotal.ttlPaidCashFloat = ttlPaidCashFloat.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCashFloat = ttlPaidNonCashFloat.ToString("#,##0.00");
                    jsonTotal.ttlPaidCash = ttlPaidCash.ToString("#,##0.00");
                    jsonTotal.ttlPaidNonCash = ttlPaidNonCash.ToString("#,##0.00");
                    jsonTotal.ttlAmount = Convert.ToDouble(ttlPaidNonCash + ttlPaidCash).ToString("#,##0.00");


                    dynamic jsonDetails = new ExpandoObject();
                    jsonDetails.summary = jsonTotal;
                    jsonDetails.records = jsonObjects;

                    string jsonString = JsonSerializer.Serialize(jsonDetails);

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

        public async Task<ServiceResponse> SaveFullPaymentAsync(PaymentReference paymentReference, int billingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            transaction = gDb.Connection.BeginTransaction();

            try
            {
                oCommand.Transaction = transaction;
                serviceResponse.SetValues(0, "Initialized", "");
                
                paymentReference.float_payment = 0;

                oCommand.Transaction = transaction;

                //Insert image -----------------------------------------------------------
                long imageID = await SavePaymentReferenceImagesAsync(paymentReference, oCommand);

                paymentReference.image_entry_id = imageID;

                //-------------------------------------------------------------------------

                //Insert payment reference------------------------------------------------
                long paymentRefenereceNo = await SavePaymentReferencesAsync(paymentReference, oCommand);               
                
                //-------------------------------------------------------------------------

                if (paymentRefenereceNo > 0)
                {
                    paymentReference.payment_reference = paymentRefenereceNo;

                    //Insert details -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`payment_reference_details`
                                    (
                                        `payment_reference`, 
	                                    `invoice_reference`
                                    )
                                    SELECT  @payment_reference, 
                                            invoice_reference
                                    FROM `laundry`.`invoices`
                                    WHERE `billing_reference` = @billing_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference.payment_reference);
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32,billingReference);

                    await oCommand.ExecuteNonQueryAsync();

                    //update billing reference as paid
                    oCommand.CommandText = @"UPDATE `laundry`.`billing_references`
                                            SET
                                                `paid` = 1, 
	                                            `payment_reference` = @payment_reference
                                            WHERE `billing_reference` = @billing_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference.payment_reference);
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);

                    await oCommand.ExecuteNonQueryAsync();

                    //update invoices as paid
                    oCommand.CommandText = @"UPDATE `laundry`.`invoices`
                                            SET
                                                `paid` = 1, 
	                                            `payment_reference` = @payment_reference
                                            WHERE `billing_reference` = @billing_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference.payment_reference);
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);

                    await oCommand.ExecuteNonQueryAsync();

                    

                    await transaction.CommitAsync();

                    string jsonString = JsonSerializer.Serialize(paymentReference);

                    serviceResponse.SetValues(200, "Success", jsonString);

                    //------------------------------------------------------------------------------                    
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                if(transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SavePaymentPerInvoiceAsync(PaymentReference paymentReference, int invoiceReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            transaction = gDb.Connection.BeginTransaction();

            try
            {
                oCommand.Transaction = transaction;
                serviceResponse.SetValues(0, "Initialized", "");

                paymentReference.float_payment = 0;

                oCommand.Transaction = transaction;

                //Insert image -----------------------------------------------------------
                long imageID = await SavePaymentReferenceImagesAsync(paymentReference, oCommand);

                paymentReference.image_entry_id = imageID;

                //------------------------------------------------------------------------


                //Insert payment reference -----------------------------------------------
                long paymentRefenereceNo = await SavePaymentReferencesAsync(paymentReference, oCommand);

                //-------------------------------------------------------------------------

                if (paymentRefenereceNo > 0)
                {
                    paymentReference.payment_reference = paymentRefenereceNo;
                    //Insert details -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`payment_reference_details`
                                    (
                                        `payment_reference`, 
	                                    `invoice_reference`
                                    )
                                    SELECT  @payment_reference, 
                                            invoice_reference
                                    FROM `laundry`.`invoices`
                                    WHERE `invoice_reference` = @invoice_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference.payment_reference);
                    commonFunctions.BindParameter(oCommand, "@invoice_reference", System.Data.DbType.Int32, invoiceReference);

                    await oCommand.ExecuteNonQueryAsync();

                    //update invoices as paid
                    oCommand.CommandText = @"UPDATE `laundry`.`invoices`
                                            SET
                                                `paid` = 1, 
	                                            `payment_reference` = @payment_reference
                                            WHERE `invoice_reference` = @invoice_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference.payment_reference);
                    commonFunctions.BindParameter(oCommand, "@invoice_reference", System.Data.DbType.Int32, invoiceReference);

                    await oCommand.ExecuteNonQueryAsync();

                   

                    await transaction.CommitAsync();

                    string jsonString = JsonSerializer.Serialize(paymentReference);

                    serviceResponse.SetValues(200, "Success", jsonString);

                    //------------------------------------------------------------------------------                    
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                if(transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveFloatPaymentAsync(PaymentReference paymentReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            transaction = gDb.Connection.BeginTransaction();

            try
            {
                oCommand.Transaction = transaction;

                serviceResponse.SetValues(0, "Initialized", "");

                paymentReference.float_payment = 1;

                oCommand.Transaction = transaction;

                //Insert image -----------------------------------------------------------
                long imageId = await SavePaymentReferenceImagesAsync(paymentReference, oCommand);

                paymentReference.image_entry_id = imageId;

                long paymentRefenereceNo = await SavePaymentReferencesAsync(paymentReference, oCommand);
                
                if (paymentRefenereceNo > 0)
                {
                    paymentReference.payment_reference = paymentRefenereceNo;                    

                    await transaction.CommitAsync();

                    string jsonString = JsonSerializer.Serialize(paymentReference);

                    serviceResponse.SetValues(200, "Success", jsonString);

                    //------------------------------------------------------------------------------                    
                }
                else
                {
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

        public async Task<Int64> SavePaymentReferencesAsync(PaymentReference paymentReference, MySqlCommand oCommand)
        {
             long paymentReferenceNo = 0;
            oCommand.CommandText = @"INSERT INTO  `laundry`.`payment_references`
                                            (
                                                    `payment_date`, 
                                                    `customer_id`, 
                                                    `amount_paid`, 
                                                    `payment_mode`, 
                                                    `float_payment`, 
                                                    `posted_by`, 
                                                    `posting_datetime`,
                                                    `image_entry_id`
                                            )
	 
	                                VALUES 
	                                        (
                                                    @payment_date,
                                                    @customer_id,
                                                    @amount_paid,
                                                    @payment_mode,
                                                    @float_payment,
                                                    @posted_by,
                                                    NOW(),
                                                    @image_entry_id
                                            );";

            oCommand.Parameters.Clear();

            commonFunctions.BindParameter(oCommand, "@payment_date", System.Data.DbType.String, paymentReference.payment_date);
            commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, paymentReference.customer_id);
            commonFunctions.BindParameter(oCommand, "@amount_paid", System.Data.DbType.Double, paymentReference.amount_paid);
            commonFunctions.BindParameter(oCommand, "@payment_mode", System.Data.DbType.String, paymentReference.payment_mode);
            commonFunctions.BindParameter(oCommand, "@float_payment", System.Data.DbType.Int32, paymentReference.float_payment);
            commonFunctions.BindParameter(oCommand, "@posted_by", System.Data.DbType.String, paymentReference.posted_by);

            if(paymentReference.image_entry_id > 0)
            {
                commonFunctions.BindParameter(oCommand, "@image_entry_id", System.Data.DbType.Int32, paymentReference.image_entry_id);
            }
            else
            {
                commonFunctions.BindParameter(oCommand, "@image_entry_id", System.Data.DbType.Int32, DBNull.Value);
            }
           

            await oCommand.ExecuteNonQueryAsync();

            paymentReferenceNo = oCommand.LastInsertedId;

            return paymentReferenceNo;
        }

        public async Task<Int64> SavePaymentReferenceImagesAsync(PaymentReference paymentReference, MySqlCommand oCommand)
        {
            long image_entry_id = 0;
            //Insert image -----------------------------------------------------------
            if (!paymentReference.payment_image.Equals(""))
            {
                oCommand.CommandText = @"INSERT INTO  `images`.`laundry_payment_references` 
                                    (
                                        `payment_image`
                                    )
                                    VALUES
                                    (
                                        @payment_image
                                    )";

                oCommand.Parameters.Clear();


                commonFunctions.BindParameter(oCommand, "@payment_image", System.Data.DbType.String, paymentReference.payment_image);

                await oCommand.ExecuteNonQueryAsync();
                image_entry_id = oCommand.LastInsertedId;
            }
            

            return image_entry_id;

        }

        public async Task<ServiceResponse> CancelPaymentAsync(int paymentReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            transaction = gDb.Connection.BeginTransaction();

            try
            {
                oCommand.Transaction = transaction;

                serviceResponse.SetValues(0, "Initialized", "");

                //cancel payment reference
                oCommand.CommandText = @"UPDATE `laundry`.`payment_references`
                                            SET
                                                `cancelled_entry` = 1
	                                            
                                            WHERE `payment_reference` = @payment_reference AND `cancelled_entry` = 0";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference);

                int i = await oCommand.ExecuteNonQueryAsync();

                if(i > 0)
                {
                    //update billing reference as unpaid
                    oCommand.CommandText = @"UPDATE `laundry`.`billing_references`
                                            SET
                                                `paid` = 0, 
	                                            `payment_reference` = NULL

                                            WHERE `payment_reference` = @payment_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference);

                    await oCommand.ExecuteNonQueryAsync();

                    //update invoices as unpaid
                    oCommand.CommandText = @"UPDATE `laundry`.`invoices`
                                            SET
                                                `paid` = 0, 
	                                            `payment_reference` = NULL

                                            WHERE `payment_reference` = @payment_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@payment_reference", System.Data.DbType.Int32, paymentReference);

                    await oCommand.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();

                    serviceResponse.SetValues(200, "Success", "");

                    //------------------------------------------------------------------------------   
                }
                else
                {
                    await transaction.RollbackAsync();

                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                if(transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }
    }
}
