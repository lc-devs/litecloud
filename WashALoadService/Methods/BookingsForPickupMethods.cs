using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class BookingsForPickupMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal BookingsForPickupMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public BookingsForPickupMethods() { }

        public async Task<ServiceResponse> FindCustomersBookingAsync(int customerID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `booking_reference`, 
	                                            `booking_datetime`,
                                                p.`customer_id`,
	                                            COALESCE(`scheduled_pickup_datetime`, '1900-01-01') as scheduled_pickup_datetime, 
	                                            COALESCE(`so_reference`, '') as so_reference,
                                                COALESCE(`picked_up_by`, '') AS `picked_up_by`,
                                                p.cancelled_booking,
                                                p.`industrial`,
                                                 (CASE WHEN c.`industrial` = 1 AND c.`non_industrial` = 1 
	                                            THEN 'Industrial/Non-Idustrial' 
	                                            ELSE 
		                                            CASE WHEN c.`industrial` = 1 THEN 'Industrial' 
		                                            ELSE
			                                            CASE WHEN c.`non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                            END
	                                            END)	AS `type`,
                                                 c.`customer_name`
	 
	                                    FROM 
	                                            `laundry`.`bookings_for_pickup` p
                                         INNER JOIN `laundry`.`customers` c ON c.`customer_id` = p.`customer_id`
	                                    WHERE 
	                                           p.`customer_id` = @customer_id ORDER BY booking_datetime DESC;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, customerID);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<BookingsForPickup>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new BookingsForPickup();
                        hasValue = true;

                        oEntity.booking_reference = oresult.GetString("booking_reference");
                        oEntity.booking_datetime = oresult.GetDateTime("booking_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.so_reference = oresult.GetString("so_reference");
                         oEntity.picked_up_by = oresult.GetString("picked_up_by");
                        oEntity.cancelled_booking = oresult.GetInt32("cancelled_booking");
                        oEntity.customer_id = oresult.GetInt32("customer_id");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.customer_type = oresult.GetString("type");
                        oEntity.scheduled_pickup_datetime = oresult.GetDateTime("scheduled_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");

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

        public async Task<ServiceResponse> FindCustomersBookingByBookingDateAsync(string bookingDate)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `booking_reference`, 
	                                            `booking_datetime`, 
	                                            COALESCE(`scheduled_pickup_datetime`, '1900-01-01') as scheduled_pickup_datetime, 
	                                            COALESCE(`so_reference`, '') as so_reference,
                                                cancelled_booking,
                                                p.`industrial`,
                                                 (CASE WHEN c.`industrial` = 1 AND c.`non_industrial` = 1 
	                                            THEN 'Industrial/Non-Idustrial' 
	                                            ELSE 
		                                            CASE WHEN c.`industrial` = 1 THEN 'Industrial' 
		                                            ELSE
			                                            CASE WHEN c.`non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                            END
	                                            END)	AS `type`,
                                                c.`customer_name`,
                                                c.`customer_id`,
                                                COALESCE(`latitude`, 10.31672) AS latitude ,
                                                COALESCE(`longitude`, 123.89071) AS longitude,
                                                CONCAT(`street_building_address`, ' ', `barangay_address`, ' ', `town_address`, ' ', `province`) AS address
	 
	                                    FROM 
	                                            `laundry`.`bookings_for_pickup` p
                                         INNER JOIN `laundry`.`customers` c ON c.`customer_id` = p.`customer_id`
	                                    WHERE 
	                                           DATE( `booking_datetime`) <= DATE(@booking_datetime) AND `picked_up_by` IS NULL ORDER BY booking_datetime DESC;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@booking_datetime", System.Data.DbType.String, bookingDate);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<BookingsForPickup>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new BookingsForPickup();
                        hasValue = true;

                        oEntity.booking_reference = oresult.GetString("booking_reference");
                        oEntity.booking_datetime = oresult.GetDateTime("booking_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.so_reference = oresult.GetString("so_reference");
                        oEntity.cancelled_booking = oresult.GetInt32("cancelled_booking");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.latitude = oresult.GetFloat("latitude");
                        oEntity.longitude = oresult.GetFloat("longitude");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.customer_type = oresult.GetString("type");
                        oEntity.scheduled_pickup_datetime = oresult.GetDateTime("scheduled_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.address = oresult.GetString("address");
                        oEntity.customer_id = oresult.GetInt32("customer_id");

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

        public async Task<ServiceResponse> FindPickUpByUserAsync(int userID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `booking_reference`, 
	                                            `booking_datetime`, 
	                                            COALESCE(`scheduled_pickup_datetime`, '1900-01-01') as scheduled_pickup_datetime, 
	                                            COALESCE(`so_reference`, '') as so_reference,
                                                (CASE WHEN c.`industrial` = 1 AND c.`non_industrial` = 1 
	                                            THEN 'Industrial/Non-Idustrial' 
	                                            ELSE 
		                                            CASE WHEN c.`industrial` = 1 THEN 'Industrial' 
		                                            ELSE
			                                            CASE WHEN c.`non_industrial` = 1 THEN 'Non-Idustrial' ELSE 'N/A' END
		                                            END
	                                            END)	AS `type`,
                                                 c.`customer_name`,
                                                p.cancelled_booking,
                                                p.`industrial`
	 
	                                    FROM 
	                                            `laundry`.`bookings_for_pickup` p
                                         INNER JOIN `laundry`.`customers` c ON c.`customer_id` = p.`customer_id`
	                                    WHERE 
	                                          p.`customer_id` = @customer_id ORDER BY booking_datetime DESC;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.String, userID);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<BookingsForPickup>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new BookingsForPickup();
                        hasValue = true;

                        oEntity.booking_reference = oresult.GetString("booking_reference");
                        oEntity.booking_datetime = oresult.GetDateTime("booking_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.so_reference = oresult.GetString("so_reference");
                        oEntity.cancelled_booking = oresult.GetInt32("cancelled_booking");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.customer_name = oresult.GetString("customer_name");
                        oEntity.customer_type = oresult.GetString("type");
                        oEntity.scheduled_pickup_datetime = oresult.GetDateTime("scheduled_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");

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

        public async Task<ServiceResponse> FindCustomersBookingByBookingReferenceAsync(string bookingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `booking_reference`, 
	                                            `booking_datetime`, 
	                                            COALESCE(`scheduled_pickup_datetime`, '1900-01-01') as scheduled_pickup_datetime, 
	                                            COALESCE(`so_reference`, '') as so_reference,
                                                cancelled_booking,
                                                `customer_id`,
                                                COALESCE(`picked_up_by`, '') AS `picked_up_by`,
                                                COALESCE(`actual_picked_up_datetime`, '1900-01-01') AS `actual_picked_up_datetime`,
                                                `industrial`,
                                                COALESCE((SELECT `wkg_per_load` FROM `laundry`.`system_parameters` LIMIT 1), 1) as weight_per_load
	 
	                                    FROM 
	                                            `laundry`.`bookings_for_pickup`
	                                    WHERE 
	                                           `booking_reference` = @booking_reference;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, bookingReference);
                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<BookingsForPickup>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new BookingsForPickup();
                        hasValue = true;

                        oEntity.booking_reference = oresult.GetString("booking_reference");
                        oEntity.booking_datetime = oresult.GetDateTime("booking_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                        oEntity.so_reference = oresult.GetString("so_reference");
                        oEntity.cancelled_booking = oresult.GetInt32("cancelled_booking");
                        oEntity.customer_id = oresult.GetInt32("customer_id");
                        oEntity.industrial = oresult.GetInt32("industrial");
                        oEntity.picked_up_by = oresult.GetString("picked_up_by");
                        oEntity.actual_picked_up_datetime = oresult.GetDateTime("actual_picked_up_datetime").ToString("yyyy-MM-dd HH:mm:ss");
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

        public async Task<ServiceResponse> FindBookingDetailsAsync(string bookingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();


                serviceResponse = await FindCustomersBookingByBookingReferenceAsync(bookingReference);

                if(serviceResponse.code != 200)
                {
                    return serviceResponse;
                }
                List<BookingsForPickup> bookingInfos = JsonSerializer.Deserialize<List<BookingsForPickup>>(serviceResponse.jsonData);

                BookingsForPickup bookingInfo = bookingInfos[0];

                BookingsForPickupDetails bookingsForPickupDetails = new BookingsForPickupDetails();

                bookingsForPickupDetails.booking_reference = bookingReference;

               if (bookingInfo.so_reference.Equals(""))
                {
                    bookingsForPickupDetails.booking = bookingInfo.booking_datetime;
                }
                else
                {
                    oCommand.CommandText = @"SELECT COALESCE(`actual_picked_up_datetime`, '1900-01-01') AS picked_up_datetime, 
	                                                COALESCE(`received_from_pickup_datetime`, '1900-01-01') AS received_from_pickup_datetime, 
	                                                COALESCE(`received_by_laundry_datetime`, '1900-01-01') AS received_by_laundry_datetime, 
	                                                COALESCE(`completed_by_laundry_datetime`, '1900-01-01') AS completed_by_laundry_datetime, 
	                                                COALESCE(`received_from_laundry_datetime`, '1900-01-01') AS received_from_laundry_datetime, 
	                                                COALESCE(`delivery_datetime`, '1900-01-01') AS delivery_datetime
	 
	                                           FROM `laundry`.`bookings_for_pickup` b ";
                    if(bookingInfo.industrial == 1)
                    {
                        oCommand.CommandText = oCommand.CommandText + @" LEFT JOIN  `laundry`.`logistics_industrial` l ON l.`so_reference`  = b.`so_reference` ";
                    }
                    else
                    {
                        oCommand.CommandText = oCommand.CommandText + @" LEFT JOIN  `laundry`.`logistics_non_industrial`  l ON l.`so_reference`  = b.`so_reference`";
                    }

                    oCommand.CommandText = oCommand.CommandText + @" 
                                            WHERE b.`so_reference` = @so_reference;";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@so_reference", System.Data.DbType.String, bookingInfo.so_reference);

                    var oresult = await oCommand.ExecuteReaderAsync();
                       
                    using (oresult)
                    {
                        while (await oresult.ReadAsync())
                        {
                            if(oresult.GetDateTime("picked_up_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.picked_up = oresult.GetDateTime("picked_up_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (oresult.GetDateTime("received_from_pickup_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.received_by_logistics = oresult.GetDateTime("received_from_pickup_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (oresult.GetDateTime("received_by_laundry_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.forwarded_to_laundry = oresult.GetDateTime("received_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (oresult.GetDateTime("completed_by_laundry_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.done_laundry = oresult.GetDateTime("completed_by_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (oresult.GetDateTime("received_from_laundry_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.forwarded_to_logistics = oresult.GetDateTime("received_from_laundry_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (oresult.GetDateTime("delivery_datetime").Year != 1900)
                            {
                                bookingsForPickupDetails.delivered = oresult.GetDateTime("delivery_datetime").ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    }
                }

                string jsonString = JsonSerializer.Serialize(bookingsForPickupDetails);

                serviceResponse.SetValues(200, "Success", jsonString);                

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse> SaveBookingAsync(BookingsForPickup bookingsForPickup)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                int series = await commonFunctions.GenerateTransactionNumber(oCommand, CommonFunctions.TransactionType.Booking);

                string strSeries = series.ToString().PadLeft(8, '0');


                oCommand.CommandText = @"INSERT INTO  `laundry`.`bookings_for_pickup`
                                            (
                                                	`booking_reference`, 
	                                                `booking_datetime`, 
	                                                `scheduled_pickup_datetime`,
                                                    `customer_id`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @booking_reference,
                                                NOW(),
                                                @scheduled_pickup_datetime,
                                                @customer_id
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, strSeries);
                commonFunctions.BindParameter(oCommand, "@scheduled_pickup_datetime", System.Data.DbType.DateTime, bookingsForPickup.scheduled_pickup_datetime);
                commonFunctions.BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, bookingsForPickup.customer_id);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    bookingsForPickup.booking_reference = strSeries;

                    string jsonString = JsonSerializer.Serialize(bookingsForPickup);

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

        public async Task<ServiceResponse> CancelBookingAsync(string bookingReference)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"UPDATE  `laundry`.`bookings_for_pickup`
                                            SET
                                                	`cancelled_booking` = 1 
                                         WHERE 
	                                         `booking_reference` = @booking_reference AND `cancelled_booking` = 0 AND (`picked_up_by` IS NULL);";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@booking_reference", System.Data.DbType.String, bookingReference);
                
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
