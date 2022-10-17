using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class IndustrialServiceMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal IndustrialServiceMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public IndustrialServiceMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `id_service`, 
	                                            `description`
	 
	                                    FROM 
	                                        `laundry`.`industrial_services`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<IndustrialService>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new IndustrialService();
                        hasValue = true;

                        oEntity.id_service = oresult.GetInt32("id_service");
                        oEntity.description = oresult.GetString("description");

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

        public async Task<ServiceResponse> SaveServiceAsync(IndustrialService service)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `laundry`.`industrial_services` 
	                                        (
                                                `description`
	                                        )
	                                     VALUES
                                            (
                                                @description
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, service.description);

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
                serviceResponse.SetValues(500, ex.Message, "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateServiceAsync(IndustrialService service)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`industrial_services` 
	                                        SET
                                                `description` = @description
	                                        WHERE
	                                            id_service = @id_service;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@id_service", System.Data.DbType.Int32, service.id_service);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, service.description);

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
                oCommand.CommandText = @"DELETE FROM `laundry`.`industrial_services` 
	                                        WHERE
	                                            id_service = @id_service
                                                AND id_service NOT IN (
                                                                        SELECT 	`service`
                                                                        FROM 
	                                                                        `laundry`.`industrial_laundry_items`
                                                                        WHERE
                                                                            `service` = @id_service
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
