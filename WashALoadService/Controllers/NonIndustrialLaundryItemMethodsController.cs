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
    public class NonIndustrialLaundryItemMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        public NonIndustrialLaundryItemMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet]
        public async Task<IActionResult> FindAllAsync([FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialLaundryItemMethods(gDb);

                serviceResponse = await oEntity.FindAllAsync();


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
        public async Task<IActionResult> SaveItemyAsync([FromBody] NonIndustrialLaundryItem item, [FromHeader] string UserKey)
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

                var oEntity = new NonIndustrialLaundryItemMethods(gDb);

                serviceResponse = await oEntity.SaveItemAsync(item);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] NonIndustrialLaundryItem item, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new NonIndustrialLaundryItemMethods(gDb);

                serviceResponse = await oEntity.UpdateItemAsync(item);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new NonIndustrialLaundryItemMethods(gDb);



                serviceResponse = await oEntity.DeleteItemAsync(itemId);

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
