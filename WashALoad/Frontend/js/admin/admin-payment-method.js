var paymentMethodTable = GetDomElement("tblBody");
var authkey = sessionStorage.getItem('authkey');


window.onload = () => {
    if (authkey == "" || authkey == null) {
        location.href = "admin-login.html";
    }
    PopulatePaymentMethod();
}
GetDomElement("btnNewBranch").addEventListener('click', function () {
    GetDomElement("txtCode").value = "";
    GetDomElement("txtCode").classList.remove('is-invalid');
    GetDomElement("txtDescription").value = "";
    GetDomElement("txtDescription").classList.remove('is-invalid');
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.add('d-none');
    GetDomElement("chkNonCash").checked = false;
    GetDomElement("chkAccounting").checked = false;
    GetDomElement("chkFloats").checked = false;
    GetDomElement("chkRequiredImage").checked = false;
});

/* SUBMIT NEW RECORD */
GetDomElement("btnSubmitNew").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll(".input-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divAddNewInvalidForm");
    var paymentCode = GetDomElement("txtCode").value;
    var paymentDescription = GetDomElement("txtDescription").value;
    if (bValidForm) {
        if (paymentCode.length <= 10) {
            GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtCode").classList.remove('is-invalid');
            /* POST REQUEST */

            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oNewPaymentMethod = {
                    "payment_code": paymentCode.toUpperCase(),
                    "description": paymentDescription.toLowerCase(),
                    "non_cash": GetCheckboxValue("chkNonCash"),
                    "accounting_only": GetCheckboxValue("chkAccounting"),
                    "float": GetCheckboxValue("chkFloats"),
                    "require_proof": GetCheckboxValue("chkRequiredImage")
                }
                var strPaymentMethod = JSON.stringify(oNewPaymentMethod);
                var oSubmitNewPaymentMethod = InvokeService(GetURL(), "PaymentModeMethods", "POST", strPaymentMethod);
                if (oSubmitNewPaymentMethod.code == 200) {
                    var oData = JSON.parse(oSubmitNewPaymentMethod.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewModel').click();
                        PopulatePaymentMethod();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewPaymentMethod.message, oSubmitNewPaymentMethod.code);
                }
            }
        } else {
            this.dataset.target = "";
            GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.add('d-none');
    }
});
/* UPDATE RECORD */
GetDomElement("btnUpdateEntry").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll(".input-update-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divUpdateInvalidForm");
    var paymentCode = GetDomElement("txtEditCode").value;
    var paymentDescription = GetDomElement("txtEditDescription").value;
    if (bValidForm) {
        if (paymentCode.length <= 10) {
            GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtCode").classList.remove('is-invalid');

            /* POST REQUEST */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oUpdatePaymentMethod = {
                    "payment_code": paymentCode.toUpperCase(),
                    "description": paymentDescription.toLowerCase(),
                    "non_cash": GetCheckboxValue("chkEditNonCash"),
                    "accounting_only": GetCheckboxValue("chkEditAccounting"),
                    "float": GetCheckboxValue("chkEditFloats"),
                    "require_proof": GetCheckboxValue("chkEditRequiredImage")
                }

                var strPaymentMethod = JSON.stringify(oUpdatePaymentMethod);
                var oSubmitUpdatedPaymentMethod = InvokeService(GetURL(), `PaymentModeMethods/${paymentCode}`, "PUT", strPaymentMethod);
                if (oSubmitUpdatedPaymentMethod.code == 200) {
                    var oData = JSON.parse(oSubmitUpdatedPaymentMethod.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateModel').click();
                        PopulatePaymentMethod();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitUpdatedPaymentMethod.message, oSubmitUpdatedPaymentMethod.code);
                }
            }
        } else {
            this.dataset.target = "";
            GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewPaymentCodeTooLongErrorAlert").classList.add('d-none');
    }
});

function PopulatePaymentMethod() {
    GetDomElement("emptyTable").classList.add('d-none');
    var oPaymentMethodList = InvokeService(GetURL(), "PaymentModeMethods", "GET", "");
    if (oPaymentMethodList.code == 200) {
        var oData = JSON.parse(oPaymentMethodList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            paymentMethodTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                paymentMethodTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].payment_code +
                    "</td><td class='text-uppercase'>" + oJsonData[i].payment_code +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='col-12 col-md-4 col-xl-3'>" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    " NonCash" + PopulateCheckboxTable(oJsonData[i].non_cash) +
                    " </p>" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    " Acctg " + PopulateCheckboxTable(oJsonData[i].accounting_only) +
                    "</p>" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    " Floats" + PopulateCheckboxTable(oJsonData[i].Float) +
                    " </p>" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    " Require Image " + PopulateCheckboxTable(oJsonData[i].require_proof) +
                    "</p>" +
                    "</td><td class='px-0 '>" +
                    "<div class='d-flex justify-content-between'>" +
                    "<p class='text-danger btnDelete d-flex justify-content-center align-items-center px-3' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary update d-flex justify-content-center align-items-center px-3' data-toggle='modal' data-target ='#btnUpdateRecord' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.btnDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var paymentMethodCode = this.parentNode.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deletePaymentMethod = InvokeService(GetURL(), `PaymentModeMethods/${paymentMethodCode}`, "DELETE", "");
                        if (deletePaymentMethod.code == 200) {
                            var oData = JSON.parse(deletePaymentMethod.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulatePaymentMethod();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deletePaymentMethod.message, deletePaymentMethod.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.update');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearUpdatePaymentMethod();
                    var paymentCode = this.parentNode.parentNode.parentNode.cells[0].innerText;
                    var getPaymentMethodDetails = InvokeService(GetURL(), `PaymentModeMethods/details/${paymentCode}`, "GET", "");
                    if (getPaymentMethodDetails.code == 200) {
                        let oData = JSON.parse(getPaymentMethodDetails.data);
                        if (oData.code == 200) {
                            let oJsonData = JSON.parse(oData.jsonData);
                            GetDomElement("txtEditCode").value = oJsonData[0].payment_code;
                            GetDomElement("txtEditDescription").value = oJsonData[0].description;
                            PopulateCheboxInputField(oJsonData[0].non_cash, "chkEditNonCash");
                            PopulateCheboxInputField(oJsonData[0].accounting_only, "chkEditAccounting");
                            PopulateCheboxInputField(oJsonData[0].Float, "chkEditFloats");
                            PopulateCheboxInputField(oJsonData[0].require_proof, "chkEditRequiredImage");

                        } else {
                            ShowErrorModalOnLoad(oData.message, oData.code);
                        }
                    } else {
                        ShowErrorModalOnLoad(getPaymentMethodDetails.message, getPaymentMethodDetails.code);
                    }

                });
            }

        } else if (oData.code.code == 404 || oData.code.code == 204) {
            paymentMethodTable.innerHTML = "";
            GetDomElement("emptyTable").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oPaymentMethodList.code.code == 204 || oPaymentMethodList.code.code == 404) {
        paymentMethodTable.innerHTML = "";
        GetDomElement("emptyTable").classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oPaymentMethodList.message, oPaymentMethodList.code);
    }

}



function ClearUpdatePaymentMethod() {
    GetDomElement("txtEditCode").value = "";
    GetDomElement("txtEditCode").classList.remove('is-invalid');
    GetDomElement("txtEditDescription").value = "";
    GetDomElement("txtEditDescription").classList.remove('is-invalid');
    GetDomElement("divUpdateInvalidForm").classList.add('d-none');
    GetDomElement("divUpdatePaymentCodeTooLongErrorAlert").classList.add('d-none');
    GetDomElement("chkEditNonCash").checked = false;
    GetDomElement("chkEditAccounting").checked = false;
    GetDomElement("chkEditFloats").checked = false;
    GetDomElement("chkEditRequiredImage").checked = false;
}

ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");