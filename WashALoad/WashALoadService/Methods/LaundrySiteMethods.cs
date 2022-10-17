using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class LaundrySiteMethods
    {
        internal AppDb_WashALoad gDb { get; set; }
        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal LaundrySiteMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public LaundrySiteMethods() { }

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
	                                        `laundry`.`laundry_sites`;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<LaundrySite>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new LaundrySite();
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
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveSiteAsync(LaundrySite laundrySite)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `laundry`.`laundry_sites` 
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

                commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, laundrySite.code);
                commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, laundrySite.site);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(laundrySite);

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

        public async Task<ServiceResponse> UpdateSiteAsync(LaundrySite laundrySite)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`laundry_sites` 
	                                    SET                                                
	                                        `site` = @site
	                                    WHERE
                                            `code` = @code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@code", System.Data.DbType.String, laundrySite.code);
                commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, laundrySite.site);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(laundrySite);

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
                oCommand.CommandText = @"DELETE FROM `laundry`.`laundry_sites` 
	                                    WHERE
                                            `code` = @code
                                             AND `code` NOT IN (
                                                                    SELECT 	
	                                                                    `site`	 
	                                                                FROM 
	                                                                    `laundry`.`laundry_users`
                                                                    WHERE `site` = @code
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
