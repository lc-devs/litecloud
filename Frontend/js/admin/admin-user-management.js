var sectionType = 0;
var sectionDescription = "Admin";
var oUserTemplateInput = document.querySelectorAll('.userAccountTemplate');
var oUpdateUserTemplateInput = document.querySelectorAll('.userEditAccountTemplate');
var oUserSectionList = document.querySelectorAll('.userSection');
var oUpdateUserSectionList = document.querySelectorAll('.updateUserSectionList');
var oUserSiteList = document.querySelectorAll('.userSite');
var oUpdateUserSiteList = document.querySelectorAll('.userEditSite');
var userTable = GetDomElement("tblBody");
var authkey = sessionStorage.getItem('authkey');
var menuTemplate;
var LogisticsSiteList = [];
var LaundrySiteList = [];
var userAccessList = [];
var isAdmin = 1;
const once = {
    once : true
  };




window.addEventListener('load', function () {
    if (authkey == "" || authkey == null) {
        location.href = "admin-login.html";
    }
    PopulateAdminUsers();
    GetLaundrySiteList();
    GetLogisticsSiteList();
    GetDomElement("chkAdmin").checked = true;
    GetDomElement("chkLogistics").checked = true;
    ShowHideInputWithLabel(oUserTemplateInput, 0);
    ShowHideInputWithLabel(oUserSectionList, 0);
    ShowHideInputWithLabel(oUserSiteList, 0);
    GetAccessTemplate();
    PopulateAccessTemplate('selUserAccessTemplate', userAccessList);
    sectionDescription = "";
    sectionType = 0;
    isAdmin = 1;
});

GetDomElement("btnSubmitNew").addEventListener('click', function () {
    GetDomElement("divPasswordNotMatch").classList.add('d-none');
    GetDomElement("txtPassword1").classList.remove('is-invalid');
    GetDomElement("txtPassword2").classList.remove('is-invalid');
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divAddNewInvalidForm");
    var password1 = GetDomElement("txtPassword1");
    var password2 = GetDomElement("txtPassword2");
    if (bValidForm) {
        if (password1.value == password2.value) {
            GetDomElement("divPasswordNotMatch").classList.add('d-none');
            GetDomElement("txtPassword1").classList.remove('is-invalid');
            GetDomElement("txtPassword2").classList.remove('is-invalid');

            GetDomElement("method").innerText = "submit";
            GetDomElement("method").classList.add("text-success");
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "submit";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "submit";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";

                if (GetCheckboxValue("chkAdmin") == 1) {
                    menuTemplate = "FORADMIN"
                } else {
                    menuTemplate = ReturnInputValue('selUserAccessTemplate');
                }
                /* POST */
                var oNewUser = {
                    "user_id": ReturnInputValue('txtUserID'),
                    "user_name": ReturnInputValue('txtUsername'),
                    "admin": isAdmin,
                    "section": {
                        "idsection": sectionType,
                        "description": sectionDescription
                    },
                    "active_user": 1,
                    "user_password": ReturnInputValue('txtPassword1'),
                    "menu_template": menuTemplate,
                    "session_authentication_key": "string",
                    "oSite": {
                        "code": ReturnInputValue("selSiteLocation"),
                        "site": "string"
                    }
                }

                var strNewUser = JSON.stringify(oNewUser);
                var oSumbitNewUser = InvokeService(GetURL(), "SystemUserMethods", "POST", strNewUser);
                if (oSumbitNewUser.code == 200) {
                    var oData = JSON.parse(oSumbitNewUser.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfirm1').click();
                        GetDomElement('btnCloseAddNewModel').click();
                        GetDomElement("btnCloseConfimrTwo").click();
                        PopulateAdminUsers();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        GetDomElement('btnCloseConfirm1').click();
                        GetDomElement("btnCloseConfimrTwo").click();
                    }
                } else {
                    ShowErrorModal("btnConfirmTwo", oSumbitNewUser.message, oSumbitNewUser.code);
                    GetDomElement('btnCloseConfirm1').click();
                    GetDomElement("btnCloseConfimrTwo").click();
                }
            }


        } else {
            GetDomElement("divPasswordNotMatch").classList.remove('d-none');
            GetDomElement("txtPassword1").classList.add('is-invalid');
            GetDomElement("txtPassword2").classList.add('is-invalid');
        }


    }

});

/* SUBMIT UPDATED RECORD */
GetDomElement("btnSubmitUpdate").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.update-input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divUpdateInvalidForm");

    if (bValidForm) {
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";

                if (GetCheckboxValue("chkEditAdmin") == 1) {
                    menuTemplate = "FORADMIN"
                } else {
                    menuTemplate = ReturnInputValue('selEditUserAccessTemplate');
                }
                /* POST */
                var oUpdatedUser = {
                    "user_id": ReturnInputValue('txtEditUserID'),
                    "user_name": ReturnInputValue('txtEditUsername'),
                    "admin": isAdmin,
                    "section": {
                        "idsection": sectionType,
                        "description": sectionDescription
                    },
                    "active_user": 1,
                    "user_password":"",
                    "menu_template": menuTemplate,
                    "session_authentication_key": "string",
                    "oSite": {
                        "code": ReturnInputValue("selEditSiteLocation"),
                        "site": "string"
                    }
                }

                var strUpdatedUser = JSON.stringify(oUpdatedUser);
                var oSumbitNewUser = InvokeService(GetURL(), `SystemUserMethods/${GetDomElement('txtEditUserID').value}`, "PUT", strUpdatedUser);
                if (oSumbitNewUser.code == 200) {
                    var oData = JSON.parse(oSumbitNewUser.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfirm1').click();
                        GetDomElement('btnCloseUpdateModel').click();
                        GetDomElement("btnCloseConfimrTwo").click();
                        PopulateAdminUsers();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        GetDomElement('btnCloseConfirm1').click();
                        GetDomElement("btnCloseConfimrTwo").click();
                    }
                } else {
                    ShowErrorModal("btnConfirmTwo", oSumbitNewUser.message, oSumbitNewUser.code);
                    GetDomElement('btnCloseConfirm1').click();
                    GetDomElement("btnCloseConfimrTwo").click();
                }
            }


    }

});


/* CLEAR FORM DATA */
GetDomElement("btnAddNewUser").addEventListener('click', function () {
    GetDomElement("txtUserID").value = "";
    GetDomElement("txtUserID").classList.remove('is-invalid');
    GetDomElement("txtUsername").value = "";
    GetDomElement("txtUsername").classList.remove('is-invalid');
    GetDomElement("txtPassword1").value = "";
    GetDomElement("txtPassword1").classList.remove('is-invalid');
    GetDomElement("txtPassword2").value = "";
    GetDomElement("txtPassword2").classList.remove('is-invalid');
    GetDomElement("selSiteLocation").selectedIndex = 0;
    GetDomElement("selSiteLocation").classList.remove('is-invalid');
    GetDomElement("selUserAccessTemplate").selectedIndex = 0;
    GetDomElement("selUserAccessTemplate").classList.remove('is-invalid');
    GetDomElement("chkAccounting").checked = false;
    GetDomElement("chkAdmin").checked = true;
    GetDomElement("chkLogistics").checked = true;
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divPasswordNotMatch").classList.add('d-none');
});
/* CLEAR FORM DATA ON UPDATE */
function ClearUserFormOnUpdate() {
    GetDomElement("txtEditUserID").value = "";
    GetDomElement("txtEditUserID").classList.remove('is-invalid');
    GetDomElement("txtEditUsername").value = "";
    GetDomElement("txtEditUsername").classList.remove('is-invalid');
    GetDomElement("selEditSiteLocation").selectedIndex = 0;
    GetDomElement("selEditSiteLocation").classList.remove('is-invalid');
    GetDomElement("selEditUserAccessTemplate").selectedIndex = 0;
    GetDomElement("selEditUserAccessTemplate").classList.remove('is-invalid');
    GetDomElement("chkEditAccounting").checked = false;
    GetDomElement("chkEditAdmin").checked = true;
    GetDomElement("chkEditLogistics").checked = true;
    GetDomElement("divUpdateInvalidForm").classList.add('d-none');
    GetDomElement("divUpdatePasswordNotMatch").classList.add('d-none');
}

GetDomElement("chkAccounting").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("chkLogistics").checked = true;


    }
});

GetDomElement("chkAdmin").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("selUserAccessTemplate").classList.remove("input-required");
        ShowHideInputWithLabel(oUserTemplateInput, 0);

        ShowHideInputWithLabel(oUserSectionList, 0);
        sectionType = 0;
        isAdmin = 1;
        sectionDescription = "Admin";


        ShowHideInputWithLabel(oUserSiteList, 0);
        GetDomElement("selSiteLocation").classList.remove("input-required");
    }
});

GetDomElement("chkNonAdmin").addEventListener('click', function () {
    if (this.checked) {
        isAdmin = 0;
        PopulateSiteLocationList("selSiteLocation", LogisticsSiteList);
        ShowHideInputWithLabel(oUserTemplateInput, 1);
        GetDomElement("selUserAccessTemplate").classList.add("input-required");
        sectionType = 1;
        GetDomElement("chkLogistics").addEventListener('click', function () {
            if (this.checked) {
                sectionType = 1;
                sectionDescription = "Logistics";
                PopulateSiteLocationList("selSiteLocation", LogisticsSiteList);
            }
        });
        GetDomElement("chkLaundry").addEventListener('click', function () {
            if (this.checked) {
                sectionType = 2;
                sectionDescription = "Laundry";
                PopulateSiteLocationList("selSiteLocation", LaundrySiteList);
            }
        });

        ShowHideInputWithLabel(oUserSectionList, 1);

        ShowHideInputWithLabel(oUserSiteList, 1);
        GetDomElement("selSiteLocation").classList.add("input-required");

    }
});
/* ADMIN UPDATE */

GetDomElement("chkEditAdmin").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("selEditUserAccessTemplate").classList.remove("update-input-required");
        ShowHideInputWithLabel(oUpdateUserTemplateInput, 0);

        ShowHideInputWithLabel(oUpdateUserSectionList, 0);
        sectionType = 0;
        isAdmin = 1;
        sectionDescription = "Admin";


        ShowHideInputWithLabel(oUpdateUserSiteList, 0);
        GetDomElement("selEditSiteLocation").classList.remove("update-input-required");
    }
});
/* NONADMIN UPDATE */
GetDomElement("chkEditNonAdmin").addEventListener('click', function () {
    sectionType = 1;
    sectionDescription = "Logistics";
    if (this.checked) {
        PopulateAccessTemplate('selEditUserAccessTemplate', userAccessList);
        isAdmin = 0;
        PopulateSiteLocationList("selEditSiteLocation", LogisticsSiteList);
        ShowHideInputWithLabel(oUpdateUserTemplateInput, 1);
        GetDomElement("selEditUserAccessTemplate").classList.add("update-input-required");

        GetDomElement("chkEditLogistics").addEventListener('click', function () {
            if (this.checked) {
                sectionType = 1;
                sectionDescription = "Logistics";
                PopulateSiteLocationList("selEditSiteLocation", LogisticsSiteList);
            }
        });
        GetDomElement("chkEditLaundry").addEventListener('click', function () {
            if (this.checked) {
                sectionType = 2;
                sectionDescription = "Laundry";
                PopulateSiteLocationList("selEditSiteLocation", LaundrySiteList);
            }
        });

        ShowHideInputWithLabel(oUpdateUserSectionList, 1);

        ShowHideInputWithLabel(oUpdateUserSiteList, 1);
        GetDomElement("selEditSiteLocation").classList.add("update-input-required");

    }
});


function PopulateAdminUsers() {
    var oAdminUsers = InvokeService(GetURL(), "SystemUserMethods", "GET", "");
    if (oAdminUsers.code == 200) {
        var oData = JSON.parse(oAdminUsers.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            userTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                userTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].user_id +
                    "</td><td>" + oJsonData[i].user_id +
                    "</td><td class='text-capitalize'>" + oJsonData[i].user_name +
                    "</td><td class='text-capitalize'>" + PopulateCheckboxTable(oJsonData[i].admin) +
                    "</td><td class='text-capitalize'>" + `${oJsonData[i].section.idsection == 0 ? 'n/a':oJsonData[i].section.description}` +
                    "</td><td class='text-capitalize'>" + `${oJsonData[i].oSite.site == "" ? 'n/a':oJsonData[i].oSite.site}` +
                    "</td><td class='text-capitalize'>" +
                    "<button class='btn btn-danger resetUser' data-toggle='modal' data-target = '#btnUpdatePassword'>Reset</button>" +
                    "</td><td class='text-capitalize text-center'>" + CheckStatus(oJsonData[i].active_user, oJsonData[i].user_id) +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger btnDelete d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "<p class='text-primary update d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateUser' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.btnDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var userCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () =>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteUser = InvokeService(GetURL(), `SystemUserMethods/${userCode}`, "DELETE", "");
                        if (deleteUser.code == 200) {
                            var oData = JSON.parse(deleteUser.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateAdminUsers();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteUser.message, deleteUser.code);
                        }
                    }
                });
            }

            var resetUser = document.querySelectorAll('.resetUser');
            for (var i = 0; i < resetUser.length; i++) {
                resetUser[i].addEventListener('click', function () {
                    ClearResetPasswordForm();
                    var userId = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement('userId').value = userId;
                });
            }


            var btnUpdate = document.querySelectorAll('.update');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearUserFormOnUpdate();
                    var userId = this.parentNode.parentNode.cells[0].innerText;
                    var userData = InvokeService(GetURL(), `SystemUserMethods/userid?userid=${userId}`, "GET", "");
                    if (userData.code == 200) {
                        var oData = JSON.parse(userData.data);
                        if (oData.code == 200) {
                            var jsonData = JSON.parse(oData.jsonData);
                            GetDomElement("txtEditUserID").value = jsonData.user_id;
                            GetDomElement("txtEditUsername").value = jsonData.user_name;
                            if (jsonData.admin == 1) {
                                GetDomElement("chkEditAdmin").click();
                            } else if (jsonData.admin == 0) {
                                GetDomElement("chkEditNonAdmin").click();
                                GetDomElement("selEditUserAccessTemplate").value = jsonData.menu_template;

                                if (jsonData.section.idsection == 1) {
                                    GetDomElement("chkEditLogistics").click();
                                    PopulateSiteLocationList("selEditSiteLocation", LogisticsSiteList);
                                    PrepopulateSite(jsonData.oSite.code);
                                } else if (jsonData.section.idsection == 2) {
                                    GetDomElement("chkEditLaundry").click();
                                    PopulateSiteLocationList("selEditSiteLocation", LaundrySiteList);
                                    PrepopulateSite(jsonData.oSite.code);
                                }
                            }


                        } else if (oData.code.code == 404 || oData.code.code == 204) {} else {
                            ShowErrorModalOnLoad(oData.message, oData.code);
                        }
                    } else if (userData.code.code == 204 || userData.code.code == 404) {} else {
                        ShowErrorModalOnLoad(userData.message, userData.code);
                    }
                });
            }

        } else if (oData.code == 204 || oData.code == 404) {
            userTable.innerHTML = "";
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oAdminUsers.code == 204 || oAdminUsers.code == 404) {
        userTable.innerHTML = "";
    } else {
        ShowErrorModalOnLoad(oAdminUsers.message, oAdminUsers.code);
        userTable.innerHTML = "";

    }


}


function PrepopulateSite(siteData) {

    var siteDropdown = GetDomElement("selEditSiteLocation");
    for (var i = 0; i < siteDropdown.options.length; i++) {
        if (siteDropdown[i].value == siteData) {

            siteDropdown.selectedIndex = i;
            break;
        }
    }
}

function CheckStatus(statusValue, userId) {
    if (statusValue == 1) {
        return "<button type='button' class='btn btn-outline-success' data-toggle='modal' data-target='#btnConfirm1' onclick = ChangeUserStatus(" + `${statusValue}` + "," + `"${userId}"` + ")" +
            ">Active" +
            "</button>"
    } else {
        return "<button type='button' class='btn btn-outline-secondary' data-toggle='modal' data-target='#btnConfirm1' onclick = ChangeUserStatus(" + `${statusValue}` + "," + `"${userId}"` + ")" +
            ">Blocked" +
            "</button>"
    }
}

function ChangeUserStatus(status, userId) {
    var changeStatus;
    var statusMessage;
    var successMessage;
    if (status == 0) {
        changeStatus = 1;
        statusMessage = "activate";
        successMessage = "activated";
    } else {
        changeStatus = 0;
        statusMessage = "block";
        successMessage = "blocked"
    }
    GetDomElement("method").innerText = statusMessage;
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = statusMessage;
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var updateUser = InvokeService(GetURL(), `SystemUserMethods/changeuserstatus/${userId}?status=${changeStatus}`, "PUT", "");
        if (updateUser.code == 200) {
            var oData = JSON.parse(updateUser.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", successMessage);
                GetDomElement('btnCloseConfimrTwo').click();
                PopulateAdminUsers();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", updateUser.message, updateUser.code);
        }

    };
}

/* UPDATE PASSWORD */
GetDomElement('btnSubmitUpdatedPassword').addEventListener('click', function () {
    GetDomElement("prefix").innerText = "";
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.password-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidPasswordForm");
    var password1 = GetDomElement('txtChangePassword1');
    var password2 = GetDomElement('txtChangePassword2');
    if (bValidForm) {
        if (password1.value != password2.value) {
            password1.classList.add('is-invalid');
            password2.classList.add('is-invalid');
            GetDomElement("divChangePasswordNotMatch").classList.remove('d-none');
        } else {
            password1.classList.remove('is-invalid');
            password2.classList.remove('is-invalid');
            GetDomElement("divChangePasswordNotMatch").classList.add('d-none');
            /* POST */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerHTML = "change password <span class='text-secondary'>on selected</span>";
            GetDomElement("method").classList.add("text-danger");
            GetDomElement("confirmTwoMethod").innerHTML = "change password <span class='text-secondary'>on selected</span>";
            GetDomElement("confirmTwoMethod").classList.add("text-danger");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var userId = GetDomElement('userId').value;

                var oPassword = {
                    "user_id": userId,
                    "password": GetDomElement("txtChangePassword1").value
                }
                var strPassword = JSON.stringify(oPassword);
                var updateUserPassword = InvokeService(GetURL(), `SystemUserMethods/changepassword/${userId}`, "PUT", strPassword);
                if (updateUserPassword.code == 200) {
                    var oData = JSON.parse(updateUserPassword.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement("prefix").innerText = "password";
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnClosePassword').click();
                        PopulateAdminUsers();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", updateUserPassword.message, updateUserPassword.code);
                }
            }
        }
    }

});

function GetLaundrySiteList() {
    LaundrySiteList = InvokeService(GetURL(), "LaundrySiteMethods", "GET", "");
}

function GetLogisticsSiteList() {
    LogisticsSiteList = InvokeService(GetURL(), "LogisticSiteMethods", "GET", "");
}

function GetAccessTemplate() {
    userAccessList = InvokeService(GetURL(), "AccessMenuTemplateMethods", "GET", "");
}


function ClearResetPasswordForm() {
    GetDomElement("txtChangePassword1").value = "";
    GetDomElement("txtChangePassword1").classList.remove('is-invalid');
    GetDomElement("txtChangePassword2").value = "";
    GetDomElement("txtChangePassword2").classList.remove('is-invalid');
    GetDomElement("divInvalidPasswordForm").classList.add('d-none');
    GetDomElement("divChangePasswordNotMatch").classList.add('d-none');

}

ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");