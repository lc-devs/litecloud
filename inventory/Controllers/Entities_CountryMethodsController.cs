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
    public class Entities_CountryMethodsController : Controller
    {
        public AppDb gDb { get; }
        private CommonFunctions commonFunctions = new CommonFunctions();

        public Entities_CountryMethodsController()
        {
            gDb = new AppDb();
        }

        [HttpGet("getone/{id}")]
        public async Task<IActionResult> GetOneCountry(int id, [FromHeader] string UserKey)
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

                var oEntity = new Entities_CountryMethods(gDb);

                serviceResponse = await oEntity.GetOneCountry(id);

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
        public async Task<IActionResult> GetAllCountries([FromHeader] string UserKey)
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

                var oEntity = new Entities_CountryMethods(gDb);

                serviceResponse = await oEntity.GetCountries();

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
        public async Task<IActionResult> SaveCountry([FromBody] Entities_CountryModel country, [FromHeader] string UserKey)
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

                var oEntity = new Entities_CountryMethods(gDb);

                serviceResponse = await oEntity.SaveCountry(country);

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
        public async Task<IActionResult> UpdateCountry([FromBody] Entities_CountryModel country, [FromHeader] string UserKey)
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

                var oEntity = new Entities_CountryMethods(gDb);

                serviceResponse = await oEntity.GetOneCountry(country.country_id);

                if(serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.UpdateCountry(country);

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
        public async Task<IActionResult> DeleteCountry(Int32 id, [FromHeader] string UserKey)
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

                var oEntity = new Entities_CountryMethods(gDb);

                serviceResponse = await oEntity.GetOneCountry(id);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.DeleteCountry(id);

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
