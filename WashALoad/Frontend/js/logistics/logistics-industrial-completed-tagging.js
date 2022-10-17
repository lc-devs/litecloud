var tblHeader = GetDomElement("tblSOReference");
var tblBody = GetDomElement("tblSOReferenceBody");
var inProcessButtons = document.querySelectorAll('.InProcessButtons');
var emptyTable = GetDomElement("emptyTable");
var customerName = GetDomElement('txtCustomer');
var soList = [];
var fetchType = 2;
var CIFTable = GetDomElement("btnCustomerList");
var customerID = GetDomElement("customerId");
var tblClothing = GetDomElement("tblClothingBody");
var tblOtherCategory = GetDomElement("tblOtherCategoryBody");
var tblOtherServices = GetDomElement("tblOtherServices");
var dateToday = new Date().toISOString().split('T')[0];

window.addEventListener('load', function () {
    // PopulateInProcessSOReference();
    GetDomElement("from").value = dateToday;
    GetDomElement("to").value = dateToday;
});

GetDomElement("btnFindCustomer").addEventListener('click', function () {
    GetDomElement("txtSearchClient").value = "";
    CIFTable.innerHTML = "";
});
GetDomElement("chkCustomer").addEventListener('click', function () {
    customerName.value = ""
    tblBody.innerHTML = "";
    customerID.value = "";

    if (this.checked) {
        customerName.classList.add('input-required');
        GetDomElement("btnFindCustomer").disabled = false;
    } else {
        customerName.classList.remove('input-required');
        customerName.classList.remove('is-invalid');
        GetDomElement("btnFindCustomer").disabled = true;

    }
});
GetDomElement("btnGenerateReport").addEventListener('click', function () {
    var dateFrom = GetDomElement('from').value;
    var dateTo = GetDomElement('to').value;
    var requiredFields = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(requiredFields, "divUpdateInvalidForm");
    if (bValidForm) {
        soList = InvokeService(GetURL(), `LogisticIndustrialMethods/laundryqueryreport?dateFrom=${dateFrom}&dateTo=${dateTo}&customerID=${customerID.value}&laundry=${fetchType}`, "GET", "");
        if (soList.code == 200) {
            if (fetchType == 2) {
                PopulateInProcessSOReference();
            }
            if (fetchType == 3) {
                PopulateForLogisticsReceiving();
            }
        } else if (soList.code == 204 || soList.code == 404) {} else {

            ShowErrorModalOnLoad(soList.message, soList.code);
        }


    }

});

/* IN PROCESS SO REFERENCE */
GetDomElement('radioInProcessSOReference').addEventListener('click', function () {
    if (this.checked) {
        fetchType = 2;
    }
    ShowHideInProcessButtons(1);
    tblHeader.innerHTML = "";
    tblHeader.innerHTML +=
        "<tr> " +
        "<th class='border col-4'>SO Reference</th>" +
        "<th class='border col-6'>Customer Name</th>" +
        "<th class='border col-2'>Completed</th> " +
        "</tr>";
    GetDomElement("btnGenerateReport").click();

});

GetDomElement("txtSearchClient").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        PopulateCISList();
    }
});

GetDomElement("btnSearchClient").addEventListener('click', function () {
    PopulateCISList();
});

/* FOR LOGISTICS RECIEVING */
GetDomElement('radioForLogisticsRecieving').addEventListener('click', function () {
    if (this.checked) {
        fetchType = 3;
    }
    ShowHideInProcessButtons(0);
    tblHeader.innerHTML = "";
    tblHeader.innerHTML +=
        "<tr> " +
        "<th class='border col-4'>SO Reference</th>" +
        "<th class='border col-8 text-left'>Customer Name</th>" +
        // "<th class='border col-8 text-left'>Location</th>" +
        "<th class='border col-8 text-left'>Delivered</th>" +
        "</tr>";

    GetDomElement("btnGenerateReport").click();

});
/* RECIEVED BY LOGISTICS */


/* REFRESH FUNCIONALITY */
GetDomElement('btnRefreshList').addEventListener('click', function () {
    GetDomElement("btnGenerateReport").click();
});

/* MOVE TO LOGISTICS */
GetDomElement("btnMoveSOReference").addEventListener('click', function () {
    this.dataset.target = "#btnConfirm1";
    GetDomElement("method").innerText = "move item to logistics";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "move item to logistics";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var moveToLogistics = InvokeService(GetURL(), ``, "POST", "");
        if (moveToLogistics.code == 200) {
            var oData = JSON.parse(moveToLogistics.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "moved to logistics");
                GetDomElement('btnCloseConfimrTwo').click();
                GetDomElement("txtSOReference").value = "";
                ClearSOReferenceDetails();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", moveToLogistics.message, moveToLogistics.code);
        }
    }
});


GetDomElement();

/* FUNCTIONS */
function PopulateInProcessSOReference() {

    var inProcessISO = soList;
    if (inProcessISO.code == 200) {
        emptyTable.classList.add('d-none');
        var oData = JSON.parse(inProcessISO.data);
        if (oData.code == 200) {
            emptyTable.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            tblBody.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                tblBody.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].so_reference +
                    "<td>" + `<p class='text-primary text-underline' style='cursor:pointer !important;' onclick='GetSODetails(${oJsonData[i].so_reference})' data-toggle='modal' data-target='#btnSOReferenceDetails'>${oJsonData[i].so_reference}</p>` +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    "</td><td class='text-center'>" + CheckCompletedByLaundry(oJsonData[i].received_from_laundry, oJsonData[i].so_reference) +
                    "</td></tr>";
            }


        } else if (oData.code == 404 || oData.code == 204) {
            tblBody.innerHTML = "";
            emptyTable.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (inProcessISO.code == 204 || inProcessISO.code == 404) {
        tblBody.innerHTML = "";
        emptyTable.classList.remove('d-none');

    } else {
        emptyTable.classList.add('d-none');
        ShowErrorModalOnLoad(inProcessISO.message, inProcessISO.code);
    }
}

function PopulateCISList() {

    var clientSearchText = GetDomElement("txtSearchClient");
    var oCustomers = InvokeService(GetURL(), `CustomerMethods/search/customer/${clientSearchText.value}`, "GET", "");
    if (oCustomers.code == 200) {
        emptyCustomerList.classList.add('d-none');
        var oData = JSON.parse(oCustomers.data);
        if (oData.code == 200) {
            emptyCustomerList.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            CIFTable.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                CIFTable.innerHTML +=
                    "<tr class='border' style='cursor:default'><td hidden>" + oJsonData[i].customer_id +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    "</td><td>" + oJsonData[i].cellular_number +
                    "</td><td>" + oJsonData[i].email_address +
                    "</td></tr>";
            }
            var cifRows = CIFTable.rows;
            for (var i = 0; i < cifRows.length; i++) {
                cifRows[i].addEventListener('click', function () {
                    customerName.value = this.cells[1].innerHTML;
                    customerID.value = this.cells[0].innerHTML;
                    GetDomElement("btnCloseClientList").click();

                });
            }


        } else if (oData.code == 404 || oData.code == 204) {
            CIFTable.innerHTML = "";
            emptyCustomerList.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oCustomers.code == 204 || oCustomers.code == 404) {
        CIFTable.innerHTML = "";
        emptyCustomerList.classList.remove('d-none');

    } else {
        emptyCustomerList.classList.add('d-none');
        ShowErrorModalOnLoad(oCustomers.message, oCustomers.code);
    }

}


function TasAsReceivedByLogistics(soReference) {

    GetDomElement("method").innerText = "mark item as received by logistics";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "mark item as received by logistics";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var doneItem = InvokeService(GetURL(), `LogisticIndustrialMethods/receivedcompleteditembylogistics/${soReference}`, "POST", "");
        if (doneItem.code == 200) {
            var oData = JSON.parse(doneItem.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "received by logistics");
                GetDomElement('btnCloseConfimrTwo').click();
                GetDomElement("btnGenerateReport").click();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", doneItem.message, doneItem.code);
        }
    }

}

function DeliverItem(soReference) {

    GetDomElement("method").innerText = "mark item as delivered";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "mark item as delivered";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var doneItem = InvokeService(GetURL(), `LogisticIndustrialMethods/deliveritem/${soReference}`, "POST", "");
        if (doneItem.code == 200) {
            var oData = JSON.parse(doneItem.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "delivered by logistics");
                GetDomElement('btnCloseConfimrTwo').click();
                GetDomElement("btnGenerateReport").click();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", doneItem.message, doneItem.code);
        }
    }

}



function CheckCompletedByLaundry(isCompleted, soReference) {
    if (isCompleted == 0) {
        return `<button class='btn btn-success' onclick ='TasAsReceivedByLogistics(${soReference})' data-toggle='modal' data-target='#btnConfirm1' >Received by logistics</button>`;
    } else {
        return "<i class='fas fa-check text-success'></i>"
    }
}

function CheckDeliveredItem(isCompleted, soReference) {
    if (isCompleted == 0) {
        return `<button class='btn btn-success' onclick='DeliverItem(${soReference})'data-toggle='modal' data-target='#btnConfirm1'>Delivered</button>`;
    } else {
        return "<i class='fas fa-check text-success'></i>"
    }
}


function PopulateForLogisticsReceiving() {
    var forLogisticsReceiving = soList;
    if (forLogisticsReceiving.code == 200) {
        emptyTable.classList.add('d-none');
        var oData = JSON.parse(forLogisticsReceiving.data);
        if (oData.code == 200) {
            emptyTable.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            tblBody.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                tblBody.innerHTML +=
                    "<tr> " +
                    "<td>" + `<p class='text-primary text-underline' style='cursor:pointer !important;' onclick='GetSODetails(${oJsonData[i].so_reference})' data-toggle='modal' data-target='#btnSOReferenceDetails'>${oJsonData[i].so_reference}</p>` +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    // "</td><td class='text-capitalize text-center'><i class='fas fa-map-marker-alt text-danger' style='cursor:pointer;' data-toggle='modal' data-target='#'" + "></i>" +
                    "</td><td class='text-capitalize text-center'>" + CheckDeliveredItem(oJsonData[i].delivered, oJsonData[i].so_reference) +
                    "</td></tr>";
            }



        } else if (oData.code == 404 || oData.code == 204) {
            tblBody.innerHTML = "";
            emptyTable.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (forLogisticsReceiving.code == 204 || forLogisticsReceiving.code == 404) {
        tblBody.innerHTML = "";
        emptyTable.classList.remove('d-none');

    } else {
        emptyTable.classList.add('d-none');
        ShowErrorModalOnLoad(forLogisticsReceiving.message, forLogisticsReceiving.code);
    }
}

function PopulateRecievedByLogistics() {
    var receivedbyLogistics = soList;
    if (receivedbyLogistics.code == 200) {
        emptyTable.classList.add('d-none');
        var oData = JSON.parse(receivedbyLogistics.data);
        if (oData.code == 200) {
            emptyTable.classList.add('d-none');
            var oJsonData = JSON.parse(oData.jsonData);
            tblBody.innerHTML = "";
            for (var i = 0; i < oJsonData.length; i++) {
                tblBody.innerHTML +=
                    "<tr> " +
                    "<td>" + `<p class='text-primary text-underline' style='cursor:pointer !important;' onclick='GetSODetails(${oJsonData[i].so_reference})' data-toggle='modal' data-target='#btnSOReferenceDetails'>${oJsonData[i].so_reference}</p>` +
                    "</td><td class='text-capitalize'>" + oJsonData[i].customer_name +
                    "</td></tr>";
            }



        } else if (oData.code == 404 || oData.code == 204) {
            tblBody.innerHTML = "";
            emptyTable.classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (receivedbyLogistics.code == 204 || receivedbyLogistics.code == 404) {
        tblBody.innerHTML = "";
        emptyTable.classList.remove('d-none');

    } else {
        emptyTable.classList.add('d-none');
        ShowErrorModalOnLoad(receivedbyLogistics.message, receivedbyLogistics.code);
    }
}

function ShowHideInProcessButtons(showFlag) {
    if (showFlag == 0) {
        for (var i = 0; i < inProcessButtons.length; i++) {
            inProcessButtons[i].classList.add('d-none');
        }
    } else if (showFlag == 1) {
        for (var i = 0; i < inProcessButtons.length; i++) {
            inProcessButtons[i].classList.remove('d-none');
        }
    }
}

function GetSODetails(SOReference) {
    tblSORerefenceBody.innerHTML = "";
    var soDetails = InvokeService(GetURL(), `LogisticIndustrialMethods/sodetails/${SOReference}`, "GET", "");
    if (soDetails.code == 200) {
        var oData = JSON.parse(soDetails.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            GetDomElement("labSOReference").innerText = oJsonData.so_reference;
            GetDomElement("soAccountDetails").innerText = oJsonData.customer_name;
            GetDomElement("labWeight").innerText = oJsonData.weight_in_kg;
            GetDomElement("labBags").innerText = oJsonData.number_of_bags;
            if (oJsonData.items != undefined) {
                var itemData = oJsonData.items;
                for (var i = 0; i < itemData.length; i++) {
                    tblSORerefenceBody.innerHTML +=
                        "<tr> " +
                        "<td class='text-capitalize'>" + itemData[i].item_description +
                        "</td><td class='text-capitalize'>" + itemData[i].item_count +
                        "</td></tr>";
                }
            }

        } else {
            GetDomElement("btnCloseSOReferenceDetails").click();
            ShowErrorModalOnLoad(oData.message, oData.code);
        }

    } else {
        GetDomElement("btnCloseSOReferenceDetails").click();
        ShowErrorModalOnLoad(soDetails.message, soDetails.code);
    }

}



ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");
ClosePopupModal("btnCloseSOReference", "btnCloseSOReferenceDetails");