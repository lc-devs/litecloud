using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;
using WashALoadService.Services;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class SendMailController : Controller
    {
        private readonly IMailService mailService;
        public SendMailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromBody] MailRequest request)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                await mailService.SendEmailAsync(request);

                serviceResponse.SetValues(200, "Email sent.", "");
            }
            catch (Exception ex)
            {
                serviceResponse.SetValues(500, ex.Message, "");
            }

            return new OkObjectResult(serviceResponse);
        }
    }
}
