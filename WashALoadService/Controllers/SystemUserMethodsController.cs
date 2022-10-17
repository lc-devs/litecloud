using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class SystemUserMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        public SystemUserMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLoginAsync([FromBody] Credential credential)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

               var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.UserLoginAsync(credential.user_id, credential.password);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet]
        public async Task<IActionResult> FindAllUsers([FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindAllUsers();


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("userid")]
        public async Task<IActionResult> FindUserByUserId(string userid, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindUserByUserId(userid);

                gDb.Dispose();
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromBody] SystemUser user, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (user.user_id.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (user id)", "");
                    return new OkObjectResult(serviceResponse);
                }

                if (user.user_password.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (user password)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.SaveUserAsync(user);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] SystemUser user, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindUserByUserId(user.user_id);

                if(serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.UpdateUserAsync(user);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("changepassword/{userid}")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] Credential credential, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (credential.user_id.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (user id)", "");
                    return new OkObjectResult(serviceResponse);
                }

                if (credential.password.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (user password)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindUserByUserId(credential.user_id);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.ChangeUserPasswordAsync(credential.user_id, credential.password);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("changeuserstatus/{userid}")]
        public async Task<IActionResult> ActivateDeactivateUserAsync(string userid, int status, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindUserByUserId(userid);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.ActivateDeactivateUserAsync(userid, status);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpDelete("{userid}")]
        public async Task<IActionResult> DeleteUserAsync(string userid, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new SystemUserMethods(gDb);

                serviceResponse = await oEntity.FindUserByUserId(userid);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.DeleteUserAsync(userid);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

    }
}
