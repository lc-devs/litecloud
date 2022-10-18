using inventory.Common;
using inventory.Methods;
using inventory.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("posinventoryservice/[controller]")]
    public class Inventory_ItemsAdjustmentsMethodsController : Controller
    {
        public AppDb gDb { get; }
        private CommonFunctions commonFunctions = new CommonFunctions();

        public Inventory_ItemsAdjustmentsMethodsController()
        {
            gDb = new AppDb();
        }

        [HttpGet("getone/{id}")]
        public async Task<IActionResult> GetOneItemAdjustment(long id, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                //var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                //if (oKeys.code != 200)
                //{
                //    gDb.Dispose();
                //    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                //    return new OkObjectResult(serviceResponse);

                //}

                var oEntity = new Inventory_ItemsAdjustmentsMethods(gDb);

                serviceResponse = await oEntity.GetOneItemAdjustment(id);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllItemAdjustmentByDate(string date_from, string date_to, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                //var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                //if (oKeys.code != 200)
                //{
                //    gDb.Dispose();
                //    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                //    return new OkObjectResult(serviceResponse);

                //}

                var oEntity = new Inventory_ItemsAdjustmentsMethods(gDb);

                serviceResponse = await oEntity.GetItemAdjustmentByDate(date_from, date_to);

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
        public async Task<IActionResult> SaveItemAdjustment([FromBody]Inventory_ItemsAdjustmentsModel adjustment, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                //var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                //if (oKeys.code != 200)
                //{
                //    gDb.Dispose();
                //    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                //    return new OkObjectResult(serviceResponse);

                //}

                var oEntity = new Inventory_ItemsAdjustmentsMethods(gDb);

                serviceResponse = await oEntity.SaveItemAdjustment(adjustment);

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
