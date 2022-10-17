var categoryTable = GetDomElement("tblCategoryListing");
var serviceTable = GetDomElement("tblServiceListing");
var laundryTable = GetDomElement("tblLaundryItems");


window.onload = () => {
    PopulateCategoryListing();
    PopulateServiceListingTable();
    PopulateIndustrialLaundryItem();
    PopulateCategoryListingDropDown("selLaundryItemCategory");
    PopulateServiceListing("selServiceList");
    PopulateCategoryListingDropDown("selEditLaundryItemCategory");
    PopulateServiceListing("selEditServiceList");
}

/*CATEGORY LISTING*/
GetDomElement("btnNewCategoryListing").onclick = () => {
    GetDomElement("txtCategoryDescription").value = "";
    GetDomElement("txtCategoryDescription").classList.remove('is-invalid');
    GetDomElement("selUnit").selectedIndex = 0;
    GetDomElement("selUnit").classList.remove('is-invalid');
    GetDomElement("divInvalidCategoryListingForm").classList.add('d-none');
    GetDomElement("divAddNewCategoryTooLongErrorAlert").classList.add('d-none');
}


/* SUBMIT NEW CATEGORY LISTING RECORD */
GetDomElement("btnSubmitNewCategoryListing").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.category-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidCategoryListingForm");
    // var categoryListingCode = GetDomElement("txtCategoryCode").value;
    var categoryListingDescription = GetDomElement("txtCategoryDescription").value;
    var categoryListingUnit = GetDomElement("selUnit").value;
    if (bValidForm) {
        // GetDomElement("txtCategoryCode").classList.remove('is-invalid');
        // if (categoryListingCode.length <= 10) {
        GetDomElement("divAddNewCategoryTooLongErrorAlert").classList.add('d-none');
        // GetDomElement("txtCategoryCode").classList.remove('is-invalid');

        /*POST REQUREST HERE */
        this.dataset.target = "#btnConfirm1";
        GetDomElement("method").innerText = "submit";
        GetDomElement("method").classList.add("text-success");
        GetDomElement("confirmTwoMethod").innerText = "submit";
        GetDomElement("confirmTwoMethod").classList.add("text-success");

        GetDomElement("btnConfirmTwo").onclick = () => {
            var oNewCategoryListing = {
                "id_category": 0,
                "description": categoryListingDescription.toLowerCase(),
                "unit": categoryListingUnit
            }
            var strcategoryListing = JSON.stringify(oNewCategoryListing);
            var oSubmitNewCategoryListing = InvokeService(GetURL(), "IndustrialCategoryMethods", "POST", strcategoryListing);
            if (oSubmitNewCategoryListing.code == 200) {
                var oData = JSON.parse(oSubmitNewCategoryListing.data);
                if (oData.code == 200) {
                    ShowSuccessModal("btnConfirmTwo", "added");
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseCategoryListingModal').click();
                    PopulateCategoryListing();
                    PopulateCategoryListingDropDown("selLaundryItemCategory");
                    PopulateCategoryListingDropDown("selEditLaundryItemCategory");
                } else {
                    ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                }

            } else {
                ShowErrorModal("btnConfirmTwo", oSubmitNewCategoryListing.message, oSubmitNewCategoryListing.code);
            }
        }
        // } else {
        //     this.dataset.target = "";
        //     GetDomElement("divAddNewCategoryTooLongErrorAlert").classList.remove('d-none');
        //     GetDomElement("txtCategoryCode").classList.add('is-invalid');
        // }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewCategoryTooLongErrorAlert").classList.add('d-none');
    }
});

/* UPDATE CATEGORY LISTING RECORD */
GetDomElement("btnSubmitUpdatedCategoryListing").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.category-update-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divUpdateInvalidCategoryListingForm");
    var categoryListingCode = GetDomElement("txtEditCategoryCode").value;
    var categoryListingDescription = GetDomElement("txtEditCategoryDescription").value;
    var categoryListingUnit = GetDomElement("selEditUnit").value;
    if (bValidForm) {
        GetDomElement("txtEditCategoryCode").classList.remove('is-invalid');
        if (categoryListingCode.length <= 10) {
            GetDomElement("divUpdateCategoryTooLongErrorAlert").classList.add('d-none');
            GetDomElement("txtEditCategoryCode").classList.remove('is-invalid');

            /*POST REQUREST HERE */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = () => {
                var oUpdatedCategoryListing = {
                    "id_category": categoryListingCode,
                    "description": categoryListingDescription.toLowerCase(),
                    "unit": categoryListingUnit
                }
                var strcategoryListing = JSON.stringify(oUpdatedCategoryListing);
                var oSubmitUpdatedCategoryListing = InvokeService(GetURL(), `IndustrialCategoryMethods/${categoryListingCode}`, "PUT", strcategoryListing);
                if (oSubmitUpdatedCategoryListing.code == 200) {
                    var oData = JSON.parse(oSubmitUpdatedCategoryListing.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateCategoryListingModal').click();
                        PopulateCategoryListing();
                        PopulateCategoryListingDropDown("selLaundryItemCategory");
                        PopulateCategoryListingDropDown("selEditLaundryItemCategory");
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitUpdatedCategoryListing.message, oSubmitUpdatedCategoryListing.code);
                }
            }
        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateCategoryTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditCategoryCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateCategoryTooLongErrorAlert").classList.add('d-none');
    }
});

/*SERVICE LISTING */

/* SUBMIT NEW SERVICE LISTING RECORD */
GetDomElement("btnSubmitNewServiceListing").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.service-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewServiceListing");
    // var serviceListingCode = GetDomElement("txtServiceListingCode").value;
    var serviceListingDescription = GetDomElement("txtServiceListingDescription").value;
    if (bValidForm) {
        // GetDomElement("txtServiceListingCode").classList.remove('is-invalid');
        // if (serviceListingCode.length <= 10) {
        //     GetDomElement("txtServiceListingCode").classList.remove('is-invalid');
        /*POST REQUREST HERE */
        this.dataset.target = "#btnConfirm1";
        GetDomElement("method").innerText = "submit";
        GetDomElement("method").classList.add("text-success");
        GetDomElement("confirmTwoMethod").innerText = "submit";
        GetDomElement("confirmTwoMethod").classList.add("text-success");

        GetDomElement("btnConfirmTwo").onclick = () => {
            var oNewServiceListing = {
                "id_service": 0,
                "description": serviceListingDescription.toLowerCase()
            }
            var strNewServiceListing = JSON.stringify(oNewServiceListing);
            var submitNewServiceListing = InvokeService(GetURL(), "IndustrialServiceMethods", "POST", strNewServiceListing);
            if (submitNewServiceListing.code == 200) {
                var oData = JSON.parse(submitNewServiceListing.data);
                if (oData.code == 200) {
                    ShowSuccessModal("btnConfirmTwo", "added");
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseAddNewServiceListingModal').click();
                    PopulateServiceListingTable();
                    PopulateServiceListing("selServiceList");
                    PopulateServiceListing("selEditServiceList");
                } else {
                    ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                }

            } else {
                ShowErrorModal("btnConfirmTwo", submitNewServiceListing.message, submitNewServiceListing.code);
            }
        }
        GetDomElement("divAddNewServiceTooLongErrorAlert").classList.add('d-none');
        // } else {
        //     this.dataset.target = "";
        //     GetDomElement("divAddNewServiceTooLongErrorAlert").classList.remove('d-none');
        //     GetDomElement("txtServiceListingCode").classList.add('is-invalid');
        // }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewServiceTooLongErrorAlert").classList.add('d-none');
    }
});
/* UPDATE SERVICE LISTING RECORD */
GetDomElement("btnSubmitUpdatedServiceListing").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.service-update-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateServiceListing");
    var serviceListingCode = GetDomElement("txtEditServiceListingCode").value;
    var serviceListingDescription = GetDomElement("txtEditServiceListingDescription").value;
    if (bValidForm) {
        GetDomElement("txtEditServiceListingCode").classList.remove('is-invalid');
        if (serviceListingCode.length <= 10) {
            GetDomElement("txtEditServiceListingCode").classList.remove('is-invalid');
            /*POST REQUREST HERE */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = () => {
                var oUpdatedServiceListing = {
                    "id_service": serviceListingCode,
                    "description": serviceListingDescription.toLowerCase()
                }
                var strUpdateServiceListing = JSON.stringify(oUpdatedServiceListing);
                var submitUpdatedServiceListing = InvokeService(GetURL(), `IndustrialServiceMethods/${serviceListingCode}`, "PUT", strUpdateServiceListing);
                if (submitUpdatedServiceListing.code == 200) {
                    var oData = JSON.parse(submitUpdatedServiceListing.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateServiceListingModal').click();
                        PopulateServiceListingTable();
                        PopulateServiceListing("selServiceList");
                        PopulateServiceListing("selEditServiceList");
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", submitUpdatedServiceListing.message, submitUpdatedServiceListing.code);
                }
            }
            GetDomElement("divUpdateServiceTooLongErrorAlert").classList.add('d-none');
        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateServiceTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditServiceListingCode").classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateServiceTooLongErrorAlert").classList.add('d-none');
    }
});

GetDomElement("btnNEwServiceListing").addEventListener('click', function () {
    // GetDomElement("txtServiceListingCode").value = "";
    // GetDomElement("txtServiceListingCode").classList.remove('is-invalid');
    GetDomElement("txtServiceListingDescription").value = "";
    GetDomElement("txtServiceListingDescription").classList.remove('is-invalid');
    GetDomElement("divInvalidNewServiceListing").classList.add('d-none');
    GetDomElement("divAddNewServiceTooLongErrorAlert").classList.add('d-none');
});

function ClearServiceListingUpdate() {
    GetDomElement("txtEditServiceListingCode").value = "";
    GetDomElement("txtEditServiceListingCode").classList.remove('is-invalid');
    GetDomElement("txtEditServiceListingDescription").value = "";
    GetDomElement("txtEditServiceListingDescription").classList.remove('is-invalid');
    GetDomElement("divInvalidUpdateServiceListing").classList.add('d-none');
    GetDomElement("divUpdateServiceTooLongErrorAlert").classList.add('d-none');
}

/* LAUNDRY ITEMS */
GetDomElement("chkManualCosting").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("txtUnitCostWithADL").value = 0;
        GetDomElement("txtUnitCostWithADL").disabled = true;
        GetDomElement("txtUnitCostWithADL").classList.remove('laundry-required');
        GetDomElement("txtUnitCostWithADL").classList.remove('is-invalid');
        GetDomElement("txtUnitCostWithoutADL").value = 0;
        GetDomElement("txtUnitCostWithoutADL").disabled = true;
        GetDomElement("txtUnitCostWithoutADL").classList.remove('laundry-required');
        GetDomElement("txtUnitCostWithoutADL").classList.remove('is-invalid');
    } else {
        GetDomElement("txtUnitCostWithADL").value = "";
        GetDomElement("txtUnitCostWithADL").disabled = false;
        GetDomElement("txtUnitCostWithoutADL").value = "";
        GetDomElement("txtUnitCostWithoutADL").disabled = false;
        GetDomElement("txtUnitCostWithADL").classList.add('laundry-required');
        GetDomElement("txtUnitCostWithoutADL").classList.add('laundry-required');
    }
});
GetDomElement("chkEditManualCosting").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("txtEditUnitCostWithADL").value = 0;
        GetDomElement("txtEditUnitCostWithADL").disabled = true;
        GetDomElement("txtEditUnitCostWithADL").classList.remove('laundry-required');
        GetDomElement("txtEditUnitCostWithADL").classList.remove('is-invalid');
        GetDomElement("txtEditUnitCostWithoutADL").value = 0;
        GetDomElement("txtEditUnitCostWithoutADL").disabled = true;
        GetDomElement("txtEditUnitCostWithoutADL").classList.remove('laundry-required');
        GetDomElement("txtEditUnitCostWithoutADL").classList.remove('is-invalid');
    } else {
        GetDomElement("txtEditUnitCostWithADL").value = "";
        GetDomElement("txtEditUnitCostWithADL").disabled = false;
        GetDomElement("txtEditUnitCostWithoutADL").value = "";
        GetDomElement("txtEditUnitCostWithoutADL").disabled = false;
        GetDomElement("txtEditUnitCostWithADL").classList.add('laundry-required');
        GetDomElement("txtEditUnitCostWithoutADL").classList.add('laundry-required');
    }
});

/* SUBMIT NEW LAUNDRY ITEM RECORD */
GetDomElement("btnSubmitNewLaundryItem").addEventListener('click', function () {
    var bZeroAmount = true;
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.laundry-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidNewLaundryItems");
    // var LaundryItemCode = GetDomElement("txtLaundryItemsCode").value;
    var LaundryItemDescription = GetDomElement("txtLaundryItemDescription").value;
    var LaundryCategory = GetDomElement("selLaundryItemCategory").value;
    var LaundryService = GetDomElement("selServiceList").value;
    var UnitCostWithADL = GetDomElement("txtUnitCostWithADL").value;
    var UnitCostWitouthADL = GetDomElement("txtUnitCostWithoutADL").value;

    if (bValidForm) {
        // GetDomElement("txtLaundryItemsCode").classList.remove("is-invalid");
        // if (LaundryItemCode.length <= 10) {
        this.dataset.target = "";
        GetDomElement("divInvalidAmountErrorAlert").classList.add('d-none');
        // GetDomElement("txtLaundryItemsCode").classList.remove("is-invalid");
        GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
        var oInputAmount = document.querySelectorAll('.amount-check');
        var bValidAmount = CheckValidAmount(oInputAmount, "divInvalidAmountErrorAlert");
        if (GetDomElement("chkManualCosting").checked === false) {
            bZeroAmount = ZeroAmountCheck(oInputAmount, "divInvalidAmountErrorAlert");
        }
        if (bValidAmount && bZeroAmount) {
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            /*POST REQUREST HERE */
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "submit";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "submit";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = () => {
                var oNewLaundryItem = {
                    "id_item": 0,
                    "description": LaundryItemDescription.toLowerCase(),
                    "category": {
                        "id_category": LaundryCategory,
                        "description": "string",
                        "unit": ""
                    },
                    "service": {
                        "id_service": LaundryService,
                        "description": ""
                    },
                    "manual_costing": GetCheckboxValue("chkManualCosting"),
                    "unit_cost_adl": UnitCostWithADL,
                    "unit_cost": UnitCostWitouthADL
                }
                var strNewLaundryItem = JSON.stringify(oNewLaundryItem);
                var oSubmitNewLaundryItem = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "POST", strNewLaundryItem);
                if (oSubmitNewLaundryItem.code == 200) {
                    var oData = JSON.parse(oSubmitNewLaundryItem.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewLaundryItemsModal').click();
                        PopulateIndustrialLaundryItem();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewLaundryItem.message, oSubmitNewLaundryItem.code);
                }
            };

        } else {
            this.dataset.target = "";
        }



        // } else {
        //     this.dataset.target = "";
        //     GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.remove('d-none');
        //     GetDomElement("txtLaundryItemsCode").classList.add("is-invalid");
        //     GetDomElement("divInvalidAmountErrorAlert").classList.add('d-none');
        // }
    } else {
        this.dataset.target = "";
        GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
        GetDomElement("divInvalidAmountErrorAlert").classList.add('d-none');
    }
});
/* SUBMIT UPDATED LAUNDRY ITEM RECORD */
GetDomElement("btnSubmitUpdatedLaundryItem").addEventListener('click', function () {
    var bZeroAmount = true;
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.laundry-update-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divInvalidUpdateLaundryItems");
    var LaundryItemCode = GetDomElement("txtEditLaundryItemsCode").value;
    var LaundryItemDescription = GetDomElement("txtEditLaundryItemDescription").value;
    var LaundryCategory = GetDomElement("selEditLaundryItemCategory").value;
    var LaundryService = GetDomElement("selEditServiceList").value;
    var UnitCostWithADL = GetDomElement("txtEditUnitCostWithADL").value;
    var UnitCostWitouthADL = GetDomElement("txtEditUnitCostWithoutADL").value;

    if (bValidForm) {
        GetDomElement("txtEditLaundryItemsCode").classList.remove("is-invalid");
        if (LaundryItemCode.length <= 10) {
            this.dataset.target = "";
            GetDomElement("divUpdateInvalidAmountErrorAlert").classList.add('d-none');
            GetDomElement("txtEditLaundryItemsCode").classList.remove("is-invalid");
            GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
            var oInputAmount = document.querySelectorAll('.amount-update-check');
            var bValidAmount = CheckValidAmount(oInputAmount, "divUpdateInvalidAmountErrorAlert");
            if (GetDomElement("chkEditManualCosting").checked === false) {
                bZeroAmount = ZeroAmountCheck(oInputAmount, "divUpdateInvalidAmountErrorAlert");
            }
            if (bValidAmount && bZeroAmount) {
                this.dataset.target = "#btnConfirm1";
                GetDomElement("method").innerText = "update";
                GetDomElement("method").classList.add("text-success");
                /*POST REQUREST HERE */
                this.dataset.target = "#btnConfirm1";
                GetDomElement("method").innerText = "update";
                GetDomElement("method").classList.add("text-success");
                GetDomElement("confirmTwoMethod").innerText = "update";
                GetDomElement("confirmTwoMethod").classList.add("text-success");

                GetDomElement("btnConfirmTwo").onclick = () => {
                    var oUpdatedLaundryItem = {
                        "id_item": LaundryItemCode,
                        "description": LaundryItemDescription.toLowerCase(),
                        "category": {
                            "id_category": LaundryCategory,
                            "description": "string",
                            "unit": ""
                        },
                        "service": {
                            "id_service": LaundryService,
                            "description": ""
                        },
                        "manual_costing": GetCheckboxValue("chkEditManualCosting"),
                        "unit_cost_adl": UnitCostWithADL,
                        "unit_cost": UnitCostWitouthADL
                    }
                    var strUpdatedLaundryItem = JSON.stringify(oUpdatedLaundryItem);
                    var oSubmitUpdatesLaundryItem = InvokeService(GetURL(), `IndustrialLaundryItemMethods/${LaundryItemCode}`, "PUT", strUpdatedLaundryItem);
                    if (oSubmitUpdatesLaundryItem.code == 200) {
                        var oData = JSON.parse(oSubmitUpdatesLaundryItem.data);
                        if (oData.code == 200) {
                            ShowSuccessModal("btnConfirmTwo", "updated");
                            GetDomElement('btnCloseConfimrTwo').click();
                            GetDomElement('btnCloseUpdateLaundryItemsModal').click();
                            PopulateIndustrialLaundryItem();
                        } else {
                            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        }

                    } else {
                        ShowErrorModal("btnConfirmTwo", oSubmitUpdatesLaundryItem.message, oSubmitUpdatesLaundryItem.code);
                    }
                }

            } else {
                this.dataset.target = "";
            }



        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.remove('d-none');
            GetDomElement("txtEditLaundryItemsCode").classList.add("is-invalid");
            GetDomElement("divUpdateInvalidAmountErrorAlert").classList.add('d-none');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
        GetDomElement("divUpdateInvalidAmountErrorAlert").classList.add('d-none');
    }
});


GetDomElement('btnNewLaundryItems').addEventListener('click', function () {
    // GetDomElement("txtLaundryItemsCode").value = "";
    // GetDomElement("txtLaundryItemsCode").classList.remove('is-invalid');
    GetDomElement("selLaundryItemCategory").selectedIndex = 0;
    GetDomElement("selLaundryItemCategory").classList.remove('is-invalid');
    GetDomElement("selServiceList").selectedIndex = 0;
    GetDomElement("selServiceList").classList.remove('is-invalid');
    GetDomElement("txtLaundryItemDescription").value = "";
    GetDomElement("txtLaundryItemDescription").classList.remove('is-invalid');
    GetDomElement("chkManualCosting").checked = false;
    GetDomElement("txtUnitCostWithADL").value = "";
    GetDomElement("txtUnitCostWithADL").disabled = false;
    GetDomElement("txtUnitCostWithADL").classList.remove("is-invalid");
    GetDomElement("txtUnitCostWithoutADL").value = "";
    GetDomElement("txtUnitCostWithoutADL").disabled = false;
    GetDomElement("txtUnitCostWithoutADL").classList.remove("is-invalid");
    GetDomElement("divInvalidNewLaundryItems").classList.add('d-none');
    GetDomElement("divAddNewLaundryTooLongErrorAlert").classList.add('d-none');
    GetDomElement("divInvalidAmountErrorAlert").classList.add('d-none');
});
/* CLEAR UPDATE LAUNDRY ITEM FORM */
function ClearLaundryItemForm() {
    GetDomElement("txtEditLaundryItemsCode").value = "";
    GetDomElement("txtEditLaundryItemsCode").classList.remove('is-invalid');
    GetDomElement("selEditLaundryItemCategory").selectedIndex = 0;
    GetDomElement("selEditLaundryItemCategory").classList.remove('is-invalid');
    GetDomElement("selEditServiceList").selectedIndex = 0;
    GetDomElement("selEditServiceList").classList.remove('is-invalid');
    GetDomElement("txtEditLaundryItemDescription").value = "";
    GetDomElement("txtEditLaundryItemDescription").classList.remove('is-invalid');
    GetDomElement("chkEditManualCosting").checked = false;
    GetDomElement("txtEditUnitCostWithADL").value = "";
    GetDomElement("txtEditUnitCostWithADL").disabled = false;
    GetDomElement("txtEditUnitCostWithADL").classList.remove("is-invalid");
    GetDomElement("txtEditUnitCostWithoutADL").value = "";
    GetDomElement("txtEditUnitCostWithoutADL").disabled = false;
    GetDomElement("txtEditUnitCostWithoutADL").classList.remove("is-invalid");
    GetDomElement("divInvalidUpdateLaundryItems").classList.add('d-none');
    GetDomElement("divUpdateLaundryTooLongErrorAlert").classList.add('d-none');
    GetDomElement("divUpdateInvalidAmountErrorAlert").classList.add('d-none');

}

function PopulateCategoryListing() {
    GetDomElement("emptyCategoryListing").classList.add('d-none');
    var oCategoryList = InvokeService(GetURL(), "IndustrialCategoryMethods", "GET", "");
    if (oCategoryList.code == 200) {
        var oData = JSON.parse(oCategoryList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            categoryTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                categoryTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].id_category +
                    "</td><td class='text-uppercase' hidden>" + oJsonData[i].id_category +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='text-capitalize'>" + oJsonData[i].unit +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger CategoryDelete d-flex align-items-center' data-toggle='modal' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-xl-block'>Delete</span></p>" +
                    "<p class='text-primary updateCategory d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateCategoryListing' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-xl-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = [];
            btnDelete = document.querySelectorAll('.CategoryDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var categoryCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteCategory = InvokeService(GetURL(), `IndustrialCategoryMethods/${categoryCode}`, "DELETE", "");
                        if (deleteCategory.code == 200) {
                            var oData = JSON.parse(deleteCategory.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateCategoryListing();
                                PopulateCategoryListingDropDown("selLaundryItemCategory");
                                PopulateCategoryListingDropDown("selEditLaundryItemCategory");
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteCategory.message, deleteCategory.code);
                        }

                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.updateCategory');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearCategoryListing();
                    var categoryCode = this.parentNode.parentNode.cells[0].innerText;
                    var categoryDescription = this.parentNode.parentNode.cells[2].innerText;
                    var categoryUnit = this.parentNode.parentNode.cells[3].innerText;
                    GetDomElement("txtEditCategoryCode").value = categoryCode;
                    GetDomElement("txtEditCategoryDescription").value = categoryDescription;
                    GetDomElement("selEditUnit").value = categoryUnit;

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            categoryTable.innerHTML = "";
            GetDomElement("emptyCategoryListing").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oCategoryList.code == 204 || oCategoryList.code == 404) {
        categoryTable.innerHTML = "";
        GetDomElement("emptyCategoryListing").classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oCategoryList.message, oCategoryList.code);
    }
}



function PopulateServiceListingTable() {
    GetDomElement("emptyServiceListing").classList.add('d-none');
    var oServiceList = InvokeService(GetURL(), "IndustrialServiceMethods", "GET", "");
    if (oServiceList.code == 200) {
        var oData = JSON.parse(oServiceList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            serviceTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                serviceTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].id_service +
                    "</td><td class='text-uppercase' hidden>" + oJsonData[i].id_service +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger serviceDelete d-flex align-items-center' data-toggle='modal' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary updateService d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateServiceListing' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = [];
            btnDelete = document.querySelectorAll('.serviceDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var serviceCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteService = InvokeService(GetURL(), `IndustrialServiceMethods/${serviceCode}`, "DELETE", "");
                        if (deleteService.code == 200) {
                            var oData = JSON.parse(deleteService.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateServiceListingTable();
                                PopulateServiceListing("selServiceList");
                                PopulateServiceListing("selEditServiceList");
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteService.message, oData.code);
                        }

                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.updateService');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearServiceListingUpdate();
                    var serviceCode = this.parentNode.parentNode.cells[0].innerText;
                    var serviceDescription = this.parentNode.parentNode.cells[2].innerText;
                    GetDomElement("txtEditServiceListingCode").value = serviceCode;
                    GetDomElement("txtEditServiceListingDescription").value = serviceDescription;

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            serviceTable.innerHTML = "";
            GetDomElement("emptyServiceListing").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oServiceList.code == 404 || oServiceList.code == 204) {
        serviceTable.innerHTML = "";
        GetDomElement("emptyServiceListing").classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oServiceList.message, oServiceList.code);
    }
}

function PopulateIndustrialLaundryItem() {
    GetDomElement("emptyLaundryItems").classList.add('d-none');
    var oLaundryItem = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "GET", "");
    if (oLaundryItem.code == 200) {
        var oData = JSON.parse(oLaundryItem.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            laundryTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                laundryTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].id_item +
                    "</td><td class='text-uppercase' hidden>" + oJsonData[i].id_item +
                    "</td><td class='text-capitalize'>" + oJsonData[i].category.description +
                    "</td><td class='text-capitalize'>" + oJsonData[i].service.description +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='text-capitalize text-center'>" + PopulateCheckboxTable(oJsonData[i].manual_costing) +
                    "</td><td class='text-capitalize'>" +
                    "With ADL <br>" +
                    "<strong>" + oJsonData[i].unit_cost_adl + "</strong><br>" +
                    "Without ADL <br>" +
                    "<strong>" + oJsonData[i].unit_cost + "</strong><br>" +
                    "</td><td hidden>" + `{"manual_costing":${oJsonData[i].manual_costing},
                                            "unit_cost_adl":${oJsonData[i].unit_cost_adl},
                                            "unit_cost":${oJsonData[i].unit_cost},
                                            "id_category":${oJsonData[i].category.id_category},
                                            "id_service":${oJsonData[i].service.id_service}}` +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger laundryDelete d-flex align-items-center' data-toggle='modal' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "<p class='text-primary updateLaundry d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateLaundryItems' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = [];
            btnDelete = document.querySelectorAll('.laundryDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var laundryCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteLaundry = InvokeService(GetURL(), `IndustrialLaundryItemMethods/${laundryCode}`, "DELETE", "");
                        if (deleteLaundry.code == 200) {
                            var oData = JSON.parse(deleteLaundry.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateIndustrialLaundryItem();

                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteLaundry.message, deleteLaundry.code);
                        }

                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.updateLaundry');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearLaundryItemForm();
                    var laundryCode = this.parentNode.parentNode.cells[0].innerText;
                    var laundryDescription = this.parentNode.parentNode.cells[4].innerText;
                    var laundryDetails = JSON.parse(this.parentNode.parentNode.cells[7].innerText);
                    GetDomElement("txtEditLaundryItemsCode").value = laundryCode;
                    GetDomElement("txtEditLaundryItemDescription").value = laundryDescription;
                    GetDomElement("selEditLaundryItemCategory").value = laundryDetails.id_category;
                    GetDomElement("selEditServiceList").value = laundryDetails.id_service;
                    GetDomElement("txtEditUnitCostWithADL").value = laundryDetails.unit_cost_adl;
                    GetDomElement("txtEditUnitCostWithoutADL").value = laundryDetails.unit_cost;
                    PopulateCheboxInputField(laundryDetails.manual_costing, "chkEditManualCosting");
                    if (laundryDetails.manual_costing === 1) {
                        GetDomElement("txtEditUnitCostWithADL").disabled = true;
                        GetDomElement("txtEditUnitCostWithoutADL").disabled = true;
                    }

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            laundryTable.innerHTML = "";
            GetDomElement("emptyLaundryItems").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oLaundryItem.code.code == 404 || oLaundryItem.code.code == 204) {
        laundryTable.innerHTML = "";
        GetDomElement("emptyLaundryItems").classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oLaundryItem.message, oLaundryItem.code);
    }
}

function ClearCategoryListing() {
    GetDomElement("txtEditCategoryCode").value = "";
    GetDomElement("txtEditCategoryCode").classList.remove('is-invalid');
    GetDomElement("txtEditCategoryDescription").value = "";
    GetDomElement("txtEditCategoryDescription").classList.remove('is-invalid');
    GetDomElement("selEditUnit").selectedIndex = 0;
    GetDomElement("selEditUnit").classList.remove('is-invalid');
    GetDomElement("divUpdateInvalidCategoryListingForm").classList.add('d-none');
    GetDomElement("divUpdateCategoryTooLongErrorAlert").classList.add('d-none');
}


ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");