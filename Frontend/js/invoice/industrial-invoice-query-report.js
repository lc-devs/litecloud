var dateToday = new Date().toISOString().split('T')[0];
var CIFTable = GetDomElement("btnCustomerList");
var customerName = GetDomElement('txtCustomer');
var customerID = GetDomElement("customerId");
var tblInvoices = GetDomElement("tblInvoices");
var tblHiddenInvoicesBody = GetDomElement("tblHiddenInvoicesBody");
var emptyTable = GetDomElement("emptyTable");
var tblHiddenInvoices = GetDomElement("tblHiddenInvoices");

window.onload = () => {
    GetDomElement("dateFrom").value = dateToday;
    GetDomElement("dateTo").value = dateToday;
}

GetDomElement("chkCustomer").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("btnFindCustomer").disabled = false;
        GetDomElement("txtCustomer").classList.add("input-required");
    } else {
        GetDomElement("btnFindCustomer").disabled = true;
        GetDomElement("txtCustomer").classList.remove("input-required");
        GetDomElement("txtCustomer").classList.remove("is-invalid");
        GetDomElement("txtCustomer").value = "";
        GetDomElement("customerId").value = "";

    }
});

GetDomElement("btnExcel").addEventListener("click" ,function(){
    var dateGenerated = new Date().toISOString().split('T')[0];
    fnExcelReport(`Industrial Query Report`, "tblHiddenInvoices", `${dateGenerated}-industrial-query-report`, dateFrom.value, dateTo.value, customerName.value);
});

GetDomElement("btnPDF").onclick = ()=>{
    GeneratePDF();
};

GetDomElement("btnPrintNow").onclick = ()=>{
    GeneratePDF();
};


function GeneratePDF(){
    var Ispaid;
    var Isunpaid;
    var Isunbilled;
    var selStatus = GetDomElement("selStatus").value;
    if (selStatus == 1) {
        Ispaid = 1;
        Isunpaid = 1;
        Isunbilled = 0;
    } else if (selStatus == 2) {
        Ispaid = 1;
        Isunpaid = 0;
        Isunbilled = 0;
    } else if (selStatus == 3) {
        Ispaid = 0;
        Isunpaid = 1;
        Isunbilled = 0;
    } else if (selStatus == 4) {
        Ispaid = 0;
        Isunpaid = 0;
        Isunbilled = 1;
    }
    var oGeneratePDF =`PDFGenerator/invoicereport?dateFrom=${dateFrom.value}&dateTo=${dateTo.value}&customerID=${customerName.value}&isIndustrial=true&isPaid=${Ispaid}&isUnPaid=${Isunpaid}&isUnbilled=${Isunbilled}`;
    Download(oGeneratePDF, `${dateToday}-industrial-invoice-query-report.pdf`);
}

GetDomElement("btnGenerateReport").addEventListener('click', function () {
    GetDomElement("btnPrintNow").disabled = true;
    GetDomElement("saveNow").disabled = true;
    var selStatus = GetDomElement("selStatus").value;
    var requiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(requiredFields, "divInvalidQuery");
    if (bValidForm) {
        var dateFrom = GetDomElement("dateFrom").value;
        var dateTo = GetDomElement("dateTo").value;
        var customerId = GetDomElement("customerId").value;
        var paid;
        var unpaid;
        var unbilled;

        if (selStatus == 1) {
            paid = 1;
            unpaid = 1;
            unbilled = 0;
        } else if (selStatus == 2) {
            paid = 1;
            unpaid = 0;
            unbilled = 0;
        } else if (selStatus == 3) {
            paid = 0;
            unpaid = 1;
            unbilled = 0;
        } else if (selStatus == 4) {
            paid = 0;
            unpaid = 0;
            unbilled = 1;
        }

        var oInvoices = InvokeService(GetURL(), `LogisticIndustrialMethods/invoicequeryreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}&isPaid=${paid}&isUnPaid=${unpaid}&isUnBilled=${unbilled}`, "GET", "");
        tblInvoices.innerHTML = "";
        if (oInvoices.code == 200) {
            emptyTable.classList.add('d-none');
            var oData = JSON.parse(oInvoices.data);
            if (oData.code == 200) {
                GetDomElement("btnPrintNow").disabled = false;
                GetDomElement("saveNow").disabled = false;
                emptyTable.classList.add('d-none');
                var oJsonData = JSON.parse(oData.jsonData);
                tblInvoices.innerHTML = "";
                for (var i = 0; i < oJsonData.length; i++) {
                    tblInvoices.innerHTML +=
                        "<tr class='border' style='cursor:default'><td class='text-capitalize'><p" +  ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnSODetails' onclick = "PopulateInvoiceSOReferenceDetails(${oJsonData[i].so_reference},${oJsonData[i].invoice_reference})"`+
                        ">" +oJsonData[i].invoice_reference +
                        "</p></td><td>" + moment(oJsonData[i].invoice_datetime).format('lll') +
                        "</td><td>" + oJsonData[i].so_reference +
                        "</td><td>" + moment(oJsonData[i].soDate).format('lll') +
                        "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                        "</td><td class='text-right' style='padding-right:0px;'>" + reformatAmount(oJsonData[i].invoice_amount) +
                        "</td><td>" + `${oJsonData[i].paid==0?"<i class='fas fa-times-circle text-danger'></i>":"<i class='fas fa-check-circle text-success'></i>"}` +
                        "</td><td><p" + ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnBillingReference' onclick = "PopulateBillingDetails(${oJsonData[i].billing_reference})"`+
                         ">"+(oJsonData[i].billing_reference == 0?"":oJsonData[i].billing_reference) +
                    "</p></td><td>" + (oJsonData[i].billing_date == ""?"":moment(oJsonData[i].billing_date).format('LL')) +
                        "</td></tr>";
                    
                }
                for (var i = 0; i < oJsonData.length; i++) {                    
                    tblHiddenInvoicesBody.innerHTML +=
                        "<tr class='border' style='cursor:default'><td class='text-capitalize'><p" +  ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnSODetails' onclick = "PopulateInvoiceSOReferenceDetails(${oJsonData[i].so_reference},${oJsonData[i].invoice_reference})"`+
                        ">" +oJsonData[i].invoice_reference +
                        "</p></td><td>" + moment(oJsonData[i].invoice_datetime).format('lll') +
                        "</td><td>" + oJsonData[i].so_reference +
                        "</td><td>" + moment(oJsonData[i].soDate).format('lll') +
                        "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                        "</td><td class='text-right' style='padding-right:0px;>" + reformatAmount(oJsonData[i].invoice_amount) +
                        "</td><td>" + `${oJsonData[i].paid==0?"No":"Yes"}` +
                        "</td><td><p" + ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnBillingReference' onclick = "PopulateBillingDetails(${oJsonData[i].billing_reference})"`+
                         ">"+ (oJsonData[i].billing_reference == 0?"":oJsonData[i].billing_reference) +
                        "</p></td><td>" + (oJsonData[i].billing_date == ""?"":moment(oJsonData[i].billing_date).format('LL')) +
                        "</td></tr>";
                }

            } else if (oData.code == 404 || oData.code == 204) {
                tblInvoices.innerHTML = "";
                emptyTable.classList.remove('d-none');
            } else {
                ShowErrorModalOnLoad(oData.message, oData.code);
            }
        } else if (oInvoices.code == 204 || oInvoices.code == 404) {
            tblInvoices.innerHTML = "";
            emptyTable.classList.remove('d-none');

        } else {
            emptyTable.classList.add('d-none');
            ShowErrorModalOnLoad(oInvoices.message, oInvoices.code);
        }


    }
});

GetDomElement("btnPrintInvoiceInfo").addEventListener('click', function(){
    var referenceNo = GetDomElement("invoice_reference").innerText;
    var oGeneratePDF =`PDFGenerator/invoice/${referenceNo}?isIndustrial=1`;
    Download(oGeneratePDF, `${referenceNo}-industrial-invoice-details.pdf`);
});

GetDomElement("btnSearchClient").onclick = () => {
    PopulatInvoiceCustomerList();
}
GetDomElement("txtSearchClient").addEventListener('keydown', function (e) {
    if (e.key == 'Enter') {
        PopulatInvoiceCustomerList();

    }
});

GetDomElement("closeInvoice").onclick = ()=>{
    GetDomElement("btnCloseInvoiceModal").click();
}
// GetDomElement("closeBilling").onclick = ()=>{
//     GetDomElement("btnCloseBillingModa").click();
// }

function PopulateInvoiceDetails(invoice_reference){
    var oInvoiceDetails = InvokeService(GetURL(),`LogisticIndustrialMethods/invoicedetails/${invoice_reference}`,"GET","");
    if (oInvoiceDetails.code == 200) {
        var oData = JSON.parse(oInvoiceDetails.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            GetDomElement("pInvoiceReference").innerText = invoice_reference;
            GetDomElement("pInvoiceDate").innerText = moment(oJsonData[0].invoice_datetime).format('lll');
            GetDomElement("pInvoiceAmount").innerText = oJsonData[0].invoice_amount;
            GetDomElement("pCustomerName").innerText = oJsonData[0].customer_name;
            GetDomElement("pSOReference").innerText = oJsonData[0].so_reference;
            GetDomElement("pSODate").innerText = moment(oJsonData[0].soDate).format('lll');
            GetDomElement("pIsPaid").innerText = `${oJsonData[0].paid==0?'no':'yes'}`;
            GetDomElement("pIsBilled").innerText = `${oJsonData[0].billed==0?'no':'yes'}`;
            GetDomElement("pPaymentReference").innerText = `${oJsonData[0].payment_reference ==''?'----':oJsonData[0].payment_reference}`;
        }  else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    }  else {
        ShowErrorModalOnLoad(oInvoiceDetails.message, oInvoiceDetails.code);
    }

}

function PopulateBillingDetails(billing_reference){
    var oBillingDetails = InvokeService(GetURL(),`BillingMethods/billingdetails/${billing_reference}`,"GET","");
    if (oBillingDetails.code == 200) {
        var oData = JSON.parse(oBillingDetails.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            GetDomElement("pBillingReference").innerText = billing_reference;
            GetDomElement("pBillingeDate").innerText = moment(oJsonData.oJSONHeader.billing_date).format('LL');
            GetDomElement("pBillingCustomerName").innerText = oJsonData.oJSONHeader.customer_name;
            GetDomElement("pCustomerAddress").innerText = oJsonData.oJSONHeader.address;
            GetDomElement("pCustomerType").innerText = oJsonData.oJSONHeader.type;
            GetDomElement("pADL").innerText =oJsonData.oJSONHeader.current_ADL;
            GetDomElement("pIsBillingPaid").innerText = `${oJsonData.oJSONHeader.paid==0?'yes':'no'}`;
            if(oJsonData.oJSONHeader.billing_reference_QR_image !=""){
                GetDomElement("divQRImage").classList.remove("d-none");
                GetDomElement('billingQRImage').src = oJsonData.oJSONHeader.billing_reference_QR_image;
            }else{
                GetDomElement("divQRImage").classList.add("d-none");
            }
            // GetDomElement("pIsBilled").innerText = `${oJsonData.oJSONHeader.billed==0?'yes':'no'}`;
            // GetDomElement("pPaymentReference").innerText = `${oJsonData.oJSONHeader.payment_reference ==''?'----':oJsonData[0].payment_reference}`;
        }  else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    }  else {
        ShowErrorModalOnLoad(oBillingDetails.message, oBillingDetails.code);
    }

}

function PopulatInvoiceCustomerList() {

    var clientSearchText = GetDomElement("txtSearchClient");
    var oCustomers = InvokeService(GetURL(), `CustomerMethods/search/customer/${clientSearchText.value}`, "GET", "");
    if (oCustomers.code == 200) {
        emptyCustomerList.classList.add('d-none');
        var oData = JSON.parse(oCustomers.data);
        if (oData.code == 200) {
            emptyCustomerList.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            CIFTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                CIFTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].customer_id +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    "</td><td>" + oJsonData[i].cellular_number +
                    "</td><td>" + oJsonData[i].email_address +
                    "</td></tr>";
            }
            var cifRows = CIFTable.rows;
            for (var i = 0; i < cifRows.length; i++) {
                cifRows[i].addEventListener('click', function () {
                    GetDomElement('btnPrintNow').disabled = true;
                    GetDomElement('saveNow').disabled = true;
                    customerName.value = this.cells[1].innerHTML;
                    customerID.value = this.cells[0].innerHTML;
                    GetDomElement("btnCloseClientList").click();


                });
            }


        } else if (oData.code == 404 || oData.code == 204) {
            CIFTable.innerHTML = "";
            emptyCustomerList.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oCustomers.code == 204 || oCustomers.code == 404) {
        CIFTable.innerHTML = "";
        emptyCustomerList.classList.remove('d-none');

    } else {
        emptyCustomerList.classList.add('d-none');
        ShowErrorModalOnLoad(oCustomers.message, oCustomers.code);
    }

}

function PopulateInvoiceSOReferenceDetails(soReference, invoiceReference) {
    var rawServerResponse = InvokeService(GetURL(), `LogisticIndustrialMethods/sodetails/${soReference}`, "GET", "");
    var rawInvoiceServiceResponse = InvokeService(GetURL(), `LogisticIndustrialMethods/invoicedetails/${invoiceReference}`, "GET", "");
    if(rawInvoiceServiceResponse.code == 200){
        var jsonInvoiceResponse = JSON.parse(rawInvoiceServiceResponse.data); 
        if(jsonInvoiceResponse.code == 200){
          var invoiceJsonData = JSON.parse(jsonInvoiceResponse.jsonData);
          console.log(invoiceJsonData)
          GetDomElement("invoice_date").textContent = moment(invoiceJsonData[0].invoice_datetime).format('LLL');
          GetDomElement("generated_by").textContent = invoiceJsonData[0].invoice_generated_by;
        }else{
            ShowErrorModalOnLoad("No data found", 404);
        }
    }else{
        ShowErrorModalOnLoad("Server Offline. Please contact administrator", 500);
    }
    if (rawServerResponse.code == 200) {
        var jsonResponse = JSON.parse(rawServerResponse.data);
        if (jsonResponse.code == 200) {
            var soDetails = JSON.parse(jsonResponse.jsonData);
            console.log(soDetails)
            GetDomElement("so_reference").innerText = soDetails.so_reference;
            GetDomElement("invoice_reference").innerText =invoiceReference;
            GetDomElement("customer_name").innerText =  decodeHtml(soDetails.customer_name);
            // GetDomElement("delivered_by").innerText = `${soDetails.delivered_by==""?"----":soDetails.delivered_by}`;
            // GetDomElement("received_by").innerText = `${soDetails.received_by_laundry_user==""?"----":soDetails.received_by_laundry_user}`;
            if (soDetails.items != undefined) {
                var divtbl = GetDomElement('tblSODetails');
                divtbl.innerHTML = "";
                for (var i = 0; i < soDetails.items.length; i++) {
                    divtbl.innerHTML +=
                        "<tr> " +
                        "<td class='border text-center px-0'>" + soDetails.items[i].item_code + "</td>" +
                        "<td class='border text-center px-0 text-capitalize'>" + soDetails.items[i].item_description + "</td>" +
                    "<td class='border text-center px-0'>" + soDetails.items[i].item_count + "</td>" +
                    "<td class='border text-center px-0'>" + `${currency((soDetails.items[i].cost), { separator: ',', symbol: ''}).format()}` + "</td>" +
                    "<td class='border text-center px-0'>" + `${currency((soDetails.items[i].item_count) * (parseFloat((soDetails.items[i].cost))), { separator: ',', symbol: ''}).format()}` + "</td>" +
                        "</tr>";
                }
                var partialTotalRows = divtbl.rows;
                var master_total = 0.0;
                for (var i = 0; i < partialTotalRows.length; i++) {
                    master_total = currency(master_total).add(partialTotalRows[i].cells[4].innerText);
                }
                GetDomElement("master_total").innerText = currency(master_total, { separator: ',', symbol: ''}).format();
            }
        }else{
            ShowErrorModalOnLoad("No data found", 404);
        }
    }else{
        ShowErrorModalOnLoad("Server Offline. Please contact administrator", 500);
        
    }
}
ClosePopupModal("btnOKSODetails", "btnCloseSODetails");