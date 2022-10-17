using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class CIFMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }
        public AppDb_SourceInfo gDbSourceInfo { get; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        private SystemUserMethods oUser { get; set; }

        public CIFMethodsController()
        {
            gDb = new AppDb_WashALoad();
            gDbSourceInfo = new AppDb_SourceInfo();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet("customers/contactnumber/{contactno}")]
        public async Task<IActionResult> FindCustomerByContactAsync(string contactno, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (contactno.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (contact number)", "");
                    return new OkObjectResult(serviceResponse);
                }
                await gDb.Connection.OpenAsync();
                await gDbSourceInfo.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    gDbSourceInfo.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CIFMethods(gDbSourceInfo);

                serviceResponse = await oEntity.FindCustomerByContactAsync(contactno);


                gDb.Dispose();
                gDbSourceInfo.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("customers/email/{emailadd}")]
        public async Task<IActionResult> FindCustomerByEmailAsync(string emailadd, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (emailadd.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (contact number)", "");
                    return new OkObjectResult(serviceResponse);
                }
                await gDb.Connection.OpenAsync();
                await gDbSourceInfo.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    gDbSourceInfo.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CIFMethods(gDbSourceInfo);

                serviceResponse = await oEntity.FindCustomerByEmailAsync(emailadd);


                gDb.Dispose();
                gDbSourceInfo.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("customers/name/{customerName}")]
        public async Task<IActionResult> FindCustomerByNameAsync(string customerName, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (customerName.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (customer name)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();
                await gDbSourceInfo.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    gDbSourceInfo.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CIFMethods(gDbSourceInfo);

                serviceResponse = await oEntity.FindCustomerByNameAsync(customerName);


                gDb.Dispose();
                gDbSourceInfo.Dispose();

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
