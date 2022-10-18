document.writeln("<script src='js/download.min.js'></script>");
document.writeln("<script src='js/download.js'></script >");

var dupPaymentMethods = [];

class ServiceResponse {
    constructor(code, message, data) {
        this.code = code;
        this.message = message;
        this.data = data;
    }

}

function GetURL() {
    //return window.location.origin + "/washaloadservice/";
    // return "http://192.168.4.202/posinventoryservice/";
    return "https://localhost:44385/posinventoryservice/";
}

function GetAuthKey() {
    return sessionStorage.getItem("authkey");
}

function InvokeService(controller, paramMethod, paramBody) {
    var url = GetURL() + controller;
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


function ReturnInputValue(input) {
    var inputField = GetDomElement(input);
    return inputField.value;

}

function ShowErrorModal( errorMessage, errorCode) {
    var triggerAlert = GetDomElement("failedMessage");
    var toggle = GetDomElement(dataToggle);
    toggle.dataset.target = "#btnFailedModal";
    triggerAlert.innerHTML = errorMessage;
    GetDomElement("btnCloseFailedAlert").onclick = () => {
        if (errorCode == 401) {
            location.href = "admin-login.html";
        } else {
            // GetDomElement('btnCloseConfimrTwo').click();
        }
    };
}
function ShowModal(msg, isSuccess, redirect_page) {
    if(isSuccess){
        var triggerAlert = GetDomElement("successMessage");
        triggerAlert.innerHTML = msg;
        $("#btnSuccessEntryModal").modal();

        GetDomElement('btnCloseSuccessEntry').onclick = () => {
            if(redirect_page != ""){
                location.href =  redirect_page;
            }
            
        }
    }else{
        var triggerAlert = GetDomElement("failedMessage");
        triggerAlert.innerHTML = msg;
        $("#btnFailedModal").modal();
        GetDomElement('btnCloseFailedAlert').onclick = () => {
            if(redirect_page != ""){
                location.href =  redirect_page;
            }else{
                GetDomElement('btnCloseFailedEntry').click();
            }
            
        }
    }
    
    
}

function ExecuteFunction(functionname, params){

    var arr = params.split(",");
    var args = new Array();

    for (var i = 0; i < arr.length; i++)
        args.push(arr[i]);

        console.log(window[functionname]);
    
    if(args.length < 1){
        window[functionname];
    }else{
        window[functionname].apply(this, args);
    }

    
}

function ShowConfirmationModal(msg, functionname, params) {
   
        var triggerAlert = GetDomElement("message");
        triggerAlert.innerHTML = msg;
       

        GetDomElement('btnExecute').onclick = () => {
            ExecuteFunction(functionname, params);
            
        }
    
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


function PopulateDropdownList(selElement, value, display) {

    selElement.innerHTML += "<option value=\"" + value + "\">" + display + "</option>";
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
