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
//         this.dataset.target = '#btnConfirm';
//     }
// });

// ClosePopupModal('btnConfirmCancel', 'btnCloseConfirm');

GetDomElement('btnSaveEntry').addEventListener('click', function () {
    var error = false;
    if (IsEmpty('txtWeight') || !+GetDomElement('txtWeight').value) {
        GetDomElement('txtWeight').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('txtWeight').classList.remove('is-invalid');
        
    }

    // if (IsEmpty('txtBags') || !+GetDomElement('txtBags').value) {
    //     GetDomElement('txtBags').classList.add('is-invalid');
    // } else {
    //     GetDomElement('txtBags').classList.remove('is-invalid');
    // }

    // if (IsEmpty('txtReference') || !+GetDomElement('txtReference').value) {
    //     GetDomElement('txtReference').classList.add('is-invalid');
    // } else {
    //     GetDomElement('txtReference').classList.remove('is-invalid');
    //     error = true;
    // }

    if (IsEmpty('txtLoads') || !+GetDomElement('txtLoads').value) {
        GetDomElement('txtLoads').classList.add('is-invalid');
        error = true;

    } else {
        GetDomElement('txtLoads').classList.remove('is-invalid');
      

    }

    if (IsEmpty('txtAmountDue') || !+GetDomElement('txtAmountDue').value) {
        GetDomElement('txtAmountDue').classList.add('is-invalid');
        error = true;

    } else {
        GetDomElement('txtAmountDue').classList.remove('is-invalid');
       

    }
    
    if (IsEmpty('selModeOfPayment')) {
        GetDomElement('selModeOfPayment').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('selModeOfPayment').classList.remove('is-invalid');
        

    }
    if (GetDomElement('filepaymentImage').value == '') {
        GetDomElement('filepaymentImage').classList.add('is-invalid');
        error = true;
    } else {
        GetDomElement('filepaymentImage').classList.remove('is-invalid');
        
    }
    this.dataset.target = "";
   
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
                    "customer_name": GetDomElement('customerName').innerHTML,
                    "cellular_number": GetDomElement('txtCellular').value,
                    "weight_in_kg": +GetDomElement('txtWeight').value,
                    "number_of_loads": +GetDomElement('txtLoads').value,
                    "amount_due": +GetDomElement('txtAmountDue').value, 
                    "payment_mode": GetDomElement('selModeOfPayment').value, 
                    "entry_datetime": "string",
                    "posted_by": sessionStorage.getItem('userId'),
                    "payment_image": GetDomElement('paymentImage').src,
                    "so_qr_image": "string"
                  }
            
            var strBody = JSON.stringify(body);
            var oData = InvokeService(GetURL(), 'NonIndustrialSelfServiceMethods', 'POST', strBody);

            if (oData.code == 200) {
    
                var parsedData = JSON.parse(oData.data);
                if (parsedData.code == 200) {
                    parsedData = JSON.parse(parsedData.jsonData)
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                  
                    GetDomElement('btnConfirmTwo').dataset.target = "#btnSuccessEntryModal";
                    location.href = 'online-non-industrial-self-service.html';
                    // var base64 = parsedData.so_reference_QR_Image;
                    // GetDomElement('imageQR').setAttribute('src', base64);
                    // GetDomElement('QRsoReference').innerHTML = parsedData.so_reference;
                
                } else {
                    ShowErrorModal('btnConfirmTwo', oData.message, oData.code);
                }
    
            } else {
                ShowErrorModal('btnConfirmTwo',parsedData.message , oData.code);
            }
        
            
        }

    } else {
        this.dataset.target = "";
    }
});

window.addEventListener('load', function () {
    // var customerId = GetDomElement('customerIdSearch').innerHTML;
    // var pickUpby = sessionStorage.getItem('userId');

    populateSelfService();

    PopulateLaundryItemDropDown('selModeOfPayment');

});

function populateSelfService() {
    var dateNow = new Date();
    var date = dateNow.toISOString().slice(0, 10);
    
    var oData = InvokeService(GetURL(), `NonIndustrialSelfServiceMethods/selfservicequeryreport?dateFrom=${date}&dateTo=${date}&customerID=0&postedBy=${sessionStorage.getItem('userId')}`, 'GET', '');
    
    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            var parsedData = JSON.parse(parsedData.jsonData);
            // var parsedData = JSON.parse(parsedData);
            populateTableQuery(parsedData)
        } else if(parsedData.code == 401) {
                
            ShowErrorModalOnLoad('', parsedData.code);

        } else {
            GetDomElement('tblQuery').innerHTML = '';
            
        }
    } else {
        ShowErrorModalOnLoad(oData.message, oData.code);

    }
}

function populateTableQuery(parsedData) {
    
    var divtbl = GetDomElement('tblQuery');
    divtbl.innerHTML = "";
    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-success so-reference' data-toggle='modal'>" + parsedData[i].so_reference + "</td>" +
            "<td class='border text-center px-0 '>" + parsedData[i].weight_in_kg + "kg </td>" +
            "<td class='border text-center px-0'>" + parsedData[i].number_of_loads + " bags </td>" +
            "<td class='border text-center px-0 text-warning'>" +  parsedData[i].payment_mode + "  </td>" +
            "<td class='border text-center px-0'>" + `${parsedData[i].non_cash == 0? parsedData[i].amount_due : '' }` + "  </td>" +
            "<td class='border text-center px-0'>" + `${parsedData[i].non_cash == 1? parsedData[i].amount_due : '' }` + "  </td>" +
            "<td class='border text-center px-0'>" + parsedData[i].cellular_number + "  </td>" +
            "<td class='border text-center px-0 text-info text-capitalize'>" + parsedData[i].customer_name + "  </td>" +
            "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteSelfService' data-toggle='modal' data-target='#btnConfirmOneModal' style='cursor: pointer!important;' ></i></td>" +
            "</tr>";
        
            var soReference = document.querySelectorAll('.so-reference')
            for (var i = 0; i < soReference.length; i++){
                // soReference[i].style.cursor = 'pointer';
                // soReference[i].style.textDecoration = 'underline';
                
                soReference[i].addEventListener('click', function () {
                        
                    if (!isNaN(this.innerHTML)) {
                        var soData = InvokeService(GetURL(), `NonIndustrialSelfServiceMethods/sodetails/${this.innerHTML}`, 'GET', '');
                        if (soData.code == 200) {
                            
                            var parseData = JSON.parse(soData.data);
                            if (parseData.code == 200) {
                                this.dataset.target = '#btnSODetails';
                                var parseData = JSON.parse(parseData.jsonData)
                                var base64 = parseData[0].payment_image;
                                
                                GetDomElement('imageQR').setAttribute('src', base64);
                                
                                so_reference.innerHTML = parseData[0].so_reference;
                                customer_name.innerHTML = parseData[0].customer_name;
                                cellular_number.innerHTML = parseData[0].cellular_number;
                                posted_by.innerHTML = parseData[0].posted_by;
                                payment_mode.innerHTML = parseData[0].payment_mode;
                                weight_in_kg.innerHTML = parseData[0].weight_in_kg;
                                entry_datetime.innerHTML = moment(parseData[0].entry_datetime).format('LLL');
                                // picked_up_datetime.innerHTML = moment(parseData.picked_up_datetime).format('LLL');
                                
                                // if (parseData.items != undefined) {
                                //     var divtbl = GetDomElement('tblSODetails');
                                //     divtbl.innerHTML = "";
                                //     for (var i = 0; i < parseData.items.length; i++) {
                                //         divtbl.innerHTML +=
                                //             "<tr> " +
                                //             "<td class='border text-center px-0 text-success'>" + parseData.items[i].item_description + "</td>" +
                                //             "<td class='border text-center px-0 text-warning'>" + parseData.items[i].item_count + "</td>" +
                                //             "</tr>";
        
                                //     }
                                // }
                                            
                                            
        
                            }
                        }
        
                    }
                });
        }
        var deleteBook = document.querySelectorAll('.deleteSelfService');

            for (var i = 0; i < deleteBook.length; i++) {
                deleteBook[i].addEventListener('click', function () {
                var referenceNumber = this.parentNode.parentNode.cells[0].innerHTML

                GetDomElement("confirmOneSeviceMethod").innerText = "cancel";
                GetDomElement("confirmOneSeviceMethod").classList.add('text-danger');
                GetDomElement("confirmServiceMethod").innerText = "cancel";
                GetDomElement("confirmServiceMethod").classList.add('text-danger');

                GetDomElement("btnConfirmTwo").onclick = () => {
                    var deleteBooking = InvokeService(GetURL(), `NonIndustrialSelfServiceMethods/cancelso/${referenceNumber}`,'DELETE','');
                  
                    
                    if (deleteBooking.code == 200) {
                        
                        GetDomElement("btnCloseConfimrTwo").click();
                        GetDomElement("btnConfirmOneModal").click();
                        // GetDomElement("btnConfirmTwo").dataset.target = "#btnSuccessEntryModal";
                        // GetDomElement("successServiceMethod").innerText = "cancelled";
                        populateSelfService();

                        
                        
                    }
                }

            });
        }

    }
}

GetDomElement('txtCellular').addEventListener('input', function () {
    GetDomElement('customerId').innerHTML = '0';
    GetDomElement('customerName').innerHTML = 'customer name';
    GetDomElement('customerAddress').innerHTML = 'customer address';
});

function PopulateLaundryItemDropDown(selectField) {
    var jsonData = InvokeService(GetURL(), "PaymentModeMethods", "GET", "");
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
            	selCategory.innerHTML += "<option value=\"" + oData[i].payment_code + "\">" + oData[i].description + "</option>";
            }


    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}


GetDomElement('btnSearch').addEventListener('click', function () {
    var cellular = GetDomElement('txtCellular').value; 
    var oData = InvokeService(GetURL(),`CustomerMethods/search/customer/contact/${cellular}`, 'GET','')
    if (oData.code == 200) {

        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            GetDomElement('txtCellular').classList.remove('is-invalid');

            this.dataset.target = "";
            var datas = JSON.parse(parsedData.jsonData);
            GetDomElement('customerId').innerHTML = datas[0].customer_id;
            if (GetDomElement('txtWeight').value != '') {
                GetDomElement('txtLoads').value = Math.ceil(+GetDomElement('txtWeight').value / +datas[0].weight_per_load);
            }
            GetDomElement('txtWeight').addEventListener('input', function () {
                GetDomElement('txtLoads').value = Math.ceil(+this.value / +datas[0].weight_per_load);
            });
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

            GetDomElement('txtCellular').classList.add('is-invalid');

        }

    } else {
        ShowErrorModal("btnSearch", oData.message, oData.code);
        
    }

}); 

GetDomElement('filepaymentImage').addEventListener('change', function () {
    
    var reader = new FileReader();
      reader.onload = function (e) { 
        document.querySelector("#paymentImage").setAttribute("src",e.target.result);
      };

      reader.readAsDataURL(this.files[0]); 

});
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
ClosePopupModal("btnOKSODetails", "btnCloseSODetails");
