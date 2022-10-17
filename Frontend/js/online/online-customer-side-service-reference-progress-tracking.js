GetDomElement('btnSearch').addEventListener('click', function () {
    GetDomElement('txtBookingReference').classList.remove('is-invalid');
    
    if (IsEmpty('txtBookingReference')) {
        GetDomElement('txtBookingReference').classList.add('is-invalid');
    } else {
        var trackingData = InvokeService(GetURL(), `BookingsForPickupMethods/bookings/reference/details/${GetDomElement('txtBookingReference').value}`, 'GET', '');
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
});

function populateTrackingData(data) {
    if (data.picked_up != null) {
        GetDomElement('txtPickupD').innerHTML = moment(data.picked_up).format('LLLL');
        colorText('.pick-up', true);
    } else {
        colorText('.pick-up', false);
    }

    if (data.received_by_logistics != null) {
        GetDomElement('txtReceivedD').innerHTML = moment(data.received_by_logistics).format('LLLL');
        colorText('.receive', true);
    } else {
        colorText('.receive', false);
    }

    if (data.forwarded_to_laundry != null) {
        GetDomElement('txtForwardedLaundry').innerHTML = moment(data.forwarded_to_laundry).format('LLLL');
        colorText('.ForwardedLaundry', true);
    } else {
        colorText('.ForwardedLaundry', false);
    }

    if (data.done_laundry != null) {
        GetDomElement('txtDoneD').innerHTML = moment(data.done_laundry).format('LLLL');
        colorText('.done', true);
    } else {
        colorText('.done', false);
    }

    if (data.forwarded_to_logistics != null) {
        GetDomElement('txtForwardedLogistics').innerHTML = moment(data.forwarded_to_logistics).format('LLLL');
        colorText('.forwardedLogistics', true);
    } else {
        colorText('.forwardedLogistics', false);
    }

    if (data.delivered != null) {
        GetDomElement('txtDeliverD').innerHTML = moment(data.delivered).format('LLLL');
        colorText('.deliver', true);
    } else {
        colorText('.deliver', false);
    }


}

function colorText(data, color) {
    var datas = document.querySelectorAll(data);
    for (var i = 0; i < datas.length; i++){
        if (color) {
            datas[i].classList.add('text-success');
            datas[i].classList.remove('d-none');
        } else {
            
        }
    }

}

window.addEventListener('load', function () {
    if (sessionStorage.getItem('bookingReference') != null) {
        GetDomElement('txtBookingReference').value = sessionStorage.getItem('bookingReference');
        btnSearch.click();
        sessionStorage.removeItem('bookingReference');
   } 
});