var logisticsSiteTable = GetDomElement("tblLogisticsSite");
var laudrySiteTable = GetDomElement("tblLaundrySite");



var authkey = sessionStorage.getItem('authkey');
window.onload = () => {
    if (authkey == "" || authkey == null) {
        location.href = "admin-login.html";
    }
    PopulateLogisticSite();
    PopulateLaundrySite();
}

GetDomElement('btnNewLogisticsSite').addEventListener('click', function () {
    GetDomElement("txtLogisticsSiteCode").value = "";
    GetDomElement("txtLogisticsSiteCode").classList.remove('is-invalid');
    GetDomElement("txtLogisticsSiteName").value = "";
    GetDomElement("txtLogisticsSiteName").classList.remove('is-invalid');
    GetDomElement("divInvalidNewLogisticsSite").classList.add('d-none');
    GetDomElement("divAddNewLogisticsTooLongErrorAlert").classList.add('d-none');
    GetDomElement("logisticsForm").classList.remove('was-validated');
});

/* SUBMIT LOGISTICS SITE */
GetDomElement('btnSubmitLogisticsSite').addEventListener('click', function () {
    this.dataset.target = "";
    GetDomElement("logisticsForm").classList.add('was-validated');
    var oRequiredFields = document.querySelectorAll('.logistics-input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewLogisticsSite");
    if (bValidForm) {
        var logisticsCode = GetDomElement("txtLogisticsSiteCode");
        var logisticsDescription = GetDomElement("txtLogisticsSiteName");
        if (logisticsCode.value.length <= 10) {
            GetDomElement("divAddNewLogisticsTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtLogisticsSiteCode").classList.remove('is-invalid');

            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";
                /* POST REQUEST TO DB */
                var oNewLosisticsRecord = {
                    "code": logisticsCode.value.toUpperCase(),
                    "site": logisticsDescription.value.toLowerCase()
                }
                var strNewLogisticsRecord = JSON.stringify(oNewLosisticsRecord);
                var oSubmitNewLogistics = InvokeService(GetURL(), "LogisticSiteMethods", "POST", strNewLogisticsRecord);
                if (oSubmitNewLogistics.code == 200) {
                    var oData = JSON.parse(oSubmitNewLogistics.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewLogisticsSiteModal').click();
                        PopulateLogisticSite();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewLogistics.message, oSubmitNewLogistics.code);
                }
            }


        } else {
            this.dataset.target = "";
            GetDomElement("divAddNewLogisticsTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtLogisticsSiteCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewLogisticsTooLongErrorAlert").classList.add('d-none');
    }
});
/* SUBMIT LAUNDRY SITE */
GetDomElement('btnSubmitNewLaundrySite').addEventListener('click', function () {
    this.dataset.target = "";
    GetDomElement("laundryForm").classList.add('was-validated');
    var oRequiredFields = document.querySelectorAll('.laundry-input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewLaundrySite");
    if (bValidForm) {
        var laundryCode = GetDomElement("txtLaundrySiteCode");
        var laundryDescription = GetDomElement("txtLaundrySiteName");
        if (laundryCode.value.length <= 10) {
            GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtLaundrySiteCode").classList.remove('is-invalid');
            /* POST REQUEST TO DB */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";
                var oNewLaundryRecord = {
                    "code": laundryCode.value.toUpperCase(),
                    "site": laundryDescription.value.toLowerCase()
                }

                var strNewLaundryRecord = JSON.stringify(oNewLaundryRecord);
                var oSubmitNewLaundryRecord = InvokeService(GetURL(), "LaundrySiteMethods", "POST", strNewLaundryRecord);
                if (oSubmitNewLaundryRecord.code == 200) {
                    var oData = JSON.parse(oSubmitNewLaundryRecord.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewLaundrySiteModal').click();
                        PopulateLaundrySite();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewLaundryRecord.message, oData.code);
                }


            }
        } else {
            this.dataset.target = "";
            GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtLaundrySiteCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
    }
});


function PopulateLogisticSite() {
    GetDomElement('emptyLogisticsSite').classList.add('d-none');
    var oLogisticSites = InvokeService(GetURL(), "LogisticSiteMethods", "GET", "");
    if (oLogisticSites.code == 200) {
        var oData = JSON.parse(oLogisticSites.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            logisticsSiteTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                logisticsSiteTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].code +
                    "</td><td class='text-uppercase'>" + oJsonData[i].code +
                    "</td><td class='text-capitalize'>" + oJsonData[i].site +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger SiteDelete d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary siteUpdate d-flex align-items-center' data-toggle='modal' data-target ='#btnEditLogisticsSite' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.SiteDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var logisticsCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteLogisticsSite = InvokeService(GetURL(), `LogisticSiteMethods/${logisticsCode}`, "DELETE", "");
                        if (deleteLogisticsSite.code == 200) {
                            var oData = JSON.parse(deleteLogisticsSite.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateLogisticSite();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteLogisticsSite.message, deleteLogisticsSite.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.siteUpdate');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearUpdateLogisticsForm();
                    var logisticsCode = this.parentNode.parentNode.cells[0].innerText;
                    var logisticsDescription = this.parentNode.parentNode.cells[2].innerText;
                    GetDomElement("txtEditLogisticsSiteCode").value = logisticsCode;
                    GetDomElement("txtEditLogisticsSiteName").value = logisticsDescription;

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            logisticsSiteTable.innerHTML = "";
            GetDomElement('emptyLogisticsSite').classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oLogisticSites.code == 404 || oLogisticSites.code == 204) {
        logisticsSiteTable.innerHTML = "";
        GetDomElement('emptyLogisticsSite').classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oLogisticSites.message, oLogisticSites.code);
    }
}

function PopulateLaundrySite() {
    GetDomElement('emptyLaundrySite').classList.add('d-none');
    var oLaundrySites = InvokeService(GetURL(), "LaundrySiteMethods", "GET", "");
    if (oLaundrySites.code == 200) {
        var oData = JSON.parse(oLaundrySites.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            laudrySiteTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                laudrySiteTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].code +
                    "</td><td class='text-uppercase'>" + oJsonData[i].code +
                    "</td><td class='text-capitalize'>" + oJsonData[i].site +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger btnDeleteLaundry d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary updateLaundry d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateLaundrySite' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDeleteLaundry = document.querySelectorAll('.btnDeleteLaundry');
            for (var i = 0; i < btnDeleteLaundry.length; i++) {
                btnDeleteLaundry[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var laundryCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteLaundryRecord = InvokeService(GetURL(), `LaundrySiteMethods/${laundryCode}`, "DELETE", "");
                        if (deleteLaundryRecord.code == 200) {
                            var oData = JSON.parse(deleteLaundryRecord.data);
                            if (oData.code == 200) {
                                GetDomElement('btnCloseConfimrTwo').click();
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                PopulateLaundrySite();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteLaundryRecord.message, oData.code);
                        }
                    }
                });
            }

            var btnUpdateLaundry = document.querySelectorAll('.updateLaundry');
            for (var i = 0; i < btnUpdateLaundry.length; i++) {
                btnUpdateLaundry[i].addEventListener('click', function () {
                    ClearUpdateLaundryForm();
                    var laundrySiteCode = this.parentNode.parentNode.cells[0].innerText;
                    var laundrySiteDescription = this.parentNode.parentNode.cells[2].innerText;
                    GetDomElement("txtEditLaundrySiteCode").value = laundrySiteCode;
                    GetDomElement("txtEditLaundrySiteName").value = laundrySiteDescription;

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            laudrySiteTable.innerHTML = "";
            GetDomElement('emptyLaundrySite').classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oLaundrySites.code == 404 || oLaundrySites.code == 204) {
        laudrySiteTable.innerHTML = "";
        GetDomElement('emptyLaundrySite').classList.remove('d-none');
    } else {

        ShowErrorModalOnLoad(oLaundrySites.message, oLaundrySites.code);
    }
}

/* UPDATE LAUNDRY */
GetDomElement("btnSubmitUpdateLaundrySite").addEventListener('click', function () {
    this.dataset.target = "";
    GetDomElement("editLaundryForm").classList.add('was-validated');
    var oRequiredFields = document.querySelectorAll('.laundry-update-input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateLaundrySite");
    if (bValidForm) {
        var laundryCode = GetDomElement("txtEditLaundrySiteCode");
        var laundryDescription = GetDomElement("txtEditLaundrySiteName");
        if (laundryCode.value.length <= 10) {
            GetDomElement("divAddUpdateLaundryTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtEditLaundrySiteCode").classList.remove('is-invalid');
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";
                /* POST REQUEST TO DB */
                var oUpdatedLaundryRecord = {
                    "code": laundryCode.value.toUpperCase(),
                    "site": laundryDescription.value
                }
                var strNewLogisticsRecord = JSON.stringify(oUpdatedLaundryRecord);
                var oSubmitUpdatedLaundry = InvokeService(GetURL(), `LaundrySiteMethods/${laundryCode.value}`, "PUT", strNewLogisticsRecord);
                if (oSubmitUpdatedLaundry.code == 200) {
                    var oData = JSON.parse(oSubmitUpdatedLaundry.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddUpdateLaundrySiteModal').click();
                        PopulateLaundrySite();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitUpdatedLaundry.message, oSubmitUpdatedLaundry.code);
                }
            }


        } else {
            this.dataset.target = "";
            GetDomElement("divAddUpdateLaundryTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditLaundrySiteCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddUpdateLaundryTooLongErrorAlert").classList.add('d-none');
    }
});



/* CLEAR LOGISTICS FORM UPDATE */
function ClearUpdateLogisticsForm() {
    GetDomElement("txtEditLogisticsSiteCode").value = "";
    GetDomElement("txtEditLogisticsSiteCode").classList.remove('is-invalid');
    GetDomElement("txtEditLogisticsSiteName").value = "";
    GetDomElement("txtEditLogisticsSiteName").classList.remove('is-invalid');
    GetDomElement("divInvalidUpdateLogisticsSite").classList.add('d-none');
    GetDomElement("divAddUpdateLogisticsTooLongErrorAlert").classList.add('d-none');
    GetDomElement("updateLogisticsForm").classList.remove('was-validated');
}


function ClearUpdateLaundryForm() {
    GetDomElement("txtEditLaundrySiteCode").value = "";
    GetDomElement("txtEditLaundrySiteCode").classList.remove('is-invalid');
    GetDomElement("txtEditLaundrySiteName").value = "";
    GetDomElement("txtEditLaundrySiteName").classList.remove('is-invalid');
    GetDomElement("divInvalidUpdateLaundrySite").classList.add('d-none');
    GetDomElement("divAddUpdateLaundryTooLongErrorAlert").classList.add('d-none');
    GetDomElement("editLaundryForm").classList.remove('was-validated');
}


/* UPDATE LOGISTICS */
GetDomElement("btnUpdateLogisticsSite").addEventListener('click', function () {
    this.dataset.target = "";
    GetDomElement("logisticsForm").classList.add('was-validated');
    var oRequiredFields = document.querySelectorAll('.logistics-edit-input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateLogisticsSite");
    if (bValidForm) {
        var logisticsCode = GetDomElement("txtEditLogisticsSiteCode");
        var logisticsDescription = GetDomElement("txtEditLogisticsSiteName");
        if (logisticsCode.value.length <= 10) {
            GetDomElement("divAddUpdateLogisticsTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtEditLogisticsSiteCode").classList.remove('is-invalid');
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                this.dataset.target = "";
                /* POST REQUEST TO DB */
                var oNewLosisticsRecord = {
                    "code": logisticsCode.value.toUpperCase(),
                    "site": logisticsDescription.value.toLowerCase()
                }
                var strNewLogisticsRecord = JSON.stringify(oNewLosisticsRecord);
                var oSubmitNewLogistics = InvokeService(GetURL(), `LogisticSiteMethods/${logisticsCode.value}`, "PUT", strNewLogisticsRecord);
                if (oSubmitNewLogistics.code == 200) {
                    var oData = JSON.parse(oSubmitNewLogistics.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateLogisticsSiteModal').click();
                        PopulateLogisticSite();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewLogistics.message, oData.code);
                }
            }


        } else {
            this.dataset.target = "";
            GetDomElement("divAddUpdateLogisticsTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditLogisticsSiteCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddUpdateLogisticsTooLongErrorAlert").classList.add('d-none');
    }
});


GetDomElement('btnNEwLaundrySite').addEventListener('click', function () {
    GetDomElement("txtLaundrySiteCode").value = "";
    GetDomElement("txtLaundrySiteCode").classList.remove('is-invalid');
    GetDomElement("txtLaundrySiteName").value = "";
    GetDomElement("txtLaundrySiteName").classList.remove('is-invalid');
    GetDomElement("divInvalidNewLaundrySite").classList.add('d-none');
    GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
    GetDomElement("laundryForm").classList.remove('was-validated');
});


ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");