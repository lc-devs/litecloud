using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.Models;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class BookingsForPickupMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        private CustomerMethods oCustomer { get; set; }

        public BookingsForPickupMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
            oCustomer = new CustomerMethods(gDb);
        }

        [HttpGet("bookings/customer/{customerid}")]
        public async Task<IActionResult> FindCustomersBookingAsync(int customerid, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindCustomersBookingAsync(customerid);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("bookings/dates/{date}")]
        public async Task<IActionResult> FindCustomersBookingByBookingDateAsync(string date, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                
                await gDb.Connection.OpenAsync();

                if (date.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid date).", "");
                    return new OkObjectResult(serviceResponse);
                }

                var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
                if (oKeys.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(oKeys);
                }

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindCustomersBookingByBookingDateAsync(date);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        [HttpGet("bookings/pickupby/{userid}")]
        public async Task<IActionResult> FindPickUpByUserAsync(int userid, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindPickUpByUserAsync(userid);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("bookings/reference/{refNo}")]
        public async Task<IActionResult> FindCustomersBookingByBookingReferenceAsync(string refNo, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                if (refNo.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid reference number).", "");
                    return new OkObjectResult(serviceResponse);
                }

                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindCustomersBookingByBookingReferenceAsync(refNo);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("bookings/reference/details/{refNo}")]
        public async Task<IActionResult> FindBookingDetailsAsync(string refNo, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await gDb.Connection.OpenAsync();

                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindBookingDetailsAsync(refNo);


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
        public async Task<IActionResult> SaveBookingAsync([FromBody] BookingsForPickup bookingsForPickup, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                System.TimeSpan diffResult = Convert.ToDateTime(bookingsForPickup.scheduled_pickup_datetime) - DateTime.Now;
                if (Convert.ToDateTime(bookingsForPickup.scheduled_pickup_datetime) < DateTime.Now || diffResult.TotalHours < 1)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (booking date)", "");
                    return new OkObjectResult(serviceResponse);
                }
                await gDb.Connection.OpenAsync();

               
                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.SaveBookingAsync(bookingsForPickup);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("cancelbooking/reference/{refNo}")]
        public async Task<IActionResult> CancelBookingAsync(string refNo, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (refNo.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (reference number)", "");
                    return new OkObjectResult(serviceResponse);
                }
                await gDb.Connection.OpenAsync();

                var oKeys = await oCustomer.VerifyCustomerKeyAsync(UserKey);

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

                var oEntity = new BookingsForPickupMethods(gDb);

                serviceResponse = await oEntity.FindCustomersBookingByBookingReferenceAsync(refNo);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.CancelBookingAsync(refNo);

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
