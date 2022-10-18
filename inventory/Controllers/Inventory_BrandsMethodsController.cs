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
   public class Inventory_BrandsMethodsController : Controller
   {
       public AppDb gDb { get; }
       private CommonFunctions commonFunctions = new CommonFunctions();
 
       public Inventory_BrandsMethodsController()
       {
           gDb = new AppDb();
       }
 
  [HttpGet("brand_name/{brand_name}")]
        public async Task<IActionResult> GetByDescription(string brand_name, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_BrandsMethods(gDb);

                serviceResponse = await oEntity.GetByDescription(brand_name);

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
       public async Task<IActionResult> GetOneInventory_Brands(int id, [FromHeader] string UserKey)
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
 
               var oEntity = new Inventory_BrandsMethods(gDb);
 
               serviceResponse = await oEntity.GetOneInventory_Brands(id);
 
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
        public async Task<IActionResult> GetAllInventory_Brands([FromHeader] string UserKey)
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

                var oEntity = new Inventory_BrandsMethods(gDb);

                serviceResponse = await oEntity.GetInventory_Brands();

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
        public async Task<IActionResult> SaveInventory_Brands([FromBody] Inventory_BrandsModel brands, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_BrandsMethods(gDb);

                serviceResponse = await oEntity.SaveInventory_Brands(brands);

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
       public async Task<IActionResult> UpdateInventory_Brands([FromBody] Inventory_BrandsModel brands, [FromHeader] string UserKey)
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
 
               var oEntity = new Inventory_BrandsMethods(gDb);
 
               serviceResponse = await oEntity.GetOneInventory_Brands(brands.brand_id);
 
               if(serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.UpdateInventory_Brands(brands);
 
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
        public async Task<IActionResult> DeleteInventory_Brands(long id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_BrandsMethods(gDb);

                serviceResponse = await oEntity.GetOneInventory_Brands(id);

                if (serviceResponse.code == 200)
                {
                    serviceResponse = await oEntity.DeleteInventory_Brands(id);
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