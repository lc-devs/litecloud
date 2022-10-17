window.addEventListener('load', function () {
    var customerId = sessionStorage.getItem('customerId');
    var customerData = InvokeService(GetURL(), `CustomerMethods/search/customer/id/${customerId}`, 'GET', '');
    if (customerData.code == 200) {
        var parseData = JSON.parse(customerData.data);
        if (parseData.code == 200) {
        var parseData = JSON.parse(parseData.jsonData);
        GetDomElement('detailsCustomerName').innerHTML = parseData[0].customer_name;
        GetDomElement('detailsCustomer').innerHTML = parseData[0].customer_name;
        GetDomElement('detailEmail').innerHTML = parseData[0].email_address;
        GetDomElement('detailCellular').innerHTML = parseData[0].cellular_number;
        GetDomElement('detailsAddress').innerHTML = capitalize(parseData[0].street_building_address) + " " +
                                                    capitalize(parseData[0].barangay_address) + ", " +
                                                    capitalize(parseData[0].town_address) + ", " +
                                                    capitalize(parseData[0].province);
        GetDomElement('detailStatus').innerHTML = getStatus(parseData[0].active_customer);
        GetDomElement('detailStreet').innerHTML = parseData[0].street_building_address;
        GetDomElement('detailBarangay').innerHTML = parseData[0].barangay_address;
        GetDomElement('detailTown').innerHTML = parseData[0].town_address;
        GetDomElement('detailProvince').innerHTML = parseData[0].province;
        
        } else {
            ShowErrorModalCustomer(parseData.message, parseData.code);
            
        }
    } else {
        ShowErrorModalCustomer(customerData.message, customerData.code);
    }
    
});

function ShowErrorModalCustomer(message, errorCode) {
    GetDomElement('btnFailedModal').classList.add('show');
    GetDomElement('btnFailedModal').style.display = "block";
    GetDomElement('failedMessage').innerHTML = message;
    GetDomElement("btnCloseFailedAlert").onclick = () => {
        if (errorCode == 401) {
            location.href = "online-customer-side-login.html";
        } else {
            GetDomElement('btnFailedModal').classList.remove('show');
            GetDomElement('btnFailedModal').style.display = "none";
        }
    };

}

function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

function getStatus(status) {
    if (status == 1) {
        GetDomElement('detailStatus').classList.add('text-success');
        return "Active";
    } else {
        GetDomElement('detailStatus').classList.add('text-secondary');
        return "Blocked";
    }
}

function clearInputModal() {
    GetDomElement('divAddNewInvalidFormEdit').classList.add('d-none');

    var requiredAll = document.querySelectorAll('.edit-required');
    for (var i = 0; i < requiredAll.length; i++){
        requiredAll[i].classList.remove('is-invalid');
    }
}
    
    
GetDomElement('btnUpdate').addEventListener('click', function () {
    var requiredAll = document.querySelectorAll('.edit-required');
    if (this.innerHTML == "UPDATE") {
        this.innerHTML = "SAVE";
        
        for (var i = 0; i < requiredAll.length; i++){
            requiredAll[i].readOnly = false;
        }

        
    } else if (this.innerHTML == "SAVE") {
        
    var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidFormEdit');
    if (BValidForm) {
    var customerId = sessionStorage.getItem('customerId');
        

        GetDomElement("confirmOneSeviceMethod").innerText = "update";
        GetDomElement("confirmOneSeviceMethod").classList.remove('text-danger');
        GetDomElement("confirmOneSeviceMethod").classList.add('text-success');

        GetDomElement("confirmServiceMethod").innerText = "update";
        GetDomElement("confirmServiceMethod").classList.remove('text-danger');
        GetDomElement("confirmServiceMethod").classList.add('text-success');

        this.dataset.target = "#btnConfirmOneModal";
        
        GetDomElement("btnConfirmTwo").onclick = () => {
            var body = {
                "customer_id": customerId,
                "source_id": "2",
                "customer_name": GetDomElement('txtName').value,
                "cellular_number": GetDomElement('cellularData').value ,
                "email_address": GetDomElement('emailData').value,
                "industrial": GetDomElement('chkindustrial').checked == true ? 1 : 0,
                "non_industrial": GetDomElement('chkNonindustrial').checked == true ? 1 : 0,
                "active_customer": 1,
                "account_reset": 0,
                "customer_password": "string",
                "street_building_address": GetDomElement('txtStreet').value,
                "barangay_address": GetDomElement('txtBarangay').value ,
                "town_address": GetDomElement('txtTown').value,
                "province": GetDomElement('txtProvince').value,
                "longitude": 123,
                "latitude": 321
            }
           
    
            var strBody = JSON.stringify(body);

            var oData = InvokeService(GetURL(), `CustomerMethods/update/customer/id/${customerId}`, 'PUT', strBody);

            if (oData.code == 200) {
    
                var parsedData = JSON.parse(oData.data);
                if (parsedData.code == 200) {
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                    ShowSuccessModal('btnConfirmTwo', "update");
                    GetDomElement('btnCloseEditModal').click();
                    location.href = 'online-customer-side-laundry-items-cancellation-pick-up-booking.html';

                } else {
                    ShowErrorModal('btnConfirmTwo', 'Please Contact Adminsitrator', oData.code);
                }
    
            } else {
                    ShowErrorModal('btnConfirmTwo', 'Please Contact Adminsitrator', oData.code);
                
                
            }
        
            
        }

    } else {
        this.dataset.target = "";
    }
    }

});



 
GetDomElement('btnUpdatePassword').addEventListener('click', function () {
    var requiredAll = document.querySelectorAll('.input-required-password');
    if (this.innerHTML == "UPDATE") {
        this.innerHTML = "SAVE";
        
        for (var i = 0; i < requiredAll.length; i++){
            requiredAll[i].readOnly = false;
        }
        GetDomElement('chkShowPass').disabled = false;

    } else if (this.innerHTML == "SAVE") {
        var samePassword;
        var BValidForm = CheckValidForm(requiredAll, 'divAddNewInvalidFormEdit');
        GetDomElement('password1').value == GetDomElement('password2').value ? samePassword = true : samePassword = false; 
    if (BValidForm && samePassword) {
        var customerId = sessionStorage.getItem('customerId');
    GetDomElement('divPasswordNotMatch').classList.add('d-none');
        

        GetDomElement("confirmOneSeviceMethod").innerText = "update";
        GetDomElement("confirmOneSeviceMethod").classList.remove('text-danger');
        GetDomElement("confirmOneSeviceMethod").classList.add('text-success');

        GetDomElement("confirmServiceMethod").innerText = "update";
        GetDomElement("confirmServiceMethod").classList.remove('text-danger');
        GetDomElement("confirmServiceMethod").classList.add('text-success');

        this.dataset.target = "#btnConfirmOneModal";
        
        GetDomElement("btnConfirmTwo").onclick = () => {
            var body = GetDomElement('password1').value;
           
            var strBody = JSON.stringify(body);

            var oData = InvokeService(GetURL(), `CustomerMethods/changepassword/customer/id/${customerId}`, 'PUT', strBody);

            if (oData.code == 200) {
    
                var parsedData = JSON.parse(oData.data);
                if (parsedData.code == 200) {
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                    ShowSuccessModal('btnConfirmTwo', "update");
                    GetDomElement('btnCloseEditModal').click();
                } else {
                    ShowErrorModal('btnConfirmTwo', 'Please Contact Adminsitrator', oData.code);

                }
    
            } else {
                    ShowErrorModal('btnConfirmTwo', 'Please Contact Adminsitrator', oData.code);
                
                
            }
        
            
        }

    } else {
        this.dataset.target = "";
        if (!samePassword && BValidForm) {
            GetDomElement('divPasswordNotMatch').classList.remove('d-none');
        }
    }
    }

});

GetDomElement('btnEditProfile').addEventListener('click', function () {
    openEditModal();
});
GetDomElement('detailsCustomer').addEventListener('click', function () {
    openEditModal();
    
});

function openEditModal() {
    var requiredAll = document.querySelectorAll('.edit-required');

    for (var i = 0; i < requiredAll.length; i++){
        requiredAll[i].classList.remove('is-invalid');
        requiredAll[i].readOnly = true;
    }
    // GetDomElement('basicInfoTab').classList.add('active');
    // GetDomElement('passwordTab').classList.remove('active');

    // GetDomElement('basicInfoTab').classList.add('show');
    // GetDomElement('passwordTab').classList.remove('show');

    // GetDomElement('basicInfoTab').ariaSelected = true;
    // GetDomElement('passwordTab').ariaSelected = false;

    GetDomElement('password1').readOnly = true;
    GetDomElement('password2').readOnly = true;

    GetDomElement('password1').value = '';
    GetDomElement('password2').value = '';

    GetDomElement('password1').classList.remove('is-invalid');
    GetDomElement('password2').classList.remove('is-invalid');

    GetDomElement('chkShowPass').disabled = true;


    GetDomElement('divAddNewInvalidFormEdit').classList.add('d-none');
    GetDomElement('divPasswordNotMatch').classList.add('d-none');

    
    GetDomElement('btnUpdatePassword').innerHTML = 'UPDATE';
    GetDomElement('btnUpdate').innerHTML = 'UPDATE';

    var customerId = sessionStorage.getItem('customerId');
    var customerData = InvokeService(GetURL(), `CustomerMethods/search/customer/id/${customerId}`, 'GET', '');
    if (customerData.code == 200) {
        var parseData = JSON.parse(customerData.data);
        var parseData = JSON.parse(parseData.jsonData);

        //modal values
        GetDomElement('txtName').value = parseData[0].customer_name;
        GetDomElement('cellularData').value = parseData[0].cellular_number;
        GetDomElement('emailData').value = parseData[0].email_address;
        GetDomElement('txtStreet').value = parseData[0].street_building_address;
        GetDomElement('txtBarangay').value = parseData[0].barangay_address;
        GetDomElement('txtTown').value = parseData[0].town_address;
        GetDomElement('txtProvince').value = parseData[0].province;
        parseData[0].industrial == 1 ? (GetDomElement('chkindustrial').checked = true) : (GetDomElement('chkindustrial').checked = false);
        parseData[0].non_industrial == 1 ? (GetDomElement('chkNonindustrial').checked = true) : (GetDomElement('chkNonindustrial').checked = false);

    }
}

GetDomElement('chkShowPass').addEventListener('change', function () {
    if (this.checked == true) {
        GetDomElement("password1").type = "text";
        GetDomElement("password2").type = "text";
    } else {
        GetDomElement("password1").type = "password";
        GetDomElement("password2").type = "password";
   }
});

ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
