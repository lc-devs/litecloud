using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class LogisticSiteMethods
    {
        internal AppDb_WashALoad gDb { get; set; }
        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal LogisticSiteMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public LogisticSiteMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `code`, 
	                                        `site`
	 
	                                     FROM 
	                                        `laundry`.`logistics_sites`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<LogisticSite>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new LogisticSite();
                        hasValue = true;

                        oEntity.code = oresult.GetString("code");
                        oEntity.site = oresult.GetString("site");
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
                serviceResponse.SetValues(500, ex.Message, "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveSiteAsync(LogisticSite logisticSite)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `laundry`.`logistics_sites` 
	                                        (
                                                `code`, 
	                                            `site`
	                                        )
	                                     VALUES
                                            (
                                                @code, 
                                                @site
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, logisticSite.code);
                commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, logisticSite.site);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(logisticSite);

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

        public async Task<ServiceResponse> UpdateSiteAsync(LogisticSite logisticSite)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`logistics_sites` 
	                                    SET                                                
	                                        `site` = @site
	                                    WHERE
                                            `code` = @code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, logisticSite.code);
                commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, logisticSite.site);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(logisticSite);

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

        public async Task<ServiceResponse> DeleteSiteAsync(string code)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM `laundry`.`logistics_sites` 
	                                    WHERE
                                            `code` = @code
                                             AND `code` NOT IN (SELECT 	
	                                                                `site`	 
	                                                            FROM 
	                                                                `laundry`.`logistics_users`
                                                                WHERE
                                                                    `site` = @code
                                                                );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, code);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Site because it does not existed or it is assigned to the user.", "");
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
