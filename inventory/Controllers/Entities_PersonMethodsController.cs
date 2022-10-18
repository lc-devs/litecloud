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
   public class Entities_PersonMethodsController : Controller
   {
       public AppDb gDb { get; }
       private CommonFunctions commonFunctions = new CommonFunctions();
 
       public Entities_PersonMethodsController()
       {
           gDb = new AppDb();
       }
 
       
       [HttpGet("getdetails/{personId}")]
       public async Task<IActionResult> GetPersonDetails(int personId, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_PersonMethods(gDb);
 
               serviceResponse = await oEntity.GetPersonDetails(personId);
 
               gDb.Dispose();
 
 
           }
           catch (Exception ex)
           {
               commonFunctions.CreateLog(ex.ToString());
               serviceResponse.SetValues(500, "Internal Server Error", "");
           }
 
           return new OkObjectResult(serviceResponse);
       }

        [HttpGet("searchbyname/{name}")]
        public async Task<IActionResult> GetPersonByName(string name, [FromHeader] string UserKey)
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

                var oEntity = new Entities_PersonMethods(gDb);

                serviceResponse = await oEntity.GetPersonbyName(name);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("searchall")]
        public async Task<IActionResult> GetAllPerson([FromHeader] string UserKey)
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

                var oEntity = new Entities_PersonMethods(gDb);

                serviceResponse = await oEntity.GetAllPersons();

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
       public async Task<IActionResult> SavePerson([FromBody] Entities_PersonModel person, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_PersonMethods(gDb);
 
               serviceResponse = await oEntity.SavePerson(person);
 
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
       public async Task<IActionResult> UpdatePerson([FromBody] Entities_PersonModel person, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_PersonMethods(gDb);
 
               serviceResponse = await oEntity.GetPersonDetails(person.person_id);
 
               if(serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.UpdatePerson(person);
 
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
       public async Task<IActionResult> DeletePerson(Int32 id, [FromHeader] string UserKey)
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
 
               var oEntity = new Entities_PersonMethods(gDb);
 
               serviceResponse = await oEntity.GetPersonDetails(id);
 
               if (serviceResponse.code != 200)
               {
                   gDb.Dispose();
                   return new OkObjectResult(serviceResponse);
               }
 
               serviceResponse = await oEntity.DeletePerson(id);
 
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