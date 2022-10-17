using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class AccessMenuTemplateMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal AccessMenuTemplateMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public AccessMenuTemplateMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `template_code`, 
	                                        t.`description`, 
	                                        t.`section`,
                                            s.`description` as section_description
	 
	                                    FROM 
	                                        `laundry`.`access_menu_template` t
                                        INNER JOIN `laundry`.`sections` s ON t.section = s.idsection;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<AccessMenuTemplate>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new AccessMenuTemplate();
                        hasValue = true;

                        oEntity.description  = oresult.GetString("description");
                        oEntity.template_code = oresult.GetString("template_code");

                        oEntity.section = new Section();
                        oEntity.section.idsection = oresult.GetInt32("section");
                        oEntity.section.description = oresult.GetString("section_description");

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

        public async Task<ServiceResponse> FindTemplateDetailsAsync(string template_code)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT COALESCE(t.`function_code`, '') AS function_code,
	                                            f.`description`, 
	                                            f.`section`,
	                                           COALESCE(mg.`description`, '') AS `menu_group`,
                                                COALESCE(f.`page_path_filename`, '') AS page_path_filename,
                                                s.`description` AS section_description,
                                                `css_icon`, 
                                                (
							                    SELECT COUNT(ctr.section) 
							                    FROM  `laundry`.`predefined_functions` ctr 
							                    INNER JOIN `laundry`.`access_menu_template_details` t2 ON ctr.`function_code` = t2.`function_code` 
								                    AND t2.template_code = @template_code
							                    WHERE ctr.`menu_group_id` = mg.`idmenu_group` AND ctr.`section` = f.`section`) AS ttlsubmenu	 	 
                                        FROM 
	                                        `laundry`.`predefined_functions` f
                                        INNER JOIN 
	                                        `laundry`.`access_menu_template_details` t ON f.`function_code` = t.`function_code`
                                        INNER JOIN `laundry`.`sections` s ON f.section = s.idsection
                                        LEFT JOIN `laundry`.`menu_group` mg ON mg.`idmenu_group` = f.`menu_group_id`
                                        WHERE t.template_code = @template_code
                                        ORDER BY f.`section`, menu_group, function_code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, template_code);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<PredefinedFunction>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new PredefinedFunction();
                        hasValue = true;

                        oEntity.function_code = oresult.GetString("function_code");
                        oEntity.description = oresult.GetString("description");

                        oEntity.section = new Section();
                        oEntity.section.idsection = oresult.GetInt32("section");
                        oEntity.section.description = oresult.GetString("section_description");

                        oEntity.menu_group = oresult.GetString("menu_group");
                        oEntity.page_path_filename = oresult.GetString("page_path_filename");
                        oEntity.css_icon = oresult.GetString("css_icon");
                        oEntity.ttlsubmenu = oresult.GetInt32("ttlsubmenu");

                        oEntities.Add(oEntity);
                    }
                }

                if (hasValue == false)
                    serviceResponse.SetValues(404, "No data found", "");

                string jsonString = JsonSerializer.Serialize(oEntities);

                serviceResponse.SetValues(200, "Success", jsonString);

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveTemplateAsync(AccessMenuTemplate template)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `laundry`.`access_menu_template` 
	                                        (
                                                `template_code`, 
	                                            `description`, 
	                                            `section`
	                                        )
	                                     VALUES
                                            (
                                                @template_code, 
                                                @description, 
                                                @section
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, template.template_code);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, template.description);
                commonFunctions.BindParameter(oCommand, "@section", System.Data.DbType.Int32, template.section.idsection);


                int i = await oCommand.ExecuteNonQueryAsync();

                if(i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(template);

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

        public async Task<ServiceResponse> UpdateTemplateAsync(AccessMenuTemplate template)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `laundry`.`access_menu_template` 
	                                        SET
	                                            `description` = @description, 
	                                            `section` =  @section
	                                     WHERE
                                            `template_code` = @template_code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, template.template_code);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, template.description);
                commonFunctions.BindParameter(oCommand, "@section", System.Data.DbType.Int32, template.section.idsection);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(template);

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

        public async Task<ServiceResponse> DeleteTemplateAsync(string template_code)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"DELETE FROM `laundry`.`access_menu_template_details`
	                                     WHERE
                                              `template_code` =  @template_code
                                               AND `template_code` NOT IN (SELECT 	
	                                                                        `menu_template`	 
	                                                                    FROM 
	                                                                        `laundry`.`system_users`
                                                                        WHERE
                                                                             `menu_template` = @template_code
                                                                        );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, template_code);

                await oCommand.ExecuteNonQueryAsync();


                oCommand.CommandText = @"DELETE FROM `laundry`.`access_menu_template` 
	                                     WHERE
                                            `template_code` = @template_code
                                            AND `template_code` NOT IN (SELECT 	
	                                                                        `menu_template`	 
	                                                                    FROM 
	                                                                        `laundry`.`system_users`
                                                                        WHERE
                                                                             `menu_template` = @template_code
                                                                        );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, template_code);

                int i = await oCommand.ExecuteNonQueryAsync();

                if(i >= 1)
                {
                   serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Access Template because it does not existed or it is assigned to the system user.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveTemplateDetailsAsync(List<AccessMenuTemplateDetails> templateDetails)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                if(templateDetails.Count > 0)
                {
                    using var oCommand = gDb.Connection.CreateCommand();

                    oCommand.CommandText = @"DELETE FROM `laundry`.`access_menu_template_details` 
	                                     WHERE
                                              `template_code` =  @template_code;";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@template_code", System.Data.DbType.String, templateDetails[0].oTemplate.template_code);

                    await oCommand.ExecuteNonQueryAsync();


                    oCommand.CommandText = @"INSERT INTO `laundry`.`access_menu_template_details`  
	                                        (
                                                `template_code`, 
	                                            `function_code`
	                                        )
	                                     VALUES";

                    string values = "";
                    bool hasData = false;

                    for (int i = 0; i < templateDetails.Count; i++)
                    {
                        var oDetail = templateDetails[i];
                        if (!oDetail.function_code.Equals(""))
                        {
                            hasData = true;
                            values = values + "(" + "'" + oDetail.oTemplate.template_code + "'" + ", " + "'" + oDetail.function_code + "'" + ")";

                            if (i < templateDetails.Count - 1)
                            {
                                values = values + ",";
                            }
                        }

                    }

                    if (hasData)
                    {
                        oCommand.CommandText = oCommand.CommandText + values;

                        await oCommand.ExecuteNonQueryAsync();

                        serviceResponse.SetValues(200, "Success.", "");
                    }
                    else
                    {
                        serviceResponse.SetValues(400, "Bad request.", "");
                    }
                }
                else
                {
                    serviceResponse.SetValues(400, "Bad request.", "");
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
