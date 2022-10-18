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
    public class Entities_ProvinceStateMethodsController : Controller
    {
        public AppDb gDb { get; }
        private CommonFunctions commonFunctions = new CommonFunctions();

        public Entities_ProvinceStateMethodsController()
        {
            gDb = new AppDb();
        }

        [HttpGet("getone/{id}")]
        public async Task<IActionResult> GetOneProvinceState(int id, [FromHeader] string UserKey)
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

                var oEntity = new Entities_ProvinceStateMethods(gDb);

                serviceResponse = await oEntity.GetOneProvinceState(id);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("getall/{countryId}")]
        public async Task<IActionResult> GetAllProvinceState(int countryId, [FromHeader] string UserKey)
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

                var oEntity = new Entities_ProvinceStateMethods(gDb);

                serviceResponse = await oEntity.GetProvinceState(countryId);

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
        public async Task<IActionResult> SaveProvinceState([FromBody] Entities_ProvinceStateModel provincestate, [FromHeader] string UserKey)
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

                var oEntity = new Entities_ProvinceStateMethods(gDb);

                serviceResponse = await oEntity.SaveProvinceState(provincestate);

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
        public async Task<IActionResult> UpdateProvinceState([FromBody] Entities_ProvinceStateModel provincestate, [FromHeader] string UserKey)
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

                var oEntity = new Entities_ProvinceStateMethods(gDb);

                serviceResponse = await oEntity.GetOneProvinceState(provincestate.province_state_id);

                if(serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.UpdateProvinceState(provincestate);

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
        public async Task<IActionResult> DeleteProvinceState(Int32 id, [FromHeader] string UserKey)
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

                var oEntity = new Entities_ProvinceStateMethods(gDb);

                serviceResponse = await oEntity.GetOneProvinceState(id);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.DeleteProvinceState(id);

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
