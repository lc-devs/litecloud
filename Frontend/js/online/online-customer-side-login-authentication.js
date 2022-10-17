// GetDomElement('btnAuthenticate').addEventListener('click', function () {
//     var error = false;

//     if (IsEmpty('txtAuthentication')) {
//         GetDomElement('divRequireAlert').classList.remove('d-none');
//         error = true;
//     } else {
//         GetDomElement('divRequireAlert').classList.add('d-none');
//     }
//     if (!error) {
//         location.href="online-customer-side-laundry-items-cancellation-pick-up-booking.html"
//     }
// });


GetDomElement('btnAuthenticate').addEventListener('click', function () {
    var customerId = sessionStorage.getItem('customerId');
    var code = GetDomElement('txtAuthentication').value;
    if (!IsEmpty('txtAuthentication')) {
        GetDomElement('txtAuthentication').classList.remove('is-invalid');

        var oData= InvokeService(GetURL(), `TwoFactorAuthentication/authenticate/${customerId}?securityCode=${code}`, 'POST', '');
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);

            if (parsedData.code == 200) {
            location.href="online-customer-side-laundry-items-cancellation-pick-up-booking.html"
            } else {
                GetDomElement('divUnauthorizedAlert').classList.remove('d-none');
                GetDomElement('divOtherErrorAlert').classList.add('d-none');

            }
        } else {
            GetDomElement('divOtherErrorAlert').classList.remove('d-none');
            GetDomElement('divUnauthorizedAlert').classList.add('d-none');

        }
    } else {
        GetDomElement('txtAuthentication').classList.add('is-invalid');
    }
});
