GetDomElement('btnLogin').addEventListener('click', function () {
    logIn();

});
GetDomElement('txtPassword').addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        logIn();
    }
});


function logIn() {
    if ((GetDomElement('txtLoginID').value == '') || (GetDomElement('txtPassword').value == '') ) {
        GetDomElement('divRequireAlert').classList.remove('d-none');
        GetDomElement('divUnauthorizedAlert').classList.add('d-none');
        GetDomElement('divOtherErrorAlert').classList.add('d-none');
        
    } else {
        GetDomElement('divRequireAlert').classList.add('d-none');

        var body = {
            "user_id": GetDomElement('txtLoginID').value ,
            "password": GetDomElement('txtPassword').value
        }
        
        var strBody = JSON.stringify(body);
        var logIn = InvokeService(GetURL(), 'CustomerMethods/login', 'POST', strBody);
        
        if (logIn.code == 200) {
            GetDomElement('divOtherErrorAlert').classList.add('d-none');
            var parsedLogin = JSON.parse(logIn.data);
            
            if (parsedLogin.code == 200) {
                sessionStorage.clear();
                var parsedLogin = JSON.parse(parsedLogin.jsonData);
                sessionStorage.setItem('customerId', parsedLogin.customer_id);
                sessionStorage.setItem('customerName', parsedLogin.customer_name);
                sessionStorage.setItem('email', parsedLogin.email_address);
                sessionStorage.setItem('cellular_number', parsedLogin.cellular_number);
                sessionStorage.setItem('authkey', parsedLogin.session_authentication_key);

                location.href = 'online-customer-side-login-authentication.html';
            } else {
                GetDomElement('divUnauthorizedAlert').classList.remove('d-none');
            }

        } else {
            GetDomElement('divOtherErrorAlert').classList.remove('d-none');
            
        }
    }
}



