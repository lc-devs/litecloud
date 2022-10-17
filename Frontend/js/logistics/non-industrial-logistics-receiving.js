var itemArray = [];
var itemTbl = [];

var itemArrayOthers = [];
var itemTblOthers = [];

var itemArrayServices = [];
var itemTblServices = [];

GetDomElement('btnConfirmLogistics').addEventListener('click', function () {
    
    var error = false;
    getArray();
    getArrayServices();
    getArrayOther();
    
    
   
    allItem = itemArray.concat(itemArrayOther);



    if (IsEmpty('txtWeight') || !+GetDomElement('txtWeight').value) {
        GetDomElement('txtWeight').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('txtWeight').classList.remove('is-invalid');
        
    }
    if (IsEmpty('txtBags') || !+GetDomElement('txtBags').value) {
        GetDomElement('txtBags').classList.add('is-invalid');
        error = true;

    } else {
        GetDomElement('txtBags').classList.remove('is-invalid');
        
    }

    if (GetDomElement('customerName').innerHTML == 'Name') {
        error = true;
        GetDomElement('txtSOReference').classList.add('is-invalid');
    } else {
        GetDomElement('txtSOReference').classList.remove('is-invalid');
        
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
            var body = {
                "so_reference": +GetDomElement('txtSOReference').value,
                "customer_id": +GetDomElement('customerId').innerHTML,
                "booking_reference": GetDomElement('booking_reference').innerHTML,
                "picked_up_by": GetDomElement('picked_up_by').innerHTML,
                "picked_up_datetime": GetDomElement('picked_up_datetime').innerHTML,
                "weight_in_kg": +GetDomElement('txtWeight').value,
                "number_of_loads": 0,
                "number_of_bags": +GetDomElement('txtBags').value,
                 //change
                
                "received_by_logistics_user": sessionStorage.getItem('userId'),
                "received_from_pickup_datetime": "string",
                "received_by_laundry": "string",
                "received_by_laundry_datetime": "string",
                "completed_by_laundry": "string",
                "completed_by_laundry_datetime": "string",
                "received_from_laundry": 0,
                "received_from_laundry_datetime": "string",
                "for_invoicing": 0,
                "with_invoice": 0,
                "delivered": 0,
                "delivery_datetime": "string",
                "items": allItem,
                "services": itemArrayServices,
            }

            
            GetDomElement('btnCloseConfirmOne').click();
            var strBody = JSON.stringify(body);

            var oData = InvokeService(GetURL(), 'LogisticNonIndustrialMethods/receiveditemfrompickup', 'POST', strBody);
            if (oData.code == 200) {
    
                var parsedData = JSON.parse(oData.data);
                if (parsedData.code == 200) {
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                    ShowSuccessModal('btnConfirmTwo', "added");
                    // location.href = "online-non-industrial-pickup-query-report.html"
                    
                    GetDomElement('txtSOReference').value = '';

                     GetDomElement('txtWeight').value = '';
                     GetDomElement('txtBags').value = '';
                     GetDomElement('txtLoads').value = '';
                     
                     GetDomElement('cellular').innerHTML = 'Contact Number';
                     GetDomElement('customerName').innerHTML = 'Name';
                    GetDomElement('customerAddress').innerHTML = 'Address';


                    var divtbl = GetDomElement('tblItems');
                    divtbl.innerHTML = "";
    
                    var divtblServices = GetDomElement('tblServices');
                    divtblServices.innerHTML = "";
    
                    var divtblOthers = GetDomElement('tblOthers');
                    divtblOthers.innerHTML = "";
                    
                    
                } else {
                    ShowErrorModal('btnConfirmTwo', parsedData.message, oData.code);
                }
    
            } else {
                ShowErrorModal('btnConfirmTwo',oData.message, oData.code);
            }
        
            
        }

    } else {
        this.dataset.target = "";
    }
});


GetDomElement('btnSubmitModal').addEventListener('click', function () {

    var amount = document.querySelectorAll('.amounts');
    var requiredAll = document.querySelectorAll('.input-required-modal');
    var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
    if (BValidForm) {
        GetDomElement('divAddNewInvalidForm').classList.add('d-none');
        var Bcheck = CheckValidAmount(amount, 'divAddNewInvalidAmount');
    } else {
        GetDomElement('divAddNewInvalidAmount').classList.add('d-none');
        
    }
    if (BValidForm && Bcheck) {
    GetDomElement('divAddLaundryItems').classList.add('d-none');

        
        itemTblOthers.push({
            "so_reference": GetDomElement('txtSOReference').value,
            "item_description": GetDomElement('itemDescription').value,
            "item_count":  GetDomElement('txtItemCount').value,
        })

        populateTableOthers();
    }
});

GetDomElement('btnSearch').addEventListener('click', function () {
    var soRef = GetDomElement('txtSOReference'); 
    if (GetDomElement('txtSOReference').value != '') {
        
            
        var soData = InvokeService(GetURL(), `PickupNonIndustrialMethods/${soRef.value}`, 'GET', '');
        if (soData.code == 200) {
            this.dataset.target = "";
            
            var parseData = JSON.parse(soData.data);
            if (parseData.code == 200) {
                this.dataset.target = "";
                soRef.classList.remove('is-invalid');
                var parseData = JSON.parse(parseData.jsonData);
                if (parseData.received_by_logistics == 0) {
                        
                    GetDomElement('customerId').innerHTML = parseData.customer_id;
                    GetDomElement('customerName').innerHTML = parseData.customer_name;

                    
                    GetDomElement('booking_reference').innerHTML = parseData.booking_reference;
                    GetDomElement('picked_up_by').innerHTML = parseData.picked_up_by;
                    GetDomElement('picked_up_datetime').innerHTML = parseData.picked_up_datetime;

                    GetDomElement('txtWeight').value = parseData.weight_in_kg;
                    GetDomElement('txtBags').value = parseData.number_of_bags;
                    GetDomElement('txtLoads').value = parseData.number_of_loads;
                    if (parseData.items != undefined) {
                        for (var i = 0; i < parseData.items.length; i++){
                            if (parseData.items[i].item_code == 1) {
                                
                                itemTblOthers.push({
                                    "item_code": parseData.items[i].item_code,
                                    "so_reference": parseData.items[i].so_reference,
                                    "item_description": parseData.items[i].item_description,
                                    "item_count":  parseData.items[i].item_count,
                                })
                            } else {
                                
                                itemTbl.push({
                                    "item_code": parseData.items[i].item_code,
                                    "so_reference": parseData.items[i].so_reference,
                                    "item_description": parseData.items[i].item_description,
                                    "item_count":  parseData.items[i].item_count,
                                })
                            }
                        }
                    }
                    if (parseData.services != undefined ) {
                        for (var i = 0; i < parseData.services.length; i++){
                        
                                itemTblServices.push({
                                    
                                    "so_reference": parseData.services[i].so_reference,
                                    "description": parseData.services[i].description,
                                    "service_code": parseData.services[i].service_code,
                                    
                                })
                            
                        }
                    }
                    



                    var oCustomers = InvokeService(GetURL(),`CustomerMethods/search/customer/id/${parseData.customer_id}`, 'GET','')

                    if (oCustomers.code == 200) {
                        var oCustomer = JSON.parse(oCustomers.data);
                        if (oCustomer.code == 200) {
                            var oCustomerDetails = JSON.parse(oCustomer.jsonData);
                            GetDomElement('cellular').innerHTML = oCustomerDetails[0].cellular_number;
                            GetDomElement('customerAddress').innerHTML = oCustomerDetails[0].street_building_address + " " +
                                oCustomerDetails[0].barangay_address + ", " +
                                oCustomerDetails[0].town_address + ", " +
                                oCustomerDetails[0].province + " ";
                            
                            
                            populateTableItems();
                            populateTableOthers();
                            populateTableServices();
                            
                        }
                        
                    }
                } else {
                    
                    this.dataset.target = "#btnAlreadyReceived";
                    
                    var divtbl = GetDomElement('tblItems');
                    divtbl.innerHTML = "";

                    var divtblServices = GetDomElement('tblServices');
                    divtblServices.innerHTML = "";

                    var divtblOthers = GetDomElement('tblOthers');
                    divtblOthers.innerHTML = "";
                }
                
            } else {
                soRef.classList.add('is-invalid');
                ShowErrorModal('btnSearch',parseData.message,parseData.code)
                var divtbl = GetDomElement('tblItems');
                divtbl.innerHTML = "";

                var divtblServices = GetDomElement('tblServices');
                divtblServices.innerHTML = "";

                var divtblOthers = GetDomElement('tblOthers');
                divtblOthers.innerHTML = "";
            }
            
        } else {

            ShowErrorModal('btnSearch',parseData.message,parseData.code)
            var divtbl = GetDomElement('tblItems');
            divtbl.innerHTML = "";

            var divtblServices = GetDomElement('tblServices');
            divtblServices.innerHTML = "";

            var divtblOthers = GetDomElement('tblOthers');
            divtblOthers.innerHTML = "";



            
        }
    } else {
        soRef.classList.add('is-invalid');
    }

});

GetDomElement('txtSOReference').addEventListener('input', function () {
    GetDomElement('customerId').innerHTML = "";
    GetDomElement('customerName').innerHTML = "Name";
    GetDomElement('customerAddress').innerHTML = "Address";
    GetDomElement('cellular').innerHTML = "Contact Number";

    GetDomElement('txtWeight').value = "";
    GetDomElement('txtBags').value = "";
    
});
// ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
// ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
// ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
// ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");

ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");

GetDomElement('openCamera').addEventListener('click', function () {
    ReadCamera("selCameraList", "divReader", "txtSOReference", "btnSearch", "btnCloseQRModal");
});

function populateTableItems() {
    // industrialDetails = {
    //     "so_reference": parseData.items[i].so_reference,
    //     "item_code": parseData.items[i].item_code,
    //     "item_count": parseData.items[i].item_count,
    //     "adl_cost": 0,
    //     "adl_adjustment": 0,
    // }
        
    // industrialForTbl = {
    //     "so_reference": parseData.items[i].so_reference,
    //     "item_description": parseData.items[i].item_description,
    //     "item_count":  parseData.items[i].item_count,
    // }


    var itemCount = document.querySelectorAll('.item-count');
    for (var i = 0; i < itemTbl.length; i++){
        for (var j = 0; j < itemCount.length; j++){
            if (itemCount[j].parentNode.parentNode.cells[1].innerHTML == itemTbl[i].item_description) {
                itemCount[j].value = itemTbl[i].item_count;
                itemCount[j].parentNode.parentNode.cells[0].innerHTML = itemTbl[i].item_code;
                break;
            }
            // else {
            //     itemCount[j].value = '';
                
            // }
        }

        
    }
    // var divtbl = GetDomElement('tblItems');
    // divtbl.innerHTML = "";
    // for (var i = 0; i < industrialForTbl.length; i++){
    //     divtbl.innerHTML +=
    //     "<tr> " +
    //     "<td class='border text-center'>"+ industrialForTbl[i].item_description +"</td>" +
    //     "<td class='border text-center'>"+ industrialForTbl[i].item_count +"</td>" +
    //     "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
    //     "</tr>";
    // }
    // var deleteItems = document.querySelectorAll('.deleteItems');
    // for (var i = 0; i < deleteItems.length; i++){
    //     deleteItems[i].addEventListener('click', function () {
    //         var rowIndex = this.parentNode.parentNode.rowIndex;

    //         industrialDetails.splice(rowIndex-1, 1);
    //         industrialForTbl.splice(rowIndex - 1, 1);
    //         if (industrialDetails.length <= 0) {
    //             GetDomElement('divAddLaundryItems').classList.remove('d-none');
    //             error = true;
        
    //         } else {
    //             GetDomElement('divAddLaundryItems').classList.add('d-none');
    //         }
    //         populateTableItems();
    //     });
    // }

}

function populateTableServices() {

    var checkService = document.querySelectorAll('.check-service');
    
    for (var i = 0; i < itemTblServices.length; i++){
        for (var j = 0; j < checkService.length; j++){
            if (checkService[j].parentNode.parentNode.cells[0].innerHTML == itemTblServices[i].service_code) {
                checkService[j].checked = true;
                break;
            } else {
                checkService[j].checked = false;
            }
            
        }

    }
   

}

function populateTableOthers() {

    var divtbl = GetDomElement('tblOthers');
    divtbl.innerHTML = "";
    for (var i = 0; i < itemTblOthers.length; i++){
        divtbl.innerHTML +=
        "<tr> " +
        "<td class='border text-center'>"+ itemTblOthers[i].item_description +"</td>" +
        "<td class='border text-center'>"+ itemTblOthers[i].item_count +"</td>" +
        "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
        "</tr>";
    }
    var deleteItems = document.querySelectorAll('.deleteItems');
    for (var i = 0; i < deleteItems.length; i++){
        deleteItems[i].addEventListener('click', function () {
            var rowIndex = this.parentNode.parentNode.rowIndex;

            itemTblOthers.splice(rowIndex-1, 1);
            
            populateTableOthers();
        });
    }

}


window.addEventListener('load', function () {
    // GetDomElement('btnAdd').disabled = true;
    // GetDomElement('btnConfirmLogistics').disabled = true;
    // PopulateLaundryItemDropDown('selItem');
    populateLundryServices();
    populateLundryItemTable();
});


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
            "<td class='border text-center px-0'>"+ oData[i].unit_cost +"</td>" +
            // "<td class='border text-center px-2'></td>" +
            "</tr>";
        }

    }else if(parseJsonData.code== 404){

    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}

function populateLundryItemTable() {
    var jsonData = InvokeService(GetURL(), "NonIndustrialLaundryItemMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = GetDomElement('tblItems');
        divtbl.innerHTML = "";
        for (var i = 1; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center d-none'>"+ oData[i].id_item +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].description +"</td>" +
            "<td class='border text-center px-2'><input class='form-control item-count text-center'></td>" +
            "</tr>";
        }

    } else if(parseJsonData.code == 404){
        
    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}

function getArray() {
    var itemCount = document.querySelectorAll('.item-count');
    itemArray = [];
    for (var i = 0; i < itemCount.length; i++){
        
        if (itemCount[i].value != '') {
            itemArray.push({
                            "so_reference": GetDomElement('txtSOReference').value,
                            "item_code": +itemCount[i].parentNode.parentNode.cells[0].innerHTML,
                             "item_count": +itemCount[i].value,
                            "description": "",
                    })
        } else {
            
        }
    }

    // return itemArray;
    
}


function getArrayServices() {
    var check = document.querySelectorAll('.check-service');
    itemArrayServices = [];
    
    for (var i = 0; i < check.length; i++){
        
        if (check[i].checked == true) {
            
            itemArrayServices.push({
                            "so_reference": GetDomElement('txtSOReference').value,
                            "service_code": +check[i].parentNode.parentNode.cells[0].innerHTML,
                            "cost":  +check[i].parentNode.parentNode.cells[3].innerHTML,
                    })
        } else {
            
        }
    }
    
}

function getArrayOther() {
    itemArrayOther = [];
    
    for (var i = 0; i < itemTblOthers.length; i++){    
        itemArrayOther.push({
            "so_reference": GetDomElement('txtSOReference').value,
            "item_code": 1,
            "item_count": itemTblOthers[i].item_count,
            "description": itemTblOthers[i].item_description,
        })

    }
    
}
function PopulateLaundryItemDropDown(selectField) {
    var jsonData = InvokeService(GetURL(), "NonIndustrialLaundryItemMethods", "GET", "");
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
ClosePopupModal("btnOKAlreadyReceived", "btnCloseAlreadyReceived");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnSubmitOne", "btnCloseConfirmOne");
ClosePopupModal("btnCancelOne", "btnCloseConfirmOne");
