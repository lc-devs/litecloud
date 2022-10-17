using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class NonIndustrialServiceMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal NonIndustrialServiceMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public NonIndustrialServiceMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `id_service`, 
	                                            `description`, 
	                                            `manual_costing`, 
	                                            `unit_cost`	 
	                                    FROM 
	                                        `laundry`.`non_industrial_services`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<NonIndustrialService>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new NonIndustrialService();
                        hasValue = true;

                        oEntity.id_service = oresult.GetInt32("id_service");
                        oEntity.description = oresult.GetString("description");
                        oEntity.manual_costing = oresult.GetInt32("manual_costing");
                        oEntity.unit_cost = oresult.GetDouble("unit_cost");

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

        public async Task<ServiceResponse> SaveServiceAsync(NonIndustrialService service)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `laundry`.`non_industrial_services` 
	                                        (
                                                `description`, 
	                                            `manual_costing`, 
	                                            `unit_cost`	
	                                        )
	                                     VALUES
                                            (
                                                @description, 
	                                            @manual_costing, 
	                                            @unit_cost	
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, service.description);
                commonFunctions.BindParameter(oCommand, "@manual_costing", System.Data.DbType.Int32, service.manual_costing);

                if (service.manual_costing != 1)
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, service.unit_cost);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, 0.00);
                }


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    service.id_service = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(service);

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

        public async Task<ServiceResponse> UpdateServiceAsync(NonIndustrialService service)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`non_industrial_services` 
	                                        SET
                                                `description` = @description, 
	                                            `manual_costing` = @manual_costing, 
	                                            `unit_cost` = @unit_cost	
	                                        WHERE
	                                            id_service = @id_service;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_service", System.Data.DbType.Int32, service.id_service);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, service.description);
                commonFunctions.BindParameter(oCommand, "@manual_costing", System.Data.DbType.Int32, service.manual_costing);

                if (service.manual_costing != 1)
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, service.unit_cost);
                }
                else
                {
                    commonFunctions.BindParameter(oCommand, "@unit_cost", System.Data.DbType.Double, 0.00);
                }

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(service);

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

        public async Task<ServiceResponse> DeleteServiceAsync(int serviceID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM `laundry`.`non_industrial_services` 
	                                        WHERE
	                                            id_service = @id_service
                                                AND id_service NOT IN (
                                                                        SELECT 	`service_code`
                                                                        FROM 
	                                                                        `laundry`.`pickups_non_industrial_services`
                                                                        WHERE
                                                                            `service_code` = @id_service
                                                                        );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_service", System.Data.DbType.Int32, serviceID);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Service because it does not existed or it is being used.", "");
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
