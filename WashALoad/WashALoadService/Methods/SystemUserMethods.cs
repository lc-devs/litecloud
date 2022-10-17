using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class SystemUserMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal SystemUserMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public SystemUserMethods() { }
        public async Task<ServiceResponse> UserLoginAsync(string userid, string password)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT DISTINCT * 
                                        FROM 
                                        (
	                                        SELECT	u.`user_id`, 
							                        `user_name`, 
                                                    `user_password`,
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
                                                    UUID() as`session_authentication_key`,
							                        COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code
                                                    

						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.site 
                                            WHERE u.`user_id`= @userid AND `active_user` = 1 AND u.`admin`= 0
                                        
                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`,
                                                    `user_password`,
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
                                                    UUID() as `session_authentication_key`,
						                            COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`laundry_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`laundry_sites` ls ON ls.`code` = lu.site 
                                            WHERE u.`user_id`= @userid AND `active_user` = 1 AND u.`admin`= 0

                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`,
                                                    `user_password`,
							                        `admin`, 
							                        `section`,
							                        '' as `description`,
							                        `active_user`, 
							                        `menu_template` ,
                                                    UUID() as `session_authentication_key`,
						                            '' AS site,
							                        '' AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
                                            WHERE u.`user_id`= @userid AND `active_user` = 1 AND u.`admin`= 1 
                                        )xx;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@userid", System.Data.DbType.String, userid);

                var userDetails = new SystemUser();

                userDetails.user_id = "";

                var oresult = await oCommand.ExecuteReaderAsync();

                bool hasData = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        hasData = true;
                        string pwd = oresult.GetString("user_password");

                        if (BCrypt.Net.BCrypt.Verify(password, pwd, true))
                        {                           
                            userDetails.user_id = oresult.GetString("user_id");
                            userDetails.user_name = oresult.GetString("user_name");
                            userDetails.admin = oresult.GetInt32("admin");

                            userDetails.section = new Section();
                            userDetails.section.idsection = oresult.GetInt32("section");
                            userDetails.section.description = oresult.GetString("description");

                            userDetails.active_user = oresult.GetInt32("active_user");
                            userDetails.menu_template = oresult.GetString("menu_template");
                            userDetails.session_authentication_key = oresult.GetString("session_authentication_key");

                            userDetails.oSite = new Site();
                            userDetails.oSite.site = oresult.GetString("site");
                            userDetails.oSite.code = oresult.GetString("site_code");
                        }

                    }
                }

                if(hasData == true)
                {
                    if (!userDetails.user_id.Equals(""))
                    {
                        if(userDetails.active_user == 0)
                        {
                            serviceResponse.SetValues(401, "User is not active. Please contact admin.", "");
                            return serviceResponse;
                        }

                        oCommand.CommandText = @"UPDATE `laundry`.`system_users`  
                                             SET 
                                                `session_authentication_key` = @authkey, 
                                                `session_datetime`= NOW()
                                            WHERE `user_id` = @user_id;";

                        oCommand.Parameters.Clear();

                        commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, userDetails.user_id);
                        commonFunctions.BindParameter(oCommand, "@authkey", System.Data.DbType.String, userDetails.session_authentication_key);

                        await oCommand.ExecuteNonQueryAsync();

                        string jsonString = JsonSerializer.Serialize(userDetails);

                        serviceResponse.SetValues(200, "Success", jsonString);

                    }
                    else
                    {
                        serviceResponse.SetValues(401, "User and password mismatched.", "");
                    }
                }
                else
                {
                    serviceResponse.SetValues(401, "User could not be indentified or user and password mismatched.", "");
                }
                
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> FindUserByUserId(string userid)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT DISTINCT * 
                                        FROM 
                                        (
	                                        SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
							                        COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code

						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.site 
                                            WHERE u.`user_id`= @userid AND u.`admin`= 0
                                        
                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
						                            COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`laundry_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`laundry_sites` ls ON ls.`code` = lu.site 
                                            WHERE u.`user_id`= @userid AND u.`admin`= 0

                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        '' as `description`,
							                        `active_user`, 
							                        `menu_template` ,
						                            '' AS site,
							                        '' AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
                                            WHERE u.`user_id`= @userid AND u.`admin`= 1
                                        )xx;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@userid", System.Data.DbType.String, userid);

                var userDetails = new SystemUser();

                userDetails.user_id = "";

                var oresult = await oCommand.ExecuteReaderAsync();

                bool hasData = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        hasData = true;
                        
                        userDetails.user_id = oresult.GetString("user_id");
                        userDetails.user_name = oresult.GetString("user_name");
                        userDetails.admin = oresult.GetInt32("admin");
                        userDetails.section = new Section();
                        userDetails.section.idsection = oresult.GetInt32("section");
                        userDetails.section.description = oresult.GetString("description");
                        userDetails.active_user = oresult.GetInt32("active_user");
                        userDetails.menu_template = oresult.GetString("menu_template");

                        userDetails.oSite = new Site();
                        userDetails.oSite.site = oresult.GetString("site");
                        userDetails.oSite.code = oresult.GetString("site_code");

                    }
                }

                if (hasData == true)
                {
                    string jsonString = JsonSerializer.Serialize(userDetails);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(401, "User not found.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> FindAllUsers()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT DISTINCT * 
                                        FROM 
                                        (
	                                        SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
							                        COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code

						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`logistics_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`logistics_sites` ls ON ls.`code` = lu.site
                                            WHERE  u.`admin`= 0
                                        
                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        `description`,
							                        `active_user`, 
							                        `menu_template` ,
						                            COALESCE(ls.`site`, '') AS site,
							                        COALESCE(ls.`code`, '') AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
						                    INNER JOIN  `laundry`.`sections` ON `idsection` = section
						                    INNER JOIN `laundry`.`laundry_users` lu ON lu.`user_id` = u.`user_id`
						                    INNER JOIN `laundry`.`laundry_sites` ls ON ls.`code` = lu.site
                                            WHERE  u.`admin`= 0

                                        UNION ALL
                                            
                                            SELECT	u.`user_id`, 
							                        `user_name`, 
							                        `admin`, 
							                        `section`,
							                        '' AS `description`,
							                        `active_user`, 
							                        `menu_template` ,
						                            '' AS site,
							                        '' AS site_code
						                    FROM 
						                        `laundry`.`system_users` u
						                    WHERE  u.`admin`= 1
                                        )xx;";

                oCommand.Parameters.Clear();



                List<SystemUser> oUserDetails = new List<SystemUser>();

                var oresult = await oCommand.ExecuteReaderAsync();

                bool hasData = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        hasData = true;
                        var userDetails = new SystemUser();
                        userDetails.user_id = oresult.GetString("user_id");
                        userDetails.user_name = oresult.GetString("user_name");
                        userDetails.admin = oresult.GetInt32("admin");
                        userDetails.section = new Section();
                        userDetails.section.idsection = oresult.GetInt32("section");
                        userDetails.section.description = oresult.GetString("description");
                        userDetails.active_user = oresult.GetInt32("active_user");
                        userDetails.menu_template = oresult.GetString("menu_template");

                        userDetails.oSite = new Site();
                        userDetails.oSite.site = oresult.GetString("site");
                        userDetails.oSite.code = oresult.GetString("site_code");

                        oUserDetails.Add(userDetails);

                    }
                }

                if (hasData == true)
                {
                    string jsonString = JsonSerializer.Serialize(oUserDetails);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(404, "User not found.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> VerifyUserKeyAsync(string authKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                using var oCommand = gDb.Connection.CreateCommand();

                oCommand.CommandText = @"SELECT	`user_id`, 
	                                        `user_name`, 
	                                        `admin`, 
	                                        `section`,
	                                        `description`,
	                                        `active_user`, 
	                                        `menu_template`, 
	                                        `user_password`, 
	                                        `session_authentication_key`,
                                            TIMESTAMPDIFF(MINUTE, `session_datetime`, NOW()) AS session_duration
                                        FROM 
                                            `laundry`.`system_users`
                                        INNER JOIN  `laundry`.`sections` ON `idsection` = section 
                                        WHERE `session_authentication_key`= @session_authentication_key;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@session_authentication_key", System.Data.DbType.String, authKey);

                var userDetails = new SystemUser();

                var oresult = await oCommand.ExecuteReaderAsync();

                int session_duration = 0;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        session_duration = oresult.GetInt32("session_duration");

                        userDetails.user_id = oresult.GetString("user_id");
                        userDetails.user_name = oresult.GetString("user_name");
                        userDetails.admin = oresult.GetInt32("admin");
                        userDetails.section = new Section();
                        userDetails.section.idsection = oresult.GetInt32("section");
                        userDetails.section.description = oresult.GetString("description");
                        userDetails.active_user = oresult.GetInt32("active_user");
                        userDetails.menu_template = oresult.GetString("menu_template");
                        userDetails.session_authentication_key = oresult.GetString("session_authentication_key");

                    }
                }

                if(session_duration > 60)
                {
                    serviceResponse.SetValues(401, "Your session expired.", "");
                    return serviceResponse;
                }

                if (userDetails.user_id != null)
                {
                    oCommand.CommandText = @"UPDATE `laundry`.`system_users`  
                                             SET 
                                                `session_datetime`= NOW()
                                            WHERE `user_id` = @user_id;";

                    oCommand.Parameters.Clear();

                    commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, userDetails.user_id);
                    commonFunctions.BindParameter(oCommand, "@authkey", System.Data.DbType.String, userDetails.session_authentication_key);

                    await oCommand.ExecuteNonQueryAsync();

                    string jsonString = JsonSerializer.Serialize(userDetails);

                    serviceResponse.SetValues(200, "Success", jsonString);

                }
                else
                {
                    serviceResponse.SetValues(401, "User could not be verified.", "");
                }
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> SaveUserAsync(SystemUser user)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`system_users`
                                            (
                                                `user_id`, 
	                                            `user_name`, 
	                                            `admin`, 
	                                            `section`,
	                                            `active_user`, 
	                                            `menu_template`, 
	                                            `user_password`  
                                            )
	 
	                                    VALUES 
	                                         (
                                                @user_id,
                                                @user_name,
                                                @admin,
                                                @section,
                                                @active_user,
                                                @menu_template,
                                                @user_password
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);
                commonFunctions.BindParameter(oCommand, "@user_name", System.Data.DbType.String, user.user_name);
                commonFunctions.BindParameter(oCommand, "@admin", System.Data.DbType.Int32, user.admin);
                commonFunctions.BindParameter(oCommand, "@section", System.Data.DbType.Int32, user.section.idsection);
                commonFunctions.BindParameter(oCommand, "@active_user", System.Data.DbType.Int32, user.active_user);
                commonFunctions.BindParameter(oCommand, "@menu_template", System.Data.DbType.String, user.menu_template);
                commonFunctions.BindParameter(oCommand, "@user_password", System.Data.DbType.String, BCrypt.Net.BCrypt.EnhancedHashPassword(user.user_password));

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    if(user.section.idsection == 1) //Logistic users
                    {
                        oCommand.CommandText = @"INSERT INTO  `laundry`.`logistics_users`
                                            (
                                                `user_id`, 
	                                            `site`  
                                            )
	 
	                                    VALUES 
	                                         (
                                                @user_id,
                                                @site
                                             );";
                    }
                    else if (user.section.idsection == 2) //Laundry users
                    {
                        oCommand.CommandText = @"INSERT INTO  `laundry`.`laundry_users`
                                            (
                                                `user_id`, 
	                                            `site`  
                                            )
	 
	                                    VALUES 
	                                         (
                                                @user_id,
                                                @site
                                             );";
                    }
                    
                    if(user.section.idsection == 1 || user.section.idsection == 2)
                    {
                        oCommand.Parameters.Clear();
                        commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);
                        commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, user.oSite.code);
                        await oCommand.ExecuteNonQueryAsync();
                    }                   

                    string jsonString = JsonSerializer.Serialize(user);

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

        public async Task<ServiceResponse> UpdateUserAsync(SystemUser user)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`system_users`
                                            SET
                                                `user_name` = @user_name, 
	                                            `admin` = @admin, 
	                                            `section` = @section,
	                                            `menu_template` = @menu_template
                                            WHERE
                                                 `user_id` = @user_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);
                commonFunctions.BindParameter(oCommand, "@user_name", System.Data.DbType.String, user.user_name);
                commonFunctions.BindParameter(oCommand, "@admin", System.Data.DbType.Int32, user.admin);
                commonFunctions.BindParameter(oCommand, "@section", System.Data.DbType.Int32, user.section.idsection);
                commonFunctions.BindParameter(oCommand, "@menu_template", System.Data.DbType.String, user.menu_template);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    oCommand.CommandText = @"DELETE FROM  `laundry`.`laundry_users`
                                            WHERE
                                                `user_id` =  @user_id;";

                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);

                    await oCommand.ExecuteNonQueryAsync();


                    oCommand.CommandText = @"DELETE FROM  `laundry`.`logistics_users`
                                            WHERE
                                                `user_id` =  @user_id;";

                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);

                    await oCommand.ExecuteNonQueryAsync();

                    if (user.section.idsection == 1) //Logistic users
                    {
                        oCommand.CommandText = @"INSERT INTO  `laundry`.`logistics_users`
                                            (
                                                `user_id`, 
	                                            `site`  
                                            )
	 
	                                    VALUES 
	                                         (
                                                @user_id,
                                                @site
                                             );";
                    }
                    else if (user.section.idsection == 2) //Laundry users
                    {

                        oCommand.CommandText = @"INSERT INTO  `laundry`.`laundry_users`
                                            (
                                                `user_id`, 
	                                            `site`  
                                            )
	 
	                                    VALUES 
	                                         (
                                                @user_id,
                                                @site
                                             );";
                    }                    
                    oCommand.Parameters.Clear();
                    commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user.user_id);
                    commonFunctions.BindParameter(oCommand, "@site", System.Data.DbType.String, user.oSite.code);

                    await oCommand.ExecuteNonQueryAsync();
                    string jsonString = JsonSerializer.Serialize(user);

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

        public async Task<ServiceResponse> ChangeUserPasswordAsync(string user_id, string password)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`system_users`
                                            SET
                                                `user_password` =  @user_password
                                            WHERE
                                                 `user_id` = @user_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user_id);

                if (password.Equals(""))
                {
                    password = Guid.NewGuid().ToString();
                }

                commonFunctions.BindParameter(oCommand, "@user_password", System.Data.DbType.String, BCrypt.Net.BCrypt.EnhancedHashPassword(password));

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

        public async Task<ServiceResponse> ActivateDeactivateUserAsync(string user_id, int status)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`system_users`
                                            SET
                                                `active_user` = @active_user
                                            WHERE
                                                 `user_id` = @user_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user_id);
                commonFunctions.BindParameter(oCommand, "@active_user", System.Data.DbType.Int32, status);

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

        public async Task<ServiceResponse> DeleteUserAsync(string user_id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `laundry`.`system_users`
                                            WHERE
                                                 `user_id` = @user_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@user_id", System.Data.DbType.String, user_id);
                
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
                serviceResponse.SetValues(500, "User could not be deleted due to existing transaction record.", "");
            }

            return serviceResponse;
        }

    }

}
