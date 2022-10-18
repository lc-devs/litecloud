using inventory.Common;
using inventory.Methods;
using inventory.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Controllers
{
 
    [EnableCors("CorsPolicy")]
    [Route("posinventoryservice/[controller]")]
    public class Inventory_InventoryUnitsItemsController : Controller
    {
        public AppDb gDb { get; }
        private CommonFunctions commonFunctions = new CommonFunctions();

        public Inventory_InventoryUnitsItemsController()
        {
            gDb = new AppDb();
        }


 //getall
      [HttpGet("getall")]
        public async Task<IActionResult> GetAllInventory_InventoryUnitsItems([FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.GetInventory_InventoryUnitsItems();

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", ""); 
            }

            return new OkObjectResult(serviceResponse);
        }
        
        [HttpGet("getone/{id}")]
        public async Task<IActionResult> GetOne(int id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.GetOne(id);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("searchbyunitlocation/{unit_id}")]
        public async Task<IActionResult> GetItemsByUnitLocation(int unit_id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.GetItemsByUnitLocation(unit_id);

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
        public async Task<IActionResult> Save([FromBody]Inventory_InventoryUnitsItemsModel unitsitems, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.Save(unitsitems);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("id")]
        public async Task<IActionResult> Update([FromBody]Inventory_InventoryUnitsItemsModel unitsitems, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.GetOne(unitsitems.unit_item_id);

                if(serviceResponse.code == 200)
                {
                    serviceResponse = await oEntity.Update(unitsitems);
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

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(long id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryUnitsItemsMethods(gDb);

                serviceResponse = await oEntity.GetOne(id);

                if (serviceResponse.code == 200)
                {
                    serviceResponse = await oEntity.Delete(id);
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
