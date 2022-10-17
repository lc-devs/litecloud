


GetDomElement('btnSearchReport').addEventListener('click', function () {
    var requiredAll = document.querySelectorAll('.input-required');
    var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
    if (BValidForm) {
        var customerId = GetDomElement('customerIdSearch').innerHTML;
        var pickUpby = sessionStorage.getItem('userId');
        var dateNow = new Date();
        var date = dateNow.toISOString().slice(0, 10);

        var oData = InvokeService(GetURL(), `PickupIndustrialMethods?dateFrom=${date}&dateTo=${date}&customerID=${customerId}&pickupBy=${pickUpby}&isAllEntries=${getAllEntries()}&isForPickupOnly=${getAllPickUp()}`, 'GET', '');
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                var parsedData = JSON.parse(parsedData.jsonData);
                populateTableQuery(parsedData)
            } else {
                var divtbl = GetDomElement('tblQuery');
                divtbl.innerHTML = "";
            }
        } else {
            ShowErrorModalOnLoad('Please Contact Adminsitrator', oData.code);

        }
    }
});

GetDomElement('btnPrintQR').addEventListener('click', function () {
    // location.href = 'online-industrial-pickup-query-report.html';
    var soRef = GetDomElement('so_reference').innerHTML;
    Download(`PDFGenerator/printQRCode?qrCode=${soRef}`, `qrCode${soRef}`);
});

function populateTableQuery(parsedData) {
    var divtbl = GetDomElement('tblQuery');
    divtbl.innerHTML = "";
    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-success'>" + `${parsedData[i].so_reference == 0 ? "<label class='text-danger'>For pickup</label>": parsedData[i].so_reference}`+ "</td>" +
            "<td class='border text-center px-0 text-info text-capitalize'>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0'>" + parsedData[i].weight_in_kg + " kg </td>" +
            "<td class='border text-center px-0'>" + parsedData[i].number_of_bags + " bags </td>" +
            "<td class='border text-center px-0 text-warning'>" + moment(parsedData[i].picked_up_datetime).format('lll') + "</td>" +
            "<td class='border text-center px-0'>" + `${parsedData[i].received_by_logistics == 1 ? "<i class='fas fa-check text-success'></i>" : "" }` + "</td>" +
            // "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
            "</tr>";


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
    var customerId = GetDomElement('customerIdSearch').innerHTML;
    var pickUpby = sessionStorage.getItem('userId');
    var dateNow = new Date();
    var date = dateNow.toISOString().slice(0, 10);
    var oData = InvokeService(GetURL(), `PickupIndustrialMethods?dateFrom=${date}&dateTo=${date}&customerID=${customerId}&pickupBy=${pickUpby}&isAllEntries=${getAllEntries()}&isForPickupOnly=${getAllPickUp()}`, 'GET', '');
    
    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            var parsedData = JSON.parse(parsedData.jsonData);
            // var parsedData = JSON.parse(parsedData);
            populateTableQuery(parsedData)
        } else {
            var divtbl = GetDomElement('tblQuery');
            divtbl.innerHTML = "";
        }
    } else {
        ShowErrorModalOnLoad('Please Contact Adminsitrator', oData.code);

    }
});

function getAllEntries() {
    if (GetDomElement('selStatus').value == 0) {
        return true;
    }
    return false;
}
function getAllPickUp() {
    if (GetDomElement('selStatus').value == 1) {
        return true;
    }
    return false;
}