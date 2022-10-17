using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;
using WashALoadService.Services;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class CustomerMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        private readonly IMailService mailService;

        public CustomerMethodsController(IMailService mailService)
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
            this.mailService = mailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> CustomerLoginAsync([FromBody] Credential credential)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if(credential.user_id.Equals("") || credential.password.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (credentials)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();
               
                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.CustomerLoginAsync(credential.user_id, credential.password);

                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("search/customer/{any_value}")]
        public async Task<IActionResult> FindCustomerByValueAsync(string any_value, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.FindCustomerByValueAsync(any_value);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("search/customer/contact/{any_value}")]
        public async Task<IActionResult> FindCustomerByEmailOrContactNoAsync(string any_value, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.FindCustomerByEmailOrContactNoAsync(any_value);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        [HttpGet("search/customer/id/{custID}")]
        public async Task<IActionResult> FindCustomerDetailsAsync(int custID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();
                var oEntity = new CustomerMethods(gDb);

                var oKeys = await oEntity.VerifyCustomerKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                    if (oKeys.code != 200)
                    {
                        gDb.Dispose();
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
                        return new OkObjectResult(serviceResponse);
                    }

                }                

                serviceResponse = await oEntity.FindCustomerDetailsAsync(custID);


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
        public async Task<IActionResult> SaveCustomerAsync([FromBody] Customer customer, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (customer.customer_name.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (customer name)", "");
                    return new OkObjectResult(serviceResponse);
                }

                if (customer.email_address.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (email address)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.SaveUserAsync(customer);



                if(serviceResponse.code == 200)
                {
                    ServiceResponse mailResponse = new ServiceResponse();
                    try
                    {
                        dynamic jsonObjects = JsonSerializer.Deserialize<ExpandoObject > (serviceResponse.jsonData);

                        MailRequest mailRequest = new MailRequest(); //JsonSerializer.Deserialize<MailRequest>(jsonObjects.mail);

                        mailRequest.ToEmail = jsonObjects.ToEmail.GetString();
                        
                        mailRequest.Body = jsonObjects.Body.GetString();

                        mailRequest.Subject = jsonObjects.Subject.GetString();

                        mailRequest.textFormat = MimeKit.Text.TextFormat.Html;


                        await mailService.SendEmailAsync(mailRequest);

                        serviceResponse.SetValues(200, "Customer is notified through email.", serviceResponse.jsonData);
                    }
                    catch (Exception ex)
                    {
                        mailResponse.SetValues(500, ex.Message, "");
                    }

                    if(mailResponse.code != 200)
                    {
                        serviceResponse.SetValues(200, "Customer is saved but could not send the activation email. \r\nError: " + mailResponse.message, serviceResponse.jsonData);
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

        [HttpPut("reset/customer/id/{custID}")]
        public async Task<IActionResult> ResetCustomer(int custID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }               

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.FindCustomerDetailsAsync(custID);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                List<Customer> oCustomers = JsonSerializer.Deserialize<List<Customer>>(serviceResponse.jsonData);

                Customer oCustomer = oCustomers[0];

                serviceResponse = await oEntity.ResetUserAsync(oCustomer);

                if (serviceResponse.code == 200)
                {
                    ServiceResponse mailResponse = new ServiceResponse();
                    try
                    {
                        MailRequest mailRequest = new MailRequest();

                        mailRequest.ToEmail = oCustomer.email_address;
                        mailRequest.Body = serviceResponse.jsonData;
                        mailRequest.Subject = "WASH A LOAD ACCOUNT RESET";
                        mailRequest.textFormat = MimeKit.Text.TextFormat.Html;

                        await mailService.SendEmailAsync(mailRequest);

                        serviceResponse.SetValues(200, "Customer is notified through email.", serviceResponse.jsonData);
                    }
                    catch (Exception ex)
                    {
                        mailResponse.SetValues(500, ex.Message, "");
                    }

                    if (mailResponse.code != 200)
                    {
                        serviceResponse.SetValues(200, "Customer is saved but could not send the activation email \r\nError: " + mailResponse.message, serviceResponse.jsonData);
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

        [HttpPut("changestatus/customer/id/{custID}")]
        public async Task<IActionResult> ChangeCustomerStatus(int custID, int isActive, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.ActivateDeactivateUserAsync(custID, isActive);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("onlineactivation/customer/id/{custID}")]
        public async Task<IActionResult> OnlineUserActivationAsync(int custID, double lat, double lng, string password, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
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

                 serviceResponse = await oCustomer.OnlineUserActivationAsync(custID, lat, lng);

                if(serviceResponse.code == 200)
                {
                    serviceResponse = await oCustomer.ChangeUserPasswordAsync(custID, password);
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

        [HttpPut("changepassword/customer/id/{custID}")]
        public async Task<IActionResult> ChangePassword(int custID, [FromBody] string password, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (password.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (customer password)", "");
                    return new OkObjectResult(serviceResponse);
                }

                await gDb.Connection.OpenAsync();

                var oEntity = new CustomerMethods(gDb);

                var oKeys = await oEntity.VerifyCustomerKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                    if (oKeys.code != 200)
                    {
                        gDb.Dispose();
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
                        return new OkObjectResult(serviceResponse);
                    }

                }

                serviceResponse = await oEntity.ChangeUserPasswordAsync(custID, password);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("update/customer/id/{custID}")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] Customer customer, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oEntity = new CustomerMethods(gDb);

                var oKeys = await oEntity.VerifyCustomerKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                    if (oKeys.code != 200)
                    {
                        gDb.Dispose();
                        serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
                        return new OkObjectResult(serviceResponse);
                    }

                }

                serviceResponse = await oEntity.UpdateUserAsync(customer);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpDelete("delete/customer/id/{custID}")]
        public async Task<IActionResult> DeleteUserAsync(int custID, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);

                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);

                }

                var oEntity = new CustomerMethods(gDb);

                serviceResponse = await oEntity.DeleteUserAsync(custID);

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
