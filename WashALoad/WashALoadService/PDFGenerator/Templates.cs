using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Methods;

namespace WashALoadService.PDFGenerator
{
    public class Templates
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Templates(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public Templates() { }

        public string QRGetHTMLString(ServiceResponse oData)
        {

            var sb = new StringBuilder();

            sb.Append(@"
                        <html>
                             " + GenerateHeaderTag() + @"
                            <body>
                    ");

            if(oData.code == 200)
            {
                dynamic data = JsonSerializer.Deserialize<ExpandoObject>(oData.jsonData);

                DateTime pickupDate = Convert.ToDateTime(data.picked_up_datetime.GetString());

                sb.Append(@"<table class='table table-borderless' style='font-size:70px'; padding:0px>");
                sb.Append(@"<tbody>");
                sb.Append(@"<tr>
                                <td colspan=3 style='padding:0px'>
                                    <img style='height=50px; padding:0px' src='" + data.so_reference_QR_Image + @"' alt='' id='imageQR'>
                                </td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>SO:</td>
                                <td style='padding: 0px'>" + data.so_reference + @"</td>
                                <td style='padding:0px'>&nbsp;</td>
                            </tr>");
                sb.Append(@"</tbody>");
                sb.Append(@"</table>");

                sb.Append(@"<br/><br/><br/><br/><br/>");
                sb.Append(@"<br/><br/><br/><br/><br/>");

                sb.Append(@"<table class='table table-borderless' style='font-size:50px'; padding:0px>");
                sb.Append(@"<tbody>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>SO Reference</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.so_reference + @"</td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>Customer Name</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.customer_name + @"</td>
                            </tr>");
                sb.Append(@"<tr>
                                <td style='padding:0px'>Number of bags</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.number_of_bags + @"</td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>Number of loads</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.number_of_loads + @"</td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>Pickedup by</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.picked_up_by + @"</td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>Pickedup date/time</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + pickupDate.ToString("yyyy-MM-dd hh:mm:ss tt") + @"</td>
                            </tr>");

                sb.Append(@"<tr>
                                <td style='padding:0px'>Weight in kg</td>
                                <td style='padding: 0px'>:</td>
                                <td style='padding:0px'>" + data.weight_in_kg + @"</td>
                            </tr>");

                sb.Append(@"</tbody>");
                sb.Append(@"</table>");

                sb.Append(@"<br/><br/><br/><br/><br/>");

                sb.Append(@"<table id='' class='table table-borderless' style='font-size:50px; padding:0px;'>                                
                                <tbody>
                                    <tr>
                                        <td>Laundry Items</td>
                                        <td>Count</td>
                                    </tr>");

                                dynamic items = JsonSerializer.Deserialize<List<ExpandoObject>>(data.items.ToString());
                                foreach (dynamic item in items)
                                {
                                    sb.AppendFormat(@"
                                                    <tr>
                                                    <td>{0}</td>
                                                    <td>{1}</td>
                                                    </tr>", item.item_description,
                                                            item.item_count

                                    );

}

                                sb.Append(@"</tbody>
                            </table>");            
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            <br/><br/><div class = 'col-md-12 text-left'  style='font-size:30px; padding-left: 50px;'>
                                <div>
                                    <h2>Terms and Conditions:</h2> <br/>
                                </div>
                                <ol class = 'vue-ordered'>
                                    <li>Laundry will not be released without Job Order Slip</li>
                                    <li>
                                        Customer is responsible for:
                                        <ul class ='vue-list-inner'>
                                            <li>the sorting and separating of garments;</li>
                                            <li>checking pockets for items left inside them;</li>
                                            <li>checking garment labels for their proper wash and dry care;</li>
                                        </ul>
                                    </li>
                                    <li>WASH A LOAD is not responsible for clothing that bleeds, shrinks, or otherwise changes as a result of normal washing.</li
                                    <li>WASH A LOAD is not liable for lost or damaged items left in pockets.</li>
                                    <li>WASH A LOAD is not responsible for loss of or damage to any personal or non-cleanable items left in the clothing or bags such as money, jewelry, or anything else.</li>
                                    <li>WASH A LOAD will not be liable for any damages caused due to the natural effect of washer and dryer to the garments.</li>
                                    <li>WASH A LOAD will not be liable for any damage/s such as loss of buttons, shrinkage, fatiguing or fading. </li>
                                    <li>WASH A LOAD shall not be liable for any damage/s in case of fire, flood and other umforseen events.</li>
                                    <li>Articles not claimed within 30 days will be charged double to cover service cost and storage.</li>
                                    <li>Articles not claimed within 60 days will be disposed to cover service cost and storage.</li>
                                    <li>Liability of loss is limited to an amount not exceeding three (3) times the laundry charge.</li>
                                    <li>WASH A LOAD reserves the right to confirm accuracy of the articles for laundry and inform the customers of the descrepancy within 24 hours.</li>
                                    <li>Customer can lodge complaint within 24 hours from date of delivery or pick-up.</li>
                                </ol>
                            </div>
                       ");



            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }
        public string CollectionReportGetHTMLString(ServiceResponse oData, string dateFrom, string dateTo)
        {
            PaymentReferenceMethods paymentReference = new PaymentReferenceMethods(gDb);

            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            sb.Append(@"
                        <html>
                             " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Collection Report</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>
                            <br/>
                    ");
            
            if (oData.code == 200)
            {
                dynamic data = JsonSerializer.Deserialize<ExpandoObject>(oData.jsonData);

                sb.Append(@"    <table  class='table table-borderless' style='font-size:14px'>
                                    <tr>
                                        <th colspan='5'>Details</th>
                                        <th colspan='3'>Cash</th>
                                        <th colspan='3'>Non-Cash</th>
                                        <th colspan='2'>Site</th>
                                    </tr>
                                    <tr>
                                        <th>Payment Reference</th>
                                        <th>Date</th>
                                        <th>customer</th>
                                        <th>Type</th>
                                        <th>Payment Mode</th>
                                        <th>Full Billing Payment</th>
                                        <th>Per Invoice</th>
                                        <th>Floats</th>
                                        <th>Full Billing Payment</th>
                                        <th>Per Invoice</th>
                                        <th>Floats</th>
                                        <th>Site Posted</th>
                                        <th>Posted By</th>
                                    </tr>");
                dynamic records = JsonSerializer.Deserialize<List<ExpandoObject>>(data.records.ToString());

                foreach (dynamic item in records)
                {
                    DateTime payment_date = Convert.ToDateTime(item.payment_date.GetString());

                    double paid_cash_full_billing_payment = Convert.ToDouble(item.paid_cash_full_billing_payment.GetString());
                    double paid_cash_per_invoice_payment = Convert.ToDouble(item.paid_cash_per_invoice_payment.GetString());
                    double paid_cash_float_payment = Convert.ToDouble(item.paid_cash_float_payment.GetString());
                    double paid_noncash_full_billing_payment = Convert.ToDouble(item.paid_noncash_full_billing_payment.GetString());

                    double paid_noncash_per_invoice_payment = Convert.ToDouble(item.paid_noncash_per_invoice_payment.GetString());
                    double paid_noncash_float_payment = Convert.ToDouble(item.paid_noncash_float_payment.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td style = 'text-align:center'>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td style = 'text-align:center'>{4}</td>
                                    <td style = 'text-align:right'>{5}</td>
                                    <td style = 'text-align:right'>{6}</td>
                                    <td style = 'text-align:right'>{7}</td>
                                    <td style = 'text-align:right'>{8}</td>
                                    <td style = 'text-align:right'>{9}</td>
                                    <td style = 'text-align:right'>{10}</td>
                                    <td>{11}</td>
                                    <td>{12}</td>
                                  </tr>", item.payment_reference,
                                          payment_date.ToString("yyyy MMMM dd"), 
                                          item.customer_name, 
                                          item.type,
                                          item.payment_mode,
                                          paid_cash_full_billing_payment.ToString("#,##0.00"),
                                          paid_cash_per_invoice_payment.ToString("#,##0.00"),
                                          paid_cash_float_payment.ToString("#,##0.00"),
                                          paid_noncash_full_billing_payment.ToString("#,##0.00"),
                                          paid_noncash_per_invoice_payment.ToString("#,##0.00"),
                                          paid_noncash_float_payment.ToString("#,##0.00"),
                                          item.site,
                                          item.posted_by

                                );
                }

                dynamic summary = JsonSerializer.Deserialize<ExpandoObject>(data.summary.ToString());

                double total = Convert.ToDouble(summary.ttlPaidCash.GetString()) + Convert.ToDouble(summary.ttlPaidNonCash.GetString());
                double ttlPaidCashFullPayment = Convert.ToDouble(summary.ttlPaidCashFullPayment.GetString());
                double ttlPaidCashPerInvoice = Convert.ToDouble(summary.ttlPaidCashPerInvoice.GetString());
                double ttlPaidCashFloat = Convert.ToDouble(summary.ttlPaidCashFloat.GetString());
                double ttlPaidNonCashFullPayment = Convert.ToDouble(summary.ttlPaidNonCashFullPayment.GetString());
                double ttlPaidNonCashPerInvoice = Convert.ToDouble(summary.ttlPaidNonCashPerInvoice.GetString());
                double ttlPaidNonCashFloat = Convert.ToDouble(summary.ttlPaidNonCashFloat.GetString());
                double ttlPaidCash = Convert.ToDouble(summary.ttlPaidCash.GetString());
                double ttlPaidNonCash = Convert.ToDouble(summary.ttlPaidNonCash.GetString());

                sb.Append(@"
                                    <tr>
                                        <thcolspan='2'></th>
                                        <th>Count</th>
                                        <th>" + summary.ttlRecords + @"</th>
                                        <th></th>
                                        <th></th>
                                        <th>Total</th>
                                        <th style = 'text-align:right'>" + ttlPaidCashFullPayment.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'>" + ttlPaidCashPerInvoice.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'>" + ttlPaidCashFloat.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'>" + ttlPaidNonCashFullPayment.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'>" + ttlPaidNonCashPerInvoice.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'>" + ttlPaidNonCashFloat.ToString("#,##0.00") + @"</th>
                                        <th style = 'text-align:right'></th>
                                        <th style = 'text-align:right'></th>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th>Cash Collection</th>
                                        <th colspan='3' style = 'text-align:right'>" + ttlPaidCash.ToString("#,##0.00") + @"</th>                                       
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th>Non-Cash Collection</th>
                                        <th  colspan='3' style = 'text-align:right'>" + ttlPaidNonCash.ToString("#,##0.00") + @"</th>                                       
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th>Total Collection</th>
                                        <th  colspan='3' style = 'text-align:right'>" + total.ToString("#,##0.00") + @"</th>                                       
                                    </tr>
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");
            
            return sb.ToString();
        }
        public string PickUpQueryReportGetHTMLString(ServiceResponse oData, bool isIndustrial, string dateFrom, string dateTo)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            if (isIndustrial)
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial Pick-up Report</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <br/>"
                );
            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                        <div class='header'>Non-Industrial Pick-up Report</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );

            }
           

            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>SO Date</th>
                                        <th>SO Reference</th>
                                        <th>Customer</th>
                                        <th>Weight (Kg)</th>
                                        <th>Bag Count</th>
                                        <th>Pick-up Date and Time</th>
                                        <th>In Logistics</th>
                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {
                    int received_by_logistics = item.received_by_logistics.GetInt16();//J.Value == 1 ? "Yes" : "No";
                    DateTime picked_up_datetime = Convert.ToDateTime(item.picked_up_datetime.GetString());

                    if(received_by_logistics == 1)
                    {
                        sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>", picked_up_datetime.ToString("yyyy-MM-dd"),
                                          item.so_reference,
                                          item.customer_name,
                                          item.weight_in_kg,
                                          item.number_of_bags,
                                          item.picked_up_datetime,
                                         "Yes"

                                );
                    }
                    
                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string LogisticReceivedQueryReportGetHTMLString(ServiceResponse oData, bool isIndustrial, string dateFrom, string dateTo)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            if (isIndustrial)
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial SO References Received by Logistics for Laundry Servicing</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <br/>"
                );
            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Non-Industrial SO References Received by Logistics for Laundry Servicing</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );

            }


            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>SO Date</th>
                                        <th>SO Reference</th>
                                        <th>Customer</th>
                                        <th>Weight (Kg)</th>
                                        <th>Bag Count</th>
                                        <th>Date and Time Received</th>
                                        <th>Transfered To Laundry</th>
                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {
                    int received_by_laundry = item.received_by_laundry.GetInt16();//J.Value == 1 ? "Yes" : "No";
                    DateTime received_from_pickup_datetime = Convert.ToDateTime(item.received_from_pickup_datetime.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>", received_from_pickup_datetime.ToString("yyyy-MM-dd"),
                                          item.so_reference,
                                          item.customer_name,
                                          item.weight_in_kg,
                                          item.number_of_bags,
                                          item.received_from_pickup_datetime,
                                          received_by_laundry == 1 ? "Yes" : "No"

                                );

                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string LaundryCompletedQueryReportGetHTMLString(ServiceResponse oData, bool isIndustrial, string dateFrom, string dateTo)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            if (isIndustrial)
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial SO References Completed by Laundry and Received by Logistics</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <br/>"
                );
            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Non-Industrial SO References Completed by Laundry and Received by Logistics</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );

            }


            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>SO Date</th>
                                        <th>SO Reference</th>
                                        <th>Customer</th>
                                        <th>Bag Count</th>
                                        <th>Date and Time Received</th>
                                        <th>With Invoice</th>
                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {
                    int with_invoice = item.with_invoice.GetInt16();//J.Value == 1 ? "Yes" : "No";
                    
                    DateTime soDate = Convert.ToDateTime(item.soDate.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                  </tr>", soDate.ToString("yyyy-MM-dd"),
                                          item.so_reference,
                                          item.customer_name,
                                          item.number_of_bags,
                                          item.received_from_laundry_datetime,
                                          with_invoice == 1 ? "Yes" : "No"

                                );

                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string LaundryQueryReportGetHTMLString(ServiceResponse oData, bool isIndustrial, string dateFrom, string dateTo)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            if (isIndustrial)
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial SO References Laundry Query</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <br/>"
                );
            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Non-Industrial SO References Laundry Query</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );

            }


            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>SO Date</th>
                                        <th>SO Reference</th>
                                        <th>Customer</th>
                                        <th>Weight(KG)</th>
                                        <th>Bags</th>
                                        <th>Date and Time Received</th>
                                        <th>Done</th>
                                        <th>Transfered</th>
                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {

                    int completed_by_laundry = item.completed_by_laundry.GetInt16();
                    int received_from_laundry = item.received_from_laundry.GetInt16();

                    DateTime soDate = Convert.ToDateTime(item.soDate.GetString());

                    DateTime receviedDate = Convert.ToDateTime(item.received_by_laundry_datetime.GetString());


                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    <td>{7}</td>
                                  </tr>", soDate.ToString("yyyy-MM-dd"),
                                          item.so_reference,
                                          item.customer_name,
                                          item.weight_in_kg,
                                          item.number_of_bags,
                                          receviedDate.Year == 1900 ? "-" : receviedDate.ToString("yyyy-MM-dd hh:mm:ss tt"),
                                          completed_by_laundry == 1 ? "Yes" : "No",
                                          received_from_laundry == 1 ? "Yes" : "No"

                                );

                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string BillingGetHTMLString(ServiceResponse oData)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];


            //DateTime dtFrom = Convert.ToDateTime(dateFrom);
            //DateTime dtTo = Convert.ToDateTime(dateTo);

            sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Billing Statement</div>
                            <br/>"
               );


            if (oData.code == 200)
            {
                dynamic jsonObjects = JsonSerializer.Deserialize<ExpandoObject>(oData.jsonData);

                dynamic oJSONHeader = JsonSerializer.Deserialize<ExpandoObject>(jsonObjects.oJSONHeader.ToString());

                //display CUSTOMER AND BILL INFORMATION
                sb.Append(@" <table class='table table-borderless'>
                                <tbody>
                                    <tr>
                                        <td style='padding:0px'>Name of Client</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + oJSONHeader.customer_name  + @"</td>
                                        <td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td style='padding:0px'>Billing Reference</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + oJSONHeader.billing_reference.ToString() + @"</td>
                                        <td style='padding:0px' rowspan='4'><img src ='" + commonFunctions.GenerateQR(Convert.ToString(oJSONHeader.billing_reference.ToString())) + @"' style='height: 150px;'></td>


                                     </tr>");

                string billing_date = Convert.ToDateTime(oJSONHeader.billing_date.GetString()).ToString("yyyy MMMM dd");

                sb.Append(@" 
                                    <tr>
                                        <td style='padding:0px'>Client Type</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'> " + oJSONHeader.type.GetString() + @"</td>
                                        <td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td style='padding:0px'>Billing Date</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'> " + billing_date + @"</td>

                                    </tr>");

                sb.Append(@" 
                                    <tr>
                                        <td style='padding:0px'>Client Address</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'> " + oJSONHeader.address.GetString() + @"</td>
                                        <td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td style='padding:0px'>Current Period ADL</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'> " + oJSONHeader.current_ADL.GetDouble().ToString("#,##0.00") + @"</td>

                                    </tr>");

                sb.Append(@" 
                                    <tr>
                                        <td style='padding:0px'></td>
                                        <td style='padding:0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td><td style='padding:0px'>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td style='padding:0px'>Required ADL</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'> " + oJSONHeader.required_ADL.GetInt32() + @"</td>

                                    </tr>");

                sb.Append(@" </tbody></table>");




                //display PRIVIOUS UNPAID BILLS
                sb.Append(@" <h6>Previous unpaid bill(s)</h6> <br/>");
                sb.Append(@" <table class='table table-borderless table-earning'>
                                <tbody>
                                    <tr>
                                        <th>Entry #</th>
                                        <th>Billing Date</th>
                                        <th>Billing Reference</th>
                                        <th class='text-right'>Billing Ammount</th>

                                    </tr>");

                double totoalUnpaidBills = 0;
                int ctr = 1;

                dynamic UnpaidBills = JsonSerializer.Deserialize<List<ExpandoObject>>(jsonObjects.UnpaidBills.ToString());
                foreach (dynamic item in UnpaidBills)
                {                   
                    DateTime unPaidBillingDate = Convert.ToDateTime(item.billing_date.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td class='text-right'>{3}</td>
                                  </tr>", ctr,
                                          unPaidBillingDate.ToString("yyyy MMMM dd"),
                                          item.billing_reference,
                                          item.bill_amount

                                );
                    totoalUnpaidBills = totoalUnpaidBills + Convert.ToDouble(item.bill_amount.GetString());
                    ctr++;

                }
                if(totoalUnpaidBills > 0)
                {
                    sb.AppendFormat(@"<tr>
                                    <td></td>
                                    <td></td>
                                    <td class='text-right'><h6>Total</h6></td>
                                    <td class='text-right'><h6>{0}</h6></td>
                                  </tr>", totoalUnpaidBills.ToString("#,##0.00")

                               );
                }
                
                sb.Append(@"
                                </tbody>    
                                </table>");


                //display PRIVIOUS UNPAID INVOICES
                sb.Append(@" <br/><h6>Previous unpaid invoice(s)</h6> <br/>");
                sb.Append(@" <table class='table table-borderless table-earning'>
                                <tbody>
                                    <tr>
                                        <th>Entry #</th>
                                        <th>S.O. Reference</th>
                                        <th>S.O. Date</th>
                                        <th>Invoice Reference</th>
                                        <th>Invoice Date</th>
                                        <th>Invoice Ammount</th>
                                        <th>ADL Cost Adjustment</th>
                                        <th class='text-right'>Due Ammount</th>

                                    </tr>");

                ctr = 1;
                double totalUnpaidInvoice = 0;

                dynamic UnpaidInvoice = JsonSerializer.Deserialize<List<ExpandoObject>>(jsonObjects.UnpaidInvoice.ToString());

                foreach (dynamic item in UnpaidInvoice)
                {
                    DateTime SO_date = Convert.ToDateTime(item.SO_date.GetString());
                    DateTime invoice_datetime = Convert.ToDateTime(item.invoice_datetime.GetString());
                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td class='text-right'>{5}</td>
                                    <td class='text-right'>{6}</td>
                                    <td class='text-right'>{7}</td>
                                  </tr>", ctr, 
                                  item.so_reference, 
                                  SO_date.ToString("yyyy MMMM dd"), 
                                  item.invoice_reference,
                                  invoice_datetime.ToString("yyyy MMMM dd"),
                                  item.invoice_amount,
                                  item.ADL_adjustment,
                                  item.amount_due

                              );

                    totalUnpaidInvoice = totalUnpaidInvoice + Convert.ToDouble(item.amount_due.GetString());
                    ctr++;
                }

                if(totalUnpaidInvoice > 0)
                {
                    sb.AppendFormat(@"<tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class='text-right'><h6>Total<h/6></td>
                                    <td class='text-right'><h6>{0}</h6></td>
                                  </tr>", totalUnpaidInvoice.ToString("#,##0.00")

                              );
                }
                
                
                sb.Append(@"
                                </tbody>    
                                </table>");



                //display PRIVIOUS PAID INVOICES
                sb.Append(@" <br/><h6>Previous paid invoice(s) ADL Cost Adjustment</h6> <br/>");
                sb.Append(@" <table class='table table-borderless table-earning'>
                                <tbody>
                                    <tr>
                                        <th>Entry #</th>
                                        <th>S.O. Reference</th>
                                        <th>S.O. Date</th>
                                        <th>Payment Reference</th>
                                        <th>Payment Date</th>
                                        <th>Amount Paid</th>
                                        <th>ADL Cost Adjustment</th>
                                        <th class='text-right'>Due Ammount</th>

                                    </tr>");

                ctr = 1;
                double totalPaidInvoiceAmountDue = 0;

                dynamic PaidInvoice = JsonSerializer.Deserialize<List<ExpandoObject>>(jsonObjects.PaidInvoice.ToString());

                foreach (dynamic item in PaidInvoice)
                {
                    DateTime SO_date = Convert.ToDateTime(item.SO_date.GetString());
                    DateTime payment_date = Convert.ToDateTime(item.payment_date.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td class='text-right'>{5}</td>
                                    <td class='text-right'>{6}</td>
                                    <td class='text-right'>{7}</td>
                                  </tr>", ctr,
                                  item.so_reference,
                                  SO_date.ToString("yyyy MMMM dd"),
                                  item.payment_reference,
                                  payment_date.ToString("yyyy MMMM dd"),
                                  item.paid_amount,
                                  item.ADL_adjustment,
                                  Convert.ToDouble(Convert.ToDouble(item.paid_amount) - Convert.ToDouble(item.ADL_adjustment)).ToString("#,##0.00")

                              );

                    totalPaidInvoiceAmountDue = totalPaidInvoiceAmountDue - Convert.ToDouble(item.ADL_adjustment.GetString());
                    ctr++;
                }

                if(totalPaidInvoiceAmountDue > 0)
                {
                    sb.AppendFormat(@"<tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class='text-right'><h6>Total</h6></td>
                                    <td class='text-right'><h6>{0}</h6></td>
                                  </tr>", totalPaidInvoiceAmountDue.ToString("#,##0.00")

                              );
                }

                
                sb.Append(@"
                                </tbody>    
                                </table>");
                double totalAmountDue = totoalUnpaidBills + totalUnpaidInvoice - totalPaidInvoiceAmountDue;
                sb.Append(@"<br/><br/><div>
                                <label>Total amount due: &nbsp;</label><label><h5><u>" + AmountInWords .ConvertToWords(totalAmountDue.ToString()) + "(" + totalAmountDue.ToString("#,##0.00") + ")" + @"</u></h5></label>
                            </div>");

            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string InvoiceReportGetHTMLString(ServiceResponse oData, bool isIndustrial, string dateFrom, string dateTo, int isPaid, int isUnPaid, int isUnbilled)
        {
            var sb = new StringBuilder();
            var status = "";
            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);
            
            if((isPaid == 1)&&(isUnPaid) == 1)
            {
                status = "Paid and Unpaid";
            }else if((isPaid == 1) && (isUnPaid == 0))
            {
                status = "Paid Only";
            }else if((isPaid == 0)&&(isUnPaid == 1))
            {
                status = "Unpaid Only";
            }
            else if((isPaid == 0) && (isUnPaid == 0)&&(isUnbilled==1))
            {
                status = "Unbilled";
            }

            if (isIndustrial)
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial Invoice Report</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <div class='header'>Status: " + status+ @"</div>   
                            <br/>"
                );
            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Non-Industrial Invoice Report</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                         <div class='header'>Status: " + status + @"</div>   
                        <br/>"
                );

            }


            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>Invoice No</th>
                                        <th>Invoice Date</th>
                                        <th>SO Reference</th>
                                        <th>SO Date</th>
                                        <th>Customer</th>
                                        <th>Amount Due</th>
                                        <th>Paid</th>
                                        <th>Billing Reference</th>
                                        <th>Billing Date</th>
                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {
                    int paid = item.paid.GetInt16();//J.Value == 1 ? "Yes" : "No";
                    DateTime soDate = Convert.ToDateTime(item.soDate.GetString());
                    DateTime invoice_datetime = Convert.ToDateTime(item.invoice_datetime.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td class='text-right'>{5}</td>
                                    <td class='text-right'>{6}</td>
                                    <td>{7}</td>
                                    <td>{8}</td>
                                  </tr>", item.invoice_reference,
                                          invoice_datetime.ToString("yyyy-MM-dd"),
                                          item.so_reference,
                                          soDate.ToString("yyyy-MM-dd"),
                                          item.customer_name,
                                          item.invoice_amount,
                                          paid == 1 ? "Yes" : "No",
                                          item.billing_reference,
                                          item.billing_date

                                );

                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string BillingReportGetHTMLString(ServiceResponse oData, string type, string dateFrom, string dateTo)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            DateTime dtFrom = Convert.ToDateTime(dateFrom);
            DateTime dtTo = Convert.ToDateTime(dateTo);

            if (type.ToLower().Equals("industrial"))
            {
                sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'>Industrial Billing Report</div>
                            <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                            <br/>"
                );
            }
            else if (type.ToLower().Equals("non-industrial"))
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Non-Industrial Billing Report</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );

            }
            else
            {
                sb.Append(@"
                    <html>
                         " + GenerateHeaderTag() + @"
                        <body>
                        <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                         <div class='header'>Billing Report</div>
                         <div class='header'>Period: " + dtFrom.ToString("MMMM dd, yyyy") + " to " + dtTo.ToString("MMMM dd, yyyy") + @"</div>                            
                        <br/>"
                );
            }

            if (oData.code == 200)
            {
                List<ExpandoObject> jsonObjects = JsonSerializer.Deserialize<List<ExpandoObject>>(oData.jsonData);

                sb.Append(@"    <table class='table table-borderless table-earning'>
                                <tbody>
                                    <tr>
                                        <th style='text-align:left;'>Customer</th>
                                        <th style='text-align:center;'>Type</th>
                                        <th style='text-align:center;'>Billing Reference</th>
                                        <th style='text-align:center;'>Billing Date</th>
                                        <th style='text-align:right;'>Amount Due</th>
                                        <th style='text-align:center;'>Paid</th>

                                    </tr>");
                foreach (dynamic item in jsonObjects)
                {
                    int paid = item.paid.GetInt16();
                    DateTime billing_date = Convert.ToDateTime(item.billing_date.GetString());
                    //DateTime invoice_datetime = Convert.ToDateTime(item.invoice_datetime.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td style='text-align:left;'>{0}</td>
                                    <td style='text-align:center;'>{1}</td>
                                    <td style='text-align:center;'>{2}</td>
                                    <td style='text-align:center;'>{3}</td>
                                    <td style='text-align:right;'>{4}</td>
                                    <td style='text-align:center;'>{5}</td>
                                  </tr>", item.customer_name,
                                          item.type,
                                          item.billing_reference,
                                          billing_date.ToString("yyyy-MM-dd"),
                                          item.bill_amount,
                                          paid == 1 ? "Yes" : "No"
                                );

                }
                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        public string InvoiceHTMLString(ServiceResponse oData, int invoiceNo, string invoiceDateTime, string invoiceBy)
        {
            var sb = new StringBuilder();

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];


            sb.Append(@"
                        <html>
                            " + GenerateHeaderTag() + @"
                            <body>
                            <div><img src ='" + appURL + @"images/icon/logo.png'></div>
                            <div class='header'><h6>Invoice</h6></div>
                            <br/>"
                );


            if (oData.code == 200)
            {
                dynamic jsonObject = JsonSerializer.Deserialize<ExpandoObject>(oData.jsonData);

                sb.Append(@" <table class='table table-borderless'>
                                <tbody>
                                    <tr>
                                        <td style='padding:0px'>Invoice No</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + invoiceNo.ToString() + @"</td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding:0px'>Delivered By</td> 
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>_______________________</td> 
                                     </tr>
                                    <tr>
                                        <td style='padding:0px'>Invoice Date/Time</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + Convert.ToDateTime(invoiceDateTime).ToString("MMM dd, yyyy hh:mm tt") + @"</td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                     </tr>
                                    <tr>
                                        <td style='padding:0px'>SO Reference</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + jsonObject.so_reference + @"</td> 
                                        <td style='padding: 0px'></td>
                                        <td style='padding:0px'>Received By</td> 
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>_______________________</td>
                                     </tr>
                                    <tr>
                                        <td style='padding:0px'>Name of Client</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + jsonObject.customer_name + @"</td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                     </tr>
                                    <tr>
                                        <td style='padding:0px'>Generated By</td>
                                        <td style='padding: 0px'>:</td>
                                        <td style='padding:0px'>" + invoiceBy + @"</td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                        <td style='padding: 0px'></td>
                                     </tr>
                                </tbody>
                            </table>");

                sb.Append(@"<table class='table table-borderless table-striped table-earning'>
                                <tbody>
                                    <tr>
                                        <th>Item Code</th>
                                        <th class='text-left'>Item Description</th>
                                        <th class='text-center'>Count</th>
                                        <th class='text-center'>Cost</th>
                                        <th class='text-center'>Total</th>
                                    </tr>");


                dynamic items = JsonSerializer.Deserialize<List<ExpandoObject>>(jsonObject.items.ToString());

                double total = 0.0;

                foreach (dynamic item in items)
                {


                    double subtotal = Convert.ToInt32(item.item_count.GetInt32()) * Convert.ToDouble(item.cost.GetString());

                    sb.AppendFormat(@"<tr>
                                    <td class='text-center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='text-center'>{2}</td>
                                    <td class='text-right'>{3}</td>
                                    <td class='text-right'>{4}</td>
                                  </tr>", item.item_code,
                                          item.item_description,
                                          item.item_count,
                                          item.cost,
                                          subtotal.ToString("#,##0.00")

                                );
                    total = total + subtotal;
                }

                sb.Append(@"<tr>
                                <td colspan=4 class='text-right'><h4>Total</h4></td>
                                <td class='text-right'><h4>" + total.ToString("#,##0.00") + @"</h4></td>
                            </tr>");

                sb.Append(@"
                                </tbody>    
                                </table>");
            }
            else
            {
                sb.Append(oData.message);
            }

            sb.Append(@"
                        
                            </body>
                        </html>");

            return sb.ToString();
        }

        private string GenerateHeaderTag()
        {
            string header = "";

            var appURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WashALoadURL")["URL"];

            header = @"
                        <head>
                                <!-- Fontfaces CSS-->
                                <link href='"+ appURL + @"css/font-face.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/font-awesome-4.7/css/font-awesome.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/font-awesome-5/css/fontawesome-all.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/mdi-font/css/material-design-iconic-font.min.css' rel='stylesheet' media='all'>

                                <!-- Bootstrap CSS-->
                                <link href='" + appURL + @"vendor/bootstrap-4.1/bootstrap.min.css' rel='stylesheet' media='all'>

                                <!-- Vendor CSS-->
                                <link href='" + appURL + @"vendor/animsition/animsition.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/bootstrap-progressbar/bootstrap-progressbar-3.3.4.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/wow/animate.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/css-hamburgers/hamburgers.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/slick/slick.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/select2/select2.min.css' rel='stylesheet' media='all'>
                                <link href='" + appURL + @"vendor/perfect-scrollbar/perfect-scrollbar.css' rel='stylesheet' media='all'>

                                <!-- Main CSS-->
                                <link href='" + appURL + @"css/theme.css' rel='stylesheet' media='all'>
                        </head>";
            return header;
        }
    }
}
