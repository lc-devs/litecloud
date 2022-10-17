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
    public class LogisticIndustrialMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal LogisticIndustrialMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public LogisticIndustrialMethods() { }

        public async Task<ServiceResponse> FindSODetailsAsync(int soReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"select `so_reference`, 
	                                        i.`customer_id`, 
	                                        c. `customer_name`,
	                                        `booking_reference`, 
	                                        COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`picked_up_by`), '') AS `picked_up_by`, 
	                                        COALESCE(`picked_up_datetime`, '1900-01-01') as picked_up_datetime, 
	                                        `weight_in_kg`, 
	                                        `number_of_bags`, 
	                                        COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`received_by_logistics_user`), '') as `received_by_logistics_user`, 
	                                        COALESCE(`received_from_pickup_datetime`, '1900-01-01') AS received_from_pickup_datetime, 
	                                        COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`received_by_laundry_user`), '') AS `received_by_laundry_user`, 
	                                            `received_by_laundry`, 
	                                        COALESCE(`received_by_laundry_datetime`, '1900-01-01') AS received_by_laundry_datetime, 
	                                        COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`completed_by_laundry_user`), '') AS `completed_by_laundry_user`, 
	                                        `completed_by_laundry`, 
	                                        COALESCE(`completed_by_laundry_datetime`, '1900-01-01') AS completed_by_laundry_datetime, 
	                                       COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`received_from_laundry_user`), '') AS `received_from_laundry_user`, 
	                                        `received_from_laundry`, 
	                                        COALESCE(`received_from_laundry_datetime`, '1900-01-01') AS received_from_laundry_datetime, 
	                                        `for_invoicing`, 
	                                        `with_invoice`, 
	                                        COALESCE((SELECT `user_name` FROM `laundry`.`system_users` u WHERE u.`user_id` = i.`delivered_by`), '') AS `delivered_by`, 
	                                        `delivered`, 
	                                        COALESCE(`delivery_datetime`, '1900-01-01') AS delivery_datetime
	 
	                                FROM 
	                                        `laundry`.`logistics_industrial` i
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                     WHERE 
                                             `so_reference` = @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);
                var oresult = await oCommand.ExecuteReaderAsync();

                dynamic jsonObject = new ExpandoObject();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {

                        hasValue = true;
                        jsonObject.so_reference = oresult.GetInt32("so_reference");
                        jsonObject.customer_id = oresult.GetInt32("customer_id");
                        jsonObject.customer_name = oresult.GetString("customer_name");
                        jsonObject.booking_reference = oresult.GetString("booking_reference");
                        jsonObject.picked_up_by = oresult.GetString("picked_up_by");
                        jsonObject.picked_up_datetime = oresult.GetDateTime("picked_up_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        jsonObject.number_of_bags = oresult.GetInt32("number_of_bags");
                        jsonObject.received_by_logistics_user = oresult.GetString("received_by_logistics_user");
                        jsonObject.received_from_pickup_datetime = oresult.GetDateTime("received_from_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.received_by_laundry_user = oresult.GetString("received_by_laundry_user");
                        jsonObject.received_by_laundry = oresult.GetInt32("received_by_laundry");
                        jsonObject.received_by_laundry_datetime = oresult.GetDateTime("received_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.completed_by_laundry_user = oresult.GetString("completed_by_laundry_user");
                        jsonObject.completed_by_laundry = oresult.GetInt32("completed_by_laundry");
                        jsonObject.completed_by_laundry_datetime = oresult.GetDateTime("completed_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.received_from_laundry_user = oresult.GetString("received_from_laundry_user");
                        jsonObject.received_from_laundry = oresult.GetInt32("received_from_laundry");
                        jsonObject.received_from_laundry_datetime = oresult.GetDateTime("received_from_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.for_invoicing = oresult.GetInt32("for_invoicing");
                        jsonObject.with_invoice = oresult.GetInt32("with_invoice");
                        jsonObject.delivered_by = oresult.GetString("delivered_by");
                        jsonObject.delivered = oresult.GetInt32("delivered");
                        jsonObject.delivery_datetime = oresult.GetDateTime("delivery_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    oCommand.CommandText = @"SELECT `so_reference`, 
	                                                `item_code`, 
	                                                li.`description` item_description,
	                                                `category`,
	                                                c.`description` AS category_description,
	                                                li.service,
	                                                s.`description` AS service_description,
	                                                `item_count`,
                                                    `adl_cost` as cost
	 
	                                        FROM 
	                                        `laundry`.`logistics_industrial_details` id
	                                        INNER JOIN `laundry`.`industrial_laundry_items` li ON li.`id_item` = id.`item_code`
	                                        INNER JOIN `laundry`.`industrial_services` s ON s.`id_service` = li.`service`
	                                        INNER JOIN `laundry`.`industrial_categories` c ON c.`id_category` = li.`category`
                                            WHERE `so_reference` = @so_reference";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);

                    oresult = await oCommand.ExecuteReaderAsync();

                    List<ExpandoObject> jsonItems = new List<ExpandoObject>();

                    bool hasItems = false;
                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            hasItems = true;

                            dynamic jsonItem = new ExpandoObject();
                            hasValue = true;
                            jsonItem.so_reference = oresult.GetInt32("so_reference");
                            jsonItem.item_code = oresult.GetInt32("item_code");
                            jsonItem.item_description = oresult.GetString("item_description");
                            jsonItem.category = oresult.GetInt32("category");
                            jsonItem.category_description = oresult.GetString("category_description");
                            jsonItem.service = oresult.GetInt32("service");
                            jsonItem.service_description = oresult.GetString("service_description");
                            jsonItem.item_count = oresult.GetInt32("item_count");
                            jsonItem.cost = oresult.GetDouble("cost").ToString("#,##0.00");
                            jsonItems.Add(jsonItem);

                        }
                    }

                    if (hasItems)
                    {
                        jsonObject.items = jsonItems;
                    }
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
        public async Task<ServiceResponse> ReceivedQueryReportByDate(string dateFrom, string dateTo, int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"
                                            SELECT `so_reference`, 
	                                                    i.`customer_id`, 
                                                        c.`customer_name`,
	                                                    `picked_up_datetime` as soDate, 
	                                                    `weight_in_kg`, 
	                                                    `number_of_bags`, 
	                                                    `received_from_pickup_datetime`, 
	                                                    `received_by_laundry`
	 
	                                        FROM 
	                                            `laundry`.`logistics_industrial` i
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                            WHERE DATE(`received_from_pickup_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `customer_id` = @customer_id";
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }

                oCommand.CommandText = oCommand.CommandText + " ORDER BY soDate";

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
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.picked_up_datetime = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        oJSON.number_of_bags = oresult.GetInt32("number_of_bags");
                        oJSON.received_from_pickup_datetime = oresult.GetDateTime("received_from_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.received_by_laundry = oresult.GetInt32("received_by_laundry");

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
        public async Task<ServiceResponse> ReceivedItemByLogisticssAsync(LogisticIndustrial logisticIndustrial)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                transaction = gDb.Connection.BeginTransaction();

                oCommand.Transaction = transaction;

                oCommand.CommandText = @"INSERT INTO  `laundry`.`logistics_industrial`
                                            (
                                                	`so_reference`, 
	                                                `customer_id`, 
	                                                `booking_reference`, 
	                                                `picked_up_by`, 
	                                                `picked_up_datetime`, 
	                                                `weight_in_kg`, 
	                                                `number_of_bags`,
                                                    `received_by_logistics_user`, 
	                                                `received_from_pickup_datetime`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @so_reference,
                                                @customer_id,
                                                @booking_reference,
                                                @picked_up_by,
                                                NOW(),
                                                @weight_in_kg,
                                                @number_of_bags,
                                                @received_by_logistics_user,
                                                NOW()
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, logisticIndustrial.so_reference);
                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, logisticIndustrial.customer_id);
                commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, logisticIndustrial.booking_reference);
                commonFunctions.BindParameter(oCommand, "@picked_up_by", System.Data.DbType.String, logisticIndustrial.picked_up_by);
                commonFunctions.BindParameter(oCommand, "@weight_in_kg", System.Data.DbType.Double, logisticIndustrial.weight_in_kg);
                commonFunctions.BindParameter(oCommand, "@number_of_bags", System.Data.DbType.Int32, logisticIndustrial.number_of_bags);
                commonFunctions.BindParameter(oCommand, "@received_by_logistics_user", System.Data.DbType.String, logisticIndustrial.received_by_logistics_user);

                int j = await oCommand.ExecuteNonQueryAsync();

                if (j > 0)
                {

                    bool hasData = false;
                    //Insert items -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`logistics_industrial_details`
                                    (
                                        `so_reference`, 
	                                    `item_code`, 
	                                    `item_count`, 
	                                    `adl_cost`
                                    )
                                    VALUES";

                    string values = "";

                    for (int i = 0; i < logisticIndustrial.industrialDetails.Count; i++)
                    {
                        var oItem = logisticIndustrial.industrialDetails[i];
                        hasData = true;
                        values = values + "(" + logisticIndustrial.so_reference.ToString() + ", " + oItem.item_code.ToString() + ", " + oItem.item_count.ToString() + ", " + oItem.adl_cost.ToString() + ")";

                        if (i < logisticIndustrial.industrialDetails.Count - 1)
                        {
                            values = values + ",";
                        }

                    }

                    if (hasData)
                    {

                        oCommand.CommandText = oCommand.CommandText + values;

                        await oCommand.ExecuteNonQueryAsync();

                        //update booking status
                        oCommand.CommandText = @"UPDATE  `laundry`.`pickups_industrial`
                                            SET
                                                	`received_by_logistics` = 1
                                         WHERE 
	                                         `so_reference` = @so_reference;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, logisticIndustrial.so_reference);

                        await oCommand.ExecuteNonQueryAsync();

                        await transaction.CommitAsync();

                        serviceResponse.SetValues(200, "Success", "");
                    }
                    else
                    {
                        await transaction.RollbackAsync();

                        serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                    }

                    //------------------------------------------------------------------------------                    
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
        public async Task<ServiceResponse> LaundryQueryReportByDate(string dateFrom, string dateTo, int customerID, LaundryStatus laundryStatus)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT * FROM (
                                            SELECT `so_reference`, 
	                                            i.`customer_id`, 
                                                c.`customer_name`,
	                                            `picked_up_datetime` as soDate, 
	                                            `weight_in_kg`, 
	                                            `number_of_bags`, 
	                                            `received_by_laundry_datetime`, 
	                                            `completed_by_laundry`,
                                                `received_from_laundry`,
                                                `delivered`
                                                
	 
	                                        FROM 
	                                            `laundry`.`logistics_industrial` i
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                            WHERE DATE(`received_by_laundry_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)
                                    
                                            UNION ALL
                                
                                            SELECT `so_reference`, 
	                                            i.`customer_id`, 
                                                c.`customer_name`,
	                                            `picked_up_datetime` as soDate, 
	                                            `weight_in_kg`, 
	                                            `number_of_bags`, 
	                                            COALESCE(`received_by_laundry_datetime`,'1900-01-01') AS received_by_laundry_datetime, 
	                                            `completed_by_laundry`,
                                                `received_from_laundry`,
                                                `delivered`
                                                
	 
	                                        FROM 
	                                            `laundry`.`logistics_industrial` i
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                            WHERE `received_by_laundry_datetime` IS NULL
                                        )xx WHERE 1=1";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `customer_id` = @customer_id";
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }

                if (laundryStatus == LaundryStatus.InProgress)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `completed_by_laundry` = 0";
                }
                else if (laundryStatus == LaundryStatus.Completed)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `completed_by_laundry` = 1 AND received_from_laundry = 0";
                }
                else if (laundryStatus == LaundryStatus.ForwardedToLogistics)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  `received_from_laundry` = 1";
                }
                else if (laundryStatus == LaundryStatus.ForReceive)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  DATE(`received_by_laundry_datetime`) = DATE('1900-01-01')";
                }
                else if (laundryStatus == LaundryStatus.Received)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  DATE(`received_by_laundry_datetime`) <> DATE('1900-01-01')";
                }

                oCommand.CommandText = oCommand.CommandText + " ORDER BY received_by_laundry_datetime";
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
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        oJSON.number_of_bags = oresult.GetInt32("number_of_bags");
                        oJSON.received_by_laundry_datetime = oresult.GetDateTime("received_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.completed_by_laundry = oresult.GetInt32("completed_by_laundry");
                        oJSON.received_from_laundry = oresult.GetInt32("received_from_laundry");
                        oJSON.delivered = oresult.GetInt32("delivered");

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
        public async Task<ServiceResponse> ReceivedItemByLaundrysAsync(int soReference, string laundryUser)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial`
                                            SET
                                                	`received_by_laundry_user` = @received_by_laundry_user, 
	                                                `received_by_laundry` = 1, 
	                                                `received_by_laundry_datetime` = NOW()
                                            WHERE
	                                                so_reference = @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);
                commonFunctions.BindParameter(oCommand, "@received_by_laundry_user", System.Data.DbType.String, laundryUser);

                int j = await oCommand.ExecuteNonQueryAsync();

                if (j > 0)
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
        public async Task<ServiceResponse> ReceivedCompletedItemQueryReportByDate(string dateFrom, string dateTo, int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `so_reference`, 
	                                            i.`customer_id`, 
                                                c.`customer_name`,
	                                            `picked_up_datetime` as soDate, 
	                                            `weight_in_kg`, 
	                                            `number_of_bags`, 
	                                            `delivered`, 
	                                            `received_from_laundry_user`,
                                                `received_from_laundry_datetime`,
                                                `with_invoice`                                               
	 
	                                FROM 
	                                    `laundry`.`logistics_industrial` i
	                                INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                    WHERE DATE(`received_from_laundry_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
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
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        oJSON.number_of_bags = oresult.GetInt32("number_of_bags");
                        oJSON.received_from_laundry_datetime = oresult.GetDateTime("received_from_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.received_from_laundry_user = oresult.GetString("received_from_laundry_user");
                        oJSON.delivered = oresult.GetInt32("delivered");
                        oJSON.with_invoice = oresult.GetInt32("with_invoice");

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
        public async Task<ServiceResponse> ReceivedCompletedItemByLogisticsAsync(int soReference, string logisticUser)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial`
                                            SET
                                                	`received_from_laundry_user` = @received_from_laundry_user, 
	                                                `received_from_laundry` = 1, 
	                                                `received_from_laundry_datetime` = NOW()
                                            WHERE
	                                                so_reference = @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);
                commonFunctions.BindParameter(oCommand, "@received_from_laundry_user", System.Data.DbType.String, logisticUser);

                int j = await oCommand.ExecuteNonQueryAsync();

                if (j > 0)
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
        public async Task<ServiceResponse> CompletedItemQueryReportByDate(string dateFrom, string dateTo, int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `so_reference`, 
	                                            i.`customer_id`, 
                                                c.`customer_name`,
	                                            `picked_up_datetime` as soDate, 
	                                            `weight_in_kg`, 
	                                            `number_of_bags`, 
	                                            `delivered`, 
	                                            `completed_by_laundry_user`,
                                                `completed_by_laundry_datetime`,
                                                `with_invoice`,
                                                CONACT(`street_building_address`, ' ',`town_address`,' ',`province`) AS address
	 
	                                FROM 
	                                    `laundry`.`logistics_industrial` i
	                                INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                    WHERE DATE(`completed_by_laundry_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
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
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        oJSON.number_of_bags = oresult.GetInt32("number_of_bags");
                        oJSON.completed_by_laundry_datetime = oresult.GetDateTime("completed_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.completed_by_laundry_user = oresult.GetString("completed_by_laundry_user");
                        oJSON.delivered = oresult.GetInt32("delivered");
                        oJSON.with_invoice = oresult.GetInt32("with_invoice");
                        oJSON.address = oresult.GetInt32("address");

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
        public async Task<ServiceResponse> CompletedItemByLogisticsAsync(int soReference, string logisticUser)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial`
                                            SET
                                                	`completed_by_laundry_user` = @completed_by_laundry_user, 
	                                                `completed_by_laundry` = 1, 
	                                                `completed_by_laundry_datetime` = NOW(),
                                                    `for_invoicing` = 1
                                            WHERE
	                                                so_reference = @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);
                commonFunctions.BindParameter(oCommand, "@completed_by_laundry_user", System.Data.DbType.String, logisticUser);

                int j = await oCommand.ExecuteNonQueryAsync();

                if (j > 0)
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
        public async Task<ServiceResponse> DeliveryQueryReportByDate(string dateFrom, string dateTo, int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT `so_reference`, 
	                                            i.`customer_id`, 
                                                c.`customer_name`,
	                                            `picked_up_datetime` as soDate, 
	                                            `weight_in_kg`, 
	                                            `number_of_bags`, 
	                                            `delivery_datetime`, 
	                                            `delivered_by`,
                                                `delivered`
                                                
	 
	                                FROM 
	                                    `laundry`.`logistics_industrial` i
	                                INNER JOIN `laundry`.`customers` c ON c.`customer_id` = i.`customer_id`
                                    WHERE DATE(`delivery_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
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
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        oJSON.number_of_bags = oresult.GetInt32("number_of_bags");
                        oJSON.delivery_datetime = oresult.GetDateTime("delivery_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.delivered_by = oresult.GetString("delivered_by");
                        oJSON.delivered = oresult.GetInt32("delivered");

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
        public async Task<ServiceResponse> DeliverItemByLogisticsAsync(int soReference, string logistecUser)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial`
                                            SET
                                                	`delivered_by` = @delivered_by, 
	                                                `delivered` = 1, 
	                                                `delivery_datetime` = NOW()
                                            WHERE
	                                                so_reference = @so_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);
                commonFunctions.BindParameter(oCommand, "@delivered_by", System.Data.DbType.String, logistecUser);

                int j = await oCommand.ExecuteNonQueryAsync();

                if (j > 0)
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
        public async Task<ServiceResponse> GetSOForInvoicing(bool isManualInvoicing)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                if (isManualInvoicing)
                {
                    oCommand.CommandText = @"SELECT s.`so_reference`, 
                                                SUM(`adl_cost` * `item_count`) AS cost, 
                                                c.`customer_name`
                                        FROM 
	                                        `laundry`.`logistics_industrial` s
                                        INNER JOIN 
	                                        `laundry`.`logistics_industrial_details` d ON s.`so_reference` = d.`so_reference`
                                        INNER JOIN 
	                                        `laundry`.`designated_laundry_items` i ON i.`laundry_item_id` = d.`item_code` AND i.`manual_costing` = 1 AND i.`customer_id` = s.`customer_id`
                                        INNER JOIN 
	                                        `laundry`.`customers` c ON c.`customer_id` = s.`customer_id`
                                        WHERE
	                                        s.`for_invoicing` = 1 AND `with_invoice` = 0
                                        GROUP BY s.`so_reference`, c.`customer_name`;";
                }
                else
                {
                    oCommand.CommandText = @"SELECT s.`so_reference`, 
                                                SUM(`adl_cost` * `item_count`) AS cost, 
                                                c.`customer_name`
                                        FROM 
	                                        `laundry`.`logistics_industrial` s
                                        INNER JOIN 
	                                        `laundry`.`logistics_industrial_details` d ON s.`so_reference` = d.`so_reference`
                                        INNER JOIN 
	                                        `laundry`.`designated_laundry_items` i ON i.`laundry_item_id` = d.`item_code` AND i.`manual_costing` = 0 AND i.`customer_id` = s.`customer_id`
                                        INNER JOIN 
	                                        `laundry`.`customers` c ON c.`customer_id` = s.`customer_id`
                                        WHERE
	                                        s.`for_invoicing` = 1 AND `with_invoice` = 0
                                        GROUP BY s.`so_reference`, c.`customer_name`;";
                }


                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObject = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic oJSON = new ExpandoObject();

                        hasValue = true;
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.cost = oresult.GetDouble("cost").ToString("#,##0.00");

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
        public async Task<ServiceResponse> GenerateInvoiceAsync(List<ForInvoice> soReferences, string generatedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();            

            MySqlTransaction transaction = null;

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                bool hasData = false;


                for (int i = 0; i < soReferences.Count; i++)
                {
                    hasData = true;

                    using var oCommand = gDb.Connection.CreateCommand();

                    int invoiceNumber = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.Invoice);

                    transaction = gDb.Connection.BeginTransaction();

                    oCommand.Transaction = transaction;

                    ForInvoice oSOReference = soReferences[i];
                    

                    hasData = true;

                    //Insert items -----------------------------------------------------------
                    oCommand.CommandText = @"INSERT INTO  `laundry`.`invoices`
                                (
                                    `invoice_reference`, 
	                                `invoice_datetime`, 
	                                `invoice_amount`, 
	                                `invoice_generated_by`
                                )
                                VALUES  
                                (
                                        @invoice_reference,
                                        NOW(),
                                        @amount,
                                        @invoice_generated_by
                                )";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@invoice_reference", System.Data.DbType.Int32, invoiceNumber);
                    commonFunctions.BindParameter(oCommand, "@amount", System.Data.DbType.Double, oSOReference.amount);
                    commonFunctions.BindParameter(oCommand, "@invoice_generated_by", System.Data.DbType.String, generatedBy);

                    int isSuccess = await oCommand.ExecuteNonQueryAsync();

                    if (isSuccess > 0)
                    {
                        oCommand.CommandText = @"INSERT INTO  `laundry`.`industrial_invoices`
                                (
                                    `invoice_reference`, 
	                                `so_reference`
                                )
                                VALUES
                                (
                                    @invoice_reference,
                                    @so_reference
                                )";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@invoice_reference", System.Data.DbType.Int32, invoiceNumber);
                        commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Double, oSOReference.soReference);

                        isSuccess = await oCommand.ExecuteNonQueryAsync();

                        if (isSuccess > 0)
                        {
                            oCommand.CommandText = @"UPDATE  `laundry`.`logistics_industrial`
                                                     SET
                                                        `for_invoicing` = 0,
                                                        `with_invoice` = 1
                                                    WHERE
                                                        `so_reference`= @so_reference;";

                            oCommand.Parameters.Clear();

                            commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Double, oSOReference.soReference);

                            isSuccess = await oCommand.ExecuteNonQueryAsync();

                            if (isSuccess > 0)
                            {
                                await transaction.CommitAsync();
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                            }

                        }
                    }

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
        public async Task<ServiceResponse> InvoiceQueryReportByDate(string dateFrom, string dateTo, int customerID, int isPaid, int isUnPaid, int isUnBilled)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT 	i.`invoice_reference`, 
	                                                `invoice_datetime`,
	                                                li.`so_reference`,
	                                                li.`picked_up_datetime` AS soDate,
	                                                c.`customer_name`,
                                                    c.customer_id,
	                                                `invoice_amount`, 
	                                                i.`paid`, 
	                                                COALESCE(i.`payment_reference`, '') AS payment_reference, 
	                                                `billed`, 
	                                                COALESCE(i.`billing_reference`, 0) AS billing_reference,
	                                                COALESCE(br.`billing_date`,'1900-01-01') as billing_date
	 
	                                FROM 
	                                `laundry`.`invoices` i
	                                INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
	                                LEFT JOIN `billing_references` br ON br.`billing_reference` = i.`billing_reference`
                                    WHERE DATE(`invoice_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) ";

                oCommand.Parameters.Clear();

                if (customerID != 0)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                }

                if (isPaid == 1 && isUnPaid == 1)
                {
                    //no filter on paid field
                }
                else
                {
                    if (isPaid == 1)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  i.`paid` = 1";
                    }
                    else if (isUnPaid == 1)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  i.`paid` = 0";
                    }
                }

                if (isUnBilled == 1)
                {
                    oCommand.CommandText = oCommand.CommandText + " AND  i.`billed` = 0";
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
                        oJSON.invoice_reference = oresult.GetInt32("invoice_reference");
                        oJSON.invoice_datetime = oresult.GetDateTime("invoice_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.invoice_amount = oresult.GetDouble("invoice_amount").ToString("#,##0.00");
                        oJSON.paid = oresult.GetInt32("paid");
                        oJSON.payment_reference = oresult.GetString("payment_reference");
                        oJSON.billed = oresult.GetInt32("billed");
                        oJSON.billing_reference = oresult.GetInt32("billing_reference");
                        if(oresult.GetDateTime("billing_date").Year != 1900)
                        {
                            oJSON.billing_date = oresult.GetDateTime("billing_date").ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            oJSON.billing_date = "";
                        }
                        

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
        public async Task<ServiceResponse> GetInvoiceDetails(int invoiceReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT 	i.`invoice_reference`, 
	                                                `invoice_datetime`,
	                                                li.`so_reference`,
	                                                li.`picked_up_datetime` AS soDate,
	                                                c.`customer_name`,
                                                    c.customer_id,
	                                                `invoice_amount`, 
	                                                `paid`, 
	                                                COALESCE(`payment_reference`, '') AS payment_reference, 
	                                                `billed`, 
	                                                COALESCE(`billing_reference`, 0) AS billing_reference,
                                                    i.`invoice_generated_by`
	 
	                                FROM 
	                                `laundry`.`invoices` i
	                                INNER JOIN `laundry`.`industrial_invoices` ii ON ii.`invoice_reference` = i.`invoice_reference`
	                                INNER JOIN `laundry`.`logistics_industrial` li ON li.`so_reference` = ii.`so_reference`
	                                INNER JOIN`laundry`.`customers` c ON c.`customer_id` = li.`customer_id`
                                    WHERE i.`invoice_reference` = @invoice_reference ";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@invoice_reference", System.Data.DbType.Int32, invoiceReference);


                var oresult = await oCommand.ExecuteReaderAsync();

                List<ExpandoObject> jsonObject = new List<ExpandoObject>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        dynamic oJSON = new ExpandoObject();
                        hasValue = true;
                        oJSON.invoice_reference = oresult.GetInt32("invoice_reference");
                        oJSON.invoice_datetime = oresult.GetDateTime("invoice_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.so_reference = oresult.GetInt32("so_reference");
                        oJSON.soDate = oresult.GetDateTime("soDate").ToString("yyyy-MM-dd HH:mm:ss");
                        oJSON.customer_id = oresult.GetInt32("customer_id");
                        oJSON.customer_name = oresult.GetString("customer_name");
                        oJSON.invoice_amount = oresult.GetDouble("invoice_amount").ToString("#,##0.00");
                        oJSON.paid = oresult.GetInt32("paid").ToString("#,##0.00");
                        oJSON.payment_reference = oresult.GetString("payment_reference");
                        oJSON.billed = oresult.GetInt32("billed");
                        oJSON.billing_reference = oresult.GetInt32("billing_reference");
                        oJSON.invoice_generated_by = oresult.GetString("invoice_generated_by");

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


    } 
}
