using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class AccessMenuTemplateMethodsController : Controller
    {
        public AppDb_WashALoad gDb { get; }

        private SystemUserMethods oUser { get; set; }

        private CommonFunctions commonFunctions = new CommonFunctions();

        public AccessMenuTemplateMethodsController()
        {
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
        }

        [HttpGet]
        public async Task<IActionResult> FindAllAsync([FromHeader] string UserKey)
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

                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.FindAllAsync();


                gDb.Dispose();

               
            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return  new OkObjectResult(serviceResponse);

        }

        [HttpGet("{template_code}")]
        public async Task<IActionResult> FindTemplateDetailsAsync(string template_code, [FromHeader] string UserKey)
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

                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.FindTemplateDetailsAsync(template_code);


                gDb.Dispose();


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpGet("getmenu/{template_code}")]
        public async Task<IActionResult> GetMenuAsync(string template_code, [FromHeader] string UserKey)
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

                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.FindTemplateDetailsAsync(template_code);

                if(serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Bad request", "");
                    return new OkObjectResult(serviceResponse);
                }

                gDb.Dispose();

                List<PredefinedFunction> oEntities = JsonSerializer.Deserialize<List<PredefinedFunction>>(serviceResponse.jsonData);
                List<string> sections = new List<string>();
                List<string> menus = new List<string>();
                List<string> submenus = new List<string>();

                string strMenuDesktop = "";
                string strMenuMobile = "";

                int ttlSubMenu = 0;

                strMenuDesktop = strMenuDesktop + "\r\n" + @"<div class='logo'>
                                                    <a href='#'><img src='images/icon/logo.png' alt='Wash-A-Load' /></a>
                                                </div>
                                                <div class='col-12 px-0'>
                                                    <div class='card'>

                                                        <div class='card-body' id='userDetails'>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='menu-sidebar__content js-scrollbar1'>
                                                    <nav class='navbar-sidebar'>
                                                        <ul class='list-unstyled navbar__list'>";

                strMenuMobile = strMenuMobile + "\r\n" + @"<div class='header-mobile__bar'>
                                                                <div class='container-fluid'>
                                                                    <div class='header-mobile-inner'>
                                                                        <a class='logo' href='index.html'><img src='images/icon/logo.png' alt='WashALoad' /></a>
                                                                        <button class='hamburger hamburger--slider' type='button'>
                                                                            <span class='hamburger-box'>
                                                                                <span class='hamburger-inner'></span>
                                                                            </span>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <nav class='navbar-mobile'>
                                                                <div class='col-12 px-0'>
                                                                    <div class='card mb-0'>
                                                                        <div class='card-body' id='mobileUserDetails'>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class='container-fluid'>
                                                                    <ul class='navbar-mobile__list list-unstyled'>";

                foreach (var item in oEntities)
                {
                    if (!sections.Contains(item.section.description))
                    {
                        strMenuDesktop = strMenuDesktop + "\r\n" + $@"<div class='card' style='border: none;margin: 0px;'>
	                                            <div class='card-body' id='userDetails' style='padding: 0px;'> <p class='card-text' style='background-color: #e5e5e5;'><strong>{item.section.description}</strong></p></div>
                                            </div>";

                        strMenuMobile = strMenuMobile + "\r\n" + $@"<div class='card' style='border: none;margin: 0px;'>
	                                            <div class='card-body' id='mobileUserDetails' style='padding: 0px;'> <p class='card-text' style='background-color: #e5e5e5;'><strong>{item.section.description}</strong></p></div>
                                            </div>";

                        sections.Add(item.section.description);
                        menus.Clear();
                    }
                    //menu tag
                    if (item.menu_group.Equals(""))
                    {
                        strMenuDesktop = strMenuDesktop + "\r\n" + $@"<li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";
                        strMenuMobile = strMenuMobile + "\r\n" + $@"<li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";
                    }
                    else
                    {
                        if (!menus.Contains(item.menu_group))
                        {
                            ttlSubMenu = item.ttlsubmenu;


                            strMenuDesktop = strMenuDesktop + "\r\n" + $@"<li class='has-sub'>";
                            strMenuDesktop = strMenuDesktop + "\r\n" + $@"<a href='#' class='js-arrow'><i class='fas fa-caret-right'></i>{item.menu_group}</a>";
                            strMenuDesktop = strMenuDesktop + "\r\n" + $@" <ul class='list-unstyled navbar__sub-list js-sub-list'>";

                            strMenuDesktop = strMenuDesktop + "\r\n" + $@"  <li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";

                            strMenuMobile = strMenuMobile + "\r\n" + $@"<li class='has-sub'>";
                            strMenuMobile = strMenuMobile + "\r\n" + $@"<a href='#' class='js-arrow'><i class='fas fa-caret-right'></i>{item.menu_group}</a>";
                            strMenuMobile = strMenuMobile + "\r\n" + $@" <ul class='list-unstyled navbar__sub-list js-sub-list'>";

                            strMenuMobile = strMenuMobile + "\r\n" + $@"  <li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";
                            ttlSubMenu--;
                               
                            if(ttlSubMenu <= 0)
                            {
                                strMenuDesktop = strMenuDesktop + "\r\n" + $@"
                                                        </ul></li>";
                                strMenuMobile = strMenuMobile + "\r\n" + $@"
                                                        </ul></li>";
                            }
                               
                            menus.Add(item.menu_group);
                        }
                        else
                        {
                            //sub menu tag
                            strMenuDesktop = strMenuDesktop + "\r\n" + $@"  <li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";
                            strMenuMobile = strMenuMobile + "\r\n" + $@"  <li>
                                                        <a href='{item.page_path_filename}'><i class='{item.css_icon}'></i>{item.description}</a>
                                                    </li>";
                            ttlSubMenu--;

                            if (ttlSubMenu <= 0)
                            {
                                strMenuDesktop = strMenuDesktop + "\r\n" + $@"
                                                        </ul></li>";
                                strMenuMobile = strMenuMobile + "\r\n" + $@"
                                                        </ul></li>";
                            }
                        }
                    }

                }

                strMenuDesktop = strMenuDesktop + "\r\n" + $@"                            
                                                    </ul>
                                                </nav>
                                            </div>";

                strMenuMobile = strMenuMobile + "\r\n" + $@"                            
                                                    </ul>
                                                </div>
                                            </nav>";

                dynamic oMenus = new ExpandoObject();

                oMenus.desktop = strMenuDesktop;
                oMenus.mobile = strMenuMobile;

                string jsonString = JsonSerializer.Serialize(oMenus);


                serviceResponse.SetValues(200, "Success", jsonString);

                return new OkObjectResult(serviceResponse);


            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }
        [HttpPost]
        public async Task<IActionResult> SaveTemplateAsync([FromBody] AccessMenuTemplate template, [FromHeader] string UserKey)
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

                if (template.template_code.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid template code).", "");
                    return new OkObjectResult(serviceResponse);
                }

                if (template.section == null)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid section).", "");
                    return new OkObjectResult(serviceResponse);
                }

                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.SaveTemplateAsync(template);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPost("details/{template_code}")]
        public async Task<IActionResult> SaveTemplateDetailsAsync([FromBody] List<AccessMenuTemplateDetails> templateDetails, [FromHeader] string UserKey)
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

                var oEntity = new AccessMenuTemplateMethods(gDb);
                              
                serviceResponse = await oEntity.SaveTemplateDetailsAsync(templateDetails);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpPut("{template_code}")]
        public async Task<IActionResult> UpdateTemplateAsync([FromBody] AccessMenuTemplate template, [FromHeader] string UserKey)
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

                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.FindTemplateDetailsAsync(template.template_code);

                if(serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                if (template.template_code.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid template code).", "");
                    return new OkObjectResult(serviceResponse);
                }

                if (template.section == null)
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid section).", "");
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.UpdateTemplateAsync(template);

                gDb.Dispose();

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return new OkObjectResult(serviceResponse);

        }

        [HttpDelete("{template_code}")]
        public async Task<IActionResult> DeleteTemplateAsync(string template_code, [FromHeader] string UserKey)
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

                if (template_code.Equals(""))
                {
                    gDb.Dispose();
                    serviceResponse.SetValues(404, "Invalid request (invalid template code)", "");
                    return new OkObjectResult(serviceResponse);

                }


                var oEntity = new AccessMenuTemplateMethods(gDb);

                serviceResponse = await oEntity.FindTemplateDetailsAsync(template_code);

                if (serviceResponse.code != 200)
                {
                    gDb.Dispose();
                    return new OkObjectResult(serviceResponse);
                }

                serviceResponse = await oEntity.DeleteTemplateAsync(template_code);

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
