using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Models;
using WashALoadService.Common;

namespace WashALoadService.Methods
{
    public class SectionMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        internal SectionMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public SectionMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `idsection`, 
	                                        `description`
	 
	                                     FROM 
	                                        `laundry`.`sections` 
	 
	                                     WHERE 
	                                        section = @section;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Section>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Section();
                        hasValue = true;

                        oEntity.description = oresult.GetString("description");
                        oEntity.idsection = oresult.GetInt32("idsection");
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
    }
}
