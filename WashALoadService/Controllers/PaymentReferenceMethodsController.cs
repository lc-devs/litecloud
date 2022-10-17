using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;
using static WashALoadService.Common.Common;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class PaymentReferenceMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        public PaymentReferenceMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet("image/{paymentReference}")]
        public async Task<IActionResult> GetPaymentReferenceImage(int paymentReference, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);

                serviceResponse = await oEntity.GetPaymentReferenceImage(paymentReference);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("queryreport")]
        public async Task<IActionResult> QueryReport(string dateFrom, string dateTo, int customerID, string postedBy, BillingType billingType, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);
                if(billingType == BillingType.ByBillingReference)
                {
                    serviceResponse = await oEntity.QueryReportByBillingReference(dateFrom, dateTo, customerID, postedBy);
                }
                else if (billingType == BillingType.ByInvoice)
                {
                    serviceResponse = await oEntity.QueryReportByInvoice(dateFrom, dateTo, customerID, postedBy);
                }
                else if (billingType == BillingType.ByFloat)
                {
                    serviceResponse = await oEntity.QueryReportByFloats(dateFrom, dateTo, customerID, postedBy);
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

        [HttpGet("collectionreport")]
        public async Task<IActionResult> GetCollectionReport(string dateFrom, string dateTo, int customerID, string postedBy, string logicSite, bool isIndustrial, bool isNonIndustrial, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);
               
                serviceResponse = await oEntity.GetCollectionReport(dateFrom, dateTo, customerID, postedBy, logicSite, isIndustrial, isNonIndustrial);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("fullpaymentbybilling/{billingReference}")]
        public async Task<IActionResult> SaveFullPaymentAsync([FromBody] PaymentReference paymentReference, int billingReference, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);

                serviceResponse = await oEntity.SaveFullPaymentAsync(paymentReference, billingReference);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("fullpaymentbyinvoice/{invoiceReference}")]
        public async Task<IActionResult> SavePaymentPerInvoiceAsync([FromBody] PaymentReference paymentReference, int invoiceReference, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);

                serviceResponse = await oEntity.SavePaymentPerInvoiceAsync(paymentReference, invoiceReference);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("floatpayment")]
        public async Task<IActionResult> SaveFloatPaymentAsync([FromBody] PaymentReference paymentReference, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);

                serviceResponse = await oEntity.SaveFloatPaymentAsync(paymentReference);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpDelete("cancelpayment/{paymentReference}")]
        public async Task<IActionResult> SaveFullPaymentAsync(int paymentReference, [FromHeader] string UserKey)
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

                var oEntity = new PaymentReferenceMethods(gDb);



                serviceResponse = await oEntity.CancelPaymentAsync(paymentReference);

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
