GetDomElement('btnSearchReport').addEventListener('click', function () {
    // var requiredAll = document.querySelectorAll('.input-required');
    // var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
    var BValidForm = true;
    if (BValidForm) {
        var customerId = sessionStorage.getItem('customerId');
        var dateFrom = GetDomElement('dateFrom').value;
        var dateTo = GetDomElement('dateTo').value;
        var pickUpby = sessionStorage.getItem('userId');

        var oData= InvokeService(GetURL(), `BillingMethods/billingreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}&type=all`, 'GET', '');
        
       

        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                this.dataset.target = "";
                var parsedDatas = JSON.parse(parsedData.jsonData);
                populateTableQuery(parsedDatas)
            } else {

                ShowErrorModal("btnSearchReport", parsedData.message, parsedData.code);

                var divtbl = GetDomElement('tblQuery');
                divtbl.innerHTML = "";
            }
        } else {
            ShowErrorModal("btnSearchReport", oData.message, oData.code);


        }
    }
});

function populateTableQuery(parsedData) {
    var divtbl = GetDomElement('tblQuery');
    divtbl.innerHTML = "";
    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-warning text-capitalize'>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0 text-info text-capitalize'>" + parsedData[i].type + "</td>" +
            "<td class='border text-center px-0'>" + parsedData[i].billing_reference  + "</td>" +
            "<td class='border text-center px-0'>" +   moment(parsedData[i].billing_date).format('LL')+ "</td>" +
            "<td class='border text-center px-0 text-warning'> ₱" +  (parsedData[i].bill_amount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')+ "</td>" +
            "<td class='border text-center px-0'>" + `${parsedData[i].paid == 1 ? "<i class='fas fa-check text-success'></i>" : "" }` + "</td>" +
            // "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
            "</tr>";

        // var soReference = document.querySelectorAll('.so-reference')
        // for (var i = 0; i < soReference.length; i++){
            
        //     soReference[i].addEventListener('click', function () {

        //     });
        // }
        

    }
}

function generateSODetails() {
    var soReference = document.querySelectorAll('.so-reference')
    for (var i = 0; i < soReference.length; i++){
        // soReference[i].style.cursor = 'pointer';
        // soReference[i].style.textDecoration = 'underline';
        
        soReference[i].addEventListener('click', function () {
                    
            if (!isNaN(this.innerHTML)) {
                var soData = InvokeService(GetURL(), `PickupIndustrialMethods/${this.innerHTML}`, 'GET', '');
                if (soData.code == 200) {
                    
                    var parseData = JSON.parse(soData.data);
                    if (parseData.code == 200) {
                        this.dataset.target = '#btnSODetails';
                        var parseData = JSON.parse(parseData.jsonData)
                        
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
                                    
                                    

                    }
                }

            }
        });
    }
  
}
GetDomElement('xlsfile').addEventListener('click', function () {
    var pdfTitle = "Logistics Industrial Query Report";
    var dateGenerated = new Date().toISOString().split('T')[0];
    startDate = `${moment(GetDomElement("dateFrom").value).format('LL')}`;
    endDate = `${moment(GetDomElement("dateTo").value).format('LL')}`;
    customer = `${capitalize(GetDomElement('txtcustomerName').value)}`;
    fnExcelReport(pdfTitle, "tblQuery", `${dateGenerated}-industrial_query_report`, startDate, endDate, customer);
    GetDomElement("btnCloseFileType").click();
});

function soMouseOut() {
    var soReference = document.querySelectorAll('.so-reference')
    for (var i = 0; i < soReference.length; i++){
        soReference[i].style.cursor = 'auto';
        soReference[i].style.textDecoration = 'none';
    }
}
// GetDomElement('btnSearchModal').addEventListener('click', function () {
//     populateCustomerModal();
// });
// GetDomElement('txtCustomerModal').addEventListener('keypress', function (e) {
//     if (e.key == "Enter") {
//         populateCustomerModal();
//    } 
// });

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
                            "<td class='border text-center px-0 '>" + parsedData[i].cellular_number + "</td>" +
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
    
    var customerId = sessionStorage.getItem('customerId');
    var pickUpby = sessionStorage.getItem('userId');
    var dateFrom = GetDomElement('dateFrom').value;
    var dateTo = GetDomElement('dateTo').value;

    var oData= InvokeService(GetURL(), `BillingMethods/billingreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}&type=all`, 'GET', '');
        
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
                // GetDomElement('tblQuery').classList.add('mt-3');
            }
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);

        }
});

// function getStatus() {
    
//     if (GetDomElement('selStatus').value == 0) {
//         return 'all';
//     } else if (GetDomElement('selStatus').value == 1) {
//         return 'industrial';
//     } else {
//         return 'non-industrial';
//     }
  
// }
// function getAllPickUp() {
//     if (GetDomElement('selStatus').value == 1) {
//         return true;
//     }
//     return false;

//     // return true;
// }

GetDomElement('pdfile').addEventListener('click', function () {
   
    var pickUpby = sessionStorage.getItem('userId');
    var customerId = GetDomElement('customerIdSearch').innerHTML;

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
    
    // Download(`PDFGenerator/pickupreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerId}&pickupBy=${pickUpby}&isAllEntries=${allEntry}&isForPickupOnly=${isForPickupOnly}&isIndustrial=true`, `pickup-report-industrial-${dateFrom}_${dateTo}`);

   

});

function customHeader(url) {
    var client = new XMLHttpRequest();
    client.open("GET", url);
    client.setRequestHeader("UserKey", sessionStorage.getItem('authkey'));
}
  

// GetDomElement('btnSave').addEventListener('click', function () {
//     var tbl = GetDomElement('tblQuery')
//     if (tbl.rows.length > 0) {
//         this.dataset.target = "#btnAddNew";
//     } else {
//         this.dataset.target = "";
//     }
// });

function printModal() {
    GetDomElement('allDiv').style.visibility = "hidden";
    GetDomElement('btnSODetails').style.visibility = "visible";
    window.print();
    GetDomElement('allDiv').style.visibility = "visible";
  
}

// ClosePopupModal("btnOKSODetails", "btnCloseSODetails");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
