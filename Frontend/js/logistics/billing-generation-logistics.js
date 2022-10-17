var divtbl = GetDomElement('tblBilling');
var type;
var industrial = [];
var nonIndustrial = [];

GetDomElement('btnFetch').addEventListener('click', function () {

    if (IsEmpty('dateFrom')) {
        GetDomElement('dateFrom').classList.add('is-invalid');
    } else {
        GetDomElement('dateFrom').classList.remove('is-invalid');

        var dateFrom = GetDomElement('dateFrom').value;
        var dateTo = GetDomElement('dateTo').value;
        if (GetDomElement('radioAll').checked == true) {
            type = 'all';
        } else if (GetDomElement('radioIndustrial').checked == true) {
            type = 'industrial';
        } else if (GetDomElement('radioNonIndustrial').checked == true) {
            type = 'non-industrial';
        }
        populateForInvoice(dateFrom, dateTo,type)
   }
});

window.addEventListener('load', function () {
    document.getElementById('dateTo').valueAsDate = new Date();
    document.getElementById('dateFrom').valueAsDate = new Date();
    document.getElementById('dateToModal').valueAsDate = new Date();
    

    var dateNow = new Date();
    var date = dateNow.toISOString().slice(0, 10);
    type = 'all';

    populateForInvoice(date,date,type);
});

function populateForInvoice(dateFrom, dateTo,type) {
    
    divtbl.innerHTML = "";
    
    var oData = InvokeService(GetURL(), `BillingMethods/getunbilledinvoices?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=0&type=${type}`, 'GET', '');
    
    
    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            GetDomElement('btnGenerateBill').classList.remove('d-none');
            GetDomElement('emptyDiv').classList.add('d-none');

            var parsedData = JSON.parse(parsedData.jsonData);
            // var parsedData = JSON.parse(parsedData);
            populateTableQuery(parsedData);
            
        } else if(parsedData.code == 401) {
                
            ShowErrorModalOnLoad('Unauthorized', parsedData.code);

        } else {
            
            GetDomElement('btnGenerateBill').classList.add('d-none');
            GetDomElement('emptyDiv').classList.remove('d-none');
            // GetDomElement('tblQuery').innerHTML = 'No Data Found for Today';
            // GetDomElement('tblQuery').classList.add('bg-transparent');
        }
    } else {
        
        ShowErrorModalOnLoad(oData.message, oData.code);

    }
}

function populateTableQuery(parsedData) {
  
   
    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-success d-none customerId'>" + parsedData[i].customer_id + "</td>" +
            "<td class='border text-center px-0 text-success text-capitalize align-middle'>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0 align-middle'>" + parsedData[i].type + "</td>" +
            "<td class='border text-center px-0 align-middle'>" + parsedData[i].invoiceCount + " </td>" +
            "<td class='border text-center px-0 align-middle'>" + parsedData[i].totalInvoiceAmount + "</td>" +
            // "<td class='border text-center px-0'><button class='btn btn-primary generateBill'>Generate Bill</button> </td>" +
            "</tr>";
        
    }
        //     var generateBill = document.querySelectorAll('.generateBill');
        //     console.log(generateBill.length)
        //     for (var i = 0; i < generateBill.length; i++) {
        //         generateBill[i].addEventListener('click', function () {
        //         var customerId = this.parentNode.parentNode.cells[0].innerHTML
        //         console.log(customerId);
        //         GetDomElement("confirmOneSeviceMethod").innerText = "cancel";
        //         GetDomElement("confirmOneSeviceMethod").classList.add('text-danger');
        //         GetDomElement("confirmServiceMethod").innerText = "cancel";
        //         GetDomElement("confirmServiceMethod").classList.add('text-danger');

        //         GetDomElement("btnConfirmTwo").onclick = () => {
        //             var deleteBooking = InvokeService(GetURL(), `NonIndustrialSelfServiceMethods/cancelso/${referenceNumber}`,'DELETE','');
                  
                    
        //             if (deleteBooking.code == 200) {
                        
        //                 GetDomElement("btnCloseConfimrTwo").click();
        //                 GetDomElement("btnConfirmOneModal").click();
        //                 // GetDomElement("btnConfirmTwo").dataset.target = "#btnSuccessEntryModal";
        //                 // GetDomElement("successServiceMethod").innerText = "cancelled";
        //                 populateSelfService();

                        
                        
        //             }
        //         }

        //     });
        // }

    
}

function getArray() {
    var customerList = [];
    var generateBill = document.querySelectorAll('.customerId');
    for (var i = 0; i < generateBill.length; i++){
        
        customerList.push(generateBill[i].innerHTML);
    }
    return customerList;
}

function getAll() {
    
    var generateBill = document.querySelectorAll('.customerId');
    for (var i = 0; i < generateBill.length; i++){
        if (generateBill[i].parentNode.cells[2].innerHTML == 'Industrial') {
            industrial.push(generateBill[i].parentNode.cells[0].innerHTML);

        } else if (generateBill[i].parentNode.cells[2].innerHTML == 'Non-Industrial') {
            nonIndustrial.push(generateBill[i].parentNode.cells[0].innerHTML);
        }
    }
}

GetDomElement('btnGenerateBill').addEventListener('click', function () {
    var customerList = getArray();

    if (customerList.length > 0) {
        this.dataset.target = '#btnConfirmOneModal';
        GetDomElement("confirmOneSeviceMethod").innerText = "Generate a billing for this";
        GetDomElement("confirmOneSeviceMethod").classList.add('text-primary');
        GetDomElement("confirmServiceMethod").innerText = "Generate a billing for this";
        GetDomElement("confirmServiceMethod").classList.add('text-primary');

        GetDomElement("btnConfirmTwo").onclick = () => {
            var body = customerList;
            
            var strBody = JSON.stringify(body);
            
            var generateBilling;
            if (GetDomElement('radioIndustrial').checked == true) {
            
                generateBilling = InvokeService(GetURL(), `BillingMethods/generatebillingindustrial`, 'POST', strBody);
            
            } else if (GetDomElement('radioNonIndustrial').checked == true) {
            
                generateBilling = InvokeService(GetURL(), `BillingMethods/generatebillingnonindustrial`, 'POST', strBody);
            } else if (GetDomElement('radioAll').checked == true) {
                getAll();

                if (industrial.length > 0) {
                    body = industrial;
            
                    strBody = JSON.stringify(body);
                    generateBilling = InvokeService(GetURL(), `BillingMethods/generatebillingindustrial`, 'POST', strBody);
                   
                }
                if (nonIndustrial.length > 0) {
                    body = nonIndustrial;
            
                    strBody = JSON.stringify(body);
                    generateBilling = InvokeService(GetURL(), `BillingMethods/generatebillingnonindustrial`, 'POST', strBody);
                   
                }

            
            } 
            
            if (generateBilling.code == 200) {
    
                var parsedData = JSON.parse(generateBilling.data);
                if (parsedData.code == 200) {
                   
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                  
                    ShowSuccessModal('btnConfirmTwo', "added");

                    populateForInvoice();
                 
                } else {
                    ShowErrorModal('btnConfirmTwo', parsedData.message, parsedData.code);
                }
    
            } else {
                ShowErrorModal('btnConfirmTwo',generateBilling.message , generateBilling.code);
            }
        }
    } else {
        
    }
});
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
