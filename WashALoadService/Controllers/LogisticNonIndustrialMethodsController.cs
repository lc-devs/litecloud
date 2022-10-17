using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;
using static WashALoadService.Common.Common;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class LogisticNonIndustrialMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        public LogisticNonIndustrialMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet("sodetails/{soReference}")]
        public async Task<IActionResult> FindSODetailsAsync(int soReference, [FromHeader] string UserKey)
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.FindSODetailsAsync(soReference);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("receivedqueryreport")]
        public async Task<IActionResult> ReceivedQueryReportByDate(string dateFrom, string dateTo, int customerID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.ReceivedQueryReportByDate(dateFrom, dateTo, customerID);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("laundryqueryreport")]
        public async Task<IActionResult> LaundryQueryReportByDate(string dateFrom, string dateTo, int customerID, LaundryStatus laundryStatus, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.LaundryQueryReportByDate(dateFrom, dateTo, customerID, laundryStatus);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("receivedcompleteditemqueryreport")]
        public async Task<IActionResult> ReceivedCompletedItemQueryReportByDate(string dateFrom, string dateTo, int customerID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.ReceivedCompletedItemQueryReportByDate(dateFrom, dateTo, customerID);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("completeditemqueryreport")]
        public async Task<IActionResult> CompletedItemQueryReportByDate(string dateFrom, string dateTo, int customerID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.CompletedItemQueryReportByDate(dateFrom, dateTo, customerID);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("deliveryqueryreport")]
        public async Task<IActionResult> DeliveryQueryReportByDate(string dateFrom, string dateTo, int customerID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.DeliveryQueryReportByDate(dateFrom, dateTo, customerID);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("invoicequeryreport")]
        public async Task<IActionResult> InvoiceQueryReportByDate(string dateFrom, string dateTo, int customerID, int isPaid, int isUnPaid, int isUnBilled, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.InvoiceQueryReportByDate(dateFrom, dateTo, customerID, isPaid, isUnPaid, isUnBilled);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("invoicedetails/{invoiceReference}")]
        public async Task<IActionResult> GetInvoiceDetails(int invoiceReference, [FromHeader] string UserKey)
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.GetInvoiceDetails(invoiceReference);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpGet("soforinvoicing")]
        public async Task<IActionResult> GetSOForInvoicing(bool isManualInvoicing, [FromHeader] string UserKey)
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.GetSOForInvoicing(isManualInvoicing);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("receiveditemfrompickup")]
        public async Task<IActionResult> ReceivedItemByLogisticssAsync([FromBody] LogisticNonIndustrial logisticNonIndustrial, [FromHeader] string UserKey)
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

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.ReceivedItemByLogisticssAsync(logisticNonIndustrial);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("receiveditembylaundry/{soReference}")]
        public async Task<IActionResult> ReceivedItemByLaundrysAsync(int soReference, [FromHeader] string UserKey)
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

                SystemUser systemUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.ReceivedItemByLaundrysAsync(soReference, systemUser.user_id);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("completeditembylaundry/{soReference}")]
        public async Task<IActionResult> CompletedItemByLogisticsAsync(int soReference, [FromHeader] string UserKey)
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

                SystemUser systemUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.CompletedItemByLogisticsAsync(soReference, systemUser.user_id);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("receivedcompleteditembylogistics/{soReference}")]
        public async Task<IActionResult> ReceivedCompletedItemByLogisticsAsync(int soReference, [FromHeader] string UserKey)
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

                SystemUser systemUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.ReceivedCompletedItemByLogisticsAsync(soReference, systemUser.user_id);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("deliveritem/{soReference}")]
        public async Task<IActionResult> DeliverItemByLogisticsAsync(int soReference, [FromHeader] string UserKey)
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

                SystemUser systemUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.DeliverItemByLogisticsAsync(soReference, systemUser.user_id);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("generateinvoice")]
        public async Task<IActionResult> GenerateInvoiceAsync([FromBody] List<ForInvoice> soReferences, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (soReferences.Count < 1)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (empty invoice list)", "");
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

                SystemUser systemUser = JsonSerializer.Deserialize<SystemUser>(oKeys.jsonData);

                var oEntity = new LogisticNonIndustrialMethods(gDb);

                serviceResponse = await oEntity.GenerateInvoiceAsync(soReferences, systemUser.user_id);

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
