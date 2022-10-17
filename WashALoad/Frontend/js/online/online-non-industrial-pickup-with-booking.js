var error = false;

var laundryTbl = [];
var itemArrayOther = []
var itemArray = [];
var itemArrayService = [];

var allItem = []

window.addEventListener('load', function () {
    // GetDomElement('btnAdd').disabled = true;
    // GetDomElement('btnConfirmLogistics').disabled = true;
    // PopulateLaundryItemDropDown('selItem');
        
    GetDomElement('pickupDateTime').value = moment(new Date()).format('LLL');
    if (sessionStorage.getItem('bookingReference') != null) {
        GetDomElement('txtbookingReference').value = sessionStorage.getItem('bookingReference');
        GetDomElement('txtbookingReference').readonly = true;
        GetDomElement('btnSearch').click();
    }
    sessionStorage.removeItem('bookingReference');
    
    populateLundryItemTable();
    populateLundryServices();
});

function printModal() {
    GetDomElement('allDiv').style.visibility = "hidden";
    GetDomElement('btnSuccessEntryModal').style.visibility = "visible";
    window.print();
    GetDomElement('allDiv').style.visibility = "visible";
}

function populateLundryServices() {
    var jsonData = InvokeService(GetURL(), "NonIndustrialServiceMethods", "GET", "");
    
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = GetDomElement('tblServices');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center d-none'>"+ oData[i].id_service +"</td>" +
            "<td class='border text-center px-0'>"+ "<input type='checkbox' class='form-control check-service'></input>" +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].description +"</td>" +
            // "<td class='border text-center px-2'></td>" +
            "</tr>";
        }

    }else if(parseJsonData.code == 404){

    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}

function populateLundryItemTable() {
    var jsonData = InvokeService(GetURL(), "NonIndustrialLaundryItemMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = GetDomElement('tblLaundry');
        divtbl.innerHTML = "";
        for (var i = 1; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center d-none'>"+ oData[i].id_item +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].description +"</td>" +
            "<td class='border text-center px-2'><input class='form-control item-count text-center'></td>" +
            // "<td class='border text-center px-2'></td>" +
            "</tr>";
        }

    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}

function getArrayServices() {
    var check = document.querySelectorAll('.check-service');
    itemArrayService = [];
    
    for (var i = 0; i < check.length; i++){
        
        if (check[i].checked == true) {
            
            itemArrayService.push({
                            "so_reference": 0,
                            "service_code": +check[i].parentNode.parentNode.cells[0].innerHTML,
                            "cost":  0,
                    })
        } else {
            
        }
    }
    
}

function getArrayOther() {
    itemArrayOther = [];
    
    for (var i = 0; i < laundryTbl.length; i++){    
        itemArrayOther.push({
            "so_reference": 0,
            "item_code": 1,
            "item_count": laundryTbl[i].item_count,
            "description": laundryTbl[i].item_description,
        })

    }
    
}

function getArray() {
    var itemCount = document.querySelectorAll('.item-count');
    itemArray = [];
    for (var i = 0; i < itemCount.length; i++){
        if (itemCount[i].value != '') {

            itemArray.push({
                            "so_reference": 0,
                            "item_code": +itemCount[i].parentNode.parentNode.cells[0].innerHTML,
                            "item_count": +itemCount[i].value,
                            "description":  "",
                    })
        } else {
            
        }
    }

    // return itemArray;
    
}

function PopulateLaundryItemDropDown(selectField, service, idValue) {
    var jsonData = InvokeService(GetURL(), service, "GET", "");
    var id;
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        
            var selCategory = document.getElementById(selectField);

            var defaultOption = document.createElement('option');

            defaultOption.innerHTML = "Select a dategory";
            defaultOption.value = "";
            defaultOption.hidden = true;
            selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
           
            selCategory.innerHTML += "<option value=\"" + oData[i].id_item + "\">" + oData[i].description + "</option>";
        }


    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

GetDomElement('btnAddLaundryItems').addEventListener('click', function () {
    this.dataset.target = "#btnAddNewLaundryItems";
});


GetDomElement('btnSubmitModalNewLaundry').addEventListener('click', function () {
    
    var amount = document.querySelectorAll('.amount');
    var requiredAll = document.querySelectorAll('.input-required');
    var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
    if (BValidForm) {
        GetDomElement('divAddNewInvalidForm').classList.add('d-none');
        var Bcheck = CheckValidAmount(amount, 'divAddNewInvalidAmount');
    } else {
        GetDomElement('divAddNewInvalidAmount').classList.add('d-none');
        
    }
    if (BValidForm && Bcheck) {
    GetDomElement('divAddLaundryItems').classList.add('d-none');
        
        laundryTbl.push({
            "so_reference": 0,
            "item_description": GetDomElement('itemDescription').value,
            "item_count":  GetDomElement('txtItemCount').value,
        })

        populateTableLaundry();
    }
});


GetDomElement('btnConfirmLogistics').addEventListener('click', function () {
    getArray();
    getArrayServices();
    getArrayOther();


    allItem = itemArray.concat(itemArrayOther);

    error = false;

    var bookingRef = GetDomElement('txtbookingReference').value; 
    var oData = InvokeService(GetURL(),`BookingsForPickupMethods/bookings/reference/${bookingRef}`, 'GET','')
    
    if (oData.code == 200) {
        var parseData = JSON.parse(oData.data);
        
        if (parseData.code != 200) {

            alert('BookingReference not recognize');
        }
    } else {
        alert('BookingReference not recognize');
    }

    if (IsEmpty('txtWeight') || !+GetDomElement('txtWeight').value) {
        GetDomElement('txtWeight').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('txtWeight').classList.remove('is-invalid');
        
    }
    if (IsEmpty('txtLoads') || !+GetDomElement('txtLoads').value) {
        GetDomElement('txtLoads').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('txtLoads').classList.remove('is-invalid');
        
    }
    if (IsEmpty('txtBags') || !+GetDomElement('txtBags').value) {
        GetDomElement('txtBags').classList.add('is-invalid');
        error = true;

    } else {
        GetDomElement('txtBags').classList.remove('is-invalid');
        
    }
    // if (IsEmpty('txtReference') || !+GetDomElement('txtReference').value) {
    //     GetDomElement('txtReference').classList.add('is-invalid');
    //     error = true;

    // } else {
    //     GetDomElement('txtReference').classList.remove('is-invalid');
        
    // }
    if (allItem.length <= 0) {
        GetDomElement('divAddLaundryItems').classList.remove('d-none');
        error = true;

    } else {
        GetDomElement('divAddLaundryItems').classList.add('d-none');
    }
    
    if (!error) {
        

        GetDomElement("confirmOneSeviceMethod").innerText = "add";
        GetDomElement("confirmOneSeviceMethod").classList.remove('text-danger');
        GetDomElement("confirmOneSeviceMethod").classList.add('text-success');

        GetDomElement("confirmServiceMethod").innerText = "add";
        GetDomElement("confirmServiceMethod").classList.remove('text-danger');
        GetDomElement("confirmServiceMethod").classList.add('text-success');

        this.dataset.target = "#btnConfirmOneModal";
        
        GetDomElement("btnConfirmTwo").onclick = () => {
            GetDomElement("btnConfirmTwo").innerHTML = "";
            var body = {
                "so_reference": 0,
                "customer_id": +GetDomElement('customerId').innerHTML,
                "booking_reference":  GetDomElement('txtbookingReference').value,
                "picked_up_by": sessionStorage.getItem('userId'),
                "picked_up_datetime": 0,
                "weight_in_kg": +GetDomElement('txtWeight').value,
                "number_of_loads": +GetDomElement('txtLoads').value,
                "number_of_bags": +GetDomElement('txtBags').value,
                "received_by_logistics": "string",
                "so_reference_QR_Image": "string",
                "pickupNonIndustrialDetail": allItem,
                "services": itemArrayService
            }
            var strBody = JSON.stringify(body);
            var oData = InvokeService(GetURL(), 'PickupNonIndustrialMethods', 'POST', strBody);

            if (oData.code == 200) {
    
                var parsedData = JSON.parse(oData.data);
                if (parsedData.code == 200) {
                    parsedData = JSON.parse(parsedData.jsonData)
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                  
                    GetDomElement('btnConfirmTwo').dataset.target = "#btnSuccessEntryModal";
                    
                    var base64 = parsedData.so_reference_QR_Image;
                    GetDomElement('imageQR').setAttribute('src', base64);
                    GetDomElement('QRsoReference').innerHTML = parsedData.so_reference;
                
                    GetDomElement('txtbookingReference').value = '';
                    GetDomElement('bookingDateTime').value = '';
                    GetDomElement('txtWeight').value = '';
                    GetDomElement('txtBags').value = '';
                    GetDomElement('txtLoads').value = '';
                    
                    GetDomElement('cellular').innerHTML = 'Contact Number';
                    GetDomElement('customerName').innerHTML = 'Name';
                    GetDomElement('customerAddress').innerHTML = 'Address';
                    
                } else {
                    ShowErrorModal('btnConfirmTwo', parsedData.message, parsedData.code);
                }
    
            } else {
                ShowErrorModal('btnConfirmTwo', oData.message, oData.code);
            }
        
            
        }

    } else {
        this.dataset.target = "";
    }
    
});
GetDomElement('btnPrintQR').addEventListener('click', function () {
    // location.href = 'online-non-industrial-pickup-query-report.html';
    var soRef = GetDomElement('QRsoReference').innerHTML;
    Download(`PDFGenerator/printQRCode?soReference=${soRef}&isIndustrial=false`, `qrCode${soRef}`);
});
GetDomElement('btnSearch').addEventListener('click', function () {
    var bookingRef = GetDomElement('txtbookingReference').value; 
    var oData = InvokeService(GetURL(),`BookingsForPickupMethods/bookings/reference/${bookingRef}`, 'GET','')
    
    if (oData.code == 200) {

        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            this.dataset.target = "";
            var datas = JSON.parse(parsedData.jsonData);
                if (datas[0].picked_up_by != 0) {
                    this.dataset.target = "#btnAlreadyPickedup";
                    
                } else {
                    GetDomElement('txtWeight').addEventListener('input', function () {
                        GetDomElement('txtLoads').value = Math.ceil(+this.value / +datas[0].weight_per_load);
                    });
                    this.dataset.target = "";
                    GetDomElement('customerId').innerHTML = datas[0].customer_id;
                    GetDomElement('bookingDateTime').value = moment(datas[0].booking_datetime).format('LLL');
                    
                    var oCustomers = InvokeService(GetURL(),`CustomerMethods/search/customer/id/${datas[0].customer_id}`, 'GET','')
                    
                    if (oCustomers.code == 200) {
                        var oCustomer = JSON.parse(oCustomers.data);
                        if (oCustomer.code == 200) {
                            var oCustomerDetails = JSON.parse(oCustomer.jsonData);
                            GetDomElement('cellular').innerHTML = oCustomerDetails[0].cellular_number;
                            GetDomElement('customerName').innerHTML = oCustomerDetails[0].customer_name;
                            GetDomElement('customerAddress').innerHTML = oCustomerDetails[0].street_building_address + " " +
                                oCustomerDetails[0].barangay_address + ", " +
                                oCustomerDetails[0].town_address + ", " +
                                oCustomerDetails[0].province + " " ;
                        }
                        
                    }
            }

        } else {


            ShowErrorModal("btnSearch", parsedData.message, parsedData.code);

        }

    } else {
        ShowErrorModal("btnSearch", oData.message, oData.code);
        
    }

}); 



function populateTableLaundry() {
    var divtbl = GetDomElement('tblOthers');
    
    divtbl.innerHTML = '';
    for (var i = 0; i < laundryTbl.length; i++){
        divtbl.innerHTML +=
        "<tr> " +
        "<td class='border text-center d-none'>"+ "1" +"</td>" +
        "<td class='border text-center'>"+ laundryTbl[i].item_description +"</td>" +
        "<td class='border text-center'>"+ laundryTbl[i].item_count +"</td>" +
        "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteLaundry' style='cursor: pointer!important;' ></i></td>" +
        "</tr>";
    }

    var deleteLaundry = document.querySelectorAll('.deleteLaundry');
    for (var i = 0; i < deleteLaundry.length; i++){
        deleteLaundry[i].addEventListener('click', function () {
            var rowIndex = this.parentNode.parentNode.rowIndex;
            
            laundryTbl.splice(rowIndex - 1, 1);

            
            populateTableLaundry();
        });
    }

}

// GetDomElement('txtReference').addEventListener('input', function () {
//     if (this.value == '' || isNaN(this.value)) {
//         GetDomElement('btnAddLaundryItems').disabled = true;
//         GetDomElement('btnAddOtherCategory').disabled = true;
//         GetDomElement('btnAddOtherServices').disabled = true;
//     } else {
//         GetDomElement('btnAddLaundryItems').disabled = false;
//         GetDomElement('btnAddOtherCategory').disabled = false;
//         GetDomElement('btnAddOtherServices').disabled = false;
//    }
// });

GetDomElement('txtbookingReference').addEventListener('input', function () {
    GetDomElement('bookingDateTime').value = '';
    GetDomElement('cellular').innerHTML = 'Contact number';
    GetDomElement('customerName').innerHTML = 'Name';
    GetDomElement('customerAddress').innerHTML = 'Address';
});
// ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
// ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
// ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
// ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
ClosePopupModal("btnOKAlreadyPickedup", "btnCloseAlreadyPickedup");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnSubmitOne", "btnCloseConfirmOne");
ClosePopupModal("btnCancelOne", "btnCloseConfirmOne");

