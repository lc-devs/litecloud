using Google.Authenticator;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class TwoFactorAuthenticationController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private CommonFunctions commonFunctions = new CommonFunctions();
        public TwoFactorAuthenticationController()
        {
            gDb = new AppDb_WashALoad();
        }

        [HttpGet("setup/{custID}")]
        public async Task<IActionResult> SetupAsync(int custID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            serviceResponse.SetValues(0, "Initialized", "");

            try
            {
                await gDb.Connection.OpenAsync();

                var oCustomer = new CustomerMethods(gDb);
                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                LoginDetails loginDetails = JsonSerializer.Deserialize<LoginDetails>(oKeys.jsonData);

                if (custID != loginDetails.customer_id)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Bad request", "");
                }

                TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
                var setupInfo = twoFactor.GenerateSetupCode("wash-a-load", loginDetails.email_address, TwoFactorKey(loginDetails), false, 3);

                dynamic jsonObject = new ExpandoObject();


                jsonObject.SetupCode = setupInfo.ManualEntryKey;
                jsonObject.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;

                string jsonString = JsonSerializer.Serialize(jsonObject);

                serviceResponse.SetValues(200, "Success", jsonString);
            }
            catch(Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }          

            return new OkObjectResult(serviceResponse);
        }

        [HttpPost("authenticate/{custID}")]
        public async Task<IActionResult> AuthenticateAsync(int custID, string securityCode, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            serviceResponse.SetValues(0, "Initialized", "");

            try
            {
                await gDb.Connection.OpenAsync();

                var oCustomer = new CustomerMethods(gDb);
                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                LoginDetails loginDetails = JsonSerializer.Deserialize<LoginDetails>(oKeys.jsonData);

                if (custID != loginDetails.customer_id)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Bad request", "");
                }

                TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();

                bool isValid = twoFactor.ValidateTwoFactorPIN(TwoFactorKey(loginDetails), securityCode);

                if(isValid)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(401, "Unauthorized", "");
                }

                

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);
        }

        private static string TwoFactorKey(LoginDetails user)
        {
            return $"wash-a-load+{user.email_address}";
        }

    }
}
