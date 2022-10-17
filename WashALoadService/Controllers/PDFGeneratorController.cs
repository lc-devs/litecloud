using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;
using WashALoadService.PDFGenerator;
using static WashALoadService.Common.Common;

namespace WashALoadService.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("washaloadservice/[controller]")]
    public class PDFGeneratorController : ControllerBase
    {
        private IConverter _converter;
        Templates templates  { get; set; }
        public AppDb_WashALoad gDb { get; }
        private SystemUserMethods oUser { get; set; }

        public PDFGeneratorController(IConverter converter)
        {
            _converter = converter;
            gDb = new AppDb_WashALoad();
            oUser = new SystemUserMethods(gDb);
            templates = new Templates(gDb);
        }

        [HttpGet("printQRCode")]
        public async Task<IActionResult> QRGetHTMLString(int soReference, bool isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();

            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }

            if (isIndustrial)
            {
                PickupIndustrialMethods pickup = new PickupIndustrialMethods(gDb);

                serviceResponse = await pickup.GetPickUpBySOReference(soReference);
            }
            else
            {
                PickupNonIndustrialMethods pickup = new PickupNonIndustrialMethods(gDb);

                serviceResponse = await pickup.GetPickUpBySOReference(soReference);
            }

            

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "QR Code",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.QRGetHTMLString(serviceResponse),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") }  
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            gDb.Dispose();

            var file = _converter.Convert(pdf);
            string filename = "qrcode" + soReference.ToString() + ".pdf";
            return File(file, "application/octet-stream", filename);

        }

        [HttpGet("collectionreport")]
        public async Task<IActionResult> CollectionReportGetHTMLString(string dateFrom, string dateTo, int customerID, string postedBy, string logicSite, bool isIndustrial, bool isNonIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            
            await gDb.Connection.OpenAsync();

            var oEntity = new PaymentReferenceMethods(gDb);

            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    serviceResponse = await oEntity.GetCollectionReport(dateFrom, dateTo, customerID, postedBy, logicSite, isIndustrial, isNonIndustrial);
                }
                
            }            

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10},
                DocumentTitle = "Collection Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.CollectionReportGetHTMLString(serviceResponse, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false},
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true,  Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            gDb.Dispose();

            var file = _converter.Convert(pdf);
            string filename = "collectionreport_" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);

        }

        [HttpGet("pickupreport")]
        public async Task<IActionResult> PickupReportGetHTMLString(string dateFrom, string dateTo, int customerID, string pickupBy, bool isAllEntries, bool isForPickupOnly, bool isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();

            

            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    if (isIndustrial)
                    {
                        var oEntity = new PickupIndustrialMethods(gDb);
                        serviceResponse = await oEntity.PickupQueryReportByDate(dateFrom, dateTo, customerID, pickupBy, isAllEntries, isForPickupOnly);
                    }
                    else
                    {
                        var oEntity = new PickupNonIndustrialMethods(gDb);
                        serviceResponse = await oEntity.PickupQueryReportByDate(dateFrom, dateTo, customerID, pickupBy, isAllEntries, isForPickupOnly);
                    }
                }
                
            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Pick-up Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.PickUpQueryReportGetHTMLString(serviceResponse, isIndustrial, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };          

            var file = _converter.Convert(pdf);

            string filename = "pickupreport_" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }
        
        [HttpGet("logisticreceivedreport")]
        public async Task<IActionResult> LogisticReceivedReportGetHTMLString(string dateFrom, string dateTo, int customerID, bool isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    if (isIndustrial)
                    {
                        var oEntity = new LogisticIndustrialMethods(gDb);
                        serviceResponse = await oEntity.ReceivedQueryReportByDate(dateFrom, dateTo, customerID);
                    }
                    else
                    {
                        var oEntity = new LogisticNonIndustrialMethods(gDb);
                        serviceResponse = await oEntity.ReceivedQueryReportByDate(dateFrom, dateTo, customerID);
                    }
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Logistic Received Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.LogisticReceivedQueryReportGetHTMLString(serviceResponse, isIndustrial, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "logisticreceivedreport_" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("laundryqueryreport")]
        public async Task<IActionResult> LaundryQueryReportGetHTMLString(string dateFrom, string dateTo, int customerID, LaundryStatus laundryStatus, bool isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    if (isIndustrial)
                    {
                        var oEntity = new LogisticIndustrialMethods(gDb);
                        serviceResponse = await oEntity.LaundryQueryReportByDate(dateFrom, dateTo, customerID,laundryStatus);
                    }
                    else
                    {
                        var oEntity = new LogisticNonIndustrialMethods(gDb);
                        serviceResponse = await oEntity.LaundryQueryReportByDate(dateFrom, dateTo, customerID, laundryStatus);
                    }
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Laundry Completed Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.LaundryQueryReportGetHTMLString(serviceResponse, isIndustrial, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "laundryqueryreport" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("laundrycompletedreport")]
        public async Task<IActionResult> LaundryCompletedQueryReportGetHTMLString(string dateFrom, string dateTo, int customerID, bool isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    if (isIndustrial)
                    {
                        var oEntity = new LogisticIndustrialMethods(gDb);
                        serviceResponse = await oEntity.ReceivedCompletedItemQueryReportByDate(dateFrom, dateTo, customerID);
                    }
                    else
                    {
                        var oEntity = new LogisticNonIndustrialMethods(gDb);
                        serviceResponse = await oEntity.ReceivedCompletedItemQueryReportByDate(dateFrom, dateTo, customerID);
                    }
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Laundry Completed Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.LaundryCompletedQueryReportGetHTMLString(serviceResponse, isIndustrial, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "laundrycompletedreport" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("invoicereport")]
        public async Task<IActionResult> InvoiceReportGetHTMLString(string dateFrom, string dateTo, int customerID, bool isIndustrial, [FromHeader] string UserKey, int isPaid, int isUnPaid, int isUnbilled)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    if (isIndustrial)
                    {
                        var oEntity = new LogisticIndustrialMethods(gDb);
                        serviceResponse = await oEntity.InvoiceQueryReportByDate(dateFrom, dateTo, customerID, isPaid, isUnPaid, isUnbilled);
                    }
                    else
                    {
                        var oEntity = new LogisticNonIndustrialMethods(gDb);
                        serviceResponse = await oEntity.InvoiceQueryReportByDate(dateFrom, dateTo, customerID, isPaid, isUnPaid, isUnbilled);
                    }
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Invoice Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.InvoiceReportGetHTMLString(serviceResponse, isIndustrial, dateFrom, dateTo, isPaid, isUnPaid, isUnbilled),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "invoicereport" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("billingreport")]
        public async Task<IActionResult> BillingReportGetHTMLString(string dateFrom, string dateTo, int customerID, string type, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                {
                    serviceResponse.SetValues(404, "Invalid request (dates)", "");
                }
                else
                {
                    var oEntity = new BillingMethods(gDb);
                    serviceResponse = await oEntity.GenerateBillingReport(dateFrom, dateTo, customerID, type);
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Billing Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.BillingReportGetHTMLString(serviceResponse, type, dateFrom, dateTo),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "billingreport" + dateFrom + "_" + dateTo + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("billing")]
        public async Task<IActionResult> BillingGetHTMLString(int billingReference, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();



            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {
                var oEntity = new BillingMethods(gDb);
                serviceResponse = await oEntity.GetBillingDetails(billingReference);

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Billing",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.BillingGetHTMLString(serviceResponse),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "billing" + "_" + billingReference.ToString() + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

        [HttpGet("invoice/{invoiceReference}")]
        public async Task<IActionResult> InvoiceGetHTMLString(int invoiceReference, int isIndustrial, [FromHeader] string UserKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            await gDb.Connection.OpenAsync();

            string invoiceDatetime = "";
            string invoiceBy = "";

            var oKeys = await oUser.VerifyUserKeyAsync(UserKey);
            if (oKeys.code != 200)
            {
                gDb.Dispose();
                serviceResponse.SetValues(StatusCodes.Status401Unauthorized, "Unauthorized access", "");
            }
            else
            {

                if(isIndustrial == 1)
                {
                    var oEntity = new LogisticIndustrialMethods(gDb);
                    serviceResponse = await oEntity.GetInvoiceDetails(invoiceReference);

                    if(serviceResponse.code == 200)
                    {
                        dynamic details = JsonSerializer.Deserialize<List<ExpandoObject>>(serviceResponse.jsonData);

                        invoiceDatetime = details[0].invoice_datetime.GetString();
                        invoiceBy = details[0].invoice_generated_by.GetString();

                        serviceResponse = await oEntity.FindSODetailsAsync(details[0].so_reference.GetInt32());
                    }
                }
                else
                {

                    var oEntity = new LogisticNonIndustrialMethods(gDb);
                    serviceResponse = await oEntity.GetInvoiceDetails(invoiceReference);

                    if (serviceResponse.code == 200)
                    {
                        dynamic details = JsonSerializer.Deserialize<List<ExpandoObject>>(serviceResponse.jsonData);
                        
                        invoiceDatetime = details[0].invoice_datetime;
                        invoiceBy = details[0].invoice_generated_by;

                        serviceResponse = await oEntity.FindSODetailsAsync(details[0].so_reference.GetInt32());
                    }
                }

            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Billing",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templates.InvoiceHTMLString(serviceResponse, invoiceReference, invoiceDatetime, invoiceBy),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 4, Line = true, Center = "This Report is a controlled document and shall not be reproduced or transmitted in any form or by any means, electronic or mechanical, including phototyping, recording, or by any information storage or retrieval system, without permission." }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            string filename = "invoice" + "_" + invoiceReference.ToString() + ".pdf";
            return File(file, "application/octet-stream", filename);
        }

    }
}
