GetDomElement('btnSearchReport').addEventListener('click', function () {
    this.dataset.target = '';
    var requiredAll = document.querySelectorAll('.input-required');
    var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
    if (BValidForm) {
        var customerId = GetDomElement('customerIdSearch').innerHTML;
        var dateFrom = GetDomElement('dateFrom').value;
        var dateTo = GetDomElement('dateTo').value;
        var pickUpby = sessionStorage.getItem('userId');

        var oData = InvokeService(GetURL(), `LogisticNonIndustrialMethods/receivedqueryreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}'}`, 'GET', '');
        
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                var parsedData = JSON.parse(parsedData.jsonData);
                // var parsedData = JSON.parse(parsedData);
                populateTableQuery(parsedData)
                
            } else {
                var divtbl = GetDomElement('tblQuery');
                divtbl.innerHTML = "";
                ShowErrorModal("btnSearchReport", parsedData.message, parsedData.code);

            }
        } else {
            ShowErrorModal("btnSearchReport", oData.message, oData.code);
            
        }
    }
});

GetDomElement("printNow").addEventListener('click', function(){
    GetDomElement("pdfile").click();
});

GetDomElement("btnPrintNow").addEventListener('click', function(){
    var soRef = GetDomElement('so_reference').innerHTML;
    Download(`PDFGenerator/printQRCode?soReference=${soRef}&isIndustrial=false`, `qrCode${soRef}`);
});
// function printModal() {
//     GetDomElement('allDiv').style.visibility = "hidden";
//     GetDomElement('btnSODetails').style.visibility = "visible";
//     window.print();
//     GetDomElement('allDiv').style.visibility = "visible";
// }

function populateTableQuery(parsedData) {
    var divtbl = GetDomElement('tblQuery');
    
    divtbl.innerHTML = "";
    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-success so-reference' onmouseover='soMouseHover()' onmouseout='soMouseOut()' data-toggle='modal'>" +`${parsedData[i].so_reference == 0 ? "<label class='text-danger'>For pickup</label>": parsedData[i].so_reference }`+ "</td>" +
            "<td class='border text-center px-0 text-info text-capitalize'>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0'>" + parsedData[i].weight_in_kg + " kg </td>" +
            "<td class='border text-center px-0'>" + parsedData[i].number_of_bags + " bags </td>" +
            "<td class='border text-center px-0 text-warning'>" + `${parsedData[i].so_reference == 0 ? "<label class='text-danger'>For pickup</label>" : moment(parsedData[i].picked_up_datetime).format('lll') }` + "</td>" +
            "<td class='border text-center px-0'>" + `${parsedData[i].received_by_logistics == 1 ? "<i class='fas fa-check text-success'></i>" : "" }` + "</td>" +
            // "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
            "</tr>";


    }
}
GetDomElement('xlsfile').addEventListener('click', function () {
    var pdfTitle = "Logistics NonIndustrial Query Report";
    var dateGenerated = new Date().toISOString().split('T')[0];
    startDate = `${moment(GetDomElement("dateFrom").value).format('LL')}`;
    endDate = `${moment(GetDomElement("dateTo").value).format('LL')}`;
    customer = `${capitalize(GetDomElement('txtcustomerName').value)}`;
    fnExcelReport(pdfTitle, "tblQuery", `${dateGenerated}-nonindustrial_query_report`, startDate, endDate, customer);
    GetDomElement("btnCloseFileType").click();
});


GetDomElement('pdfile').addEventListener('click', function () {
    var customerId = GetDomElement('customerIdSearch').innerHTML;
   
    // var pickUpby = sessionStorage.getItem('userId');
    var dateFrom = GetDomElement('dateFrom').value;
    var dateTo = GetDomElement('dateTo').value;
    var allEntry = true;
    var isForPickupOnly = false;
    // if (GetDomElement('selStatus').value == 0) {
    //     allEntry = true;
    // } else {
    //     allEntry = false;
    // }

    // if (GetDomElement('selStatus').value == 1) {
    //     isForPickupOnly = true;
    // } else if(GetDomElement('selStatus').value == 2){
    //     isForPickupOnly = false;
    // }

    Download(`PDFGenerator/logisticreceivedreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}&isIndustrial=false`, `received-report${dateFrom}_${dateTo}`);


   
});

function soMouseHover() {
    var soReference = document.querySelectorAll('.so-reference')
    for (var i = 0; i < soReference.length; i++){
        soReference[i].style.cursor = 'pointer';
        // soReference[i].style.textDecoration = 'underline';
        soReference[i].addEventListener('click', function () {
            
            if (!isNaN(this.innerHTML)) {
                var soData = InvokeService(GetURL(), `PickupNonIndustrialMethods/${this.innerHTML}`, 'GET', '');
                if (soData.code == 200) {
                    
                    var parseData = JSON.parse(soData.data);
                    if (parseData.code == 200) {
                        this.dataset.target = '#btnSODetails';
                        var parseData = JSON.parse(parseData.jsonData);
                        var base64 = parseData.so_reference_QR_Image;
                        GetDomElement('imageQR').setAttribute('src', base64);
                        
                        so_reference.innerHTML = parseData.so_reference;
                        customer_name.innerHTML = parseData.customer_name;
                        number_of_bags.innerHTML = parseData.number_of_bags;
                        number_of_loads.innerHTML = parseData.number_of_loads;
                        picked_up_by.innerHTML = parseData.picked_up_by;
                        picked_up_datetime.innerHTML = moment(parseData.picked_up_datetime).format('LLL');
                        weight_in_kg.innerHTML = parseData.weight_in_kg;

                        if (parseData.items != undefined) {
                            var divtbl = GetDomElement('tblSODetails');
                            divtbl.innerHTML = "";
                            for (var i = 0; i < parseData.items.length; i++) {
                                divtbl.innerHTML +=
                                    "<tr> " +
                                    "<td class='border text-center px-0 text-success'>" + parseData.items[i].item_description + "</td>" +
                                    "<td class='border text-center px-0 text-warning'>" + parseData.items[i].item_count + "</td>" +
                                    "</tr>";

                            }
                        }
                        if (parseData.services != undefined) {
                            var divtbl = GetDomElement('tblServices');
                            divtbl.innerHTML = "";
                            for (var i = 0; i < parseData.services.length; i++) {
                            divtbl.innerHTML +=
                                "<tr> " +
                                "<td class='border text-center px-0 text-success'>" + parseData.services[i].description + "</td>" +
                                "</tr>";

                            }
                        }
                        
                                    
                                    

                    }
                }

            }
        });
    }
}
function soMouseOut() {
    var soReference = document.querySelectorAll('.so-reference')
    for (var i = 0; i < soReference.length; i++){
        soReference[i].style.cursor = 'auto';
        soReference[i].style.textDecoration = 'none';
    }
}
GetDomElement('btnSearchModal').addEventListener('click', function () {
    populateCustomerModal();
});
GetDomElement('txtCustomerModal').addEventListener('keypress', function (e) {
    if (e.key == "Enter") {
        populateCustomerModal();
   } 
});

function populateCustomerModal() {
    var customer = GetDomElement('txtCustomerModal');
    if (customer.value != '') {
        var oData = InvokeService(GetURL(), `CustomerMethods/search/customer/${customer.value}`, 'GET', '');
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                var parsedData = JSON.parse(parsedData.jsonData);

                var divtbl = GetDomElement('tblCustomerModal');

                divtbl.innerHTML = "";
                for (var i = 0; i < parsedData.length; i++) {
                    divtbl.innerHTML +=
                        "<tr class='customerId' style='cursor: pointer;'> " +
                            "<td class='border text-center px-0 d-none'>" + parsedData[i].customer_id + "</td>" +
                            "<td class='border text-center px-0 text-info'>" + parsedData[i].customer_name + "</td>" +
                            "<td class='border text-center px-0'>" + parsedData[i].cellular_number + "</td>" +
                            "<td class='border text-center px-0'>" + parsedData[i].email_address + "</td>" +
                        "</tr>";

                }
                divtbl.innerHTML +=
                "<tr class='customerId' style='cursor: pointer;'> " +
                    "<td class=' text-center px-0 d-none'>" + 0 + "</td>" +
                    "<td class=' text-center px-0 text-danger'> Search all Customer </td>" +
                    
                "</tr>";
                var customerId = document.querySelectorAll('.customerId');
                for (var i = 0; i < customerId.length; i++){
                    customerId[i].addEventListener('click', function () {
                        
                        GetDomElement('customerIdSearch').innerHTML = this.cells[0].innerHTML;
                        GetDomElement('txtcustomerName').value = this.cells[1].innerHTML;
                        GetDomElement('btnCloseSearchCustomerModal').click();
                    });
                }
            }
        }
    }

}

window.addEventListener('load', function () {
    var dateNow = new Date();
    GetDomElement('dateFrom').value = dateNow.toISOString().slice(0, 10);
    GetDomElement('dateTo').value = dateNow.toISOString().slice(0, 10);
    
    var customerId = GetDomElement('customerIdSearch').innerHTML;
    var pickUpby = sessionStorage.getItem('userId');
    var dateFrom = GetDomElement('dateFrom').value;
    var dateTo = GetDomElement('dateTo').value;

    var oData= InvokeService(GetURL(), `LogisticNonIndustrialMethods/receivedqueryreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}'}`, 'GET', '');
        
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                var parsedData = JSON.parse(parsedData.jsonData);
                // var parsedData = JSON.parse(parsedData);
                populateTableQuery(parsedData)
            }  else if(parsedData.code == 401) {
                
                ShowErrorModalOnLoad('', parsedData.code);

            } else {
                // GetDomElement('tblQuery').innerHTML = 'No Data Found for Today';
                // GetDomElement('tblQuery').classList.add('bg-transparent');
            }
            
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);

        }
});

function getAllEntries() {
    // if (GetDomElement('selStatus').value == 0) {
    //     return true;
    // }
    // return false;
}
function getAllPickUp() {
    // if (GetDomElement('selStatus').value == 1) {
    //     return true;
    // }
    // return false;
}
ClosePopupModal("btnOKSODetails", "btnCloseSODetails");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");

