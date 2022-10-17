document.writeln("<script src='js/download.min.js'></script>");
document.writeln("<script src='js/download.js'></script >");

var dupPaymentMethods = [];
function GetURL() {
    //return window.location.origin + "/washaloadservice/";
    return "https://localhost:44328/washaloadservice/";

}

function GetAuthKey() {
    return sessionStorage.getItem("authkey");
}

function InvokeService(serviceURL, controller, paramMethod, paramBody) {
    var url = serviceURL + controller;
    var serviceResponse;
    var xhttp = new XMLHttpRequest();
    try {
        serviceResponse = new ServiceResponse(0, "", null);
        xhttp.open(paramMethod, url, false);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.setRequestHeader("UserKey", GetAuthKey());
        xhttp.send(paramBody);
        xhttp.onreadystatechange = function () {
            if (xhttp.readyState == 4) {}
        };

    } catch (e) {
        console.log('catch', e);
    }
    serviceResponse.code = xhttp.status;


    if (xhttp.status == 200) {
        serviceResponse.data = xhttp.responseText;
        serviceResponse.message = "Success";
    } else {
        if (xhttp.statusText == "") {
            if (xhttp.status == 404) {
                serviceResponse.message = "Not found";
            } else if (xhttp.status == 500) {
                serviceResponse.message = "Internal Error";
            } else if (xhttp.status == 401) {
                serviceResponse.message = "Unauthorized access";
            } else {
                serviceResponse.message = "Service unavailable at this moment. Please try again later";
            }
        } else {
            serviceResponse.message = xhttp.statusText;
        }

    }

    return serviceResponse;

}
function Download(controller, filename) {
    var url = GetURL() + controller;

    if (!filename.includes(".pdf")) {
        filename = filename + ".pdf";
    }
    
    $.ajax({
        type: "GET",
        url: url,
        headers: {
            'UserKey': GetAuthKey()
        },
        xhrFields: {
            responseType: 'blob'
        },

        success: function (blob) {

            download(blob, filename, "application/pdf");

        },
        error: function (jqxhr, textStatus, errorThrown) {
            alert("Server Error");
            console.log(textStatus, errorThrown)
        }
    });

}



function GetDomElement(elementId) {
    return document.getElementById(elementId);
}

function IsEmpty(elementId) {
    var input = GetDomElement(elementId).value;
    var isEmpty = false;
    if (input == "") {
        isEmpty = true;
    }
    return isEmpty;
}

/* Check if all the required fileds has value */
function CheckValidForm(inputList, errorAlertDiv) {
    var intValidInputCounter = 0;
    for (var i = 0; i < inputList.length; i++) {
        if (IsEmpty(`${inputList[i].getAttribute('id')}`)) {
            inputList[i].classList.add('is-invalid');
        } else {
            inputList[i].classList.remove('is-invalid');
            intValidInputCounter++;
        }
    }

    if (intValidInputCounter == inputList.length) {
        GetDomElement(errorAlertDiv).classList.add('d-none');
        return true;
    } else {
        GetDomElement(errorAlertDiv).classList.remove('d-none');
        return false;
    }
}

function ShowHideInputWithLabel(inputList, showFlag) {
    if (showFlag == 0) {
        for (var i = 0; i < inputList.length; i++) {
            inputList[i].classList.add('d-none');
        }
    } else if (showFlag == 1) {
        for (var i = 0; i < inputList.length; i++) {
            inputList[i].classList.remove('d-none');
        }
    }
}


function ClosePopupModal(triggerButton, closeButton) {
    var trigger = GetDomElement(triggerButton);
    var close = GetDomElement(closeButton);
    trigger.addEventListener('click', function () {
        close.click();
    })
}


function CheckValidAmount(inputList, divError) {
    var validCounter = 0;
    var errorModal = GetDomElement(divError);
    for (var i = 0; i < inputList.length; i++) {
        if (!isNaN(inputList[i].value)) {
            validCounter++;
            inputList[i].classList.remove('is-invalid');
        } else {
            inputList[i].classList.add('is-invalid');
        }
    }
    if (validCounter == inputList.length && validCounter != 0) {
        errorModal.classList.add('d-none');
        return true;
    } else {
        errorModal.classList.remove('d-none');
        return false;
    }
}


function ZeroAmountCheck(inputList, divError) {
    var validCounter = 0;
    var errorModal = GetDomElement(divError);
    for (var i = 0; i < inputList.length; i++) {
        if (inputList[i].value != 0) {
            validCounter++;
            inputList[i].classList.remove('is-invalid');
        } else {
            inputList[i].classList.add('is-invalid');
        }
    }
    if (validCounter == inputList.length && validCounter != 0) {
        errorModal.classList.add('d-none');
        return true;
    } else {
        errorModal.classList.remove('d-none');
        return false;
    }
}


function IsValidEmail(inputText) {
    var mailformat = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    var isValid = false;
    if (inputText.value.match(mailformat)) {
        isValid = true;
    }
    return isValid;
}

function GetCheckboxValue(input) {
    var checkbox = GetDomElement(input);
    if (checkbox.checked == true) {
        return 1;
    } else {
        return 0;
    }
}

class ServiceResponse {
    constructor(code, message, data) {
        this.code = code;
        this.message = message;
        this.data = data;
    }

}


function ReturnInputValue(input) {
    var inputField = GetDomElement(input);
    return inputField.value;

}

function ShowErrorModal(dataToggle, errorMessage, errorCode) {
    var triggerAlert = GetDomElement("failedMessage");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnFailedModal";
    triggerAlert.innerHTML = errorMessage;
    GetDomElement("btnCloseFailedAlert").onclick = () => {
        if (errorCode == 401) {
            location.href = "admin-login.html";
        } else {
            GetDomElement('btnCloseConfimrTwo').click();
        }
    };
}

function ShowErrorModalOnLoad(message, errorCode) {
    GetDomElement('btnFailedModal').classList.add('show');
    GetDomElement('btnFailedModal').style.display = "block";
    GetDomElement('failedMessage').innerHTML = message;
    GetDomElement("btnCloseFailedAlert").onclick = () => {
        if (errorCode == 401) {
            location.href = "admin-login.html";
        } else {
            GetDomElement('btnFailedModal').classList.remove('show');
            GetDomElement('btnFailedModal').style.display = "none";
        }
    };

}

function ShowSuccessModal(dataToggle, successMessage) {
    var triggerAlert = GetDomElement("successServiceMethod");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnSuccessEntryModal";
    
    triggerAlert.innerHTML = successMessage;
    

    GetDomElement('btnCloseSuccessEntry').onclick = () => {
        GetDomElement('btnCloseSuccessAlert').click();
    }
}
function ShowCustomSuccessModal(dataToggle, successMessage) {
    var triggerAlert = GetDomElement("CustomsuccessServiceMethod");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnCustomSuccessEntryModal";
    
    triggerAlert.innerHTML = successMessage;
    

    GetDomElement('btnCloseSuccessEntry').onclick = () => {
        GetDomElement('btnCloseSuccessAlert').click();
    }
}
function ShowCustomSuccess(dataToggle, successMessage) {
    var triggerAlert = GetDomElement("successCustomMessage");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnCustomSuccessEntryModal";
    triggerAlert.innerHTML = successMessage;
    GetDomElement('btnCloseCustomSuccessEntry').onclick = () => {
        GetDomElement('btnCloseCustomSuccessAlert').click();
    }
}


function PopulateCategoryListingDropDown(selectField) {
    var selCategory = document.getElementById(selectField);
    selCategory.innerHTML = "";

    var jsonData = InvokeService(GetURL(), "IndustrialCategoryMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var defaultOption = document.createElement('option');
        defaultOption.innerHTML = "Select a category";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selCategory.innerHTML += "<option value=\"" + oData[i].id_category + "\">" + capitalize(oData[i].description) + "</option>";
        }


    } else if (parseJsonData.code == 404 || parseJsonData.code == 204) {

    } else {
        // ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

function PopulatePaymentMethods(selectField) {
    var selCategory = document.getElementById(selectField);
    selCategory.innerHTML = "";

    var jsonData = InvokeService(GetURL(), "PaymentModeMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var defaultOption = document.createElement('option');
        defaultOption.innerHTML = "Select";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selCategory.innerHTML += "<option value=\"" + oData[i].payment_code + "\" data-Float=\"" + oData[i].Float + "\"" + "\" data-requireProof=\"" + oData[i].non_cash + "\">" + capitalize(oData[i].description) + "</option>";
        }


    } else if (parseJsonData.code == 404 || parseJsonData.code == 204) {

    } else {
        // ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

function PopulateFilteredPaymentMethods(selectField, filteredPaymentList) {
    var selCategory = document.getElementById(selectField);
    selCategory.innerHTML = "";

    var defaultOption = document.createElement('option');
    defaultOption.innerHTML = "Select";
    defaultOption.value = "";
    defaultOption.hidden = true;
    selCategory.appendChild(defaultOption);

    for (var i = 0; i < filteredPaymentList.length; i++) {
        selCategory.innerHTML += "<option value=\"" + filteredPaymentList[i].payment_code + "\" data-Float=\"" + filteredPaymentList[i].Float + "\"" + "\" data-requireProof=\"" + filteredPaymentList[i].require_proof + "\">" + capitalize(filteredPaymentList[i].description) + "</option>";
    }



}

function PopulateSiteLocationList(selectField, jsonData) {
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var selCategory = document.getElementById(selectField);
        selCategory.innerHTML = "";
        var defaultOption = document.createElement('option');

        defaultOption.innerHTML = "Select ";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selCategory.innerHTML += "<option value=\"" + oData[i].code + "\"'>" + capitalize(oData[i].site) + "</option>";
        }


    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

function PopulateAccessTemplate(selectField, jsonData) {
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var selCategory = document.getElementById(selectField);
        selCategory.innerHTML = "";
        var defaultOption = document.createElement('option');

        defaultOption.innerHTML = "Select ";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selCategory.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selCategory.innerHTML += "<option value=\"" + oData[i].template_code + "\"'>" + capitalize(oData[i].description) + "</option>";
        }


    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

function PopulateServiceListing(selectField) {
    var selService = document.getElementById(selectField);
    selService.innerHTML = "";
    var jsonData = InvokeService(GetURL(), "IndustrialServiceMethods", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);

        var defaultOption = document.createElement('option');

        defaultOption.innerHTML = "Select a service";
        defaultOption.value = "";
        defaultOption.hidden = true;
        selService.appendChild(defaultOption);

        for (var i = 0; i < oData.length; i++) {
            selService.innerHTML += "<option value=\"" + oData[i].id_service + "\">" + capitalize(oData[i].description) + "</option>";
        }


    } else {
        // ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }

}

function PopulateCheckboxTable(checkboxValue) {
    if (checkboxValue == 0) {
        return "<i class='fas fa-times-circle text-danger'></i>"
    } else {
        return " <i class='fas fa-check text-success'></i>"
    }
}

function PopulateCheboxInputField(checkboxValue, CheckboxInput) {
    if (checkboxValue == 0) {
        return GetDomElement(CheckboxInput).checked = false;
    } else {
        return GetDomElement(CheckboxInput).checked = true;
    }

}


function EncodeBASE64(input, txtImage) {
    var fileUploader = document.getElementById(input);
    if (fileUploader.files && fileUploader.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById(txtImage).value = e.target.result;
        };
        reader.readAsDataURL(fileUploader.files[0]);
    }
}



function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

function reformatAmount(amount) {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function truncateName(input, charCount) {
    if (input.length > charCount) {
        return input.substring(0, charCount) + '...';
    }
    return input;
};




function ProofOfPaymentPopup(paymentReference, paymentMode, imgSource, PaymentInput) {
    var isProofRequired = 0;
    for(var i=0;i<dupPaymentMethods.length;i++){
        if(paymentMode == dupPaymentMethods[i].payment_code){
            isProofRequired = dupPaymentMethods[i].non_cash;
            break;
        }
    }
    if (isProofRequired == 0) {
        return `<p data-target='#' id='withoutProof'>${paymentReference}</p>`
    } else {
        return `<p class='text-primary text-underline' id='withProof' onclick='ShowProof(${paymentReference},${imgSource}, ${PaymentInput})' data-toggle="modal" data-target="#proofModal">${paymentReference}</p>`
    }
}

function DisplayBillingMethod(paymentCode, paymentModeList) {
    for (var j = 0; j < paymentModeList.length; j++) {
        if (paymentCode == paymentModeList[j].payment_code) {
            return paymentModeList[j].description;
        }
    }
}


function ShowProof(paymentReference, imgSource, PaymentInput) {
    var paymentProof = InvokeService(GetURL(), `PaymentReferenceMethods/image/${paymentReference}`, "GET", "");
    if (paymentProof.code == 200) {
        oData = JSON.parse(paymentProof.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            imgSource.src = oJsonData[0].payment_image;
            PaymentInput.innerText = oJsonData[0].payment_reference;
        } else if (oData.code == 404) {

        } else {
            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
        }

    } else {
        ShowErrorModal("btnConfirmTwo", paymentProof.message, paymentProof.code);
    }
}


function GetAllPaymentMethods() {
    var jsonData = InvokeService(GetURL(), "PaymentModeMethods", "GET", "");
    if (jsonData.code == 200) {
        var parseJsonData = JSON.parse(jsonData.data);
        if (parseJsonData.code == 200) {
            var oData = JSON.parse(parseJsonData.jsonData);
            for (var i = 0; i < oData.length; i++) {
                paymentModeList.push(oData[i]);
                dupPaymentMethods.push(oData[i])
            }


        } else if (parseJsonData.code == 404 || parseJsonData.code == 204) {

        } else {
            ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

        }
    } else {
        ShowErrorModalOnLoad(jsonData.message, jsonData.code);

    }
}

function FilterPaymentMethods(paymentModeList, filteredPaymentList) {
    for (var i = 0; i < paymentModeList.length; i++) {
        if (paymentModeList[i].Float == 0) {
            filteredPaymentList.push(paymentModeList[i]);
        }
    }
}
function FilterFloatPaymentMethods(paymentModeList, filteredPaymentList) {
    for (var i = 0; i < paymentModeList.length; i++) {
        if (paymentModeList[i].Float == 1) {
            filteredPaymentList.push(paymentModeList[i]);
        }
    }
}

function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim(); // Never return a text node of whitespace as the result
    template.innerHTML = html;
    return template.content.firstChild;
}

//Prevent & and other special characters from encoding
function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}