var userAccessTable = GetDomElement('tblBody');
const excludedId = GetDomElement("excludedList");
const includedId = GetDomElement("includedList");
const excludedEditId = GetDomElement("excludedEditList");
const includedEditId = GetDomElement("includedEditList");
var chkEditLaundry = GetDomElement('chkEditLaundry');
var chkEditLogistics = GetDomElement("chkEditLogistics");
var section = 0;
var editSection = 0;
var oLogisticsFunctions = [];
var oLaundryFunctions = [];
var oAdminFunctions = [];



window.addEventListener('load', function () {
    GetDomElement('chkAdmin').checked = true;
    PopulateUserAccessManagement();
    GetLogisticsFunctions();
    GetLaundryFunctions();
    GetAdminFunctions();
    PopulatePredefinedFunctions(oAdminFunctions, "excludedList");
});

GetDomElement('chkLogistics').addEventListener('click', function () {
    if (this.checked) {
        // includedId.innerHTML = "";
        // excludedId.innerHTML = "";
        // PopulatePredefinedFunctions(oLogisticsFunctions, "excludedList");
        section = 1;
    }
});
GetDomElement('chkAdmin').addEventListener('click', function () {
    if (this.checked) {
        section = 0;
    }
});
GetDomElement('chkLaundry').addEventListener('click', function () {
    if (this.checked) {
        // includedId.innerHTML = "";
        // excludedId.innerHTML = "";
        // PopulatePredefinedFunctions(oLaundryFunctions, "excludedList");

        section = 2;
    }
});

GetDomElement('chkEditLogistics').addEventListener('click', function () {
    if (this.checked) {
        // includedEditId.innerHTML = "";
        // excludedEditId.innerHTML = "";
        // PopulatePredefinedFunctions(oLogisticsFunctions, "excludedEditList");
        editSection = 1;
    }
});
GetDomElement('chkEditAdmin').addEventListener('click', function () {
    if (this.checked) {
        editSection = 0;
    }
});
GetDomElement('chkEditLaundry').addEventListener('click', function () {
    if (this.checked) {
        // includedEditId.innerHTML = "";
        // excludedEditId.innerHTML = "";
        // PopulatePredefinedFunctions(oLaundryFunctions, "excludedEditList");

        editSection = 2;
    }
});

GetDomElement("btnSubmitNewUserTemplate").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divAddNewInvalidForm");
    var templateCode = GetDomElement("txtTemplateCode").value;
    if (bValidForm) {
        this.dataset.target = "";
        if (templateCode.length <= 10) {
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");

            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oNewAccessTemplate = {
                    "template_code": templateCode,
                    "description": ReturnInputValue("txtTemplateDescription"),
                    "section": {
                        "idsection": section,
                        "description": "Logistics"
                    }
                }
                var strPaymentMethod = JSON.stringify(oNewAccessTemplate);
                var oSubmitNewAccessMethod = InvokeService(GetURL(), "AccessMenuTemplateMethods", "POST", strPaymentMethod);
                if (oSubmitNewAccessMethod.code == 200) {
                    var includedValues = geAllValues(includedId);
                    var predefinedList = [];
                    for (var i = 0; i < includedValues.length; i++) {
                        predefinedList.push({
                            "oTemplate": {
                                "template_code": templateCode,
                                "description": "string",
                                "section": {
                                    "idsection": section,
                                    "description": "string"
                                }
                            },
                            "function_code": includedValues[i].value
                        });
                    }


                    var strPredefinedList = JSON.stringify(predefinedList);
                    var submitPredefinedList = InvokeService(GetURL(), `AccessMenuTemplateMethods/details/${templateCode}`, "POST", strPredefinedList)
                    if (submitPredefinedList.code == 200) {
                        var oJsonData = JSON.parse(submitPredefinedList.data)

                        if (oJsonData.code == 200) {

                        } else {
                            ShowErrorModal("btnConfirmTwo", oJsonData.message, oJsonData.code);
                        }
                    } else {
                        ShowErrorModal("btnConfirmTwo", submitPredefinedList.message, submitPredefinedList.code);
                    }
                    var oData = JSON.parse(oSubmitNewAccessMethod.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewModel').click();
                        PopulateUserAccessManagement();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitNewAccessMethod.message, oSubmitNewAccessMethod.code);
                }
            }

            /* POST HERE */
        } else {
            GetDomElement('divAddTemplateCodeTooLongErrorAlert').classList.remove('d-none');
            GetDomElement("txtTemplateCode").classList.add('is-invalid');
        }

    } else {
        GetDomElement('divAddTemplateCodeTooLongErrorAlert').classList.add('d-none');
    }
});

/* SUBMIT UPDATED USER TEMPLATE */
GetDomElement("btnSubmitUpdatedUserTemplate").addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFields = document.querySelectorAll('.input-update-required');
    var bValidForm = CheckValidForm(oRequiredFields, "divUpdateInvalidForm");
    var templateCode = GetDomElement("txtEditTemplateCode").value;
    if (bValidForm) {
        this.dataset.target = "";
        if (templateCode.length <= 10) {
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "update";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "update";
            GetDomElement("confirmTwoMethod").classList.add("text-success");
            if (chkEditLogistics.checked == false && chkEditLaundry.checked == false) {
                editSection = 0;
            }else if(chkEditLogistics.checked){
                editSection = 1;
            }else if(chkEditLaundry.checked){
                editSection = 2;
            }
            GetDomElement("btnConfirmTwo").onclick = ()=>{
                var oUpdatedAccessTemplate = {
                    
                    "template_code": templateCode,
                    "description": ReturnInputValue("txtEditTemplateDescription"),
                    "section": {
                        "idsection": editSection,
                        "description": ""
                    }
                }
                var strUpdatedAccessTemplate = JSON.stringify(oUpdatedAccessTemplate);
                var oSubmitUpdatedAccessMethod = InvokeService(GetURL(), `AccessMenuTemplateMethods/${templateCode}`, "PUT", strUpdatedAccessTemplate);
                if (oSubmitUpdatedAccessMethod.code == 200) {
                    var includedValues = geAllValues(includedEditId);
                    var predefinedList = [];
                    for (var i = 0; i < includedValues.length; i++) {
                        predefinedList.push({
                            "oTemplate": {
                                "template_code": templateCode,
                                "description": "string",
                                "section": {
                                    "idsection": editSection,
                                    "description": "string"
                                }
                            },
                            "function_code": includedValues[i].value
                        });
                    }

                    var strPredefinedList = JSON.stringify(predefinedList);
                    var submitPredefinedList = InvokeService(GetURL(), `AccessMenuTemplateMethods/details/${templateCode}`, "POST", strPredefinedList);
                    if (submitPredefinedList.code == 200) {
                        var oJsonData = JSON.parse(submitPredefinedList.data)

                        if (oJsonData.code == 200) {

                        } else {
                            ShowErrorModal("btnConfirmTwo", oJsonData.message, oJsonData.code);
                        }
                    } else {
                        ShowErrorModal("btnConfirmTwo", submitPredefinedList.message, submitPredefinedList.code);
                    }
                    var oData = JSON.parse(oSubmitUpdatedAccessMethod.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "updated");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseUpdateModel').click();
                        PopulateUserAccessManagement();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", oSubmitUpdatedAccessMethod.message, oSubmitUpdatedAccessMethod.code);
                }
            }

            /* POST HERE */
        } else {
            GetDomElement('divUpdateTemplateCodeTooLongErrorAlert').classList.remove('d-none');
            GetDomElement("txtEditTemplateCode").classList.add('is-invalid');
        }

    } else {
        GetDomElement('divUpdateTemplateCodeTooLongErrorAlert').classList.add('d-none');
    }
});

/* Include-Exclude Functions */
GetDomElement('includeMultiple').addEventListener('click', function () {
    var includedValues = getSelectValues(excludedId);
    PopulateIncludedFunctions(includedValues, "includedList", "excludedList");
});

GetDomElement('includeAll').addEventListener('click', function () {
    var includedValues = geAllValues(excludedId);
    PopulateIncludedFunctions(includedValues, "includedList", "excludedList");
});

GetDomElement('excludeMultiple').addEventListener('click', function () {
    var includedValues = getSelectValues(includedId);
    PopulateIncludedFunctions(includedValues, "excludedList", "includedList");
});

GetDomElement('excludeAll').addEventListener('click', function () {
    var includedValues = geAllValues(includedId);
    PopulateIncludedFunctions(includedValues, "excludedList", "includedList");
});

GetDomElement('includeEditMultiple').addEventListener('click', function () {
    var includedValues = getSelectValues(excludedEditId);
    PopulateIncludedFunctions(includedValues, "includedEditList", "excludedEditList");
});

GetDomElement('includeEditAll').addEventListener('click', function () {
    var includedValues = geAllValues(excludedEditId);
    PopulateIncludedFunctions(includedValues, "includedEditList", "excludedEditList");
});

GetDomElement('excludeEditMultiple').addEventListener('click', function () {
    var includedValues = getSelectValues(includedEditId);
    PopulateIncludedFunctions(includedValues, "excludedEditList", "includedEditList");
});

GetDomElement('excludeEditAll').addEventListener('click', function () {
    var includedValues = geAllValues(includedEditId);
    PopulateIncludedFunctions(includedValues, "excludedEditList", "includedEditList");
});




function getSelectValues(select) {
    var result = [];
    var options = select && select.options;
    var opt;

    for (var i = 0, iLen = options.length; i < iLen; i++) {
        opt = options[i];

        if (opt.selected) {
            result.push({
                "value": opt.value,
                "description": opt.text
            });
        }
    }
    return result;
}

function geAllValues(select) {
    var result = [];
    var selectOptions = select.options;

    for (var i = 0, iLen = selectOptions.length; i < iLen; i++) {
        result.push({
            "value": selectOptions[i].value,
            "description": selectOptions[i].text
        });
    }

    return result;
}


function PopulateIncludedFunctions(includedFunctions, selectField, removeField) {

    var selFunctionList = document.getElementById(selectField);
    var removeSelectField = document.getElementById(removeField).options;

    for (var i = 0; i < includedFunctions.length; i++) {
        selFunctionList.innerHTML += "<option value=\"" + includedFunctions[i].value + "\">" + capitalize(includedFunctions[i].description) + "</option>";
        for (var j = 0; j < removeSelectField.length; j++) {
            if (includedFunctions[i].value == removeSelectField[j].value) {
                removeSelectField[j].parentNode.removeChild(removeSelectField[j]);
            }
        }
    }



}

/* CLEAR FORM */
GetDomElement("btnNewUserAccess").addEventListener('click', function () {
    GetDomElement("txtTemplateCode").value = "";
    GetDomElement("txtTemplateCode").classList.remove('is-invalid');
    GetDomElement("txtTemplateDescription").value = "";
    GetDomElement("txtTemplateDescription").classList.remove('is-invalid');
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divAddTemplateCodeTooLongErrorAlert").classList.add('d-none');
    includedId.innerHTML = "";
    GetDomElement("chkLogistics").checked = false;
    GetDomElement("chkLaundry").checked = false;
    PopulatePredefinedFunctions(oAdminFunctions, "excludedList");
});
/* CLEAR UPDATE FORM */
function ClearUpdateAccessTemplate() {
    GetDomElement("txtEditTemplateCode").value = "";
    GetDomElement("txtEditTemplateCode").classList.remove('is-invalid');
    GetDomElement("txtEditTemplateDescription").value = "";
    GetDomElement("txtEditTemplateDescription").classList.remove('is-invalid');
    GetDomElement("chkEditLogistics").checked = true;
    GetDomElement("divUpdateInvalidForm").classList.add('d-none');
    GetDomElement("divUpdateTemplateCodeTooLongErrorAlert").classList.add('d-none');
    includedEditId.innerHTML = "";
    excludedEditId.innerHTML = "";
    GetDomElement("chkEditLogistics").checked = false;
    GetDomElement("chkEditLaundry").checked = false;
}

function PopulateUserAccessManagement() {
    GetDomElement("emptyTable").classList.add('d-none');
    var oUserAccessList = InvokeService(GetURL(), "AccessMenuTemplateMethods", "GET", "");
    if (oUserAccessList.code == 200) {
        var oData = JSON.parse(oUserAccessList.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            userAccessTable.innerHTML = "";

            for (var i = 0; i < oJsonData.length; i++) {
                userAccessTable.innerHTML +=
                    "<tr class='border' ><td hidden>" + oJsonData[i].template_code +
                    "</td><td class='text-uppercase'>" + oJsonData[i].template_code +
                    "</td><td class='text-capitalize'>" + oJsonData[i].description +
                    "</td><td class='text-capitalize'>" + oJsonData[i].section.description +
                    "</td><td class='d-flex justify-content-around px-0'>" +
                    "<p class='text-danger btnDelete d-flex align-items-center' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'>" +
                    "<i  class='fas fa-trash mr-1'></i><span class='d-none d-md-block'>Delete</span></p>" +
                    "<p class='text-primary update d-flex align-items-center' data-toggle='modal' data-target ='#btnUpdateAccess' style='cursor:pointer'>" +
                    "<i  class='fas fa-edit mr-1'></i><span class='d-none d-md-block'>Update</span></p>" +
                    "</td>" +
                    "</tr>";
            }
            var btnDelete = document.querySelectorAll('.btnDelete');
            for (var i = 0; i < btnDelete.length; i++) {
                btnDelete[i].addEventListener('click', function () {
                    GetDomElement("btnConfirmTwo").dataset.target = "";
                    var accessCode = this.parentNode.parentNode.cells[0].innerText;
                    GetDomElement("method").innerText = "delete";
                    GetDomElement("method").classList.add("text-danger");
                    GetDomElement("confirmTwoMethod").innerText = "delete";
                    GetDomElement("confirmTwoMethod").classList.add("text-danger");

                    GetDomElement("btnConfirmTwo").onclick = ()=>{
                        GetDomElement("btnConfirmTwo").dataset.target = "";
                        var deleteAccessTemplate = InvokeService(GetURL(), `AccessMenuTemplateMethods/${accessCode}`, "DELETE", "");
                        if (deleteAccessTemplate.code == 200) {
                            var oData = JSON.parse(deleteAccessTemplate.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "deleted");
                                GetDomElement('btnCloseConfimrTwo').click();
                                PopulateUserAccessManagement();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", deleteAccessTemplate.message, deleteAccessTemplate.code);
                        }
                    }
                });
            }

            var btnUpdate = document.querySelectorAll('.update');
            for (var i = 0; i < btnUpdate.length; i++) {
                btnUpdate[i].addEventListener('click', function () {
                    ClearUpdateAccessTemplate();
                    var accessCode = this.parentNode.parentNode.cells[0].innerText;
                    var accessDescription = this.parentNode.parentNode.cells[2].innerText;
                    var userAccessDetails = InvokeService(GetURL(), `AccessMenuTemplateMethods/${accessCode}`, "GET", "");
                    if (userAccessDetails.code == 200) {
                        var oData = JSON.parse(userAccessDetails.data);
                        if (oData.code == 200) {
                            var oAccessData = JSON.parse(oData.jsonData);
                            PopulateIncludedPreselectedFunctions(oAccessData, "includedEditList");
                            PopulateExcludedPreselectedFunctions(oAccessData, "excludedEditList");
                            GetDomElement("btnConfirmTwo").dataset.target = "#";
                            if (oAccessData[0].section.idsection == 1) {
                                GetDomElement("chkEditLogistics").checked = true;
                            } else if (oAccessData[0].section.idsection == 2) {
                                GetDomElement("chkEditLaundry").checked = true;
                            }else if(oAccessData[0].section.idsection == 0){
                                GetDomElement("chkEditAdmin").checked = true;
                            }
                            GetDomElement("txtEditTemplateCode").value = accessCode;
                            GetDomElement("txtEditTemplateDescription").value = accessDescription;
                        } else {
                            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        }

                    } else {
                        ShowErrorModal("btnConfirmTwo", userAccessDetails.message, userAccessDetails.code);
                    }

                });
            }

        } else if (oData.code == 204 || oData.code == 404) {
            userAccessTable.innerHTML = "";
            GetDomElement("emptyTable").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oUserAccessList.code == 204 || oUserAccessList.code == 404) {
        userAccessTable.innerHTML = "";
        GetDomElement("emptyTable").classList.remove('d-none');
    } else {
        ShowErrorModalOnLoad(oUserAccessList.message, oUserAccessList.code);
    }
}


function PopulatePredefinedFunctions(functionList, selectField) {
        var oData =functionList
        var selFunctionList = document.getElementById(selectField);
        selFunctionList.innerHTML = "";
        for (var i = 0; i < oData.length; i++) {
            selFunctionList.innerHTML += "<option value=\"" + oData[i].function_code + "\">" + ReformatDescription(oData[i].section.description,oData[i].menu_group, oData[i].description) + "</option>";
        }
}

function ReformatDescription(section,menu_group, description){
    var templateSection = section != ""? section:"";
    var templateMenuGroup = menu_group !=""?`/${menu_group}`:"";
    var description = description !=""?`/${description}`:"";
    
    return `${templateSection}${templateMenuGroup}${description}`;
}


function PopulateIncludedPreselectedFunctions(functionList, selectField) {
    var selFunctionList = document.getElementById(selectField);
    selFunctionList.innerHTML = "";

    for (var i = 0; i < functionList.length; i++) {

        selFunctionList.innerHTML += "<option value=\"" + functionList[i].function_code + "\">" + ReformatDescription(functionList[i].section.description,functionList[i].menu_group, functionList[i].description) + "</option>";

    }

}

function PopulateExcludedPreselectedFunctions(functionList, selectField) {
    var excludeList = [];
    var matchFlag;
    var filteredData = [];
    excludeList = oAdminFunctions;

    var selFunctionList = document.getElementById(selectField);
    selFunctionList.innerHTML = "";
    for (var i = 0; i < excludeList.length; i++) {
        matchFlag = 0;
        for (var j = 0; j < functionList.length; j++) {
            if (functionList[j].function_code == excludeList[i].function_code) {
                // parsedExcludeList.splice(i,1);
                matchFlag = 1;
            }
        }
        if (matchFlag == 0) {
            filteredData.push(excludeList[i]);
        }
    }
    for (var i = 0; i < filteredData.length; i++) {

        selFunctionList.innerHTML += "<option value=\"" + filteredData[i].function_code + "\">" + ReformatDescription(filteredData[i].section.description,filteredData[i].menu_group, filteredData[i].description) + "</option>";

    }

}



function GetAdminFunctions() {
    var oRawAdminFunctions = InvokeService(GetURL(), "PredefinedFunctionMethods/0", "GET", "");
    if(oRawAdminFunctions.code == 200){
        var oJsonData = JSON.parse(oRawAdminFunctions.data);
        if(oJsonData.code == 200){
            oData = JSON.parse(oJsonData.jsonData);
            for(i=0;i<oData.length;i++){
                oAdminFunctions.push(oData[i]);
            }
        }else{
            ShowErrorModalOnLoad(oJsonData.message, oJsonData.code);
        }
    }else{
        ShowErrorModalOnLoad(oAdminFunctions.message, oAdminFunctions.code);
    }
}

function GetLogisticsFunctions() {
    oLogisticsFunctions = InvokeService(GetURL(), "PredefinedFunctionMethods/1", "GET", "");
    if(oLogisticsFunctions.code == 200){
        var oJsonData = JSON.parse(oLogisticsFunctions.data);
        if(oJsonData.code == 200){
            var oData = JSON.parse(oJsonData.jsonData);
            for(var i=0;i<oData.length;i++){
                oAdminFunctions.push(oData[i]);
            }
        }else{
            ShowErrorModalOnLoad(oJsonData.message, oJsonData.code);
        }
      
    }else{
        ShowErrorModalOnLoad(oLogisticsFunctions.message, oLogisticsFunctions.code);
    }
    
}

function GetLaundryFunctions() {
    oLaundryFunctions = InvokeService(GetURL(), "PredefinedFunctionMethods/2", "GET", "");
    if(oLaundryFunctions.code == 200){
        var oJsonData = JSON.parse(oLaundryFunctions.data);
        if(oJsonData.code == 200){
            var oData = JSON.parse(oJsonData.jsonData);
            for(var i=0;i<oData.length;i++){
                oAdminFunctions.push(oData[i]);
            }
        }else{
            ShowErrorModalOnLoad(oJsonData.message, oJsonData.code);
        }
    }else{
        ShowErrorModalOnLoad(oLaundryFunctions.message, oLaundryFunctions.code);
    }
}

ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");