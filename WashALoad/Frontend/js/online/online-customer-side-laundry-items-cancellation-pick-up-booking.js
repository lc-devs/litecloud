var customerId = sessionStorage.getItem('customerId');

GetDomElement('btnDateTime').addEventListener('click', function () {
    var time = GetDomElement('timeData').value;
    var date = GetDomElement('dateData').value;
    

    var dateNow = new Date();
    dateNow = date.toLocaleString('en-GB');
    dateNow = date.replace(',', '');
    dateNow = date.replace('/', '-');
    
    if (time == "" || date == "") {
        GetDomElement('divAddNewInvalidForm').classList.remove('d-none');
        this.dataset.target = "";

    } else {
        GetDomElement('divAddNewInvalidForm').classList.add('d-none'); 

        GetDomElement("confirmOneSeviceMethod").innerText = "add";
        GetDomElement("confirmOneSeviceMethod").classList.remove('text-danger');
        GetDomElement("confirmOneSeviceMethod").classList.add('text-success');

        GetDomElement("confirmServiceMethod").innerText = "add";
        GetDomElement("confirmServiceMethod").classList.remove('text-danger');
        GetDomElement("confirmServiceMethod").classList.add('text-success');
        


        this.dataset.target = "#btnConfirmOneModal";
        
        GetDomElement("btnConfirmTwo").onclick = () => {
            var oData = 
                {
                    "booking_reference": "string",
                    "booking_datetime": dateNow,
                    "customer_id": customerId,
                    "scheduled_pickup_datetime": date+ ' ' +time,
                    "so_reference": "string",
                    "industrial": 1,
                    "picked_up_by": "string",
                    "actual_picked_up_datetime": "string",
                    "cancelled_booking": 0
            }

            
            var strData = JSON.stringify(oData);
            
            var oBooking = InvokeService(GetURL(), 'BookingsForPickupMethods', 'POST', strData);
            if (oBooking.code == 200) {
    
                var parsedBooking = JSON.parse(oBooking.data);
                if (parsedBooking.code == 200) {
                    
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                    ShowSuccessModal("btnConfirmTwo","added")
                    populateBookings();
                } else {
                    // GetDomElement('divUnauthorizedAlert').classList.remove('d-none');
                    //service not avai
                    ShowErrorModalCustomers('btnConfirmTwo', parsedBooking.message, parsedBooking.code);

                }
    
            } else {
                ShowErrorModalCustomers('btnConfirmTwo', oBooking.message, oBooking.code);
                
            }
        
            
        }
    }
});

function ShowErrorModalCustomers(dataToggle, errorMessage, errorCode) {
    var triggerAlert = GetDomElement("failedMessage");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnFailedModal";
    triggerAlert.innerHTML = errorMessage;
    GetDomElement("btnCloseFailedAlert").onclick = () => {
        if (errorCode == 401) {
            location.href = "online-customer-side-login.html";
        } else {
            GetDomElement('btnCloseConfimrTwo').click();
        }
    };
}

window.addEventListener('load', function () {
    // tblCustomerBooking 
    populateBookings();
});

function populateBookings() {
    var dataCustomer = InvokeService(GetURL(), `BookingsForPickupMethods/bookings/customer/${customerId}`,'GET','');
    
    if (dataCustomer.code == 200) {
        var parsedData = JSON.parse(dataCustomer.data);
        if (parsedData.code == 200) {
            var parsedData = JSON.parse(parsedData.jsonData);
            

            var tblBoody = GetDomElement('tblCustomerBooking');
            tblBoody.innerHTML = "";
            for (var i = 0; i < parsedData.length; i++){
                if (parsedData[i].cancelled_booking == 0) {
                    
                tblBoody.innerHTML +=
                "<tr> " +
                "<td class='border booking-ref text-center text-info' onclick='tracking(this)' data-toggle='modal' data-target='#btnTracking'>" + parsedData[i].booking_reference + "</td>" +
                "<td class='border text-center '>" +  `${parsedData[i].so_reference == "" ? "<label class='text-danger'>N/A</label>": parsedData[i].so_reference}` + "</td>" +
                "<td class='border text-center '>" + momentDate(parsedData[i].scheduled_pickup_datetime.split(' ')[0]) + "</td>" +
                "<td class='border text-center '>" + momentTime(parsedData[i].scheduled_pickup_datetime)  +"</td> " +
                "<td class='border text-center'>" + `${ parsedData[i].picked_up_by == "" ? "<i class='fas fa-trash text-danger deleteBook' data-toggle='modal' data-target='#btnConfirmOneModal' ></i>": "<label class='text-success'>PICKED UP</label>" }`+"</td> " +
                "</tr>";
                } else {
                    tblBoody.innerHTML +=
                    "<tr> " +
                    "<td class='border text-center text-danger'>" + parsedData[i].booking_reference + "</td>" +
                    "<td class='border  text-center text-danger'> N/A </td>" +
                    "<td class='border  text-center text-danger'>" + momentDate(parsedData[i].booking_datetime.split(' ')[0]) + "</td>" +
                    "<td class='border text-center text-danger'> N/A </td> " +
                    "<td class='border  text-center'>"+ "<label class='text-danger mb-0'>CANCELLED</label>"+"</td> " +
                    "</tr>"; 
                }

            }
            
            var deleteBook = document.querySelectorAll('.deleteBook');

            for (var i = 0; i < deleteBook.length; i++) {
                deleteBook[i].addEventListener('click', function () {
                var referenceNumber = this.parentNode.parentNode.cells[0].innerHTML

                GetDomElement("confirmOneSeviceMethod").innerText = "cancel";
                GetDomElement("confirmOneSeviceMethod").classList.add('text-danger');
                GetDomElement("confirmServiceMethod").innerText = "cancel";
                GetDomElement("confirmServiceMethod").classList.add('text-danger');

                GetDomElement("btnConfirmTwo").onclick = () => {
                    var deleteBooking = InvokeService(GetURL(), `BookingsForPickupMethods/cancelbooking/reference/${referenceNumber}`,'PUT','');
                  
                    
                    if (deleteBooking.code == 200) {
                        
                        GetDomElement("btnCloseConfimrTwo").click();
                        GetDomElement("btnConfirmTwo").dataset.target = "#btnSuccessEntryModal";
                        GetDomElement("successServiceMethod").innerText = "cancelled";
                        populateBookings();
                        
                        
                    }
                }

            });
        }
        } else {
            // ShowErrorModalOnLoadCustomer(parsedData.message, parsedData.code)
        }
    } else {
        ShowErrorModalOnLoadCustomer(dataCustomer.message, dataCustomer.code)
    }
    
    
}
function tracking(data) {
    var booking = data.innerHTML;
    var trackingData = InvokeService(GetURL(), `BookingsForPickupMethods/bookings/reference/details/${booking}`, 'GET', '');
    if (trackingData.code == 200) {
            var parsedData = JSON.parse(trackingData.data);
            
            if (parsedData.code == 200) {
                var parsedDatas = JSON.parse(parsedData.jsonData);
                populateTrackingData(parsedDatas);
            } else {
                ShowErrorModal("btnSearch", parsedData.message, parsedData.code);
            }
        } else {
            ShowErrorModal("btnSearch", trackingData.message, trackingData.code);
        }
    
}
function ShowErrorModalOnLoadCustomer(message, errorCode) {
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
function momentDate(date) {
    //convert to yyyy-mm-dd
    var tdrDate = date.split('/').join('-');
    return moment(tdrDate).format('LL');
}

function momentTime(datetime) {
    
    return moment(datetime).format('LT');
}

function populateTrackingData(data) {
    if (data.picked_up != null) {
        GetDomElement('txtPickupD').innerHTML = moment(data.picked_up).format('LLL');
        colorText('.pick-up', true);
    } else {
        GetDomElement('txtPickupD').innerHTML = '';

        colorText('.pick-up', false);
    }

    if (data.received_by_logistics != null) {
        GetDomElement('txtReceivedD').innerHTML = moment(data.received_by_logistics).format('LLL');
        colorText('.receive', true);
    } else {
        GetDomElement('txtReceivedD').innerHTML = '';

        colorText('.receive', false);
    }

    if (data.forwarded_to_laundry != null) {
        GetDomElement('txtForwardedLaundry').innerHTML = moment(data.forwarded_to_laundry).format('LLL');
        colorText('.ForwardedLaundry', true);
    } else {
        GetDomElement('txtForwardedLaundry').innerHTML = '';

        colorText('.ForwardedLaundry', false);
    }

    if (data.done_laundry != null) {
        GetDomElement('txtDoneD').innerHTML = moment(data.done_laundry).format('LLL');
        colorText('.done', true);
    } else {
        GetDomElement('txtDoneD').innerHTML = '';

        colorText('.done', false);
    }

    if (data.forwarded_to_logistics != null) {
        GetDomElement('txtForwardedLogistics').innerHTML = moment(data.forwarded_to_logistics).format('LLL');
        colorText('.forwardedLogistics', true);
    } else {
        GetDomElement('txtForwardedLogistics').innerHTML = '';
        colorText('.forwardedLogistics', false);
    }

    if (data.delivered != null) {
        GetDomElement('txtDeliverD').innerHTML = moment(data.delivered).format('LLL');
        colorText('.deliver', true);
    } else {
        colorText('.deliver', false);
        GetDomElement('txtDeliverD').innerHTML = '';
    }


}

function colorText(data, color) {
    var datas = document.querySelectorAll(data);
    for (var i = 0; i < datas.length; i++){
        if (color) {
            datas[i].classList.add('text-success');
            datas[i].classList.remove('d-none');
            
        } else {
            datas[i].classList.remove('text-success');
            
        }
    }

}

ClosePopupModal("btnOKCloseTracking", "btnCloseTracking");

ClosePopupModal("btnCancelOne", "btnCloseConfirmOne");
ClosePopupModal("btnSubmitOne", "btnCloseConfirmOne");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
