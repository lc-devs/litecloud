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
    public class PickupNonIndustrialMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        private SystemUserMethods oUser { get; set; }

        public PickupNonIndustrialMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet]
        public async Task<IActionResult> PickupQueryReportByDate(string dateFrom, string dateTo, int customerID, string pickupBy, bool isAllEntries, bool isForPickupOnly, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new PickupNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.PickupQueryReportByDate(dateFrom, dateTo, customerID, pickupBy, isAllEntries, isForPickupOnly);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("{soreference}")]
        public async Task<IActionResult> GetSODetails(int soreference, [FromHeader] string UserKey)
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

                var oEntity = new PickupNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.GetPickUpBySOReference(soreference);


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
        public async Task<IActionResult> SavePickupItemsAsync([FromBody] PickupNonIndustrial pickupNonIndustrial, [FromHeader] string UserKey)
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

                var oEntity = new PickupNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.SavePickupItemsAsync(pickupNonIndustrial);

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
