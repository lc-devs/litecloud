using inventory.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventory.Methods;
using inventory.Models;
 
namespace inventory.Controllers 
{
   [EnableCors("CorsPolicy")]
   [Route("posinventoryservice/[controller]")]
   public class Inventory_InventoryItemsMethodsController : Controller
   {
       public AppDb gDb { get; }
       private CommonFunctions commonFunctions = new CommonFunctions();
 
       public Inventory_InventoryItemsMethodsController()
       {
           gDb = new AppDb();
        
       }

 
      //getall
      [HttpGet("getall")]
        public async Task<IActionResult> GetAllInventory_InventoryItems([FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryItemsMethods(gDb);

                serviceResponse = await oEntity.GetInventory_InventoryItems();

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", ""); 
            }

            return new OkObjectResult(serviceResponse);
        }


        [HttpGet("model_description/{model_description}")]
        public async Task<IActionResult> GetByDescription(string model_description, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryItemsMethods(gDb);

                serviceResponse = await oEntity.GetByDescription(model_description);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }


        [HttpGet("getbycategory/{category_id}")]
        
        public async Task<IActionResult> GetBy(int category_id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_InventoryItemsMethods(gDb);

                serviceResponse = await oEntity.GetByCategoryID(category_id);

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
       public async Task<IActionResult> GetOneInventory_InventoryItems(int id, [FromHeader] string UserKey)
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
 
               var oInventory = new Inventory_InventoryItemsMethods(gDb);
               serviceResponse = await oInventory.GetOneInventory_InventoryItems(id);
 
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
        public async Task<IActionResult> SaveInventory_InventoryItems([FromBody]Inventory_InventoryItemsModel inventory_items, [FromHeader] string UserKey)
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

                var oInventory = new Inventory_InventoryItemsMethods(gDb);

                serviceResponse = await oInventory.SaveInventory_InventoryItems(inventory_items);

                gDb.Dispose();


            } 
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        
 
       [HttpPut]
       public async Task<IActionResult> UpdateInventory_InventoryItems([FromBody] Inventory_InventoryItemsModel inventory_items, [FromHeader] string UserKey)
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
 
               var oInventory = new Inventory_InventoryItemsMethods(gDb);
 
               serviceResponse = await oInventory.GetOneInventory_InventoryItems(inventory_items.item_id);
 
               if(serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oInventory.UpdateInventory_InventoryItems(inventory_items);
 
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
        public async Task<IActionResult> DeleteInventory_InventoryItems(long id, [FromHeader] string UserKey)
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

                var oInventory = new Inventory_InventoryItemsMethods(gDb);

                serviceResponse = await oInventory.GetOneInventory_InventoryItems(id);

                if (serviceResponse.code == 200)
                {
                    serviceResponse = await oInventory.Delete(id);
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