var error = false;
var itemArray = []

// GetDomElement('btnAdd').addEventListener('click', function () {
//     this.dataset.target = "#btnAddNew";
// });

// GetDomElement('btnSubmitModal').addEventListener('click', function () {

//     var amount = document.querySelectorAll('.amount');
//     var requiredAll = document.querySelectorAll('.input-required');
//     var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidForm');
//     if (BValidForm) {
//         GetDomElement('divAddNewInvalidForm').classList.add('d-none');
//         var Bcheck = CheckValidAmount(amount, 'divAddNewInvalidAmount');
//     } else {
//         GetDomElement('divAddNewInvalidAmount').classList.add('d-none');
        
//     }
//     if (BValidForm && Bcheck) {
//     GetDomElement('divAddLaundryItems').classList.add('d-none');

//         industrialDetails.push({
//             "so_reference": 0,
//             "item_code": GetDomElement('selItem').value,
//             "item_count":  GetDomElement('txtItemCount').value,
//         })
        
//         industrialForTbl.push({
//             "so_reference": 0,
//             "item_description": GetDomElement('selItem').options[GetDomElement('selItem').selectedIndex].text,
//             "item_count":  GetDomElement('txtItemCount').value,
//         })

//         populateTableItems();
//     }
// });

GetDomElement('btnConfirmLogistics').addEventListener('click', function () {
    error = false;
    getArray()
    var cellular = GetDomElement('txtCellular').value; 
    var oData = InvokeService(GetURL(),`CustomerMethods/search/customer/contact/${cellular}`, 'GET','')
    this.dataset.target = "";
    
    if (oData.code == 200) {
        var parseData = JSON.parse(oData.data);
        if (parseData.code != 200) {
            alert('Cellular Number not recognize');
            error = true;

            GetDomElement('txtCellular').classList.add('is-invalid');
        } else {
            GetDomElement('txtCellular').classList.remove('is-invalid');
            
        }
    } else {
        alert('BookingReference not recognize');
        GetDomElement('txtCellular').classList.add('is-invalid');
    }

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
    // if (IsEmpty('txtReference') || !+GetDomElement('txtReference').value) {
    //     GetDomElement('txtReference').classList.add('is-invalid');
    //     error = true;

    // } else {
    //     GetDomElement('txtReference').classList.remove('is-invalid');
        
    // }
    if (itemArray.length <= 0) {
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
                "so_reference": 0,
                "customer_id": +GetDomElement('customerId').innerHTML,
                "booking_reference": "",
                "picked_up_by": sessionStorage.getItem('userId'),
                "picked_up_datetime": "string",
                "weight_in_kg": +GetDomElement('txtWeight').value,
                "number_of_bags": +GetDomElement('txtBags').value,
                "received_by_logistics": "string",
                "so_reference_QR_Image": "string",
                "pickupIndustrialDetail": itemArray
            }

            var strBody = JSON.stringify(body);
            var oData = InvokeService(GetURL(), 'PickupIndustrialMethods', 'POST', strBody);

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
                    
                   
                    GetDomElement('txtWeight').value = '';
                    GetDomElement('txtBags').value = '';
                    GetDomElement('txtCellular').value = '';

                    
                    GetDomElement('customerName').innerHTML = 'Name';
                    GetDomElement('customerAddress').innerHTML = 'Address';
                    
                } else {
                    ShowErrorModal('btnConfirmTwo',parsedData.message, parsedData.code);

                }
    
            } else {
                    ShowErrorModal('btnConfirmTwo', oData.message, oData.code);
                
                
            }
        
            
        }

    } else {
        this.dataset.target = "";
    }
    
});

GetDomElement('btnSearch').addEventListener('click', function () {
    var cellular = GetDomElement('txtCellular').value; 
    var oData = InvokeService(GetURL(),`CustomerMethods/search/customer/contact/${cellular}`, 'GET','')
    if (oData.code == 200) {

        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            this.dataset.target = "";
            var datas = JSON.parse(parsedData.jsonData);
            GetDomElement('customerId').innerHTML = datas[0].customer_id;
            
            var oCustomers = InvokeService(GetURL(),`CustomerMethods/search/customer/id/${datas[0].customer_id}`, 'GET','')
            
            if (oCustomers.code == 200) {
                var oCustomer = JSON.parse(oCustomers.data);
                if (oCustomer.code == 200) {
                    var oCustomerDetails = JSON.parse(oCustomer.jsonData);
                    GetDomElement('customerName').innerHTML = oCustomerDetails[0].customer_name;
                    GetDomElement('customerAddress').innerHTML = oCustomerDetails[0].street_building_address + " " +
                        oCustomerDetails[0].barangay_address + ", " +
                        oCustomerDetails[0].town_address + ", " +
                        oCustomerDetails[0].province + " " ;
                }
                
            }
        } else {

            ShowErrorModal("btnSearch", parsedData.message, parsedData.code);

        }

    } else {
        ShowErrorModal("btnSearch", oData.message, oData.code);
        
    }

}); 

GetDomElement('btnPrintQR').addEventListener('click', function () {
    location.href = 'online-industrial-pickup-query-report.html';
});

function PopulateLaundryItemDropDown(selectField) {
    var jsonData = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "GET", "");
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

function populateTableItems() {
    var divtbl = GetDomElement('tblItems');
    divtbl.innerHTML = "";
    for (var i = 0; i < industrialForTbl.length; i++){
        divtbl.innerHTML +=
        "<tr> " +
        "<td class='border text-center'>"+ industrialForTbl[i].item_description +"</td>" +
        "<td class='border text-center'>"+ industrialForTbl[i].item_count +"</td>" +
        "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
        "</tr>";
    }
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
  
window.addEventListener('load', function () {
    // GetDomElement('btnAdd').disabled = true;
    // GetDomElement('btnConfirmLogistics').disabled = true;
    PopulateLaundryItemDropDown('selItem');
    populateLundryItemTable();
});

function populateLundryItemTable() {
    var jsonData = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = GetDomElement('tblItems');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center d-none'>"+ oData[i].id_item +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].category.description +"</td>" +
            "<td class='border text-center px-2'><input class='form-control item-count text-center'></td>" +
            "</tr>";
        }

    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}
function printModal() {
    GetDomElement('allDiv').style.visibility = "hidden";
    GetDomElement('btnSuccessEntryModal').style.visibility = "visible";
    window.print();
    GetDomElement('allDiv').style.visibility = "visible";
}

function getArray() {
    var itemCount = document.querySelectorAll('.item-count');
    itemArray = [];
    for (var i = 0; i < itemCount.length; i++){
        if (itemCount[i].value != '') {
            itemArray.push({
                            "so_reference": 0,
                            "item_code": +itemCount[i].parentNode.parentNode.cells[0].innerHTML,
                            "item_count":  +itemCount[i].value,
                    })
        } else {
            
        }
    }

    // return itemArray;
    
}

// GetDomElement('txtReference').addEventListener('input', function () {
//     if (this.value == '' || isNaN(this.value)) {
//         GetDomElement('btnAdd').disabled = true;
//     } else {
//         GetDomElement('btnAdd').disabled = false;
//    }
// });

GetDomElement('txtCellular').addEventListener('input', function () {
    
    GetDomElement('customerName').innerHTML = 'Name';
    GetDomElement('customerAddress').innerHTML = 'Address';
});
ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
// ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
// ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");