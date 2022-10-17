var authkey = sessionStorage.getItem('authkey');
var laundryTable = GetDomElement("tblLaundryItems");
var otherserviceTable = GetDomElement("tblOtherServices");


window.onload = () => {
    if (authkey == "" || authkey == null) {
        location.href = "admin-login.html";
    }
    PopulateNonIndustrialLaundryItem();
    PopulateNonIndustrialOtherServices();
}

/* LAUNDRY ITEMS */

GetDomElement("btnNewLaundryItems").onclick = () => {
    // GetDomElement("txtLaundryItemsCode").value = "";
    // GetDomElement("txtLaundryItemsCode").classList.remove('is-invalid');
    GetDomElement("txtLaundryItemDescription").value = "";
    GetDomElement("txtLaundryItemDescription").classList.remove('is-invalid');
    GetDomElement("divInvalidNewLaundryItems").classList.add('d-none');
    GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
}

/*CLEAR LAUNDRY ITEMS */

function ClearLaundryItemForm() {
    GetDomElement("txtEditLaundryItemsCode").value = "";
    GetDomElement("txtEditLaundryItemsCode").classList.remove('is-invalid');
    GetDomElement("txtEditLaundryItemDescription").value = "";
    GetDomElement("txtEditLaundryItemDescription").classList.remove('is-invalid');
    GetDomElement("divInvalidUpdateLaundryItems").classList.add('d-none');
    GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
}

/* SUBMIT NEW LAUNDRY ITEMS RECORD */
GetDomElement("btnSubmitNewLaundryItems").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.laundry-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewLaundryItems");
    // var laundryItemcode = GetDomElement("txtLaundryItemsCode").value;
    var laundryItemDescription = GetDomElement("txtLaundryItemDescription").value;

    if (bValidForm) {
        // GetDomElement("txtLaundryItemsCode").classList.remove('is-invalid');
        // if (laundryItemcode.length <= 10) {
            GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
            // GetDomElement("txtLaundryItemsCode").classList.remove('is-invalid');
            /*POST REQUREST HERE */

            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "submit";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "submit";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oNewLaundryItem = {
                    "id_item": 0,
                    "description": laundryItemDescription
                }
                var strNewLaundryItem = JSON.stringify(oNewLaundryItem);
                var oSubmitNewLaundryItem = InvokeService(GetURL(), "NonIndustrialLaundryItemMethods", "POST", strNewLaundryItem);
                if (oSubmitNewLaundryItem.code == 200) {
                    var oData = JSON.parse(oSubmitNewLaundryItem.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewLaundryItemsModal').click();
                        PopulateNonIndustrialLaundryItem();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewLaundryItem.message, oSubmitNewLaundryItem.code);
                }
            }
        // } else {
        //     this.dataset.target = "";
        //     GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.remove('d-none');
        //     GetDomElement("txtLaundryItemsCode").classList.add('is-invalid');
        // }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
    }
});

/* SUBMIT UPDATED LAUNDRY ITEMS RECORD */
GetDomElement("btnSubmitUpdatedLaundryItems").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.laundry-update-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateLaundryItems");
    var laundryItemcode = GetDomElement("txtEditLaundryItemsCode").value;
    var laundryItemDescription = GetDomElement("txtEditLaundryItemDescription").value;
    
    if (bValidForm) {
        GetDomElement("txtEditLaundryItemsCode").classList.remove('is-invalid');
        if (laundryItemcode.length <= 10) {
            GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtEditLaundryItemsCode").classList.remove('is-invalid');
            /*POST REQUREST HERE */

            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oUpdatedLaundryItem = {
                    "id_item": laundryItemcode,
                    "description": laundryItemDescription
                }
                var strUpdatedLaundryItem = JSON.stringify(oUpdatedLaundryItem);
                var oSubmitUpdatesLaundryItem = InvokeService(GetURL(), `NonIndustrialLaundryItemMethods/${laundryItemcode}`, "PUT", strUpdatedLaundryItem);
                if (oSubmitUpdatesLaundryItem.code == 200) {
                    var oData = JSON.parse(oSubmitUpdatesLaundryItem.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateLaundryItemsModal').click();
                        PopulateNonIndustrialLaundryItem();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitUpdatesLaundryItem.message, oSubmitUpdatesLaundryItem.code);
                }
            }
        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditLaundryItemsCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
    }
});


/* OTHER SERVICES */
GetDomElement('btnSubmitNewOtherServices').addEventListener('click', function () {
    var bZeroAmount = true;
    var oRequiredFields = document.querySelectorAll('.other-services-required');
    var oServiceAmounts = document.querySelectorAll('.amount-check');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewOtherServices");
    // var otherServicesCode = GetDomElement('txtOtherServicesCode').value;
    var otherServiceDescription = GetDomElement("txtOtherServicesDescription").value;
    var otherServiceCost = GetDomElement("txtServiceCost");
    this.dataset.target = "#";
    if (bValidForm) {
        // GetDomElement("txtOtherServicesCode").classList.remove('is-invalid');
        // if (otherServicesCode.length <= 10) {
            GetDomElement("divAddNewOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
            // GetDomElement("txtOtherServicesCode").classList.remove('is-invalid');
            if (GetDomElement("chkManualCosting").checked === false) {
                bZeroAmount = ZeroAmountCheck(oServiceAmounts, "divInvalidNewOtherServicesCost");
            }
            if (!isNaN(otherServiceCost.value) && bZeroAmount) {
                GetDomElement("divInvalidNewOtherServicesCost").classList.add('d-none');
                otherServiceCost.classList.remove('is-invalid');
                /*POST REQUREST HERE */

                this.dataset.target = "#btnConfirm1";
                GetDomElement("method").innerText = "submit";
                GetDomElement("method").classList.add("text-success");
                GetDomElement("confirmTwoMethod").innerText = "submit";
                GetDomElement("confirmTwoMethod").classList.add("text-success");

                GetDomElement("btnConfirmTwo").onclick = ()=>{
                    var oNewOtherService = {
                        "id_service": 0,
                        "description": otherServiceDescription.toLowerCase(),
                        "manual_costing": GetCheckboxValue("chkManualCosting"),
                        "unit_cost": otherServiceCost.value
                    }

                    var strNewOtherService = JSON.stringify(oNewOtherService);
                    var submitNewOtherService = InvokeService(GetURL(), "NonIndustrialServiceMethods", "POST", strNewOtherService);
                    if (submitNewOtherService.code == 200) {
                        var oData = JSON.parse(submitNewOtherService.data);
                        if (oData.code == 200) {
                            ShowSuccessModal("btnConfirmTwo", "added");
                            GetDomElement('btnCloseConfimrTwo').click();
                            GetDomElement('btnCloseAddNewOtherServicesModal').click();
                            PopulateNonIndustrialOtherServices();
                        } else {
                            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        }

                    } else {
                        ShowErrorModal("btnConfirmTwo", submitNewOtherService.message, submitNewOtherService.code);
                    }
                }
            } else {
                this.dataset.target = "";
                GetDomElement("divInvalidNewOtherServicesCost").classList.remove('d-none');
                otherServiceCost.classList.add('is-invalid');
            }

        // } else {
        //     this.dataset.target = "";
        //     GetDomElement("divAddNewOtherServicesCodeTooLongErrorAlert").classList.remove('d-none');
        //     GetDomElement("divInvalidNewOtherServicesCost").classList.add('d-none');
        // }


    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
        GetDomElement("divInvalidNewOtherServicesCost").classList.add('d-none');
    }
});
/* UPDATE OTHER SERVICES */
GetDomElement('btnSubmitUpdateOtherServices').addEventListener('click', function () {
    var bZeroAmount = true;
    var oRequiredFields = document.querySelectorAll('.other-services-update-required');
    var oServiceAmounts = document.querySelectorAll('.amount-update-check');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateOtherServices");
    var otherServicesCode = GetDomElement('txtEditOtherServicesCode').value;
    var otherServiceDescription = GetDomElement("txtEditOtherServicesDescription").value;
    var otherServiceCost = GetDomElement("txtEditServiceCost");
    this.dataset.target = "#";
    if (bValidForm) {
        GetDomElement("txtEditOtherServicesCode").classList.remove('is-invalid');
        if (otherServicesCode.length <= 10) {
            GetDomElement("divUpdateOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtEditOtherServicesCode").classList.remove('is-invalid');
            if (GetDomElement("chkEditManualCosting").checked === false) {
                bZeroAmount = ZeroAmountCheck(oServiceAmounts, "divInvalidUpdateOtherServicesCost");
            }
            if (!isNaN(otherServiceCost.value) && bZeroAmount) {

                GetDomElement("divInvalidUpdateOtherServicesCost").classList.add('d-none');
                otherServiceCost.classList.remove('is-invalid');
                /*POST REQUREST HERE */

                this.dataset.target = "#btnConfirm1";
                GetDomElement("method").innerText = "update";
                GetDomElement("method").classList.add("text-success");
                GetDomElement("confirmTwoMethod").innerText = "update";
                GetDomElement("confirmTwoMethod").classList.add("text-success");

                GetDomElement("btnConfirmTwo").onclick = ()=>{
                    var oUpdatedOtherService = {
                        "id_service": otherServicesCode,
                        "description": otherServiceDescription.toLowerCase(),
                        "manual_costing": GetCheckboxValue("chkEditManualCosting"),
                        "unit_cost": otherServiceCost.value
                    }

                    var strUpdatedOtherService = JSON.stringify(oUpdatedOtherService);
                    var submitUpdatesOtherService = InvokeService(GetURL(), `NonIndustrialServiceMethods/${otherServicesCode}`, "PUT", strUpdatedOtherService);
                    if (submitUpdatesOtherService.code == 200) {
                        var oData = JSON.parse(submitUpdatesOtherService.data);
                        if (oData.code == 200) {
                            ShowSuccessModal("btnConfirmTwo", "updated");
                            GetDomElement('btnCloseConfimrTwo').click();
                            GetDomElement('btnCloseUpdateOtherServicesModal').click();
                            PopulateNonIndustrialOtherServices();
                        } else {
                            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        }

                    } else {
                        ShowErrorModal("btnConfirmTwo", submitUpdatesOtherService.message, submitUpdatesOtherService.code);
                    }
                }
            } else {
                this.dataset.target = "";
                GetDomElement("divInvalidUpdateOtherServicesCost").classList.remove('d-none');
                otherServiceCost.classList.add('is-invalid');
            }

        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateOtherServicesCodeTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("divInvalidUpdateOtherServicesCost").classList.add('d-none');
        }


    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
        GetDomElement("divInvalidUpdateOtherServicesCost").classList.add('d-none');
    }
});


GetDomElement("btnNEwOtherServices").addEventListener('click', function () {
    // GetDomElement("txtOtherServicesCode").value = "";
    // GetDomElement("txtOtherServicesCode").classList.remove('is-invalid');
    GetDomElement("txtOtherServicesDescription").value = "";
    GetDomElement("txtOtherServicesDescription").classList.remove('is-invalid');
    GetDomElement("chkManualCosting").checked = false;
    GetDomElement("txtServiceCost").value = "";
    GetDomElement("txtServiceCost").disabled = false;
    GetDomElement("txtServiceCost").classList.remove('is-invalid');
    GetDomElement("divInvalidNewOtherServices").classList.add('d-none');
    GetDomElement("divInvalidNewOtherServicesCost").classList.add('d-none');
    GetDomElement("divAddNewOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
});


function ClearOtherServiceUpdate() {
    GetDomElement("txtEditOtherServicesCode").value = "";
    GetDomElement("txtEditOtherServicesCode").classList.remove('is-invalid');
    GetDomElement("txtEditOtherServicesDescription").value = "";
    GetDomElement("txtEditOtherServicesDescription").classList.remove('is-invalid');
    GetDomElement("chkEditManualCosting").checked = false;
    GetDomElement("txtEditServiceCost").value = "";
    GetDomElement("txtEditServiceCost").disabled = false;
    GetDomElement("txtEditServiceCost").classList.remove('is-invalid');
    GetDomElement("divInvalidUpdateOtherServices").classList.add('d-none');
    GetDomElement("divInvalidUpdateOtherServicesCost").classList.add('d-none');
    GetDomElement("divUpdateOtherServicesCodeTooLongErrorAlert").classList.add('d-none');
}

function PopulateNonIndustrialLaundryItem() {
    var oLaundryItem = InvokeService(GetURL(), "NonIndustrialLaundryItemMethods", "GET", "");
    if (oLaundryItem.code == 200) {
        var oData = JSON.parse(oLaundryItem.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            laundryTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                laundryTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].id_item +
                    "</td><td class='text-uppercase' hidden>" + oJsonData[i].id_item +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger deleteLaundryItem d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    `${oJsonData[i].id_item == 1 ?"":"<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>"}` +
                    "<p class='text-primary updateLaundry d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateLaundryItems' style='cursor:pointer'>" +
                    `${oJsonData[i].id_item == 1 ? "": "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>"}` +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.deleteLaundryItem');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var laundryCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteLaundryItem = InvokeService(GetURL(), `NonIndustrialLaundryItemMethods/${laundryCode}`, "DELETE", "");
                        if (deleteLaundryItem.code == 200) {
                            var oData = JSON.parse(deleteLaundryItem.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateNonIndustrialLaundryItem();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteLaundryItem.message, deleteLaundryItem.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.updateLaundry');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearLaundryItemForm();
                    var laundryCode = this.parentNode.parentNode.cells[0].innerText;
                    var laundryDescription = this.parentNode.parentNode.cells[2].innerText;
                    GetDomElement("txtEditLaundryItemsCode").value = laundryCode;
                    GetDomElement("txtEditLaundryItemDescription").value = laundryDescription;

                });
            }

        } else if (oData.code == 204 || oData.code == 404) {
            laundryTable.innerHTML = "";
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oLaundryItem.code == 204 || oLaundryItem.code == 404) {
        laundryTable.innerHTML = "";
    } else {
        ShowErrorModalOnLoad(oLaundryItem.message, oLaundryItem.code);
    }
}

function PopulateNonIndustrialOtherServices() {
    var oOtherServices = InvokeService(GetURL(), "NonIndustrialServiceMethods", "GET", "");
    if (oOtherServices.code == 200) {
        var oData = JSON.parse(oOtherServices.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);

            otherserviceTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                otherserviceTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].id_service +
                    "</td><td class='text-uppercase' hidden>" + oJsonData[i].id_service +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='text-capitalize text-center'>" + PopulateCheckboxTable(oJsonData[i].manual_costing) +
                    "</td><td class='text-capitalize text-center' hidden>" + oJsonData[i].manual_costing +
                    "</td><td class='text-capitalize'>" + reformatAmount(oJsonData[i].unit_cost) +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger deleteOtherService d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary updateOtherServices d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateOtherServices' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.deleteOtherService');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var otherServoceCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteOtherService = InvokeService(GetURL(), `NonIndustrialServiceMethods/${otherServoceCode}`, "DELETE", "");
                        if (deleteOtherService.code == 200) {
                            var oData = JSON.parse(deleteOtherService.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateNonIndustrialOtherServices();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteOtherService.message, deleteOtherService.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.updateOtherServices');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearOtherServiceUpdate();
                    var serviceCode = this.parentNode.parentNode.cells[0].innerText;
                    var serviceDescription = this.parentNode.parentNode.cells[2].innerText;
                    var manualCosting = this.parentNode.parentNode.cells[4].innerText;
                    var cost = this.parentNode.parentNode.cells[5].innerText;
                    GetDomElement("txtEditOtherServicesCode").value = serviceCode;
                    GetDomElement("txtEditOtherServicesDescription").value = serviceDescription;
                    GetDomElement("txtEditServiceCost").value = cost;
                    PopulateCheboxInputField(manualCosting, "chkEditManualCosting");
                    if (manualCosting == 1) {
                        GetDomElement("txtEditServiceCost").disabled = true;
                    } else {
                        GetDomElement("txtEditServiceCost").disabled = false;
                    }

                });
            }

        } else if (oData.code == 204 || oData.code == 404) {
            otherserviceTable.innerHTML = "";
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oOtherServices.code == 404 || oOtherServices.code == 204) {
        otherserviceTable.innerHTML = "";
    } else {
        ShowErrorModalOnLoad(oOtherServices.message, oOtherServices.code);
    }
}

GetDomElement("chkManualCosting").addEventListener('click', function () {
    var serviceCost = GetDomElement("txtServiceCost");
    if (this.checked) {
        serviceCost.value = 0;
        serviceCost.disabled = true;
        serviceCost.classList.remove('is-invalid');
    } else {
        serviceCost.value = "";
        serviceCost.disabled = false;
    }
});
GetDomElement("chkEditManualCosting").addEventListener('click', function () {
    var serviceCost = GetDomElement("txtEditServiceCost");
    if (this.checked) {
        serviceCost.value = 0;
        serviceCost.disabled = true;
        serviceCost.classList.remove('is-invalid');
    } else {
        serviceCost.value = "";
        serviceCost.disabled = false;
    }
});

ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");