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

namespace WashALoadService.Methods
{
    public class PickupNonIndustrialMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal PickupNonIndustrialMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public PickupNonIndustrialMethods() { }

        public async Task<ServiceResponse> PickupQueryReportByDate(string dateFrom, string dateTo, int customerID, string pickupBy, bool isAllEntries, bool isForPickupOnly)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");



                using var oCommand = gDb.Connection.CreateCommand();

                if (isForPickupOnly)
                {
                    oCommand.CommandText = oCommand.CommandText + @"
                                    SELECT 	0 as `so_reference`,
	                                    b.`customer_id`, 
	                                    c.`customer_name`,
	                                    COALESCE(`booking_reference`, '') AS booking_reference, 
	                                    '' as `picked_up_by`, 
	                                    '1900-01-01' as`actual_picked_up_datetime`,
	                                    0.0 as `weight_in_kg`, 
	                                    0 as `number_of_bags`, 
	                                    0 as `received_by_logistics`,
	                                    '' as `type`,
                                        COALESCE(`latitude`, 10.31672) AS latitude ,
                                        COALESCE(`longitude`, 123.89071) AS longitude
	 
	                                FROM 
	                                    `laundry`.`bookings_for_pickup` b
	                                INNER JOIN `laundry`.`customers` c ON c.`customer_id` = b.`customer_id`

                                    WHERE DATE(`booking_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND picked_up_by IS NULL";

                    oCommand.Parameters.Clear();

                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                    }

                    commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                    commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);
                }
                else
                {
                    oCommand.CommandText = @"SELECT	`so_reference`, 
	                                        p.`customer_id`,
	                                        c.`customer_name`,
	                                        COALESCE(`booking_reference`, '') AS booking_reference, 
	                                        `picked_up_by`, 
	                                        `picked_up_datetime` as actual_picked_up_datetime, 
	                                        `weight_in_kg`, 
	                                        `number_of_bags`, 
	                                        `received_by_logistics`,
	                                        'Non-Industrial' as `type`,
                                            COALESCE(`latitude`, 10.31672) AS latitude ,
                                            COALESCE(`longitude`, 123.89071) AS longitude
	 
	                                        FROM 
	                                        `laundry`.`pickups_non_industrial` p
	                                        INNER JOIN `laundry`.`customers` c ON c.`customer_id` = p.`customer_id` AND picked_up_by IS NOT NULL
                                            WHERE DATE(`picked_up_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo)";
                                            
                    oCommand.Parameters.Clear();

                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    }
                    if (pickupBy.Equals(""))
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  picked_up_by = @picked_up_by";
                    }

                    if (isAllEntries)
                    {
                        oCommand.CommandText = oCommand.CommandText + @"
                                    UNION ALL

                                    SELECT 	0 as `so_reference`,
	                                    b.`customer_id`, 
	                                    c.`customer_name`,
	                                    COALESCE(`booking_reference`, '') AS booking_reference, 
	                                    '' as `picked_up_by`, 
	                                    '1900-01-01' as`actual_picked_up_datetime`,
	                                    0.0 as `weight_in_kg`, 
	                                    0 as `number_of_bags`, 
	                                    0 as `received_by_logistics`,
	                                    '' as `type`,
                                        COALESCE(`latitude`, 10.31672) AS latitude ,
                                        COALESCE(`longitude`, 123.89071) AS longitude
	 
	                                FROM 
	                                    `laundry`.`bookings_for_pickup` b
	                                INNER JOIN `laundry`.`customers` c ON c.`customer_id` = b.`customer_id`

                                    WHERE b.`industrial` = 0 AND DATE(`booking_datetime`) BETWEEN DATE(@dateFrom) AND DATE(@dateTo) AND picked_up_by IS NULL";
                    }

                    if (customerID != 0)
                    {
                        oCommand.CommandText = oCommand.CommandText + " AND  c.`customer_id` = @customer_id";
                    }

                    if (customerID != 0)
                    {
                        commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                    }
                    if (pickupBy.Equals(""))
                    {
                        commonFunctions.BindParameter(oCommand, "@picked_up_by", System.Data.DbType.String, pickupBy);
                    }

                    commonFunctions.BindParameter(oCommand, "@dateFrom", System.Data.DbType.String, dateFrom);
                    commonFunctions.BindParameter(oCommand, "@dateTo", System.Data.DbType.String, dateTo);
                }


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
                        jsonObject.booking_reference = oresult.GetString("booking_reference");
                        jsonObject.picked_up_by = oresult.GetString("picked_up_by");
                        jsonObject.picked_up_datetime = oresult.GetDateTime("actual_picked_up_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        jsonObject.weight_in_kg = oresult.GetDouble("weight_in_kg");
                        jsonObject.number_of_bags = oresult.GetInt32("number_of_bags");
                        jsonObject.received_by_logistics = oresult.GetInt32("received_by_logistics");
                        jsonObject.type = oresult.GetString("type");
                        jsonObject.latitude = oresult.GetFloat("latitude");
                        jsonObject.longitude = oresult.GetFloat("longitude");
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

        public async Task<ServiceResponse> GetPickUpBySOReference(int soReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                
                oCommand.CommandText = @"SELECT	`so_reference`, 
	                                    p.`customer_id`,
	                                    c.`customer_name`,
	                                    COALESCE(`booking_reference`, '') AS booking_reference, 
	                                    `picked_up_by`, 
	                                    `picked_up_datetime`, 
	                                    `weight_in_kg`,
                                        COALESCE((SELECT `wkg_per_load` FROM `laundry`.`system_parameters` LIMIT 1), 1) as weight_per_load,
	                                    `number_of_bags`, 
	                                    `received_by_logistics`,
	                                    'Non-Industrial' as `type`
	 
	                                    FROM 
	                                    `laundry`.`pickups_non_industrial` p
	                                    INNER JOIN `laundry`.`customers` c ON c.`customer_id` = p.`customer_id`
                                        WHERE `so_reference` = @so_reference";

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
                        jsonObject.received_by_logistics = oresult.GetInt32("received_by_logistics");

                        double noOfLoads = oresult.GetDouble("weight_in_kg") / oresult.GetInt32("weight_per_load");

                        if (noOfLoads <= 1.0)
                        {
                            jsonObject.number_of_loads = 1.0;
                        }
                        else
                        {
                            jsonObject.number_of_loads = Convert.ToInt32(noOfLoads);
                        }

                        jsonObject.so_reference_QR_Image = commonFunctions.GenerateQR(Convert.ToString(oresult.GetInt32("so_reference")));

                        jsonObject.type = oresult.GetString("type");
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
	                                                (CASE WHEN COALESCE(ii.`description`,'') = '' THEN li.`description` ELSE ii.`description` END) AS  item_description,
	                                                `item_count`
	 
	                                        FROM 
	                                        `laundry`.`pickups_non_industrial_items` ii
	                                        INNER JOIN `laundry`.`non_industrial_laundry_items` li ON li.`id_item` = ii.`item_code`
                                            WHERE `so_reference` = @so_reference
                                            ORDER BY item_description;";

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
                            jsonItem.item_count = oresult.GetInt32("item_count");
                            jsonItems.Add(jsonItem);

                        }
                    }

                    if (hasItems)
                    {
                        jsonObject.items = jsonItems;
                    }

                    //get services
                    oCommand.CommandText = @"SELECT `so_reference`, 
	                                                `service_code`, 
	                                                `description`,
                                                    `unit_cost`
	 
	                                                FROM 
	                                                `laundry`.`pickups_non_industrial_services`  nis
	                                                INNER JOIN `laundry`.`non_industrial_services` s ON s.`id_service` = `service_code`
                                            WHERE `so_reference` = @so_reference
                                            ORDER BY description;";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, soReference);

                    oresult = await oCommand.ExecuteReaderAsync();

                    List<ExpandoObject> jsonServices = new List<ExpandoObject>();

                    bool hasServices = false;
                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            hasServices = true;

                            dynamic jsonService = new ExpandoObject();
                            hasValue = true;
                            jsonService.so_reference = oresult.GetInt32("so_reference");
                            jsonService.service_code = oresult.GetInt32("service_code");
                            jsonService.description = oresult.GetString("description");
                            jsonService.unit_cost = oresult.GetDouble("unit_cost").ToString("#,##0.00");
                            jsonServices.Add(jsonService);

                        }
                    }

                    if (hasServices)
                    {
                        jsonObject.services = jsonServices;
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
        public async Task<ServiceResponse> SavePickupItemsAsync(PickupNonIndustrial pickupNonIndustrial)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            using var oCommand = gDb.Connection.CreateCommand();
            MySqlTransaction transaction;

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                if (pickupNonIndustrial.pickupNonIndustrialDetail.Count > 0)
                {
                    int series = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.SO);

                    transaction = gDb.Connection.BeginTransaction();

                    oCommand.Transaction = transaction;

                    oCommand.CommandText = @"INSERT INTO  `laundry`.`pickups_non_industrial`
                                            (
                                                	`so_reference`, 
	                                                `customer_id`, 
	                                                `booking_reference`, 
	                                                `picked_up_by`, 
	                                                `picked_up_datetime`, 
	                                                `weight_in_kg`, 
	                                                `number_of_bags`,
                                                    `number_of_loads`
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
                                                @weight_in_kg / COALESCE((SELECT `wkg_per_load` FROM `laundry`.`system_parameters` LIMIT 1), 1)
                                             );";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, series);
                    commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, pickupNonIndustrial.customer_id);

                    if (pickupNonIndustrial.booking_reference.Equals(""))
                    {
                        commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, DBNull.Value);
                    }
                    else
                    {
                        commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, pickupNonIndustrial.booking_reference);
                    }

                    commonFunctions.BindParameter(oCommand, "@picked_up_by", System.Data.DbType.String, pickupNonIndustrial.picked_up_by);
                    commonFunctions.BindParameter(oCommand, "@weight_in_kg", System.Data.DbType.Double, pickupNonIndustrial.weight_in_kg);
                    commonFunctions.BindParameter(oCommand, "@number_of_bags", System.Data.DbType.Int32, pickupNonIndustrial.number_of_bags);

                    int j = await oCommand.ExecuteNonQueryAsync();

                    if (j > 0)
                    {

                        bool hasData = false;
                        //Insert items -----------------------------------------------------------
                        oCommand.CommandText = @"INSERT INTO  `laundry`.`pickups_non_industrial_items`
                                    (
                                        `so_reference`, 
	                                    `item_code`, 
	                                    `item_count`,
                                        `description`
                                    )
                                    VALUES";

                        string values = "";

                        for (int i = 0; i < pickupNonIndustrial.pickupNonIndustrialDetail.Count; i++)
                        {
                            var oItem = pickupNonIndustrial.pickupNonIndustrialDetail[i];
                            hasData = true;
                            values = values + "(" + series.ToString() + ", " + oItem.item_code.ToString() + ", " + oItem.item_count.ToString() + ", '" + oItem.description + "')";

                            if (i < pickupNonIndustrial.pickupNonIndustrialDetail.Count - 1)
                            {
                                values = values + ",";
                            }

                        }

                        if (hasData)
                        {

                            oCommand.CommandText = oCommand.CommandText + values;

                            await oCommand.ExecuteNonQueryAsync();

                            hasData = false;
                            //insert services -----------------------------------------------------------
                            oCommand.CommandText = @"INSERT INTO  `laundry`.`pickups_non_industrial_services`
                                    (
                                        `so_reference`, 
	                                    `service_code`
                                    )
                                    VALUES";

                            values = "";

                            for (int i = 0; i < pickupNonIndustrial.services.Count; i++)
                            {
                                var oService = pickupNonIndustrial.services[i];
                                hasData = true;
                                values = values + "(" + series.ToString() + ", " + oService.service_code.ToString() + ")";

                                if (i < pickupNonIndustrial.services.Count - 1)
                                {
                                    values = values + ",";
                                }

                            }

                            if (hasData)
                            {
                                oCommand.CommandText = oCommand.CommandText + values;
                                await oCommand.ExecuteNonQueryAsync();
                            }

                            if (pickupNonIndustrial.booking_reference.Equals("") == false)
                            {
                                //update booking status
                                oCommand.CommandText = @"UPDATE  `laundry`.`bookings_for_pickup`
                                        SET
                                                `actual_picked_up_datetime` = NOW(),
                                                `picked_up_by` = @picked_up_by,
                                                `so_reference` = @so_reference,
                                                `industrial` = 0
                                        WHERE 
	                                        `booking_reference` = @booking_reference;";

                                oCommand.Parameters.Clear();

                                commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, pickupNonIndustrial.booking_reference);
                                commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.Int32, series);
                                commonFunctions.BindParameter(oCommand, "@picked_up_by", System.Data.DbType.String, pickupNonIndustrial.picked_up_by);

                                await oCommand.ExecuteNonQueryAsync();
                            }

                            await transaction.CommitAsync();

                            pickupNonIndustrial.so_reference = series;

                            pickupNonIndustrial.so_reference_QR_Image = commonFunctions.GenerateQR(Convert.ToString(series));

                            string jsonString = JsonSerializer.Serialize(pickupNonIndustrial);

                            serviceResponse.SetValues(200, "Success", jsonString);
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
                else
                {
                    serviceResponse.SetValues(500, "Pick up requires items.", "");
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
