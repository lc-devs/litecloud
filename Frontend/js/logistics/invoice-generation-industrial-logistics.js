var tblHeader = GetDomElement("tblSOReference");
var divtbl = GetDomElement('tblInvoice');
var arrayTbl = [];
var inProcessButtons = document.querySelectorAll('.InProcessButtons');
var errorInput = false;




GetDomElement('radioAutomatic').addEventListener('click', function () {
    // ShowHideInProcessButtons(1);

    tblHeader.innerHTML = "";
    tblHeader.innerHTML +=
        "<tr> " +
        "<th class='border col-4'>SO Reference</th>" +
        "<th class='border col-6'>Customer Name</th>" +
        "<th class='border col-2'>Amount Due</th> " +
        "</tr>";
    populateForInvoice();
});


GetDomElement('radioManual').addEventListener('click', function () {
    // ShowHideInProcessButtons(0);

    tblHeader.innerHTML = "";
    tblHeader.innerHTML +=
        "<tr> " +
        "<th class='border col-4'>SO Reference</th>" +
        "<th class='border col-6'>Customer Name</th>" +
        "<th class='border col-6'>Manual costing</th>" +
        "<th class='border col-2'>Amount Due</th>" +
        "</tr>";
    populateForInvoice();

});
/* RECIEVED BY LOGISTICS */


/* REFRESH FUNCIONALITY */
GetDomElement('btnRefreshList').addEventListener('click', function () {
    populateForInvoice();
});




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



window.addEventListener('load', function () {
    // var customerId = GetDomElement('customerIdSearch').innerHTML;
    // var pickUpby = sessionStorage.getItem('userId');

    populateForInvoice();


});

function populateForInvoice() {
    var dateNow = new Date();
    var date = dateNow.toISOString().slice(0, 10);
    var oData;

    divtbl.innerHTML = "";

    if (getRadio() == 0) {
        oData = InvokeService(GetURL(), `LogisticIndustrialMethods/soforinvoicing?isManualInvoicing=false`, 'GET', '');
    } else {
        oData = InvokeService(GetURL(), `LogisticIndustrialMethods/soforinvoicing?isManualInvoicing=true`, 'GET', '');
    }

    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            GetDomElement('btnMoveSOReference').classList.remove('d-none');
            GetDomElement('emptyDiv').classList.add('d-none');

            var parsedData = JSON.parse(parsedData.jsonData);
            // var parsedData = JSON.parse(parsedData);

            if (GetDomElement('radioAutomatic').checked) {
                populateTableQuery(parsedData);
            } else {
                populateManual(parsedData);
            }
        } else if (parsedData.code == 404) {
            GetDomElement('emptyDiv').classList.remove('d-none');
            GetDomElement('btnMoveSOReference').classList.add('d-none');

        } else if (parsedData.code == 401) {

            ShowErrorModalOnLoad('', parsedData.code);

        } else {
            GetDomElement('btnMoveSOReference').classList.add('d-none');

            // GetDomElement('tblQuery').innerHTML = 'No Data Found for Today';
            // GetDomElement('tblQuery').classList.add('bg-transparent');
        }
    } else {
        ShowErrorModalOnLoad(oData.message, oData.code);

    }
}

function populateTableQuery(parsedData) {

    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr class='border' style='cursor:default'><td class='text-capitalize'><p" + ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnSODetails' onclick = "PopulateSOReferenceDetails(${parsedData[i].so_reference})"` +
            ">" + parsedData[i].so_reference +
            "</p></td>" +
            "<td class='border text-center px-0 '>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0'>" + parsedData[i].cost + "</td>" +
            "</tr>";
    }
    //FOR TESTING PURPOSE ONLY
    // for (var i = 0; i < 2; i++) {
    //     divtbl.innerHTML +=
    //         "<tr class='border' style='cursor:default'><td class='text-capitalize'><p" + ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnSODetails' onclick = "PopulateSOReferenceDetails('39')"` +
    //         ">" + 39 +
    //         "</p><td class='border text-center px-0 text-success'>" + "Test" + "</td>" +
    //         "<td class='border text-center px-0 '>" + "12" + "</td>" +
    //         "</tr>";
    // }
}

function PopulateSOReferenceDetails(soReference) {
    var rawServerResponse = InvokeService(GetURL(), `LogisticIndustrialMethods/sodetails/${soReference}`, "GET", "");
    if (rawServerResponse.code == 200) {
        var jsonResponse = JSON.parse(rawServerResponse.data);
        if (jsonResponse.code == 200) {
            var soDetails = JSON.parse(jsonResponse.jsonData);
            GetDomElement("so_reference").innerText = soDetails.so_reference;
            GetDomElement("customer_name").innerText = soDetails.customer_name;
            if (soDetails.items != undefined) {
                var divtbl = GetDomElement('tblSODetails');
                divtbl.innerHTML = "";
                for (var i = 0; i < soDetails.items.length; i++) {
                    divtbl.innerHTML +=
                        "<tr> " +
                        "<td class='border text-center px-0'>" + soDetails.items[i].item_code + "</td>" +
                        "<td class='border text-center px-0 text-capitalize'>" + soDetails.items[i].item_description + "</td>" +
                        "<td class='border text-center px-0'>" + soDetails.items[i].item_count + "</td>" +
                    "<td class='border text-center px-0'>" + `${currency((soDetails.items[i].cost), { separator: ',', symbol: '' }).format()}` + "</td>" +
                    "<td class='border text-center px-0'>" + `${currency((parseFloat(soDetails.items[i].item_count)) * (parseFloat((soDetails.items[i].cost))), { separator: ',', symbol: '' }).format()}` + "</td>" +
                        "</tr>";
                }
                var partialTotalRows = divtbl.rows;
                var master_total = 0.0;
                for (var i = 0; i < partialTotalRows.length; i++) {
                    master_total = currency(master_total).add(partialTotalRows[i].cells[4].innerText);
                }
                GetDomElement("master_total").innerText = currency(master_total, { separator: ',', symbol: '' }).format();
            }
        }
    }
}

function populateManual(parsedData) {


    for (var i = 0; i < parsedData.length; i++) {
        divtbl.innerHTML +=
            "<tr class='border' style='cursor:default'><td class='text-capitalize'><p" + ` style='cursor:pointer;' class='text-primary' data-toggle='modal' data-target='#btnSODetails' onclick = "PopulateSOReferenceDetails(${parsedData[i].so_reference})"` +
            ">" + parsedData[i].so_reference +
            "</p></td>" +
            "<td class='border text-center px-0 '>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center'><input type='text' class='form-control item-count'> </td>" +
            "<td class='border text-center px-0'>" + parsedData[i].cost + " </td>" +
            "</tr>";


    }
}

function getRadio() {
    if (GetDomElement('radioAutomatic').checked == true) {
        return 0;
    } else {
        return 1;
    }
}

function getArray() {
    errorInput = false;
    var tbl = GetDomElement('tblInvoice');
    var tblresult = []
    if (GetDomElement('radioAutomatic').checked == true) {

        if (tbl.rows.length > 0) {
            for (var i = 0; i < tbl.rows.length; i++) {
                var p = htmlToElement(tbl.childNodes[i].cells[0].innerHTML);
                arrayTbl.push({
                    "soReference": p.textContent,
                    "amount": tbl.childNodes[i].cells[2].innerHTML.replace(/,/g, '')
                })
            }
            tblresult = arrayTbl;
        } else {
            tblresult = [];
        }

    } else {

        var itemCount = document.querySelectorAll('.item-count');
        arrayTbl = [];
        if (itemCount.length > 0) {
            for (var i = 0; i < itemCount.length; i++) {
                if (itemCount[i].value != '') {

                    arrayTbl.push({
                        "soReference": itemCount[i].parentNode.parentNode.cells[0].innerHTML,
                        "amount": itemCount[i].value.replace(/,/g, '')
                    })


                }
                if (isNaN(itemCount[i].value)) {
                    itemCount[i].classList.add('is-invalid');
                    errorInput = true;
                } else {
                    itemCount[i].classList.remove('is-invalid');

                }
            }
            tblresult = arrayTbl;
        } else {
            tblresult = [];
        }

    }

    return tblresult;
}

GetDomElement('btnMoveSOReference').addEventListener('click', function () {
    arrayTbl = []
    arrayTbl = getArray();

    if (arrayTbl.length != 0 && errorInput == false) {


        GetDomElement("confirmOneSeviceMethod").innerText = "add";
        GetDomElement("confirmOneSeviceMethod").classList.remove('text-danger');
        GetDomElement("confirmOneSeviceMethod").classList.add('text-success');

        GetDomElement("confirmServiceMethod").innerText = "add";
        GetDomElement("confirmServiceMethod").classList.remove('text-danger');
        GetDomElement("confirmServiceMethod").classList.add('text-success');

        this.dataset.target = "#btnConfirmOneModal";

        GetDomElement("btnConfirmTwo").onclick = () => {
            var body = arrayTbl;

            var strBody = JSON.stringify(body);
            var oData = InvokeService(GetURL(), 'LogisticIndustrialMethods/generateinvoice', 'POST', strBody);

            if (oData.code == 200) {

                var parsedData = JSON.parse(oData.data);


                if (parsedData.code == 200) {
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();

                    ShowSuccessModal('btnConfirmTwo', "added");

                    populateForInvoice();

                } else {
                    GetDomElement('btnCloseConfimrTwo').click();
                    GetDomElement('btnCloseConfirmOne').click();
                    ShowErrorModal('btnConfirmTwo', oData.message, oData.code);
                    // location.href = 'invoice-generation-industrial-logistics.html';
                }

            } else {
                ShowErrorModal('btnConfirmTwo', parsedData.message, oData.code);
            }


        }

    } else {
        this.dataset.target = "";
    }
});

ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnOKSODetails", "btnCloseSODetails");