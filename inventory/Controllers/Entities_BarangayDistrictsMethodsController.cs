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
   public class Entities_BarangayMethodsController : Controller
   {
       public AppDb gDb { get; }
       private CommonFunctions commonFunctions = new CommonFunctions();
 
       public Entities_BarangayMethodsController()
       {
           gDb = new AppDb();
       }
 
       [HttpGet("getone/{id}")]
       public async Task<IActionResult> GetOneBarangay(int id, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_BarangayMethods(gDb);
 
               serviceResponse = await oEntity.GetOneBarangay(id);
 
               gDb.Dispose();
 
 
           }
           catch (Exception ex)
           {
               commonFunctions.CreateLog(ex.ToString());
               serviceResponse.SetValues(500, "Internal Server Error", "");
           }
 
           return new OkObjectResult(serviceResponse);
 
       }
 
       [HttpGet("getall/{townId}")]
       public async Task<IActionResult> GetAllBarangay(int townId, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_BarangayMethods(gDb);
 
               serviceResponse = await oEntity.GetAllBarangay(townId);
 
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
       public async Task<IActionResult> SaveBarangay([FromBody] Entities_BarangayDistrictsModel barangay, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_BarangayMethods(gDb);
 
               serviceResponse = await oEntity.SaveBarangay(barangay);
 
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
       public async Task<IActionResult> UpdateBarangay([FromBody] Entities_BarangayDistrictsModel barangay, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_BarangayMethods(gDb);
 
               serviceResponse = await oEntity.GetOneBarangay(barangay.barangay_district_id);
 
               if(serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.UpdateBarangay(barangay);
 
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
       public async Task<IActionResult> DeleteBarangay(Int32 id, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_BarangayMethods(gDb);
 
               serviceResponse = await oEntity.GetOneBarangay(id);
 
               if (serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.DeleteBarangay(id);
 
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
 
