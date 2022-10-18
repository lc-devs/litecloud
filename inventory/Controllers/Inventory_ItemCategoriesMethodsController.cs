using inventory.Common;
using inventory.Methods;
using inventory.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("posinventoryservice/[controller]")]
    public class Inventory_ItemCategoriesMethodsController : Controller
    {
        public AppDb gDb { get; }
        private CommonFunctions commonFunctions = new CommonFunctions();

        public Inventory_ItemCategoriesMethodsController()
        {
            gDb = new AppDb();
        }

        [HttpGet("getone/{id}")]
        public async Task<IActionResult> GetOneBrands(int id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_ItemCategoriesMethods(gDb);

                serviceResponse = await oEntity.GetOneBrand(id);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        [HttpGet("description/{description}")]
        public async Task<IActionResult> GetByDescription(string description, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_ItemCategoriesMethods(gDb);

                serviceResponse = await oEntity.GetByDescription(description);

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
        public async Task<IActionResult> Save(Inventory_ItemCategoriesModel category, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_ItemCategoriesMethods(gDb);

                serviceResponse = await oEntity.Save(category);

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
        public async Task<IActionResult> Update(Inventory_ItemCategoriesModel category, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_ItemCategoriesMethods(gDb);

                serviceResponse = await oEntity.GetOneBrand(category.item_category_id);

                if (serviceResponse.code == 200)
                {
                    if(category.item_category_id > 9)
                    {
                        serviceResponse = await oEntity.Update(category);
                    }
                    else
                    {
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Category is System Predefined.", "");
                    }
                   
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
        public async Task<IActionResult> DeleteBrands(long id, [FromHeader] string UserKey)
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

                var oEntity = new Inventory_ItemCategoriesMethods(gDb);

                serviceResponse = await oEntity.GetOneBrand(id);

                if (serviceResponse.code == 200)
                {
                    if (id > 9)
                    {
                        serviceResponse = await oEntity.Delete(id);
                    }
                    else
                    {
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Category is System Predefined.", "");
                    }
                   
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
