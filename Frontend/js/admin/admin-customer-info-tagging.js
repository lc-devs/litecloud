var authkey = sessionStorage.getItem('authkey');
var clientTable = GetDomElement("tblBody");
var CIFTable = GetDomElement("btnCustomerList");
var emptyCIF = GetDomElement('emptyCustomerList');
var isActiveClient = 1;
var industrialTable = GetDomElement("tblIndustrial");
var updateIndustrialTable = GetDomElement("tblUpdateIndustrialItems");
var oIndustrialProductList = [];
var oNonIndustrialProductList = [];
var oItemPostList = [];
var oUpdatedPostList = [];


window.addEventListener('load', function () {
    // PopulateIndustrialProductList(industrialTable,"emptyLaundryItems");
    CustomPopulateCategoryListingDropDown("selLaundryItemCategory");
    CustomPopulateCategoryListingDropDown("selSecondaryLaundryItemCategory");
    CustomPopulateCategoryListingDropDown("selEditLaundryItemCategory");
    if (authkey == "" || authkey == null) {
        location.href = "admin-login.html";
    }
    GetDomElement("chkActive").checked = true;

});

GetDomElement("btnSubmitNewClient").addEventListener('click', function () {
    this.dataset.target = "";
    oItemPostList = [];
    var oRequiredFields = document.querySelectorAll(".client-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divAddNewInvalidForm");
    var adlAmount = GetDomElement("txtADL").value;
    var industrial = GetCheckboxValue("chkIndustrial");
    var nonIndustrial = GetCheckboxValue("chkNonIndustrial");


    GetDomElement("divInvalidClientType").classList.add('d-none');
    if (bValidForm) {
        this.dataset.target = "";
        if (!isNaN(adlAmount)) {
            GetDomElement('txtADL').classList.remove('is-invalid');
            GetDomElement("divInvalidADL").classList.add('d-none');
            if (industrial == 0 && nonIndustrial == 0) {
                this.dataset.target = "";
                GetDomElement("divInvalidClientType").classList.remove('d-none');
            } else {
                /*POST */
                var itemList = document.querySelectorAll(".cost-required");
                var bValidItems = CheckVaidItems(itemList);
                if (bValidItems) {
                    GetDomElement("divInvalidClientType").classList.add('d-none');

                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("method").innerText = "submit";
                    GetDomElement("method").classList.add("text-success");
                    GetDomElement("confirmTwoMethod").innerText = "submit";
                    GetDomElement("confirmTwoMethod").classList.add("text-success");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        this.dataset.target = "";

                        if (GetDomElement("chkActive").checked == true) {
                            isActiveClient = 1;
                        } else {
                            isActiveClient = 0;
                        }


                        /* POST */
                        var oNewCustomer = {

                            "customer_id": 0,
                            "source_id": ReturnInputValue("customerID"),
                            "customer_name": ReturnInputValue("txtClientName"),
                            "cellular_number": ReturnInputValue("txtCellularNo"),
                            "email_address": ReturnInputValue("txtEmail"),
                            "industrial": GetCheckboxValue("chkIndustrial"),
                            "non_industrial": GetCheckboxValue("chkNonIndustrial"),
                            "active_customer": 0,
                            "account_reset": 0,
                            "customer_password": "",
                            "street_building_address": GetDomElement("ad_street").innerText,
                            "barangay_address": "",
                            "town_address": GetDomElement("ad_city").innerText,
                            "province": GetDomElement("ad_prov").innerText,
                            "longitude": 123.88228647922632,
                            "latitude": 9.627516003283043,
                            "average_daily_load": ReturnInputValue("txtADL")

                        }

                        var strNewCustomer = JSON.stringify(oNewCustomer);
                        var oSumbitNewUser = InvokeService(GetURL(), "CustomerMethods", "POST", strNewCustomer);
                        if (oSumbitNewUser.code == 200) {
                            var oData = JSON.parse(oSumbitNewUser.data);

                            if (oData.code == 200) {
                                // ShowCustomSuccess("btnConfirmTwo", oData.message);
                                GetDomElement("prefix").innerHTML = "";
                                GetDomElement('btnCloseConfirm1').click();
                                GetDomElement('btnCloseAddNewModel').click();
                                GetDomElement("btnCloseConfimrTwo").click();
                                clientTable.innerHTML = "";
                                GetDomElement("txtSearch").value = ReturnInputValue("txtClientName");
                                var oCustomerData = JSON.parse(oData.jsonData);
                                var customerLastId = oCustomerData.customer.customer_id;
                                /* SAVE ITEMS */
                                if (GetDomElement("chkIndustrial").checked) {
                                    var itemsPostList = industrialTable.rows;
                                    if (itemsPostList.length != 0) {
                                        for (var i = 0; i < itemsPostList.length; i++) {

                                            var item_id = parseInt(itemsPostList[i].cells[0].innerText);
                                            var item_description = itemsPostList[i].cells[1].innerText;
                                            var isManualCosting = parseInt(itemsPostList[i].cells[2].innerText);
                                            var checkCostWithADL = parseFloat(itemsPostList[i].cells[4].innerHTML);
                                            var checkCostWithoutADL = parseFloat(itemsPostList[i].cells[5].innerHTML);
                                            var oItemArray = {
                                                "id_item": item_id,
                                                "description": item_description,
                                                "category": {
                                                    "id_category": 0,
                                                    "description": "string",
                                                    "unit": "string"
                                                },
                                                "service": {
                                                    "id_service": 0,
                                                    "description": "string"
                                                },
                                                "manual_costing": isManualCosting,
                                                "unit_cost_adl": checkCostWithADL,
                                                "unit_cost": checkCostWithoutADL
                                            }
                                            oItemPostList.push(oItemArray);
                                        }

                                        var strItemPostList = JSON.stringify(oItemPostList);
                                        var oSubmitItems = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${customerLastId}`, "POST", strItemPostList);
                                        if (oSubmitItems.code == 200) {
                                            var oSubmitJsonDataCode = JSON.parse(oSubmitItems.data)
                                            if (oSubmitJsonDataCode.code == 200) {
                                                PopulateClientList();
                                                ShowSuccessModal("btnConfirmTwo", "added");
                                            } else {
                                                ShowErrorModal("btnConfirmTwo", oSubmitJsonDataCode.message, oSubmitJsonDataCode.code);
                                                GetDomElement('btnCloseConfirm1').click();
                                                GetDomElement("btnCloseConfimrTwo").click();
                                            }

                                        } else {
                                            ShowErrorModal("btnConfirmTwo", oSubmitItems.message, oSubmitItems.code);
                                            GetDomElement('btnCloseConfirm1').click();
                                            GetDomElement("btnCloseConfimrTwo").click();
                                        }
                                    } else {
                                        ShowSuccessModal("btnConfirmTwo", "added");
                                        PopulateClientList();
                                    }
                                } else {
                                    ShowSuccessModal("btnConfirmTwo", "added");
                                    PopulateClientList();
                                }
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

            }
        } else {
            this.dataset.target = "";
            GetDomElement("divInvalidADL").classList.remove('d-none');
            GetDomElement("divInvalidClientType").classList.add('d-none');
            GetDomElement('txtADL').classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divInvalidADL").classList.add('d-none');
        GetDomElement("divInvalidClientType").classList.add('d-none');
    }
});

/* UPDATE CLIENT */
GetDomElement("btnSubmitUpdatedClient").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll(".client-update-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divUpdateInvalidForm");
    var adlAmount = GetDomElement("txtEditADL").value;
    var industrial = GetCheckboxValue("chkEditIndustrial");
    var nonIndustrial = GetCheckboxValue("chkEditNonIndustrial");
    var isActiveOnUpdate;
    GetDomElement("divInvalidClientType").classList.add('d-none');
    if (bValidForm) {
        this.dataset.target = "";
        if (!isNaN(adlAmount)) {
            GetDomElement('txtEditADL').classList.remove('is-invalid');
            GetDomElement("divUpdateInvalidADL").classList.add('d-none');
            if (industrial == 0 && nonIndustrial == 0) {
                this.dataset.target = "";
                GetDomElement("divUpdateInvalidClientType").classList.remove('d-none');
            } else {
                /*POST */
                var itemList = document.querySelectorAll(".cost-required");
                var bValidItems = CheckVaidItems(itemList);
                if (bValidItems) {
                    GetDomElement("divUpdateInvalidClientType").classList.add('d-none');

                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("method").innerText = "update";
                    GetDomElement("method").classList.add("text-success");
                    GetDomElement("confirmTwoMethod").innerText = "update";
                    GetDomElement("confirmTwoMethod").classList.add("text-success");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        this.dataset.target = "";
                        if (GetDomElement("chkEditActive").checked == true) {
                            isActiveOnUpdate = 1;
                        } else {
                            isActiveOnUpdate = 0;
                        }


                        /* POST */
                        var oUpdatedCustomer = {

                            "customer_id": ReturnInputValue("editCustomerID"),
                            "source_id": ReturnInputValue("editCustomerSourceId"),
                            "customer_name": ReturnInputValue("txtEditClientName"),
                            "cellular_number": ReturnInputValue("txtCellularNo"),
                            "email_address": ReturnInputValue("txtEditEmail"),
                            "industrial": GetCheckboxValue("chkEditIndustrial"),
                            "non_industrial": GetCheckboxValue("chkEditNonIndustrial"),
                            "active_customer": isActiveOnUpdate,
                            "account_reset": 0,
                            "customer_password": "",
                            "street_building_address": GetDomElement("edit_ad_street").innerText,
                            "barangay_address": "",
                            "town_address": GetDomElement("edit_ad_city").innerText,
                            "province": GetDomElement("edit_ad_prov").innerText,
                            "longitude": 123.88228647922632,
                            "latitude": 9.627516003283043,
                            "average_daily_load": ReturnInputValue("txtEditADL")

                        }

                        var strUpdatedCustomer = JSON.stringify(oUpdatedCustomer);
                        var oSumbitUpdatedUser = InvokeService(GetURL(), `CustomerMethods/update/customer/id/${ReturnInputValue("editCustomerID")}`, "PUT", strUpdatedCustomer);
                        if (oSumbitUpdatedUser.code == 200) {
                            var oData = JSON.parse(oSumbitUpdatedUser.data);
                            if (oData.code == 200) {
                                oUpdatedPostList = [];
                                GetDomElement('btnCloseConfirm1').click();
                                GetDomElement('btnCloseUpdateModel').click();
                                GetDomElement("btnCloseConfimrTwo").click();
                                clientTable.innerHTML = "";
                                GetDomElement("txtSearch").value = "";
                                ShowSuccessModal("btnConfirmTwo", "updated");
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                                GetDomElement('btnCloseConfirm1').click();
                                GetDomElement("btnCloseConfimrTwo").click();
                            }
                        } else {
                            ShowErrorModal("btnConfirmTwo", oSumbitUpdatedUser.message, oSumbitUpdatedUser.code);
                            GetDomElement('btnCloseConfirm1').click();
                            GetDomElement("btnCloseConfimrTwo").click();
                        }
                    }
                }


            }
        } else {
            this.dataset.target = "";
            GetDomElement("divUpdateInvalidADL").classList.remove('d-none');
            GetDomElement("divUpdateInvalidClientType").classList.add('d-none');
            GetDomElement('txtEditADL').classList.add('is-invalid');
        }
    } else {
        this.dataset.target = "";
        GetDomElement("divUpdateInvalidADL").classList.add('d-none');
        GetDomElement("divUpdateInvalidClientType").classList.add('d-none');
    }
});

GetDomElement("btnAddNewItems").addEventListener('click', function () {
    GetDomElement("selLaundryItemCategory").selectedIndex = 0;
    GetDomElement("chkManualCosting").checked = false;
    GetDomElement("txtUnitCostWithADL").value = "";
    GetDomElement("txtUnitCostWithoutADL").value = "";
});

function ClearItems() {
    GetDomElement("selLaundryItemCategory").selectedIndex = 0;
    GetDomElement("chkManualCosting").checked = false;
    GetDomElement("txtUnitCostWithADL").value = "";
    GetDomElement("txtUnitCostWithoutADL").value = "";
}

GetDomElement("btnAddItems").addEventListener('click', function () {
    var oRequiredItems = document.querySelectorAll(".item-required");
    var bValidForm = CheckValidForm(oRequiredItems, "divInvalidForm");
    if (bValidForm) {
        industrialTable.innerHTML +=
            "<tr>" +
            "<td class='col-3 text-capitalize' hidden>" + GetDomElement("selLaundryItemCategory").value +
            "</td>" +
            "<td class='col-3 text-capitalize px-1'>" + GetDomElement("selLaundryItemCategory").options[GetDomElement("selLaundryItemCategory").selectedIndex].text +
            "</td>" +
            "<td class='col-3 text-capitalize' hidden>" + `${GetDomElement("chkManualCosting").checked?1:0}` +
            "</td>" +
            "<td class='col-3 text-capitalize'>" + `${GetDomElement("chkManualCosting").checked?"<i class='fas fa-check text-success'></i>":"<i class='fas fa-times-circle text-danger'></i>"}` +
            "</td>" +
            "<td class='col-3 text-capitalize'>" + GetDomElement("txtUnitCostWithADL").value +
            "</td>" +
            "<td class='col-3 text-capitalize'>" + GetDomElement("txtUnitCostWithoutADL").value +
            "</td>" +
            "<td class='col-3 text-capitalize'><i class='fas fa-trash text-danger deleteItem' style='cursor:pointer;'></i>" +
            "</td>" +
            "</tr>"
        ClearItems();

        var deleteItems = document.querySelectorAll(".deleteItem");
        for (var i = 0; i < deleteItems.length; i++) {
            deleteItems[i].addEventListener('click', function () {
                var deleteRow = this.parentNode.parentNode;
                deleteRow.remove();
            });
        }
    }

});


GetDomElement("btnSedondaryAddItems").addEventListener('click', function () {
    this.dataset.target = "";
    var oSecondaryItems = [];
    var oRequiredFields = document.querySelectorAll(".item-secondary-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divSecondaryInvalidForm");
    if (bValidForm) {
        oSecondaryItems = [{
            "id_item": parseInt(GetDomElement("selSecondaryLaundryItemCategory").value),
            "description": GetDomElement("selSecondaryLaundryItemCategory").options[GetDomElement("selSecondaryLaundryItemCategory").selectedIndex].text,
            "category": {
                "id_category": 0,
                "description": "string",
                "unit": "string"
            },
            "service": {
                "id_service": 0,
                "description": "string"
            },
            "manual_costing": parseInt(`${GetDomElement("chkSecondaryManualCosting").checked?1:0}`),
            "unit_cost_adl": parseFloat(GetDomElement("txtSecondaryUnitCostWithADL").value),
            "unit_cost": parseFloat(GetDomElement("txtSecondaryUnitCostWithoutADL").value)
        }]
        this.dataset.target = "#btnCustomConfirm1";
        GetDomElement("CustomMethod").innerText = "add";
        GetDomElement("CustomConfirmTwoMethod").innerText = "add";
        var strSecondaryItems = JSON.stringify(oSecondaryItems);
        GetDomElement("btnCustomConfirmTwo").onclick = () => {
            var oSubmitSecondaryItems = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${ReturnInputValue("secondaryCustomerId")}`, "POST", strSecondaryItems);
            if (oSubmitSecondaryItems.code == 200) {
                var oData = JSON.parse(oSubmitSecondaryItems.data);
                if (oData.code == 200) {
                    ShowCustomSuccessModal("btnCustomConfirmTwo", "added");
                    GetDomElement("btnCustomCloseConfimrTwo").click();
                    GetDomElement("btnSecondaryClosebtnItemAddModal").click();
                    PopulateUpdatedIndustrialProductList(ReturnInputValue("secondaryCustomerId"));
                } else {
                    alert("Server Error. Please check and try again");
                }
            } else {
                alert("Server Error. Please check and try again");
            }
        }

    }
});
GetDomElement("btnEditAddItems").addEventListener('click', function () {
    this.dataset.target = "";
    var oSecondaryItems = [];
    var oRequiredFields = document.querySelectorAll(".item-edit-required");
    var bValidForm = CheckValidForm(oRequiredFields, "divEditInvalidForm");
    if (bValidForm) {
        oSecondaryItems = {
            "id_item": parseInt(GetDomElement("selEditLaundryItemCategory").value),
            "description": GetDomElement("selEditLaundryItemCategory").options[GetDomElement("selEditLaundryItemCategory").selectedIndex].text,
            "category": {
                "id_category": 0,
                "description": "string",
                "unit": "string"
            },
            "service": {
                "id_service": 0,
                "description": "string"
            },
            "manual_costing": parseInt(`${GetDomElement("chkEditManualCosting").checked?1:0}`),
            "unit_cost_adl": parseFloat(GetDomElement("txtEditUnitCostWithADL").value),
            "unit_cost": parseFloat(GetDomElement("txtEditUnitCostWithoutADL").value)
        }
        this.dataset.target = "#btnCustomConfirm1";
        GetDomElement("CustomMethod").innerText = "update";
        GetDomElement("CustomConfirmTwoMethod").innerText = "update";
        var strSecondaryItems = JSON.stringify(oSecondaryItems);
        GetDomElement("btnCustomConfirmTwo").onclick = () => {
            var oSubmitSecondaryItems = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${ReturnInputValue("EditCustomerId")}`, "PUT", strSecondaryItems);
            if (oSubmitSecondaryItems.code == 200) {
                var oData = JSON.parse(oSubmitSecondaryItems.data);
                if (oData.code == 200) {
                    ShowCustomSuccessModal("btnCustomConfirmTwo", "updated");
                    GetDomElement("btnCustomCloseConfimrTwo").click();
                    GetDomElement("btnEditClosebtnItemAddModal").click();
                    PopulateUpdatedIndustrialProductList(ReturnInputValue("EditCustomerId"));
                } else {
                    alert("Server Error. Please check and try again");
                }
            } else {
                alert("Server Error. Please check and try again");
            }
        }

    }
});

GetDomElement("selLaundryItemCategory").addEventListener('change', function () {
    var isManualCosting = this.options[this.selectedIndex].getAttribute("data-isManual");
    var unit_cost = this.options[this.selectedIndex].getAttribute("data-without_adl");
    var unit_cost_adl = this.options[this.selectedIndex].getAttribute("data-adl_cost");

    PopulateCheboxInputField(isManualCosting, "chkManualCosting");

    if (isManualCosting == 1) {
        GetDomElement("txtUnitCostWithADL").value = 0;
        GetDomElement("txtUnitCostWithADL").disabled = true;
        GetDomElement("txtUnitCostWithoutADL").value = 0;
        GetDomElement("txtUnitCostWithoutADL").disabled = true;
    } else {
        GetDomElement("txtUnitCostWithADL").value = unit_cost_adl;
        GetDomElement("txtUnitCostWithoutADL").value = unit_cost;
        GetDomElement("txtUnitCostWithADL").disabled = false;
        GetDomElement("txtUnitCostWithoutADL").disabled = false;
    }
});
GetDomElement("selSecondaryLaundryItemCategory").addEventListener('change', function () {
    var isManualCosting = this.options[this.selectedIndex].getAttribute("data-isManual");
    var unit_cost = this.options[this.selectedIndex].getAttribute("data-without_adl");
    var unit_cost_adl = this.options[this.selectedIndex].getAttribute("data-adl_cost");

    PopulateCheboxInputField(isManualCosting, "chkSecondaryManualCosting");
    GetDomElement("txtSecondaryUnitCostWithADL").value = unit_cost_adl;
    GetDomElement("txtSecondaryUnitCostWithoutADL").value = unit_cost;
    if (isManualCosting == 1) {
        GetDomElement("txtSecondaryUnitCostWithADL").value = 0;
        GetDomElement("txtSecondaryUnitCostWithADL").disabled = true;
        GetDomElement("txtSecondaryUnitCostWithoutADL").value = 0;
        GetDomElement("txtSecondaryUnitCostWithoutADL").disabled = true;
    } else {
        GetDomElement("txtSecondaryUnitCostWithADL").value = unit_cost_adl;
        GetDomElement("txtSecondaryUnitCostWithoutADL").value = unit_cost;
        GetDomElement("txtSecondaryUnitCostWithADL").disabled = false;
        GetDomElement("txtSecondaryUnitCostWithoutADL").disabled = false;
    }
});

GetDomElement("chkIndustrial").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("divItems").classList.remove('d-none');
    } else {
        oItemPostList = [];
        GetDomElement("divItems").classList.add('d-none');
    }
});

/* CLEAR FORM DATA */
GetDomElement("btnNewEntry").addEventListener('click', function () {
    GetDomElement("txtClientName").value = "";
    GetDomElement("txtClientName").classList.remove('is-invalid');
    GetDomElement("txtCellularNo").value = "";
    GetDomElement("txtCellularNo").classList.remove('is-invalid');
    GetDomElement("txtEmail").value = "";
    GetDomElement("txtEmail").classList.remove('is-invalid');
    GetDomElement("txtEmail").value = "";
    GetDomElement("txtEmail").classList.remove('is-invalid');
    GetDomElement("txtADL").value = "";
    GetDomElement("txtADL").classList.remove('is-invalid');
    GetDomElement("chkIndustrial").checked = false;
    GetDomElement("chkNonIndustrial").checked = false;
    GetDomElement("chkActive").checked = true;
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divInvalidADL").classList.add('d-none');
    GetDomElement("divInvalidClientType").classList.add('d-none');
    GetDomElement("divInvalidPassword").classList.add('d-none');
    GetDomElement("address").innerHTML = ""
    GetDomElement("tblIndustrial").innerHTML = "";

});


function PopulateIndustrialProductList(sourceTable, emptyDiv) {
    var oProductList = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "GET", "");
    if (oProductList.code == 200) {
        var oData = JSON.parse(oProductList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            sourceTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                // sourceTable.innerHTML +=
                //     "<tr>" +
                //     "<td class='col-1 text-center p-1'><input type='checkbox' class='checkListIndustrial mt-2'>" +
                //     "</td>" +
                //     "<td class='col-3 text-capitalize' hidden>" + oJsonData[i].id_item +
                //     "</td>" +
                //     "<td class='col-3 text-capitalize'>" + oJsonData[i].description +
                //     "</td>" +
                //     "<td class='col-3 text-capitalize'>" + PopulateCheckboxTable(oJsonData[i].manual_costing) +
                //     "</td>" +
                //     "<td class='col-3 text-capitalize' hidden>" + oJsonData[i].manual_costing +
                //     "</td>" +
                //     "<td class='col-3 text-capitalize px-2'><input type='number' class='form-control-success cost-required form-control border' disabled value=" +
                //     oJsonData[i].unit_cost_adl +
                //     "></td>" +
                //     "<td class='col-3 text-capitalize px-2'><input type='number' class='form-control-success cost-required form-control border' disabled value=" +
                //     oJsonData[i].unit_cost +
                //     "></td>" +
                //     "<td class='col-1 px-2'>" +
                //     "</td>" +
                //     "</tr>"
            }

            var oCheckboxList = document.querySelectorAll(".checkListIndustrial");
            for (var i = 0; i < oCheckboxList.length; i++) {
                oCheckboxList[i].addEventListener('click', function () {
                    if (this.checked) {
                        this.parentNode.parentNode.cells[5].getElementsByTagName("input")[0].disabled = false;
                        this.parentNode.parentNode.cells[6].getElementsByTagName("input")[0].disabled = false;
                    } else {
                        this.parentNode.parentNode.cells[5].getElementsByTagName("input")[0].disabled = true;
                        this.parentNode.parentNode.cells[6].getElementsByTagName("input")[0].disabled = true;
                    }
                });

            }

        } else if (oData.code == 404 || oData.code == 204) {
            sourceTable.innerHTML = "";
            GetDomElement(emptyDiv).classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oProductList.code == 204 || oProductList.code == 404) {
        sourceTable.innerHTML = "";
        GetDomElement(emptyDiv).classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oProductList.message, oProductList.code);
    }
}

function PopulateUpdatedIndustrialProductList(customerId) {
    GetDomElement("emptyUpdatedLaundryItems").classList.add('d-none');
    var oProductList = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${customerId}`, "GET", "");
    if (oProductList.code == 200) {
        var oData = JSON.parse(oProductList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            updateIndustrialTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                updateIndustrialTable.innerHTML +=
                    "<tr>" +
                    "<td class='col-3 text-capitalize' hidden>" + oJsonData[i].id_item +
                    "</td>" +
                    "<td class='col-3 text-capitalize'>" + oJsonData[i].description +
                    "</td>" +
                    "<td class='col-3 text-capitalize'>" + PopulateCheckboxTable(oJsonData[i].manual_costing) +
                    "</td>" +
                    "<td class='col-3 text-capitalize' hidden>" + oJsonData[i].manual_costing +
                    "</td>" +
                    "<td class='col-3 text-capitalize px-2'>" +
                    oJsonData[i].unit_cost_adl +
                    "</td>" +
                    "<td class='col-3 text-capitalize px-2'>" +
                    oJsonData[i].unit_cost +
                    "</td>" +
                    "<td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger btnDelete d-flex align-items-center' data-toggle='modal' data-target='#btnCustomConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "<p class='text-primary btnUpdate d-flex align-items-center' data-toggle='modal' data-target ='#btnEditItemAddModal' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "</td>" +
                    "</tr>"
            }

            var oCheckboxList = document.querySelectorAll(".checkUpdateListIndustrial");
            for (var i = 0; i < oCheckboxList.length; i++) {
                oCheckboxList[i].addEventListener('click', function () {
                    if (this.checked) {
                        this.parentNode.parentNode.cells[5].getElementsByTagName("input")[0].disabled = false;
                        this.parentNode.parentNode.cells[6].getElementsByTagName("input")[0].disabled = false;
                    } else {
                        this.parentNode.parentNode.cells[5].getElementsByTagName("input")[0].disabled = true;
                        this.parentNode.parentNode.cells[6].getElementsByTagName("input")[0].disabled = true;
                    }
                });
            }

            var oDeleteItems = document.querySelectorAll(".btnDelete");
            for (var i = 0; i < oDeleteItems.length; i++) {
                oDeleteItems[i].addEventListener('click', function () {
                    var item_id = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("CustomMethod").innerText = "delete";
                    GetDomElement("CustomConfirmTwoMethod").innerText = "delete";
                    GetDomElement("btnCustomConfirmTwo").onclick = () => {
                        var oDeleteItemRecord = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${customerId}/${item_id}`, "DELETE", "");
                        if (oDeleteItemRecord.code == 200) {
                            var oData = JSON.parse(oDeleteItemRecord.data);
                            if (oData.code == 200) {
                                ShowCustomSuccessModal("btnCustomConfirmTwo", "deleted");
                                GetDomElement('btnCustomCloseConfimrTwo').click();
                                PopulateUpdatedIndustrialProductList(customerId);
                            } else {
                                ShowErrorModal("btnCustomConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnCustomConfirmTwo", oDeleteItemRecord.message, oDeleteItemRecord.code);
                        }
                    }
                })
            }

            var oUpdate = document.querySelectorAll(".btnUpdate");
            for (var i = 0; i < oUpdate.length; i++) {
                oUpdate[i].addEventListener('click', function () {
                    GetDomElement('EditCustomerId').value = customerId;
                    GetDomElement('selEditLaundryItemCategory').value = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("txtEditUnitCostWithADL").value = this.parentNode.parentNode.cells[4].innerText;
                    GetDomElement("txtEditUnitCostWithoutADL").value = this.parentNode.parentNode.cells[5].innerText;
                    if (parseInt(this.parentNode.parentNode.cells[3].innerText) == 0) {
                        GetDomElement("chkEditManualCosting").checked = false;
                        GetDomElement("txtEditUnitCostWithADL").disabled = false;
                        GetDomElement("txtEditUnitCostWithoutADL").disabled = false;
                    } else {
                        GetDomElement("chkEditManualCosting").checked = true;
                        GetDomElement("txtEditUnitCostWithADL").disabled = true;
                        GetDomElement("txtEditUnitCostWithoutADL").disabled = true;
                        GetDomElement("txtEditUnitCostWithADL").value = 0;
                        GetDomElement("txtEditUnitCostWithoutADL").value = 0;
                    }
                })
            }

        } else if (oData.code == 404 || oData.code == 204) {
            updateIndustrialTable.innerHTML = "";
            GetDomElement("emptyUpdatedLaundryItems").classList.remove('d-none');
        } else {
            alert("Server Error. Please check and try again");
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oProductList.code == 204 || oProductList.code == 404) {
        updateIndustrialTable.innerHTML = "";
        GetDomElement("emptyUpdatedLaundryItems").classList.remove('d-none');
    } else {
        alert("Server Error. Please check and try again");
        ShowErrorModalOnLoad(oProductList.message, oProductList.code);
    }
}

function MarkItemsChecked(customerID) {
    var updateItemList = document.querySelectorAll(".checkUpdateListIndustrial");
    var oLastCustomerItems = InvokeService(GetURL(), `IndustrialLaundryItemMethods/designated/${customerID}`, "GET", "");
    if (oLastCustomerItems.code == 200) {
        var ocustomerData = JSON.parse(oLastCustomerItems.data);
        if (ocustomerData.code == 200) {
            var jsonData = JSON.parse(ocustomerData.jsonData);
            for (var i = 0; i < updateItemList.length; i++) {
                for (var j = 0; j < jsonData.length; j++) {
                    if (jsonData[j].id_item == parseInt(updateItemList[i].parentNode.parentNode.cells[1].innerHTML)) {
                        updateItemList[i].click();
                    }
                }

            }
        }
    }

}

function CheckVaidItems(inputList) {
    var validFlag = 0;
    for (var i = 0; i < inputList.length; i++) {
        if (inputList[i].value != "") {
            validFlag++;
            inputList[i].removeAttribute("style");
        } else {
            inputList[i].setAttribute("style", "border:1px solid red !important;");
        }
    }
    if (validFlag == inputList.length) {
        GetDomElement("divAddNewInvalidForm").classList.add('d-none');
        return true;
    } else {
        GetDomElement("divAddNewInvalidForm").classList.remove('d-none');
        return false;
    }
}

/* CLEAR FORM ON UPDATE DATA */
function ClearClientUpdate() {
    GetDomElement("txtEditClientName").value = "";
    GetDomElement("txtEditClientName").classList.remove('is-invalid');
    GetDomElement("txtEditCellularNo").value = "";
    GetDomElement("txtEditCellularNo").classList.remove('is-invalid');
    GetDomElement("txtEditEmail").value = "";
    GetDomElement("txtEditEmail").classList.remove('is-invalid');
    GetDomElement("txtEditADL").value = "";
    GetDomElement("txtEditADL").classList.remove('is-invalid');
    GetDomElement("chkIndustrial").checked = false;
    GetDomElement("chkEditNonIndustrial").checked = false;
    GetDomElement("chkEditActive").checked = true;
    GetDomElement("divUpdateInvalidForm").classList.add('d-none');
    GetDomElement("divUpdateInvalidADL").classList.add('d-none');
    GetDomElement("divUpdateInvalidClientType").classList.add('d-none');
    GetDomElement("Editaddress").innerHTML = ""

}
GetDomElement("btnSearchCustomer").addEventListener("click", function () {
    GetDomElement("btnCustomerList").innerHTML = "";
    GetDomElement("txtSearchClient").value = "";
    GetDomElement("selClientSearchQuery").selectedIndex = 0;
});

GetDomElement("btnSearch").addEventListener('click', function () {
    PopulateClientList();
});

GetDomElement("txtSearch").addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        if (this.value != "") {
            PopulateClientList();
        }
    }
    if (this.value == "") {
        CIFTable.innerHTML = "";
    }
});


GetDomElement("btnSearchClient").addEventListener('click', () => {
    PopulateCISList();
})

GetDomElement("txtSearchClient").addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        if (this.value != "") {
            PopulateCISList();
        }
    }
});

GetDomElement('selClientSearchQuery').addEventListener('change', () => {
    CIFTable.innerHTML = "";
    GetDomElement("txtSearchClient").value = "";
});

GetDomElement("chkEditIndustrial").addEventListener('click', function () {
    if (this.checked) {
        GetDomElement("divUpdateItems").classList.remove("d-none");
    }
});


function PopulateClientList() {
    let queryparameter;
    let searchbox = GetDomElement("txtSearch");
    var searchParam = GetDomElement("selSearchParameter");
    var emptyTable = GetDomElement("emptyTable");
    if (searchParam.value == 'c_name') {
        queryparameter = "";
    } else if (searchParam.value == 'c_num' || searchParam.value == 'c_em') {
        queryparameter = "contact/"
    } else {
        queryparameter = ""
    }
    var oClientList = InvokeService(GetURL(), `CustomerMethods/search/customer/${queryparameter}${searchbox.value}`, "GET", "");
    if (oClientList.code == 200) {
        emptyTable.classList.add('d-none');
        var oData = JSON.parse(oClientList.data);
        if (oData.code == 200) {
            emptyTable.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            clientTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                clientTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].customer_id +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    "</td><td>" + oJsonData[i].cellular_number +
                    "</td><td>" + oJsonData[i].email_address +
                    "</td><td class='col-12 col-md-4 px-4' >" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    "<span class='pr-4'>Industrial</span>" + PopulateCheckboxTable(oJsonData[i].industrial) +
                    "</p>" +
                    "<p class='d-flex justify-content-between text-left align-items-center py-1'>" +
                    "<span class='pr-4'>Non Industrial</span>" + PopulateCheckboxTable(oJsonData[i].non_industrial) +
                    "</p>" +
                    "</td><td class='text-capitalize'>" +
                    "<button class='btn btn-danger resetUser' data-toggle='modal'>Reset</button>" +
                    "</td><td class='text-capitalize text-center'>" + CheckCustomerStatus(oJsonData[i].active_customer, oJsonData[i].customer_id) +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger btnDelete d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "<p class='text-primary update d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdate' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'></span></p>" +
                    "</td>" +
                    "</tr>";
            }

            var resetUser = document.querySelectorAll('.resetUser');
            for (var i = 0; i < resetUser.length; i++) {
                resetUser[i].addEventListener('click', function () {
                    var userId = this.parentNode.parentNode.cells[0].innerText;
                    this.dataset.target = "#btnConfirm1";
                    GetDomElement("method").innerHTML = "reset password <span class='text-secondary'>on selected</span>";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerHTML = "reset password <span class='text-secondary'>on selected</span>";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        var updateUserPassword = InvokeService(GetURL(), `CustomerMethods/reset/customer/id/${userId}`, "PUT", "");
                        if (updateUserPassword.code == 200) {
                            var oData = JSON.parse(updateUserPassword.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "updated");
                                // ShowCustomSuccess("btnConfirmTwo", oData.message);
                                GetDomElement("prefix").innerText = "password";
                                GetDomElement('btnCloseConfimrTwo').click();
                                GetDomElement('btnClosePassword').click();
                                PopulateClientList();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", updateUserPassword.message, updateUserPassword.code);
                        }
                    }

                });
            }
            var btnDelete = document.querySelectorAll('.btnDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var customerCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = () => {
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        // var deleteItems = InvokeService(GetURL(), `CustomerMethods/delete/customer/id/${customerCode}`, "DELETE", "");
                        var deleteCustomer = InvokeService(GetURL(), `CustomerMethods/delete/customer/id/${customerCode}`, "DELETE", "");
                        if (deleteCustomer.code == 200) {
                            var oData = JSON.parse(deleteCustomer.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateClientList();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteCustomer.message, deleteCustomer.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.update');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearClientUpdate();
                    var customerId = this.parentNode.parentNode.cells[0].innerText;
                    PopulateUpdatedIndustrialProductList(customerId);
                    // MarkItemsChecked(customerId);
                    var userData = InvokeService(GetURL(), `CustomerMethods/search/customer/id/${customerId}`, "GET", "");
                    if (userData.code == 200) {
                        var oData = JSON.parse(userData.data);
                        if (oData.code == 200) {
                            var jsonData = JSON.parse(oData.jsonData);
                            GetDomElement("secondaryCustomerId").value = customerId;
                            GetDomElement("txtEditClientName").value = jsonData[0].customer_name;
                            GetDomElement("editCustomerID").value = jsonData[0].customer_id;
                            GetDomElement("editCustomerSourceId").value = jsonData[0].source_id;
                            GetDomElement("txtEditCellularNo").value = jsonData[0].cellular_number;
                            GetDomElement("txtEditEmail").value = jsonData[0].email_address;
                            var fullAddress = `
                            <span class='text-capitalize' id='edit_ad_street'>${jsonData[0].street_building_address}</span>, <br> 
                            <span class='text-capitalize' id='edit_ad_city'>${jsonData[0].town_address}</span>,<br> 
                            <span class='text-capitalize' id='edit_ad_prov'>  ${jsonData[0].province} </span>
                            `;
                            GetDomElement("Editaddress").innerHTML = fullAddress;
                            if (jsonData[0].industrial == 1) {
                                GetDomElement("chkEditIndustrial").checked = true;
                                GetDomElement("divUpdateItems").classList.remove("d-none");
                            } else {
                                GetDomElement("chkEditIndustrial").checked = false;
                                GetDomElement("divUpdateItems").classList.add("d-none");
                            }
                            if (jsonData[0].non_industrial == 1) {
                                GetDomElement("chkEditNonIndustrial").checked = true;
                            } else {
                                GetDomElement("chkEditNonIndustrial").checked = false;
                            }

                            GetDomElement("txtEditADL").value = jsonData[0].average_daily_load;


                            if (jsonData[0].active_customer == 1) {
                                GetDomElement("chkEditActive").click();
                            } else if (jsonData[0].active_customer == 0) {
                                GetDomElement("chkEditBlocked").click();
                            }

                        } else if (oData.code == 404 || oData.code == 204) {} else {
                            ShowErrorModalOnLoad(oData.message, oData.code);
                        }
                    } else if (userData.code == 204 || userData.code == 404) {} else {
                        ShowErrorModalOnLoad(userData.message, userData.code);
                    }

                });
            }

        } else if (oData.code == 404 || oData.code == 204) {
            clientTable.innerHTML = "";
            emptyTable.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oClientList.code == 204 || oClientList.code == 404) {
        clientTable.innerHTML = "";
        emptyTable.classList.remove('d-none');

    } else {
        emptyTable.classList.add('d-none');
        ShowErrorModalOnLoad(oClientList.message, oClientList.code);
    }
}



function ClearResetPasswordForm() {
    GetDomElement("txtChangePassword1").value = "";
    GetDomElement("txtChangePassword1").classList.remove('is-invalid');
    GetDomElement("txtChangePassword2").value = "";
    GetDomElement("txtChangePassword2").classList.remove('is-invalid');
    GetDomElement("divInvalidPasswordForm").classList.add('d-none');
    GetDomElement("divChangePasswordNotMatch").classList.add('d-none');

}

function PopulateCISList() {
    var searchParam;
    var searchDropDown = GetDomElement("selClientSearchQuery");
    if (searchDropDown.value == 'c_name') {
        searchParam = "name";
    } else if (searchDropDown.value == 'c_num') {
        searchParam = "contactnumber";
    } else {
        searchParam = "email";
    }
    var clientSearchText = GetDomElement("txtSearchClient");
    var oCustomers = InvokeService(GetURL(), `CIFMethods/customers/${searchParam}/${clientSearchText.value}`, "GET", "");
    if (oCustomers.code == 200) {
        emptyCIF.classList.add('d-none');
        var oData = JSON.parse(oCustomers.data);
        if (oData.code == 200) {
            emptyCIF.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            CIFTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                CIFTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].cust_id +
                    "</td><td class='text-capitalize'>" + oJsonData[i].cust_name +
                    "</td><td>" + oJsonData[i].mobile_number +
                    "</td><td>" + oJsonData[i].email +
                    "</td><td hidden>" + `{"address_city":"${oJsonData[i].address_city}",
                                            "address_street":"${oJsonData[i].address_street}",
                                            "province":"${oJsonData[i].province}",
                                            "zip":"${oJsonData[i].zip}"}` +
                    "</td></tr>";
            }
            var cifRows = CIFTable.rows;
            for (var i = 0; i < cifRows.length; i++) {
                cifRows[i].addEventListener('click', function () {
                    GetDomElement("customerID").value = this.cells[0].innerHTML;
                    GetDomElement("txtClientName").value = this.cells[1].innerHTML;
                    GetDomElement("txtCellularNo").value = this.cells[2].innerHTML;
                    GetDomElement("txtEmail").value = this.cells[3].innerHTML;
                    var clientRecords = JSON.parse(this.cells[4].innerText);
                    var {
                        address_street,
                        address_city,
                        province,
                        zip
                    } = clientRecords;
                    var fullAddress = `
                    <span class='text-capitalize' id='ad_street'>${address_street}</span>, <br> 
                    <span class='text-capitalize' id='ad_city'>${address_city}</span>,<br> 
                    <span class='text-capitalize' id='ad_prov'>  ${province} </span><span id='ad_zip'>${zip}</span>
                    `;
                    GetDomElement("address").innerHTML = fullAddress;
                    GetDomElement("btnCloseClientList").click();


                });
            }


        } else if (oData.code == 404 || oData.code == 204) {
            CIFTable.innerHTML = "";
            emptyCIF.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oCustomers.code == 204 || oCustomers.code == 404) {
        CIFTable.innerHTML = "";
        emptyCIF.classList.remove('d-none');

    } else {
        emptyCIF.classList.add('d-none');
        ShowErrorModalOnLoad(oCustomers.message, oCustomers.code);
    }

}


function CheckCustomerStatus(statusValue, userId) {
    if (statusValue == 1) {
        return "<button type='button' class='btn btn-outline-success' data-toggle='modal' data-target='#btnConfirm1' onclick = ChangeCustomerStatus(" + `${statusValue}` + "," + `"${userId}"` + ")" +
            ">Active" +
            "</button>"
    } else {
        return "<button type='button' class='btn btn-outline-secondary' data-toggle='modal' data-target='#btnConfirm1' onclick = ChangeCustomerStatus(" + `${statusValue}` + "," + `"${userId}"` + ")" +
            ">Blocked" +
            "</button>"
    }
}

function ChangeCustomerStatus(status, userId) {
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

    GetDomElement("btnConfirmTwo").onclick = () => {
        var updateCustomer = InvokeService(GetURL(), `CustomerMethods/changestatus/customer/id/${userId}?isActive=${changeStatus}`, "PUT", "");
        if (updateCustomer.code == 200) {
            var oData = JSON.parse(updateCustomer.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", successMessage);
                GetDomElement('btnCloseConfimrTwo').click();
                PopulateClientList();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", updateCustomer.message, updateCustomer.code);
        }

    }
}


function CustomPopulateCategoryListingDropDown(selectField) {
    var selCategory = document.getElementById(selectField);
    selCategory.innerHTML = "";

    var jsonData = InvokeService(GetURL(), "IndustrialLaundryItemMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var defaultOption = document.createElement('option');
        defaultOption.innerHTML = "Select a category";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selCategory.innerHTML += "<option value=\"" + oData[i].id_item + "\" data-isManual=\"" + oData[i].manual_costing + "\" data-adl_cost=\"" + oData[i].unit_cost_adl + "\" data-without_adl=\"" + oData[i].unit_cost + "\">" + capitalize(oData[i].description) + "</option>";
        }


    } else if (parseJsonData.code == 404 || parseJsonData.code == 204) {

    } else {
        // ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");

ClosePopupModal("btnCustomCancelOne", "btnCloseCustomConfirm1");
ClosePopupModal("btnCustomConfirmSuccessOne", "btnCloseCustomConfirm1");
ClosePopupModal("btnCustomCancelTwo", "btnCustomCloseConfimrTwo");
ClosePopupModal("btnCustomConfirmTwo", "btnCustomCloseConfimrTwo");
ClosePopupModal("btnCustomCloseSuccessEntry", "btnCustomCloseSuccessAlert");