using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class BillingMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        public BillingMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet("billingreport")]
        public async Task<IActionResult> GenerateBillingReport(string dateFrom, string dateTo, int customerID, string type,[FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                DateTime dFrom = Convert.ToDateTime(dateFrom);
                DateTime dTo = Convert.ToDateTime(dateTo);

                if (dTo < dFrom)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid date range).", "");
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    var oCustomer = new CustomerMethods(gDb);

                    var oKeysCustomer = await oCustomer.VerifyCustomerKeyAsync(UserKey);

                    if(oKeysCustomer.code != 200)
                    {
                        gDb.Dispose();
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                        return new OkObjectResult(serviceResponse);
                    }

                }

                

                var oEntity = new BillingMethods(gDb);

                serviceResponse = await oEntity.GenerateBillingReport(dateFrom, dateTo, customerID, type);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("billingdetails/{billingReference}")]
        public async Task<IActionResult> GetBillingDetails(int billingReference, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new BillingMethods(gDb);

                serviceResponse = await oEntity.GetBillingDetails(billingReference);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("billingcustomerdetails/{billingReference}")]
        public async Task<IActionResult> GetBillingCustomerDetails(int billingReference, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new BillingMethods(gDb);

                serviceResponse = await oEntity.GetBillingCustomerDetails(billingReference);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("getunbilledinvoices")]
        public async Task<IActionResult> GetUnBilledInvoicesByDate(string dateFrom, string dateTo, int customerID, string type, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                DateTime dFrom = Convert.ToDateTime(dateFrom);
                DateTime dTo = Convert.ToDateTime(dateTo);

                if (dTo < dFrom)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid date range).", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new BillingMethods(gDb);

                serviceResponse = await oEntity.GetUnBilledInvoicesByDate(dateFrom, dateTo, customerID, type);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("generatebillingindustrial")]
        public async Task<IActionResult> GenerateBillingIndustrialAsync([FromBody] List<Int32> customerIDs, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if(customerIDs.Count < 1)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid customer list).", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new BillingMethods(gDb);

                SystemUser systeUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                serviceResponse = await oEntity.GenerateBillingIndustrialAsync(customerIDs, systeUser.user_id);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("generatebillingnonindustrial")]
        public async Task<IActionResult> GenerateBillingNonIndustrialAsync([FromBody] List<Int32> customerIDs, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (customerIDs.Count < 1)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid customer list).", "");
                    return new OkObjectResult(serviceResponse);
                }
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");

                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new BillingMethods(gDb);

                SystemUser systeUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                serviceResponse = await oEntity.GenerateBillingNonIndustrialAsync(customerIDs, systeUser.user_id);

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
