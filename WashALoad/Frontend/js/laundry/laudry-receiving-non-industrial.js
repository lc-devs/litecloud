var SOReferenceErrorMessage = GetDomElement("SOReferenceErrorMessage");
var soReference = GetDomElement("txtSOReference");
var tblClothingServiceBody = GetDomElement("tblClothingServiceBody");
var OtherService = GetDomElement("OtherServiceBody");
var OtherCategory = GetDomElement("tblOtherCategoryBody");


GetDomElement('btnSearch').addEventListener('click', function () {
    SearchSOReferenceData();
});

GetDomElement("txtSOReference").addEventListener('keypress', function (e) {
    ClearSOReferenceDetails();
    if (e.key == 'Enter') {
        SearchSOReferenceData();
    }
});


GetDomElement('openCamera').addEventListener('click', function () {
   ReadCamera("selCameraList", "divReader", "txtSOReference", "btnSearch", "btnCloseQRModal");
});


GetDomElement("btnRecieveSOReference").addEventListener('click', function () {
    this.dataset.target = "#btnConfirm1";
    GetDomElement("method").innerText = "receive item";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "receive item";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var recieveItem = InvokeService(GetURL(), `LogisticNonIndustrialMethods/receiveditembylaundry/${soReference.value}`, "POST", "");
        if (recieveItem.code == 200) {
            var oData = JSON.parse(recieveItem.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "received");
                GetDomElement('btnCloseConfimrTwo').click();
                GetDomElement("txtSOReference").value = "";
                ClearSOReferenceDetails();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", recieveItem.message, recieveItem.code);
        }
    }
});

function SearchSOReferenceData() {
    SOReferenceErrorMessage.classList.add('d-none');
    soReference.classList.remove('is-invalid');
    tblClothingServiceBody.innerHTML = "";
    /* SEARCH SO REFERENCE HERE */
    var getSODetails = InvokeService(GetURL(), `LogisticNonIndustrialMethods/sodetails/${soReference.value}`, "GET", "");

    if (getSODetails.code == 200) {
        var oData = JSON.parse(getSODetails.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            if (oJsonData.received_by_laundry == 1) {
                ShowErrorModalOnLoad("Item Already Received", 500);
            } else {
                GetDomElement("btnRecieveSOReference").disabled = false;
                GetDomElement("soAccountDetails").innerText = oJsonData.customer_name;
                GetDomElement("txtWeight").value = oJsonData.weight_in_kg;
                GetDomElement("txtNoOfBags").value = oJsonData.number_of_bags;
                GetDomElement("txtNOofLoads").value = oJsonData.number_of_loads;
                   if(oJsonData.services != undefined){
                    var services = oJsonData.services;
                    if (services.length != 0) {
                        for (var i = 0; i < services.length; i++) {
                            OtherService.innerHTML +=
                                "<tr>" +
                                "<td class='border col-4 text-capitalize justify-content-start'><p class='float-left'>"+services[i].description +
                                "</p></td></tr>";
                        }
                    }
                   }
                   if(oJsonData.items != undefined){
                    var items = oJsonData.items;
                    if (items.length != 0) {
                        for (var i = 0; i < items.length; i++) {
                            if(items[i].item_code == 1){
                                OtherCategory.innerHTML +=
                                "<tr>" +
                                "<td class='border col-4 text-capitalize'>"+items[i].item_description +
                                "<td class='border col-4 text-capitalize'>"+items[i].item_count +
                               "</td></tr>";
                            }else{
                                tblClothingServiceBody.innerHTML +=
                                "<tr>" +
                                "<td class='border col-4 text-capitalize'>"+items[i].item_description +
                                "<td class='border col-4 text-capitalize'>"+items[i].item_count +
                                "</td></tr>";
                            }
                           
                        }
                    }
                   }
                   



            }

        } else if (oData.code == 404 || oData.code == 204) {
            SOReferenceErrorMessage.classList.remove('d-none');
            soReference.classList.add('is-invalid');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (getSODetails.code == 204 || getSODetails.code == 404) {
        SOReferenceErrorMessage.classList.remove('d-none');
        soReference.classList.add('is-invalid');

    } else {
        ShowErrorModalOnLoad(getSODetails.message, getSODetails.code);
    }
}


function ClearSOReferenceDetails() {
    SOReferenceErrorMessage.classList.add('d-none');
    soReference.classList.remove('is-invalid');
    GetDomElement("soAccountDetails").innerHTML = "";
    GetDomElement("tblClothingServiceBody").innerHTML = "";
    OtherService.innerHTML = "";
    OtherCategory.innerHTML = "";
    GetDomElement("txtWeight").value = "";
    GetDomElement("txtNoOfBags").value = "";
    GetDomElement("txtNOofLoads").value = "";
    GetDomElement("btnRecieveSOReference").disabled = true;
}




ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");