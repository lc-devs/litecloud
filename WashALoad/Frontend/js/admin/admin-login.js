var userType;
GetDomElement('btnLogin').addEventListener('click', function () {
    Login();
});

GetDomElement('txtPassword').addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        Login();
    }
});

window.onload = () => {
    sessionStorage.clear();
}

function Login() {
    /* CLEAR ERROR ALERTS */
    GetDomElement("divLoginErrorAlert").classList.add('d-none');

    GetDomElement('loginForm').classList.add('was-validated');
    var oRequiredFields = document.querySelectorAll('.input-required');
    var bValidLogin = CheckValidForm(oRequiredFields, "divRequireAlert");
    if (bValidLogin) {
        var username = GetDomElement("txtUsername").value;
        var password = GetDomElement("txtPassword").value;
        /* LOGIN HERE! */
        var oLoginData = {
            "user_id": username,
            "password": password,
        };
        var strLoginData = JSON.stringify(oLoginData);
        var oLogin = InvokeService(GetURL(), "SystemUserMethods/login", "POST", strLoginData);
        if (oLogin.code == 200) {
            var oUserData = JSON.parse(oLogin.data);
            if (oUserData.code == 200) {
                /* ADD SESSION STORAGE */
                var oUserResponse = JSON.parse(oUserData.jsonData);
                sessionStorage.setItem("authkey", oUserResponse.session_authentication_key);
                sessionStorage.setItem("portalType", 9999);
                sessionStorage.setItem("userName", oUserResponse.user_name);
                sessionStorage.setItem("userId", oUserResponse.user_id);
                sessionStorage.setItem("sectionId", oUserResponse.section.idsection);
                sessionStorage.setItem("sectionDescription", oUserResponse.section.description);
                sessionStorage.setItem("isAdmin", oUserResponse.admin);
                sessionStorage.setItem("userSiteCode", oUserResponse.oSite.code);
                sessionStorage.setItem("userSiteDescription", oUserResponse.oSite.site);
                sessionStorage.setItem("menu_template", oUserResponse.menu_template);
                if (oUserResponse.section.idsection == 0) {
                    location.href = "admin-user-management.html";
                } else if (oUserResponse.section.idsection == 1) {
                    location.href = "online-pickup-today.html";
                } else if (oUserResponse.section.idsection == 2) {
                    location.href = "laundry-recieving-industrial.html";
                }

            } else {
                GetDomElement("divLoginErrorAlert").classList.remove('d-none');
                GetDomElement("failedMessage").innerText = oUserData.message;
            }
        } else {
            GetDomElement("divLoginErrorAlert").classList.remove('d-none');
            GetDomElement("failedMessage").innerText = oLogin.message;
        }
    }
}