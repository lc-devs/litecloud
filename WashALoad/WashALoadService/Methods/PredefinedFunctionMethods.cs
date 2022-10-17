using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Models;
using WashALoadService.Common;

namespace WashALoadService.Methods
{
    public class PredefinedFunctionMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal PredefinedFunctionMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public PredefinedFunctionMethods() { }

        public async Task<ServiceResponse> FindAllBySectionAsync(int section)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `function_code`, 
	                                        f.`description`, 
	                                        f.`section`,
	                                        s.description AS section_description, 
	                                        s.`description`,
	                                        COALESCE(mg.`description`,'') AS `menu_group`
                                            
	 
	                                     FROM 
	                                        `laundry`.`predefined_functions`f
					                     INNER JOIN `laundry`.`sections`s ON s.`idsection` = f.`section`
                                         LEFT JOIN `laundry`.`menu_group` mg ON mg.`idmenu_group` = f.`menu_group_id`
	                                     WHERE 
	                                        section = @section;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@section", System.Data.DbType.Int32, section);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<PredefinedFunction>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new PredefinedFunction();
                        hasValue = true;

                        oEntity.description = oresult.GetString("description");
                        oEntity.function_code = oresult.GetString("function_code");

                        oEntity.section = new Section();
                        oEntity.section.idsection = oresult.GetInt32("section");
                        oEntity.section.description = oresult.GetString("section_description");
                        oEntity.menu_group = oresult.GetString("menu_group");

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
