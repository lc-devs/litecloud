var proofOfPayment = GetDomElement("paymentProof");
var paymentModeList = [];
var filteredPaymentList = [];
var requiredPaymentProof = this.document.querySelectorAll('.payment_proof');
var paymentBillingTable = GetDomElement("tblBody");
var systemUserId = sessionStorage.getItem("userId");
var dateToday = new Date().toISOString().split('T')[0];
var customerName = GetDomElement("txtCustomerName");
var CIFTable = GetDomElement("btnCustomerList");
var customerID = GetDomElement("customerId");

window.addEventListener('load', function () {
    GetAllPaymentMethods();
    FilterFloatPaymentMethods(paymentModeList, filteredPaymentList)

    if(filteredPaymentList.length < 1){
        PopulateFilteredPaymentMethods("selModeOfPayment", paymentModeList)
   }else{
        PopulateFilteredPaymentMethods("selModeOfPayment", filteredPaymentList)
   }
    //PopulateFilteredPaymentMethods("selModeOfPayment", filteredPaymentList)
    PopulatePaymentFloats();

    ShowHideInputWithLabel(requiredPaymentProof, 0);
});
GetDomElement("txtSearchClient").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        PopulateCustomerSelection();
    }
});

GetDomElement("btnSearchClient").addEventListener('click', function () {
    PopulateCustomerSelection();
});


GetDomElement("selModeOfPayment").addEventListener('change', function () {
    proofOfPayment.value = "";
    let selectedI = this.selectedIndex;
    var requireProof = this.options[selectedI].dataset.requireproof;
    if (requireProof == 1) {
        requiredFlag = 1;
        ShowHideInputWithLabel(requiredPaymentProof, 1);
        proofOfPayment.classList.add('input-required');
    } else {
        ShowHideInputWithLabel(requiredPaymentProof, 0);
        proofOfPayment.classList.remove('input-required');
        requiredFlag = 0;
    }
});

GetDomElement('btnSubmitNew').addEventListener('click', function () {
    this.dataset.target = "";
    var oRequiredFeilds = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(oRequiredFeilds, "divAddNewInvalidForm");
    var amount = GetDomElement("txtAmount").value;
    if (bValidForm) {
        if (!isNaN(amount)) {
            GetDomElement('divAddNewInvalidAmountErrorAlert').classList.add('d-none');
            GetDomElement("txtAmount").classList.remove('is-invalid');
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");
            EncodeBASE64("paymentProof", "imageBase64");
            GetDomElement("btnConfirmTwo").onclick = ()=>{
                /* POST HERE */

                var oPaymentFLoats = {
                    "payment_reference": 0,
                    "payment_date": ReturnInputValue("txtPaymentDate"),
                    "customer_id": parseInt(ReturnInputValue("customerId")),
                    "amount_paid": parseFloat(ReturnInputValue("txtAmount")),
                    "payment_mode": ReturnInputValue("selModeOfPayment"),
                    "float_payment": 1,
                    "posted_by": systemUserId,
                    "posting_datetime": "string",
                    "payment_image": ReturnInputValue("imageBase64"),
                    "image_entry_id": 0,
                    "paymentReferenceDetails": [{
                        "payment_reference": 0,
                        "invoice_reference": 0
                    }]
                }
                var strCollectionFloat = JSON.stringify(oPaymentFLoats);
                var submitNewCollection = InvokeService(GetURL(), "PaymentReferenceMethods/floatpayment", "POST", strCollectionFloat);
                if (submitNewCollection.code == 200) {
                    var oData = JSON.parse(submitNewCollection.data);
                    if (oData.code == 200) {
                        ShowSuccessModal("btnConfirmTwo", "added");
                        GetDomElement('btnCloseConfimrTwo').click();
                        GetDomElement('btnCloseAddNewModel').click();
                        PopulatePaymentFloats();
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", submitNewCollection.message, submitNewCollection.code);
                }
            }
        } else {
            this.dataset.target = "";
            GetDomElement("txtAmount").classList.add('is-invalid');
            GetDomElement('divAddNewInvalidAmountErrorAlert').classList.remove('d-none');
        }
    } else {
        this.dataset.target = "";
        GetDomElement('divAddNewInvalidAmountErrorAlert').classList.add('d-none');
    }
    /* INPUT FILE BORDER RED */
    if (proofOfPayment.files.length == 0) {
        proofOfPayment.style.border = "1px solid red";
    } else {
        proofOfPayment.style.border = "none";
    }
});

GetDomElement("btnAddNewCollection").addEventListener('click', function () {
    GetDomElement("txtCustomerName").value = "";
    GetDomElement("txtCustomerName").classList.remove('is-invalid');
    GetDomElement("txtPaymentDate").value = "";
    GetDomElement("txtPaymentDate").classList.remove('is-invalid');
    GetDomElement("txtAmount").value = "";
    GetDomElement("txtAmount").classList.remove('is-invalid');
    GetDomElement("selModeOfPayment").selectedIndex = 0;
    GetDomElement("selModeOfPayment").classList.remove('is-invalid');
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divAddNewInvalidAmountErrorAlert").classList.add('d-none');
    GetDomElement("accountDetails").innerText = "";
    proofOfPayment.style.border = "none";
    proofOfPayment.value = null;
});

function PopulatePaymentFloats() {
    GetDomElement("emptyTable").classList.add('d-none');
    var oPaymentBilling = InvokeService(GetURL(), `PaymentReferenceMethods/queryreport?dateFrom=1991-01-01&dateTo=${dateToday}&postedBy=${systemUserId}&billingType=2`, "GET", "");
    if (oPaymentBilling.code == 200) {
        var oData = JSON.parse(oPaymentBilling.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            paymentBillingTable.innerHTML = "";

            if (oJsonData.records.length != 0) {
                var jsonRecords = oJsonData.records;
                for (var i = 0; i < oJsonData.records.length; i++) {
                    paymentBillingTable.innerHTML +=
                        "<tr class='border' style='cursor:default'><td hidden>" + jsonRecords[i].payment_reference +
                        "</td><td class='text-uppercase'>" + ProofOfPaymentPopup(jsonRecords[i].payment_reference, jsonRecords[i].payment_mode, "proofPlaceholder", "paymentReferenceNo") +
                        "</td><td class='text-capitalize'>" + truncateName(jsonRecords[i].customer_name, 30) +
                        "</td><td class='text-capitalize'>" + DisplayBillingMethod(jsonRecords[i].payment_mode, paymentModeList) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_cash) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_noncash) +
                        "</td><td class='px-0 '>" +
                        "<div class='text-center'>" +
                        "<p class='text-danger btnDelete' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'" +
                        `onclick='CancelFloatPayment(${jsonRecords[i].payment_reference})'` +
                        ">" +
                        "<i  class='fas fa-trash mr-1'></i></p>" +
                        "</div>" +
                        "</td>" +
                        "</tr>";
                }
                var jsonSummary = oJsonData.summary;
                paymentBillingTable.innerHTML +=
                    "<tr class='border bg-primary' style='cursor:default'><td hidden>" +
                    "</td><td class='text-capitalize font-weight-bold text-white'>" + "Number of entries" +
                    "</td><td class='text-capitalize text-white'>" + jsonSummary.ttlRecords +
                    "</td><td class='text-capitalize text-white font-weight-bold'>" + "Total Amount" +
                    "</td><td class='text-capitalize text-white'>" + jsonSummary.ttlPaidCash +
                    "</td><td class='text-capitalize text-white'>" + jsonSummary.ttlPaidNonCash +
                    "</td><td class='text-uppercase'>" + "" +
                    "</td>" +
                    "</tr>";
                


            } else {
                paymentBillingTable.innerHTML = "";
                GetDomElement("emptyTable").classList.remove('d-none');
            }

        } else if (oData.code == 404 || oData.code == 204) {
            paymentBillingTable.innerHTML = "";
            GetDomElement("emptyTable").classList.remove('d-none');
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oPaymentBilling.code == 204 || oPaymentBilling.code == 404) {

        paymentBillingTable.innerHTML = "";
        GetDomElement("emptyTable").classList.remove('d-none');
    } else {

        ShowErrorModalOnLoad(oPaymentBilling.message, oPaymentBilling.code);
    }
}

GetDomElement("btnSearchClientRecord").addEventListener('click', function(){
    CIFTable.innerHTML = "";
    GetDomElement("txtSearchClient").value="";
});


ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");


function PopulateCustomerSelection() {

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
                    GetDomElement("accountDetails").innerHTML = `${this.cells[2].innerText} <br>${this.cells[3].innerText}`
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

function CancelFloatPayment(paymentReference) {
    GetDomElement("method").innerText = "cancel";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "cancel";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        var cancelpaymentreference = InvokeService(GetURL(), `PaymentReferenceMethods/cancelpayment/${paymentReference}`, "DELETE", "");
        if (cancelpaymentreference.code == 200) {
            var oData = JSON.parse(cancelpaymentreference.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "cancelled");
                GetDomElement('btnCloseConfimrTwo').click();
                PopulatePaymentFloats();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", cancelpaymentreference.message, cancelpaymentreference.code);
        }
    }

}