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
   public class Entities_NonPersonMethodsController : Controller
   {
       public AppDb gDb { get; }
       private CommonFunctions commonFunctions = new CommonFunctions();
 
       public Entities_NonPersonMethodsController()
       {
           gDb = new AppDb();
       }
 
      
       [HttpGet("getdetails/{nonPersonId}")]
       public async Task<IActionResult> GetNonPersonDetails(int nonPersonId, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_NonPersonMethods(gDb);
 
               serviceResponse = await oEntity.GetNonPersonDetails(nonPersonId);
 
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
       public async Task<IActionResult> SaveNonPerson([FromBody] Entities_NonPersonModel nonperson, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_NonPersonMethods(gDb);
 
               serviceResponse = await oEntity.SaveNonPerson(nonperson);
 
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
       public async Task<IActionResult> UpdateNonPerson([FromBody] Entities_NonPersonModel nonperson, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_NonPersonMethods(gDb);
 
               serviceResponse = await oEntity.GetNonPersonDetails(nonperson.nonperson_id);
 
               if(serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.UpdateNonPerson(nonperson);
 
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
       public async Task<IActionResult> DeleteNonPerson(Int32 id, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_NonPersonMethods(gDb);
 
               serviceResponse = await oEntity.GetNonPersonDetails(id);
 
               if (serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.DeleteNonPerson(id);
 
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