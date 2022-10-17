var customerName = GetDomElement('txtCustomer');
var CIFTable = GetDomElement("btnCustomerList");
var tblQuery = GetDomElement("tblBody");
var emptyTblQuery = GetDomElement("emptyTable");
var emptyCustomerList = GetDomElement("emptyCustomerList");
var SaveTable = GetDomElement("SaveTable");
var SaveTableBody = GetDomElement("SaveBody");
var customerID = GetDomElement("customerId");
var dateToday = new Date().toISOString().split('T')[0];
var dateFrom = GetDomElement("from");
var dateTo = GetDomElement("to");

window.addEventListener('load', function () {
    GetDomElement("btnFindCustomer").disabled = true;
    GetDomElement("from").value = dateToday;
    GetDomElement("to").value = dateToday;
});

GetDomElement("from").addEventListener('change', () => {
    tblQuery.innerHTML = "";
    GetDomElement('btnPrintNow').disabled = true;
    GetDomElement('saveNow').disabled = true;
});

GetDomElement("to").addEventListener('change', () => {
    tblQuery.innerHTML = "";
    GetDomElement('btnPrintNow').disabled = true;
    GetDomElement('saveNow').disabled = true;
});

customerName.addEventListener('input', () => {
    tblQuery.innerHTML = "";
    GetDomElement('btnPrintNow').disabled = true;
    GetDomElement('saveNow').disabled = true;
});

GetDomElement("chkCustomer").addEventListener('click', function () {
    customerName.value = ""
    CIFTable.innerHTML = "";
    customerID.value = "";
    if (this.checked) {
        customerName.classList.add('input-required');
        GetDomElement("btnFindCustomer").disabled = false;
    } else {
        customerName.classList.remove('input-required');
        customerName.classList.remove('is-invalid');
        GetDomElement("btnFindCustomer").disabled = true;

    }
});

GetDomElement("btnFindCustomer").addEventListener('click', function () {
    GetDomElement("txtSearchClient").value = "";
    CIFTable.innerHTML = "";
});

GetDomElement("btnExcelFile").addEventListener('click', function () {
    var pdfTitle = "Laundry Industrial Query Report";
    var dateGenerated = new Date().toISOString().split('T')[0];
    startDate = `${moment(GetDomElement("from").value).format('LL')}`;
    endDate = `${moment(GetDomElement("to").value).format('LL')}`;
    customer = `${capitalize(GetDomElement('txtCustomer').value)}`;
    fnExcelReport(pdfTitle, "SaveTable", `${dateGenerated}-industrial_query_report`, startDate, endDate, customer);
    GetDomElement("btnCloseFileType").click();
});

GetDomElement("btnPDFFILe").addEventListener('click', function () {
    PDFReport();
});

GetDomElement("btnPrintNow").addEventListener('click', function () {
    PDFReport();
});
GetDomElement("txtSearchClient").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        PopulateCISList();
    }
});

GetDomElement("btnSearchClient").addEventListener('click', function () {
    PopulateCISList();
});

GetDomElement("btnGenerateReport").addEventListener('click', function () {
    var dateFrom = GetDomElement('from').value;
    var dateTo = GetDomElement('to').value;
    var status = GetDomElement("selFetchType").value;
    var requiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(requiredFields, "divUpdateInvalidForm");
    if (bValidForm) {
        var oLaundryRecord = InvokeService(GetURL(), `LogisticIndustrialMethods/laundryqueryreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerID.value}&laundry=${status}`, "GET", "");
        if (oLaundryRecord.code == 200) {
            emptyTblQuery.classList.add('d-none');
            var oData = JSON.parse(oLaundryRecord.data);
            if (oData.code == 200) {
                GetDomElement('btnPrintNow').disabled = false;
                GetDomElement('saveNow').disabled = false;
                emptyTblQuery.classList.add('d-none');
                var oJsonData = JSON.parse(oData.jsonData);
                tblQuery.innerHTML = "";
                for (var i = 0; i < oJsonData.length; i++) {
                    var received_by_laundry_datetime = "----";
                    
                    if (moment(oJsonData[i].received_by_laundry_datetime).format("YYYY") != '1900') {
                        received_by_laundry_datetime = moment(oJsonData[i].received_by_laundry_datetime).format('LLL');
                    }
                    tblQuery.innerHTML +=
                        "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].soDate +
                        "</td><td class='text-capitalize px-2'>" + moment(oJsonData[i].soDate).format('LLL') +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].so_reference +
                        "</td><td class='text-capitalize px-1'>" + oJsonData[i].customer_name +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].weight_in_kg +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].number_of_bags +
                    "</td><td class='px-1 text-center'>" + received_by_laundry_datetime +
                        "</td><td>" + PopulateCheckboxTable(oJsonData[i].completed_by_laundry) +
                        "</td><td>" + PopulateCheckboxTable(oJsonData[i].received_from_laundry) +
                        "</td>" +
                        "</tr>";
                }
                SaveTableBody.innerHTML = "";
                for (var i = 0; i < oJsonData.length; i++) {
                    SaveTableBody.innerHTML +=
                        "<tr class='border' style='cursor:default'><td class='text-capitalize px-2'>" + moment(oJsonData[i].soDate).format('LLL') +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].so_reference +
                        "</td><td class='text-capitalize px-1'>" + capitalize(oJsonData[i].customer_name) +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].weight_in_kg +
                        "</td><td class='px-1 text-center'>" + oJsonData[i].number_of_bags +
                        "</td><td class='px-1'>" + moment(oJsonData[i].received_by_laundry_datetime).format('LLL') +
                        "</td><td>" + `${oJsonData[i].completed_by_laundry == 0?"No":"Yes"}` +
                        "</td><td>" + `${oJsonData[i].received_from_laundry ==0 ?"No":"Yes"}` +
                        "</td>" +
                        "</tr>";
                }





            } else if (oData.code == 404 || oData.code == 204) {
                tblQuery.innerHTML = "";
                emptyTblQuery.classList.remove('d-none');
            } else {
                ShowErrorModalOnLoad(oData.message, oData.code);
            }
        } else if (oLaundryRecord.code == 204 || oLaundryRecord.code == 404) {
            tblQuery.innerHTML = "";
            emptyTblQuery.classList.remove('d-none');

        } else {
            emptyTblQuery.classList.add('d-none');
            ShowErrorModalOnLoad(oLaundryRecord.message, oLaundryRecord.code);
        }
    }
});


function PDFReport() {
    var status = GetDomElement("selFetchType").value;
    var controllerURL = `PDFGenerator/laundryqueryreport?dateFrom=${dateFrom.value}&dateTo=${dateTo.value}&customerID=${GetDomElement("customerId").value}&laundryStatus=${status}&isIndustrial=true`;
    Download(controllerURL, `${dateToday}-laundry-industrial-query-report.pdf`);
}


/* FUNCTIONS */
function PopulateCISList() {

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