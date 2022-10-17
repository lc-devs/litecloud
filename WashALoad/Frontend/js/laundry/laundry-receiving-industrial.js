var SOReferenceErrorMessage = GetDomElement("SOReferenceerrorMessage");
var soReference = GetDomElement("txtSOReference");
var tblBody = GetDomElement("tblBody");

GetDomElement('btnSearch').addEventListener('click', function () {
    SearchSOReferenceData();
});

GetDomElement("txtSOReference").addEventListener('keypress', function (e) {
    ClearIndustrialSOReferenceDetails();
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
        this.disabled = true;
        var recieveItem = InvokeService(GetURL(), `LogisticIndustrialMethods/receiveditembylaundry/${soReference.value}`, "POST", "");
        if (recieveItem.code == 200) {
            var oData = JSON.parse(recieveItem.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "received");
                GetDomElement('btnCloseConfimrTwo').click();
                GetDomElement("txtSOReference").value = "";
                ClearIndustrialSOReferenceDetails();
                this.disabled = false;
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
    tblBody.innerHTML = "";
    /* SEARCH SO REFERENCE HERE */
    var getSODetails = InvokeService(GetURL(), `LogisticIndustrialMethods/sodetails/${soReference.value}`, "GET", "");

    if (getSODetails.code == 200) {
        var oData = JSON.parse(getSODetails.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            if(oJsonData.received_by_laundry == 1){
                ShowErrorModalOnLoad("Item Already Received", 500);
            }else{
                GetDomElement("btnRecieveSOReference").disabled = false;
                GetDomElement("soAccountDetails").innerText = oJsonData.customer_name;
                GetDomElement("txtWeight").value = oJsonData.weight_in_kg;
                GetDomElement("txtNoOfBags").value = oJsonData.number_of_bags;
               if(oJsonData.items != undefined){
                var SOItems = oJsonData.items;
                if (SOItems.length != 0) {
                    for (var i = 0; i < SOItems.length; i++) {
                        tblBody.innerHTML +=
                            "<tr>" +
                            "<td class='border col-4 text-capitalize'>"+SOItems[i].item_description +
                            "</td><td class='border col-8 text-center'>"+SOItems[i].item_count +
                            "</td></tr>";
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


function ClearIndustrialSOReferenceDetails() {
    SOReferenceErrorMessage.classList.add('d-none');
    soReference.classList.remove('is-invalid');
    GetDomElement("soAccountDetails").innerHTML = "";
    GetDomElement("tblBody").innerHTML = "";
    GetDomElement("txtWeight").value = "";
    GetDomElement("txtNoOfBags").value = "";
    GetDomElement("btnRecieveSOReference").disabled = true;
}




ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");