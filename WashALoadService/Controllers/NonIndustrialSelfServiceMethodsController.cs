using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class NonIndustrialSelfServiceMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        public NonIndustrialSelfServiceMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet("selfservicequeryreport")]
        public async Task<IActionResult> QueryReportByDate(string dateFrom, string dateTo, int customerID, string postedBy, [FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialSelfServiceMethods(gDb);

                serviceResponse = await oEntity.SelfServiceQueryReportByDate(dateFrom, dateTo, customerID, postedBy);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("sodetails/{soReference}")]
        public async Task<IActionResult> FindSODetailsAsync(int soReference, [FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialSelfServiceMethods(gDb);

                serviceResponse = await oEntity.GetSelfServiceDetail(soReference);


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
        public async Task<IActionResult> ReceivedItemByLogisticssAsync([FromBody] NonIndustrialSelfService selfService, [FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialSelfServiceMethods(gDb);

                serviceResponse = await oEntity.SaveSelfServiceAsync(selfService);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpDelete("cancelso/{soReference}")]
        public async Task<IActionResult> CancelSOAsync(int soReference, [FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialSelfServiceMethods(gDb);

                var oSOReference = await oEntity.GetSelfServiceDetail(soReference);

                if(oSOReference.code == 200)
                {
                    serviceResponse = await oEntity.CancelSelfServiceAsync(soReference);
                }
                else
                {
                    serviceResponse.SetValues(oSOReference.code, oSOReference.message, "");
                }

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
