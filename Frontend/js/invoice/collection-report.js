var oLogisticSites = [];
var oSystemUsers = [];
var CIFTable = GetDomElement("btnCustomerList");
var customerName = GetDomElement('txtCustomer');
var paymentMethodTable = GetDomElement("tblBody");
var customerID = GetDomElement("customerId");
var SaveTable = GetDomElement("SaveTable");
var SaveTableBody = GetDomElement("SaveBody");
var isIndustrial = true;
var isNonIndustrial = true;
var saveNow = GetDomElement("saveNow");
var printNow = GetDomElement("btnPrintNow");
var dateToday = new Date().toISOString().split('T')[0];
var dateFrom = GetDomElement("from");
var dateTo = GetDomElement("to");
var postedUser = GetDomElement("selPostedBy");

window.addEventListener('load', function () {
    GetAllLogisticsSites();
    PopulateSiteLocationList("selLogisticsSite", oLogisticSites);
    PopulateSystemUsers();
    GetDomElement("from").value = dateToday;
    GetDomElement("to").value = dateToday;
    getAllUsers();
    GetSystemUserName(1);
});

GetDomElement("clientType").addEventListener('click', function () {
    if (this.value == 0) {
        isIndustrial = true;
        isNonIndustrial = true;
    } else if (this.value == 1) {
        isIndustrial = true;
        isNonIndustrial = false;
    } else if (this.value == 2) {
        isIndustrial = false;
        isNonIndustrial = true;

    }
});

GetDomElement("btnExcelFile").addEventListener('click', function () {
    var type;
    if (isIndustrial && isNonIndustrial) {
        type = "All Customers";
    } else if (isIndustrial && !isNonIndustrial) {
        type = "Industrial";
    } else {
        type = "Non Industrial";
    }
    var logisticsSite = GetDomElement("txtCustomer").value;
    var postedUser = GetDomElement("selPostedBy").value;

    var pdfTitle = "Collection Report";
    var dateGenerated = new Date().toISOString().split('T')[0];
    startDate = `${moment(GetDomElement("from").value).format('LL')}`;
    endDate = `${moment(GetDomElement("to").value).format('LL')}`;
    customer = `${capitalize(GetDomElement('txtCustomer').value)}`;
    fnExcelCollectionReport(pdfTitle, "tblSearch", `${dateGenerated}-collection-report`, startDate, endDate, customer, type, logisticsSite, postedUser);
    GetDomElement("btnCloseFileType").click();
});

GetDomElement("btnGenerateReport").addEventListener('click', function () {
    var btnPrintNow = GetDomElement("btnPrintNow").value;
    var btnSaveNow = GetDomElement("saveNow").value;
    var requiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(requiredFields, "divUpdateInvalidForm");
    if (bValidForm) {
        btnPrintNow.disabled = false;
        btnSaveNow.disabled = false;
        PopulateCollectionQueryReport();
    } else {
        btnPrintNow.disabled = true;
        btnSaveNow.disabled = true;
    }
});

GetDomElement("txtSearchClient").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        PopulateCustomerList();
    }
});

GetDomElement("btnSearchClient").addEventListener('click', function () {
    PopulateCustomerList();
});

GetDomElement("btnPDFFILe").addEventListener('click', function(){
    PDFCollectionReport();
});
GetDomElement("btnPrintNow").addEventListener('click', function(){
    PDFCollectionReport();
});



GetDomElement("chkLogisticsSite").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("selLogisticsSite").disabled = false;
        GetDomElement("selLogisticsSite").classList.add("input-required");
    } else {
        GetDomElement("selLogisticsSite").disabled = true;
        GetDomElement("selLogisticsSite").selectedIndex = 0;
        GetDomElement("selLogisticsSite").classList.remove("input-required");
        GetDomElement("selLogisticsSite").classList.remove('is-invalid');
    }
});
GetDomElement("chkPostedBy").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("selPostedBy").disabled = false;
        GetDomElement("selPostedBy").classList.add('input-required');
    } else {
        GetDomElement("selPostedBy").disabled = true;
        GetDomElement("selPostedBy").selectedIndex = 0;
        GetDomElement("selPostedBy").classList.remove('input-required');
        GetDomElement("selPostedBy").classList.remove('is-invalid');
    }
});
GetDomElement("chkCustomer").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("btnSearchCustomer").disabled = false;
        GetDomElement("txtCustomer").classList.add('input-required');
    } else {
        GetDomElement("btnSearchCustomer").disabled = true;
        GetDomElement("txtCustomer").value = "";
        GetDomElement("txtCustomer").classList.remove('input-required');
        GetDomElement("txtCustomer").classList.remove('is-invalid');
    }
});


function GetAllLogisticsSites() {
    oLogisticSites = InvokeService(GetURL(), "LogisticSiteMethods", "GET", "");
    if (oLogisticSites.code == 200) {


    } else if (oLogisticSites.code.code == 204 || oLogisticSites.code.code == 404) {

    } else {
        ShowErrorModalOnLoad(oLogisticSites.message, oLogisticSites.code);
    }
}

function PDFCollectionReport() {
    var clientType = GetDomElement("clientType").value;
    var industrial = false;
    var nonIndustrial = false;
    if (clientType == 0) {
        industrial = true;
        nonIndustrial = true;
    } else if (clientType == 1) {
        industrial = true;
        nonIndustrial = false;
    } else if (clientType == 2) {
        industrial = false;
        nonIndustrial = true;
    }
    var controllerURL = `PDFGenerator/collectionreport?dateFrom=${dateFrom.value}&dateTo=${dateTo.value}&postedBy=${postedUser.value}&isIndustrial=${industrial}&isNonIndustrial=${nonIndustrial}`;
    Download(controllerURL, `${dateToday}-collection-report.pdf`);
}

function PopulateCollectionQueryReport() {
    saveNow.disabled = true;
    printNow.disabled = true;
    GetDomElement('totals').classList.add('d-none');
    var emptyTable = GetDomElement("emptyTable");
    var dateFrom = GetDomElement('from').value;
    var dateTo = GetDomElement('to').value;
    var branchName = ReturnInputValue("selLogisticsSite");
    var systemPosted = ReturnInputValue("selPostedBy");
    emptyTable.classList.add('d-none');
    var collectionReport = InvokeService(GetURL(), `PaymentReferenceMethods/collectionreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerID.value}&postedBy=${systemPosted}&logicSite=${branchName}&isIndustrial=${isIndustrial}&isNonIndustrial=${isNonIndustrial}`, "GET", "");
    if (collectionReport.code == 200) {
        var oData = JSON.parse(collectionReport.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            paymentMethodTable.innerHTML = "";
            var jsonRecords = oJsonData.records;
            saveNow.disabled = false;
            printNow.disabled = false;
            for (var i = 0; i < jsonRecords.length; i++) {
                paymentMethodTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td class='text-uppercase pl-3'>" + jsonRecords[i].payment_reference +
                    "</td><td class='text-capitalize pl-3'>" + moment(jsonRecords[i].payment_date).format('ll') +
                    "</td><td class='text-capitalize pl-3'>" + truncateName(jsonRecords[i].customer_name) +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].type +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].payment_mode +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_cash_full_billing_payment) +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_cash_per_invoice_payment) +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_cash_float_payment) +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_noncash_full_billing_payment) +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_noncash_per_invoice_payment) +
                    "</td><td class='text-right' style='padding-right:0.75rem'>" + reformatAmount(jsonRecords[i].paid_noncash_float_payment) +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].site +
                    "</td><td class='text-capitalize pl-3'>" + GetSystemUserName(jsonRecords[i].posted_by) +
                    "</td>" +
                    "</tr>";
            }
             SaveTableBody.innerHTML = "";
            for (var i = 0; i < jsonRecords.length; i++) {
                SaveTableBody.innerHTML +=
                    "<tr class='border' style='cursor:default'><td class='text-uppercase pl-3'>" + jsonRecords[i].payment_reference +
                    "</td><td class='text-capitalize pl-3'>" + moment(jsonRecords[i].payment_date).format('ll') +
                    "</td><td class='text-capitalize pl-3'>" + truncateName(jsonRecords[i].customer_name) +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].type +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].payment_mode +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_cash_full_billing_payment) +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_cash_per_invoice_payment) +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_cash_float_payment) +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_noncash_full_billing_payment) +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_noncash_per_invoice_payment) +
                    "</td><td class='text-right'>" + reformatAmount(jsonRecords[i].paid_noncash_float_payment) +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].site +
                    "</td><td class='text-capitalize pl-3'>" + jsonRecords[i].posted_by +
                    "</td>" +
                    "</tr>";
            }
            var jsonSummary = oJsonData.summary;
            paymentMethodTable.innerHTML +=
                "<tr class='bg-primary'>" +                
                "<th class='border text-white  text-center'>Count</th>" +
                "<th class='border text-white  text-center'>" + reformatAmount(jsonSummary.ttlRecords) +
                "<th></th>" +
                "<th></th>" +
                "</th><th class='text-white border text-center'>Total</th>" +
                "<th class='text-white border text-right'>" + jsonSummary.ttlPaidCashFullPayment +
                "</th><th class=' text-white border text-right'>" + jsonSummary.ttlPaidCashPerInvoice +
                "</th> <th class=' text-white border text-right'>" + jsonSummary.ttlPaidCashFloat + "</th>" +
                "<th class='text-white border text-right'>" + jsonSummary.ttlPaidNonCashFullPayment +
                "</th><th class='text-white border text-right'>" + jsonSummary.ttlPaidNonCashPerInvoice + "</th>" +
                "<th class='text-white border text-right'>" + reformatAmount(jsonSummary.ttlPaidNonCashFloat) +
                "</th><th>" +
                "</th><th>" +
                "</th>" +
                "</tr>"
            GetDomElement('totals').classList.remove('d-none');
            GetDomElement("totalCash").innerText = jsonSummary.ttlPaidCash;
            GetDomElement("totalNonCash").innerText = jsonSummary.ttlPaidNonCash;
            GetDomElement("masterTotal").innerText = jsonSummary.ttlAmount;
           
        } else if (oData.code == 404 || oData.code == 204) {
            GetDomElement('totals').classList.add('d-none');
            emptyTable.classList.remove('d-none');
            paymentMethodTable.innerHTML = "";
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (collectionReport.code == 204 || collectionReport.code == 404) {
        emptyTable.classList.remove('d-none');
        GetDomElement('totals').classList.add('d-none');
        paymentMethodTable.innerHTML = "";
    } else {
        ShowErrorModalOnLoad(collectionReport.message, collectionReport.code);
    }
}

function PopulateSystemUsers() {
    var oSystemUsers = InvokeService(GetURL(), "SystemUserMethods", "GET", "");
    if (oSystemUsers.code == 200) {
        var oData = JSON.parse(oSystemUsers.data);
        if (oData.code == 200) {
            var jsonData = JSON.parse(oData.jsonData);
            var selSystemUsers = document.getElementById("selPostedBy");
            selSystemUsers.innerHTML = "";
            var defaultOption = document.createElement('option');

            defaultOption.innerHTML = "Select ";
            defaultOption.value = "";
            defaultOption.hidden = true;
            selSystemUsers.appendChild(defaultOption);

            for (var i = 0; i < jsonData.length; i++) {
                selSystemUsers.innerHTML += "<option value=\"" + jsonData[i].user_id + "\"'>" + capitalize(jsonData[i].user_name) + "</option>";
            }



        } else if (oData.code.code == 404 || oData.code.code == 204) {} else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oSystemUsers.code.code == 204 || oSystemUsers.code.code == 404) {} else {
        ShowErrorModalOnLoad(oSystemUsers.message, oSystemUsers.code);
    }
}

function PopulateCustomerList() {

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





function getAllUsers(){
    var oUsers = InvokeService(GetURL(),"SystemUserMethods","GET","");
    if(oUsers.code == 200){
      var ojsonData = JSON.parse(oUsers.data);
      if(ojsonData.code == 200){
          var userData = JSON.parse(ojsonData.jsonData);
          for(var i=0;i<userData.length;i++){
              oSystemUsers.push(userData[i]);
          }
        }else if(ojsonData.code == 500){
        ShowErrorModalOnLoad(ojsonData.message, ojsonData.code);
      }
    }else if(userData.code == 500){
        ShowErrorModalOnLoad(oUsers.message, oUsers.code);
    }
}

function GetSystemUserName(userId){
    for (var i=0;i<oSystemUsers.length;i++){
     if(oSystemUsers[i].user_id == userId){
         return oSystemUsers[i].user_name;
     }
    }
}