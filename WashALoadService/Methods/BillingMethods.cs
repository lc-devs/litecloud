using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;

namespace WashALoadService.Methods
{
    public class BillingMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal BillingMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public BillingMethods() { }

        public async Task<ServiceResponse> GenerateBillingReport(string dateFrom, string dateTo, int customerID, string type)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.Parameters.Clear();

                if (type.ToLower().Equals("industrial"))
                {
                    oCommand.CommandText = @"SELECT DISTINCT br.`billing_reference`, 
	                                                `billing_date`, 
	                                                br.`customer_id`, 
	                                                c.`customer_name`,
	                                                (`previous_unpaid_bill_subtotal_amount` + `current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS bill_amount, 
	                                                `paid`,
	                                                'Industrial' AS `type`
	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = brd.`invoice_reference`
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                            WHERE DATE(`billing_date`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    }

                }
                else if (type.ToLower().Equals("non-industrial"))
                {
                    oCommand.CommandText = @"SELECT DISTINCT br.`billing_reference`, 
	                                                `billing_date`, 
	                                                br.`customer_id`, 
	                                                c.`customer_name`,
	                                                (`previous_unpaid_bill_subtotal_amount` + `current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS bill_amount, 
	                                                `paid`,
	                                                'Non-Industrial' AS `type`
	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
	                                        INNER JOIN `laundry`.`non_industrial_invoices` nii ON nii.`invoice_reference` = brd.`invoice_reference`
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                            WHERE DATE(`billing_date`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    }
                }
                else
                {
                    oCommand.CommandText = @"SELECT DISTINCT br.`billing_reference`, 
	                                                `billing_date`, 
	                                                br.`customer_id`, 
	                                                c.`customer_name`,
	                                                (`previous_unpaid_bill_subtotal_amount` + `current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS bill_amount, 
	                                                `paid`,
	                                                'Industrial' AS `type`
	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = brd.`invoice_reference`
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                            WHERE DATE(`billing_date`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";                        
                    }

                    oCommand.CommandText = oCommand.CommandText + @"
                                    
                                        UNION ALL

                                            SELECT DISTINCT br.`billing_reference`, 
	                                                `billing_date`, 
	                                                br.`customer_id`, 
	                                                c.`customer_name`,
	                                                (`previous_unpaid_bill_subtotal_amount` + `current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS bill_amount, 
	                                                `paid`,
	                                                'Non-Industrial' AS `type`
	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
	                                        INNER JOIN `laundry`.`non_industrial_invoices` nii ON nii.`invoice_reference` = brd.`invoice_reference`
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                            WHERE DATE(`billing_date`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    }
                }

                if (customerID != 0)
                {
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }
                commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObject = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic oJSON = new ExpandoObject();
                        hasValue = true;
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.paid = oresult.GetInt32("paid");
                        oJSON.bill_amount = oresult.GetDouble("bill_amount").ToString("#,##0.00");
                        oJSON.type = oresult.GetString("type");
                        oJSON.billing_reference = oresult.GetInt32("billing_reference");
                        oJSON.billing_date = oresult.GetDateTime("billing_date").ToString("yyyy-MM-dd HH:mm:ss");

                        jsonObject.Add(oJSON);


                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(jsonObject);

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
        public async Task<ServiceResponse> GetBillingDetails(int billingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                //get billing reference info----------------------------

                oCommand.CommandText = @"SELECT br.`billing_reference`, 
	                                        `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        CONCAT(`street_building_address`, ' ', `town_address`, ' ', `province`) AS address,
	                                        `paid`,
	                                        (CASE WHEN `industrial` = 1 AND `non_industrial` = 1 
	                                        THEN 'Industrial/Non-Idustrial' 
	                                        ELSE 
		                                        CASE WHEN `industrial` = 1 THEN 'Industrial' 
		                                        ELSE
			                                        CASE WHEN `non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                        END
	                                        END)	AS `type`,
	                                        `average_daily_load` AS required_ADL,
	                                        (SUM(`weight_in_kg`) /(SELECT `wkg_per_load` FROM `laundry`.`system_parameters`)) AS current_ADL

                                        FROM 
                                        `laundry`.`billing_references` br
                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = brd.`invoice_reference`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                        INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
                                        WHERE br.`billing_reference` = @billing_reference
                                            
                                        GROUP BY 
						                    `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        `type`,
	                                        required_ADL,
	                                        `paid`

                                    UNION ALL

                                        SELECT br.`billing_reference`, 
	                                        `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        CONCAT(`street_building_address`, ' ', `town_address`, ' ', `province`) AS address,
	                                        `paid`,
	                                        (CASE WHEN `industrial` = 1 AND `non_industrial` = 1 
	                                        THEN 'Industrial/Non-Idustrial' 
	                                        ELSE 
		                                        CASE WHEN `industrial` = 1 THEN 'Industrial' 
		                                        ELSE
			                                        CASE WHEN `non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                        END
	                                        END)	AS `type`,
	                                        `average_daily_load`  AS required_ADL,
	                                        (SUM(`weight_in_kg`) /(SELECT `wkg_per_load` FROM `laundry`.`system_parameters`)) AS current_ADL

                                        FROM 
                                        `laundry`.`billing_references` br
                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
                                        INNER JOIN `laundry`.`non_industrial_invoices` nii ON nii.`invoice_reference` = brd.`invoice_reference`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                        INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = nii.`so_reference`
                                            WHERE br.`billing_reference` = @billing_reference
                                            
                                        GROUP BY 
						                    `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        `type`,
	                                        required_ADL,
	                                        `paid`";
                

                oCommand.Parameters.Clear();
                commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);


                var oresult = await oCommand.ExecuteReaderAsync();

                dynamic jsonObject = new ExpandoObject();
                dynamic oJSONHeader = new ExpandoObject();

                int customerID = 0;

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                       
                        hasValue = true;
                        oJSONHeader.customer_name = oresult.GetString("customer_name");
                        oJSONHeader.address = oresult.GetString("address");
                        oJSONHeader.customer_id = oresult.GetInt32("customer_id");
                        customerID = oresult.GetInt32("customer_id");
                        oJSONHeader.paid = oresult.GetInt32("paid");
                        oJSONHeader.required_ADL = oresult.GetInt32("required_ADL");
                        //oJSONHeader.bill_amount = oresult.GetDouble("bill_amount");
                        oJSONHeader.type = oresult.GetString("type");
                        oJSONHeader.billing_reference = oresult.GetInt32("billing_reference");
                        oJSONHeader.billing_reference_QR_image = commonFunctions.GenerateQR(Convert.ToString(oresult.GetInt32("billing_reference")));
                        oJSONHeader.billing_date = oresult.GetDateTime("billing_date").ToString("yyyy-MM-dd HH:mm:ss"); ;
                        oJSONHeader.current_ADL = oresult.GetDouble("current_ADL");

                    }
                }
                //--------------------------------------------------------------------------------------------------------------------------

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    jsonObject.oJSONHeader = oJSONHeader;

                    //get unpaid bills
                    oCommand.CommandText = @"SELECT `billing_reference`, 
	                                                `billing_date`, 
	                                                `customer_id`, 
	                                                `previous_unpaid_bill_subtotal_amount`,
	                                                (`current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS billing_amount
	 
	                                        FROM 
	                                        `laundry`.`billing_references` 
	                                        WHERE  `customer_id` = @customer_id AND `paid` = 0 AND billing_reference < @billing_reference
                                            ORDER BY billing_reference ASC;";
                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);

                    oresult = await oCommand.ExecuteReaderAsync();

                    List<ExpandoObject> oJSONUnpaidBills = new List<ExpandoObject>();

                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            dynamic oJSONUnpaidBill = new ExpandoObject();

                            oJSONUnpaidBill.customer_id = oresult.GetInt32("customer_id");
                            oJSONUnpaidBill.billing_reference = oresult.GetInt32("billing_reference");
                            oJSONUnpaidBill.bill_amount = oresult.GetDouble("billing_amount").ToString("#,##0.00");
                            oJSONUnpaidBill.billing_date = oresult.GetDateTime("billing_date").ToString("yyyy-MM-dd HH:mm:ss");

                            oJSONUnpaidBills.Add(oJSONUnpaidBill);

                        }
                    }

                    jsonObject.UnpaidBills = oJSONUnpaidBills;

                    //get unpaid invoices
                    oCommand.CommandText = @"SELECT br.`billing_reference`, 
	                                                brd.`invoice_reference`,
	                                                i.`invoice_datetime`,
	                                                ii.`so_reference`,
	                                                li.`picked_up_datetime` AS SO_date,
	                                                i.`invoice_amount`,
	                                                SUM(lid.`adl_adjustment`) AS ADL_adjustment	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON br.`billing_reference` = brd.`billing_reference`
	                                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = brd.`invoice_reference` AND i.`paid` = 0
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                        INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN `laundry`.`logistics_industrial_details` lid ON lid.`so_reference` = li.`so_reference`	
	                                        WHERE br.`billing_reference` = @billing_reference
	
	                                        GROUP BY 
	                                        br.`billing_reference`, 
	                                        brd.`invoice_reference`,
	                                        i.`invoice_datetime`,
	                                        ii.`so_reference`,
	                                        li.`picked_up_datetime`,
	                                        i.`invoice_amount`
	
                                            UNION ALL

                                            SELECT 	br.`billing_reference`, 
	                                                brd.`invoice_reference`,
	                                                i.`invoice_datetime`,
	                                                ii.`so_reference`,
	                                                li.`picked_up_datetime` AS SO_date,
	                                                i.`invoice_amount`,
	                                                0.0 AS ADL_adjustment	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON br.`billing_reference` = brd.`billing_reference`
	                                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = brd.`invoice_reference` AND i.`paid` = 0
	                                        INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                        INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        WHERE br.`billing_reference` = @billing_reference;";
                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);

                    oresult = await oCommand.ExecuteReaderAsync();

                    List<ExpandoObject> oJSONUnpaidInvoice = new List<ExpandoObject>();

                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            dynamic oJSONUnpaidInvoices = new ExpandoObject();

                            oJSONUnpaidInvoices.billing_reference = oresult.GetInt32("billing_reference");
                            oJSONUnpaidInvoices.invoice_reference = oresult.GetInt32("invoice_reference");
                            oJSONUnpaidInvoices.invoice_datetime = oresult.GetDateTime("invoice_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            oJSONUnpaidInvoices.SO_date = oresult.GetDateTime("SO_date").ToString("yyyy-MM-dd HH:mm:ss");
                            oJSONUnpaidInvoices.so_reference = oresult.GetInt32("so_reference");
                            oJSONUnpaidInvoices.ADL_adjustment = oresult.GetDouble("ADL_adjustment").ToString("#,##0.00");
                            oJSONUnpaidInvoices.invoice_amount = oresult.GetDouble("invoice_amount").ToString("#,##0.00");                           
                            oJSONUnpaidInvoices.amount_due = Convert.ToDouble(oresult.GetDouble("invoice_amount") + oresult.GetDouble("ADL_adjustment")).ToString("#,##0.00");

                            oJSONUnpaidInvoice.Add(oJSONUnpaidInvoices);

                        }
                    }

                    jsonObject.UnpaidInvoice = oJSONUnpaidInvoice;

                    //get ADL Adjustment for paid invoices
                    oCommand.CommandText = @"SELECT DISTINCT br.`billing_reference`, 
	                                                brd.`invoice_reference`,
	                                                i.`invoice_datetime`,
	                                                ii.`so_reference`,
	                                                li.`picked_up_datetime` AS SO_date,
	                                                i.`invoice_amount` AS paid_amount,
	                                                SUM(lid.`adl_adjustment`) AS ADL_adjustment,
	                                                i.`payment_reference`,
                                                    `payment_date`
                                            
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON br.`billing_reference` = brd.`billing_reference`
	                                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = brd.`invoice_reference` AND i.`paid` = 1
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                        INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN `laundry`.`logistics_industrial_details` lid ON lid.`so_reference` = li.`so_reference`
                                            INNER JOIN `laundry`.`payment_reference_details` prd ON prd.`invoice_reference` = ii.`invoice_reference`
                                            INNER JOIN `laundry`.`payment_references` pr ON pr.`payment_reference`= prd.`payment_reference`
	                                        WHERE br.`billing_reference` = @billing_reference
	
	                                        GROUP BY 
	                                        br.`billing_reference`, 
	                                        brd.`invoice_reference`,
	                                        i.`invoice_datetime`,
	                                        ii.`so_reference`,
	                                        li.`picked_up_datetime`,
	                                        i.`invoice_amount`,
	                                        i.`payment_reference`
	
                                            UNION ALL

                                            SELECT 	DISTINCT br.`billing_reference`, 
	                                                brd.`invoice_reference`,
	                                                i.`invoice_datetime`,
	                                                ii.`so_reference`,
	                                                li.`picked_up_datetime` AS SO_date,
	                                                i.`invoice_amount`  AS paid_amount,
	                                                0.0 AS ADL_adjustment,
	                                                i.`payment_reference`,
                                                    `payment_date`	 
	                                        FROM 
	                                        `laundry`.`billing_references` br
	                                        INNER JOIN `laundry`.`billing_reference_details` brd ON br.`billing_reference` = brd.`billing_reference`
	                                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = brd.`invoice_reference` AND i.`paid` = 1
	                                        INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                        INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = ii.`so_reference`
                                            INNER JOIN `laundry`.`payment_reference_details` prd ON prd.`invoice_reference` = ii.`invoice_reference`
                                            INNER JOIN `laundry`.`payment_references` pr ON pr.`payment_reference`= prd.`payment_reference`
	                                        WHERE br.`billing_reference` = @billing_reference;";
                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);

                    oresult = await oCommand.ExecuteReaderAsync();

                    List<ExpandoObject> oJSONPaidInvoice = new List<ExpandoObject>();

                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            dynamic oJSONPaidInvoices = new ExpandoObject();

                            oJSONPaidInvoices.billing_reference = oresult.GetInt32("billing_reference");
                            oJSONPaidInvoices.invoice_reference = oresult.GetInt32("invoice_reference");
                            oJSONPaidInvoices.invoice_datetime = oresult.GetDateTime("invoice_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            oJSONPaidInvoices.SO_date = oresult.GetDateTime("SO_date").ToString("yyyy-MM-dd HH:mm:ss");
                            oJSONPaidInvoices.so_reference = oresult.GetInt32("so_reference");
                            oJSONPaidInvoices.ADL_adjustment = oresult.GetDouble("ADL_adjustment").ToString("#,##0.00");
                            oJSONPaidInvoices.paid_amount = oresult.GetDouble("paid_amount").ToString("#,##0.00");
                            oJSONPaidInvoices.payment_reference = oresult.GetString("payment_reference");
                            oJSONPaidInvoices.payment_date = oresult.GetDateTime("payment_date").ToString("yyyy-MM-dd HH:mm:ss");

                            oJSONPaidInvoice.Add(oJSONPaidInvoices);

                        }
                    }

                    jsonObject.PaidInvoice = oJSONPaidInvoice;


                    string jsonString = JsonSerializer.Serialize(jsonObject);

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
        public async Task<ServiceResponse> GetBillingCustomerDetails(int billingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                //get billing reference info----------------------------
                oCommand.CommandText = @"SELECT br.`billing_reference`, 
	                                        `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        CONCAT(`street_building_address`, ' ', `town_address`, ' ', `province`) AS address,
	                                        `paid`,
	                                        (CASE WHEN `industrial` = 1 AND `non_industrial` = 1 
	                                        THEN 'Industrial/Non-Idustrial' 
	                                        ELSE 
		                                        CASE WHEN `industrial` = 1 THEN 'Industrial' 
		                                        ELSE
			                                        CASE WHEN `non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                        END
	                                        END)	AS `type`,
	                                        `average_daily_load` AS required_ADL

                                        FROM 
                                        `laundry`.`billing_references` br
                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = brd.`invoice_reference`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                        WHERE br.`billing_reference` = @billing_reference

                                    UNION ALL

                                        SELECT br.`billing_reference`, 
	                                        `billing_date`, 
	                                        br.`customer_id`, 
	                                        c.`customer_name`,
	                                        CONCAT(`street_building_address`, ' ', `town_address`, ' ', `province`) AS address,
	                                        `paid`,
	                                        (CASE WHEN `industrial` = 1 AND `non_industrial` = 1 
	                                        THEN 'Industrial/Non-Idustrial' 
	                                        ELSE 
		                                        CASE WHEN `industrial` = 1 THEN 'Industrial' 
		                                        ELSE
			                                        CASE WHEN `non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                        END
	                                        END)	AS `type`,
	                                        `average_daily_load`  AS required_ADL

                                        FROM 
                                        `laundry`.`billing_references` br
                                        INNER JOIN `laundry`.`billing_reference_details` brd ON brd.`billing_reference` = br.`billing_reference`
                                        INNER JOIN `laundry`.`non_industrial_invoices` nii ON nii.`invoice_reference` = brd.`invoice_reference`
                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = br.`customer_id`
                                            WHERE br.`billing_reference` = @billing_reference";


                oCommand.Parameters.Clear();
                commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingReference);


                var oresult = await oCommand.ExecuteReaderAsync();

                dynamic jsonObject = new ExpandoObject();
                dynamic oJSONHeader = new ExpandoObject();

                int customerID = 0;

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {

                        hasValue = true;
                        oJSONHeader.customer_name = oresult.GetString("customer_name");
                        oJSONHeader.address = oresult.GetString("address");
                        oJSONHeader.customer_id = oresult.GetInt32("customer_id");
                        customerID = oresult.GetInt32("customer_id");
                        oJSONHeader.paid = oresult.GetInt32("paid").ToString("#,##0.00");
                        oJSONHeader.required_ADL = oresult.GetInt32("required_ADL");
                        oJSONHeader.type = oresult.GetString("type");
                        oJSONHeader.billing_reference = oresult.GetInt32("billing_reference");
                        oJSONHeader.billing_date = oresult.GetDateTime("billing_date").ToString("yyyy-MM-dd HH:mm:ss"); ;

                    }
                }
                //--------------------------------------------------------------------------------------------------------------------------

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oJSONHeader);

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
        public async Task<ServiceResponse> GetUnBilledInvoicesByDate(string dateFrom, string dateTo, int customerID, string type)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.Parameters.Clear();

                if (type.ToLower().Equals("industrial"))
                {
                    oCommand.CommandText = @"SELECT 	c.`customer_name`,
	                                                c.`customer_id`,
	                                                COUNT(i.`invoice_reference`) AS invoiceCount, 	
	                                                SUM(`invoice_amount`) AS totalInvoiceAmount, 
	                                                'Industrial' AS `type`
	
	 
	                                        FROM 
	                                        `laundry`.`invoices` i
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                        INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                            WHERE (DATE(`invoice_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND `billed` =0)
                                                   OR
                                                  (DATE(`invoice_datetime`) < DATE(@dateFrom) AND `billed` = 0)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                    }
                    oCommand.CommandText = oCommand.CommandText + " GROUP BY c.`customer_id`, c.`customer_name`";

                }
                else if (type.ToLower().Equals("non-industrial"))
                {
                    oCommand.CommandText = @"SELECT 	c.`customer_name`,
	                                                c.`customer_id`,
	                                                COUNT(i.`invoice_reference`) AS invoiceCount, 	
	                                                SUM(`invoice_amount`) AS totalInvoiceAmount, 
	                                                'Non-Industrial' AS `type`
	
	 
	                                        FROM 
	                                        `laundry`.`invoices` i
	                                        INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                        INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                            WHERE (DATE(`invoice_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND `billed` =0)
                                                   OR
                                                  (DATE(`invoice_datetime`) < DATE(@dateFrom) AND `billed` = 0)";
                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                    }
                    oCommand.CommandText = oCommand.CommandText + " GROUP BY c.`customer_id`, c.`customer_name`";

                }
                else
                {
                    oCommand.CommandText = @"SELECT c.`customer_name`,
	                                                c.`customer_id`,
	                                                COUNT(i.`invoice_reference`) AS invoiceCount, 	
	                                                SUM(`invoice_amount`) AS totalInvoiceAmount, 
	                                                'Non-Industrial' AS `type`
	
	 
	                                        FROM 
	                                        `laundry`.`invoices` i
	                                        INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                        INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                            WHERE (DATE(`invoice_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND `billed` =0)
                                                   OR
                                                  (DATE(`invoice_datetime`) < DATE(@dateFrom) AND `billed` = 0)
                                            GROUP BY c.`customer_id`, c.`customer_name`

                                    UNION ALL

                                            SELECT 	c.`customer_name`,
	                                                        c.`customer_id`,
	                                                        COUNT(i.`invoice_reference`) AS invoiceCount, 	
	                                                        SUM(`invoice_amount`) AS totalInvoiceAmount, 
	                                                        'Industrial' AS `type`
	
	 
	                                        FROM 
	                                        `laundry`.`invoices` i
	                                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                        INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                        INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                            WHERE (DATE(`invoice_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND `billed` =0)
                                                   OR
                                                  (DATE(`invoice_datetime`) < DATE(@dateFrom) AND `billed` = 0)
                                            GROUP BY c.`customer_id`, c.`customer_name`";
                }

                commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);               

                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObject = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic oJSON = new ExpandoObject();
                        hasValue = true;
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.invoiceCount = oresult.GetInt32("invoiceCount");
                        oJSON.totalInvoiceAmount = oresult.GetDouble("totalInvoiceAmount").ToString("#,##0.00");
                        oJSON.type = oresult.GetString("type");
                       
                        jsonObject.Add(oJSON);

                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(jsonObject);

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
        public async Task<ServiceResponse> GenerateBillingIndustrialAsync(List<Int32> customerIDs, string system_user)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            

            MySqlTransaction transaction = null;
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                bool hasData = false;


                for (int i = 0; i < customerIDs.Count; i++)
                {
                    hasData = true;
                    
                    using var oCommand = gDb.Connection.CreateCommand();


                    int billingNumber = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.Billing);

                    transaction = gDb.Connection.BeginTransaction();

                    oCommand.Transaction = transaction;

                    int custID = customerIDs[i];

                    double unPaidBillAmount = await commonFunctions.ComputeTotalUnBilledBilling(oCommand, custID);
                   
                    double currentBill = 0.0;
                    string invoiceDateFrom = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string invoiceDateTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //get current bill------------------------------------------------------
                    oCommand.CommandText = @"SELECT SUM(`invoice_amount`) AS ttl_invoice_amount, 
	                                                MIN(`invoice_datetime`) AS `invoice_date_from`,
	                                                MAX(`invoice_datetime`) AS `invoice_date_to`
                                            FROM `laundry`.`logistics_industrial` li 
                                            INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`so_reference` = li.`so_reference`
                                            INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = ii.`invoice_reference` AND `billed` = 0
                                            WHERE li.`customer_id` = @customer_id";


                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);

                    var oresult = await oCommand.ExecuteReaderAsync();

                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            currentBill = oresult.GetDouble("ttl_invoice_amount");
                            invoiceDateFrom = oresult.GetDateTime("invoice_date_from").ToString("yyyy-MM-dd HH:mm:ss");
                            invoiceDateTo = oresult.GetDateTime("invoice_date_to").ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    double adlAdjustmentAmount = 0.0;

                    // get current date and date for generation of ADL ---------------------
                    oCommand.CommandText = @"SELECT 
                                                DAY(NOW()) AS current_day,
                                                (CASE WHEN `firstbillingperiod` = 1 THEN `firstbilliongperiod_start_day` ELSE 0 END) AS first_billing_period,
                                                (CASE WHEN  `secondbillingperiod` = 1 THEN `secondbillingperiod_start_day` ELSE 0 END) AS second_billing_period
                                            FROM `laundry`.`system_parameters`";

                    var oADLResult = await oCommand.ExecuteReaderAsync();
                    int currentDay = 0;
                    int firstBillingPeriod = 0;
                    int secondBillingPeriod = 0;

                    using (oADLResult)
                    {
                        while (await oADLResult.ReadAsync())
                        {
                            currentDay = oADLResult.GetInt32("current_day");
                            firstBillingPeriod = oADLResult.GetInt32("first_billing_period");
                            secondBillingPeriod = oADLResult.GetInt32("second_billing_period");
                        }
                    }

                    if ((currentDay >= firstBillingPeriod && currentDay < secondBillingPeriod) || currentDay == firstBillingPeriod) //0 value is not set
                    {
                        adlAdjustmentAmount = await ComputeADLAdjustment(oCommand, custID, invoiceDateFrom, invoiceDateTo);

                    }
                    else if (currentDay >= secondBillingPeriod) //0 value is not set
                    {
                        adlAdjustmentAmount = await ComputeADLAdjustment(oCommand, custID, invoiceDateFrom, invoiceDateTo);

                    }

                    //Insert -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO `laundry`.`billing_references`
                        (
                            `billing_reference`, 
                            `billing_date`, 
                            `customer_id`, 
                            `invoice_date_from`, 
                            `invoice_date_to`, 
                            `previous_unpaid_bill_subtotal_amount`, 
                            `current_bill_subtotal_amount`, 
                            `paid_so_adl_adjustments_subtotal_amount`, 
                            `generated_by`
                        )
                        VALUES
                        (
                            @billing_reference,
                            NOW(),
                            @customer_id,
                            @invoice_date_from,
                            @invoice_date_to,
                            @previous_unpaid_bill_subtotal_amount,
                            @current_bill_subtotal_amount,
                            @paid_so_adl_adjustments_subtotal_amount,
                            @generated_by
                        );";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                    commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                    commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);
                    commonFunctions.BindParameter(oCommand, "@previous_unpaid_bill_subtotal_amount", System.Data.DbType.Double, unPaidBillAmount);
                    commonFunctions.BindParameter(oCommand, "@current_bill_subtotal_amount", System.Data.DbType.Double, currentBill);
                    commonFunctions.BindParameter(oCommand, "@paid_so_adl_adjustments_subtotal_amount", System.Data.DbType.Double, adlAdjustmentAmount);
                    commonFunctions.BindParameter(oCommand, "@generated_by", System.Data.DbType.String, system_user);


                    int isSuccess = await oCommand.ExecuteNonQueryAsync();

                    if (isSuccess > 0)
                    {
                        oCommand.CommandText = @"INSERT INTO `laundry`.`billing_reference_details`
                        (
                           `billing_reference`,
                            `invoice_reference`
                        )
                        SELECT @billing_reference, i.`invoice_reference`
                        FROM `laundry`.`logistics_industrial` li 
                        INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`so_reference` = li.`so_reference`
                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = ii.`invoice_reference` AND `billed` = 0
                            AND `invoice_datetime` >= @invoice_date_from AND `invoice_datetime` <= @invoice_date_to
                        WHERE li.`customer_id` = @customer_id;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                        commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                        commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);

                        isSuccess = await oCommand.ExecuteNonQueryAsync();

                        if (isSuccess > 0)
                        {
                            oCommand.CommandText = @"UPDATE  `laundry`.`invoices`
                                SET
                                    `billed` = 1, 
	                                `billing_reference` = @billing_reference
                                WHERE
                                    `invoice_datetime` >= @invoice_date_from AND `invoice_datetime` <= @invoice_date_to
                                    AND`billed` = 0
                                    AND `invoice_reference` IN (
                                                                    SELECT 	i.invoice_reference	 
	                                                                FROM 
	                                                                    `laundry`.`invoices` i
	                                                                INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                                                INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                                                INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                                                    WHERE c.`customer_id` = @customer_id 
                                                                )";

                            oCommand.Parameters.Clear();

                            commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                            commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                            commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                            commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);

                            await oCommand.ExecuteNonQueryAsync();

                        }
                    }

                    await transaction.CommitAsync();
                }

                if (hasData)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process an empty request.", "");
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
        public async Task<ServiceResponse> GenerateBillingNonIndustrialAsync(List<Int32> customerIDs, string system_user)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            MySqlTransaction transaction = null;

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                bool hasData = false;


                for (int i = 0; i < customerIDs.Count; i++)
                {
                    hasData = true;
                    using var oCommand = gDb.Connection.CreateCommand();

                    int billingNumber = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.Billing);

                    transaction = gDb.Connection.BeginTransaction();

                    oCommand.Transaction = transaction;

                    int custID = customerIDs[i];

                    double unPaidBillAmount = await commonFunctions.ComputeTotalUnBilledBilling(oCommand, custID);

                    double adlAdjustmentAmount = 0.0;

                    double currentBill = 0.0;
                    string invoiceDateFrom = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string invoiceDateTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //get current bill------------------------------------------------------
                    oCommand.CommandText = @"SELECT SUM(`invoice_amount`) AS ttl_invoice_amount, 
	                                                MIN(`invoice_datetime`) AS `invoice_date_from`,
	                                                MAX(`invoice_datetime`) AS `invoice_date_to`
                                            FROM `laundry`.`logistics_non_industrial` li 
                                            INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`so_reference` = li.`so_reference`
                                            INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = ii.`invoice_reference` AND `billed` = 0
                                            WHERE li.`customer_id` = @customer_id";


                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);

                    var oresult = await oCommand.ExecuteReaderAsync();

                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            currentBill = oresult.GetDouble("ttl_invoice_amount");
                            invoiceDateFrom = oresult.GetDateTime("invoice_date_from").ToString("yyyy-MM-dd HH:mm:ss");
                            invoiceDateTo = oresult.GetDateTime("invoice_date_to").ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    //Insert -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO `laundry`.`billing_references`
                        (
                            `billing_reference`, 
                            `billing_date`, 
                            `customer_id`, 
                            `invoice_date_from`, 
                            `invoice_date_to`, 
                            `previous_unpaid_bill_subtotal_amount`, 
                            `current_bill_subtotal_amount`, 
                            `paid_so_adl_adjustments_subtotal_amount`, 
                            `generated_by`
                        )
                        VALUES
                        (
                            @billing_reference,
                            NOW(),
                            @customer_id,
                            @invoice_date_from,
                            @invoice_date_to,
                            @previous_unpaid_bill_subtotal_amount,
                            @current_bill_subtotal_amount,
                            @paid_so_adl_adjustments_subtotal_amount,
                            @generated_by
                        );";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                    commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                    commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);
                    commonFunctions.BindParameter(oCommand, "@previous_unpaid_bill_subtotal_amount", System.Data.DbType.Double, unPaidBillAmount);
                    commonFunctions.BindParameter(oCommand, "@current_bill_subtotal_amount", System.Data.DbType.Double, currentBill);
                    commonFunctions.BindParameter(oCommand, "@paid_so_adl_adjustments_subtotal_amount", System.Data.DbType.Double, adlAdjustmentAmount);
                    commonFunctions.BindParameter(oCommand, "@generated_by", System.Data.DbType.String, system_user);


                    int isSuccess = await oCommand.ExecuteNonQueryAsync();

                    if (isSuccess > 0)
                    {
                        oCommand.CommandText = @"INSERT INTO `laundry`.`billing_reference_details`
                        (
                           `billing_reference`,
                            `invoice_reference`
                        )
                        SELECT @billing_reference, i.`invoice_reference`
                        FROM `laundry`.`logistics_non_industrial` li 
                        INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`so_reference` = li.`so_reference`
                        INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = ii.`invoice_reference` AND `paid` = 0
                            AND `invoice_datetime` >= @invoice_date_from AND `invoice_datetime` <= @invoice_date_to
                        WHERE li.`customer_id` = @customer_id;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                        commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                        commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);

                        isSuccess = await oCommand.ExecuteNonQueryAsync();

                        if (isSuccess > 0)
                        {
                            oCommand.CommandText = @"UPDATE  `laundry`.`invoices`
                                SET
                                    `billed` = 1, 
	                                `billing_reference` = @billing_reference
                                WHERE
                                    `invoice_datetime` >= @invoice_date_from AND `invoice_datetime` <= @invoice_date_to
                                    AND`billed` = 0
                                    AND `invoice_reference` IN (
                                                                    SELECT 	i.invoice_reference	 
	                                                                FROM 
	                                                                    `laundry`.`invoices` i
	                                                                INNER JOIN `laundry`.`non_industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference` AND i.`billed` = 0
	                                                                INNER JOIN `laundry`.`logistics_non_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                                                INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                                                    WHERE c.`customer_id` = @customer_id
                                                                )";

                            oCommand.Parameters.Clear();

                            commonFunctions.BindParameter(oCommand, "@billing_reference", System.Data.DbType.Int32, billingNumber);
                            commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custID);
                            commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
                            commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);

                            await oCommand.ExecuteNonQueryAsync();

                        }
                    }

                    await transaction.CommitAsync();
                }

                if (hasData)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process an empty request.", "");
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
        public async Task<double> ComputeADLAdjustment(MySqlCommand oCommand, int custId, string invoiceDateFrom, string invoiceDateTo) //billing period date and should compute all invoices with in billing period
        {
            double totalADLAdjustmentAmount = 0.0;

            oCommand.CommandText = @"SELECT lid.`so_reference`, 
                                            lid.`item_code`, 
                                            (ili.`unit_cost` - ili.`unit_cost_adl`) AS adjustment
                                    FROM `laundry`.`logistics_industrial_details` lid
                                    INNER JOIN `laundry`.`industrial_laundry_items` ili ON lid.`item_code` = ili.`id_item`
                                    INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = lid.`so_reference`
                                    INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`so_reference` = li.`so_reference`
                                    INNER JOIN `laundry`.`invoices` i ON i.`invoice_reference` = ii.`invoice_reference`
                                    WHERE li.`customer_id` = @customer_id AND `invoice_datetime` >= @invoice_date_from AND `invoice_datetime` <= @invoice_date_to;";

            oCommand.Parameters.Clear();

            commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custId);
            commonFunctions.BindParameter(oCommand, "@invoice_date_from", System.Data.DbType.DateTime, invoiceDateFrom);
            commonFunctions.BindParameter(oCommand, "@invoice_date_to", System.Data.DbType.DateTime, invoiceDateTo);


            var oresult = await oCommand.ExecuteReaderAsync();

            bool hasValues = false;

            List<ExpandoObject> itemDetails = new List<ExpandoObject>();

            using (oresult)
            {
                while (await oresult.ReadAsync())
                {
                    hasValues = true;

                    dynamic itemDetail = new ExpandoObject();

                    double itemADLAdjustment = oresult.GetDouble("adjustment");
                    int itemCode = oresult.GetInt32("item_code");
                    int soReference = oresult.GetInt32("so_reference");

                    itemDetail.itemADLAdjustment = itemADLAdjustment;
                    itemDetail.itemCode = itemCode;
                    itemDetail.soReference = soReference;

                    itemDetails.Add(itemDetail);

                    totalADLAdjustmentAmount = totalADLAdjustmentAmount + itemADLAdjustment;                   

                }
            }

            if(hasValues)
            {
                foreach(dynamic item in itemDetails)
                {
                    oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial_details`
                                            SET
                                                `adl_adjustment` = @adl_adjustment
                                                
                                            WHERE
                                                `so_reference` = @so_reference AND
                                                item_code = @item_code;";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, item.soReference);
                    commonFunctions.BindParameter(oCommand, "@adl_adjustment", System.Data.DbType.Double, item.itemADLAdjustment);
                    commonFunctions.BindParameter(oCommand, "@item_code", System.Data.DbType.Int32, item.itemCode);

                    int isSuccess = await oCommand.ExecuteNonQueryAsync();
                }

            }

            return totalADLAdjustmentAmount;

        }

        
    }
}
