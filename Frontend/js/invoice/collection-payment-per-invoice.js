var proofOfPayment = GetDomElement("paymentProof");
var paymentBillingTable = GetDomElement("tblBody");
var requiredPaymentProof = this.document.querySelectorAll('.payment_proof');
var paymentModeList = [];
var filteredPaymentList = [];
var systemUserId = sessionStorage.getItem("userId");
var dateToday = new Date().toISOString().split('T')[0];
var invoiceReference = GetDomElement("txtInvoiceReference");
var customerId = 0;
var isValidInvoice = 0;


window.addEventListener('load', function () {
    GetAllPaymentMethods();
    FilterPaymentMethods(paymentModeList, filteredPaymentList);
    PopulatePaymentPerInvoice();

    if(filteredPaymentList.length < 1){
        PopulateFilteredPaymentMethods("selModeOfPayment", paymentModeList)
   }else{
        PopulateFilteredPaymentMethods("selModeOfPayment", filteredPaymentList)
   }

    //PopulateFilteredPaymentMethods("selModeOfPayment", paymentModeList)
    ShowHideInputWithLabel(requiredPaymentProof, 0);
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
                PopulateInvoiceReference();
                if (isValidInvoice == 1) {
                    var oCollectionInvoice = {
                        "payment_reference": 0,
                        "payment_date": ReturnInputValue("txtPaymentDate"),
                        "customer_id": customerId,
                        "amount_paid": parseFloat(ReturnInputValue("txtAmount")),
                        "payment_mode": ReturnInputValue("selModeOfPayment"),
                        "float_payment": 0,
                        "posted_by": systemUserId,
                        "posting_datetime": "1991-01-01",
                        "payment_image": ReturnInputValue("imageBase64"),
                        "image_entry_id": 0,
                        "paymentReferenceDetails": [{
                            "payment_reference": 0,
                            "invoice_reference": 0
                        }]
                    }
                    var strCollectionInvoice = JSON.stringify(oCollectionInvoice);
                    var submitNewCollection = InvokeService(GetURL(), `PaymentReferenceMethods/fullpaymentbyinvoice/${invoiceReference.value}`, "POST", strCollectionInvoice);
                    if (submitNewCollection.code == 200) {
                        var oData = JSON.parse(submitNewCollection.data);
                        if (oData.code == 200) {
                            ShowSuccessModal("btnConfirmTwo", "added");
                            GetDomElement('btnCloseConfimrTwo').click();
                            GetDomElement('btnCloseAddNewModel').click();
                            PopulatePaymentPerInvoice();
                        } else {
                            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                        }

                    } else {
                        ShowErrorModal("btnConfirmTwo", submitNewCollection.message, submitNewCollection.code);
                    }
                } else {
                    ShowErrorModal("btnConfirmTwo", "Invalid Invoice Reference", 500);
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

GetDomElement("btnSearchInvoice").addEventListener("click", function () {
    PopulateInvoiceReference();
});

GetDomElement("txtInvoiceReference").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        PopulateInvoiceReference();
    }
});

function PopulateInvoiceReference() {
    GetDomElement("invalidPaymentError").classList.add('d-none');
    var InvoiceType = GetDomElement("selInvoiceType").value;
    var oInvoiceReference = InvokeService(GetURL(), `${InvoiceType}/invoicedetails/${invoiceReference.value}`, "GET", "");
    if (oInvoiceReference.code == 200) {
        GetDomElement("invalidPaymentError").classList.add('d-none');
        var oData = JSON.parse(oInvoiceReference.data);
        if (oData.code == 200) {
            var oJsonData = JSON.parse(oData.jsonData);
            isValidInvoice = 1;
            customerId = oJsonData[0].customer_id;
            GetDomElement("accountDetails").innerText = oJsonData[0].customer_name;
            invoiceReference.classList.remove("is-invalid");

        } else if (oData.code == 404 || oData.code == 204) {
            GetDomElement("invalidPaymentError").classList.remove('d-none');
            GetDomElement("accountDetails").innerText = "";
            isValidInvoice = 0;
            customerId = 0;
            invoiceReference.classList.add("is-invalid");
        } else {
            isValidInvoice = 0;
            customerId = 0;
            ShowErrorModalOnLoad(oData.message, oData.code);
        }
    } else if (oInvoiceReference.code == 404 || oInvoiceReference.code == 204) {
        GetDomElement("invalidPaymentError").classList.remove('d-none');
        GetDomElement("accountDetails").innerText = "";
        invoiceReference.classList.add("is-invalid");
    } else {
        ShowErrorModalOnLoad(oInvoiceReference.message, oInvoiceReference.code);
    }

}

GetDomElement("btnAddNewCollection").addEventListener('click', function () {
    GetDomElement("txtPaymentDate").value = "";
    GetDomElement("txtPaymentDate").classList.remove('is-invalid');
    GetDomElement("txtInvoiceReference").value = "";
    GetDomElement("txtInvoiceReference").classList.remove('is-invalid');
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

function PopulatePaymentPerInvoice() {
    GetDomElement("emptyTable").classList.add('d-none');
    var oPaymentBilling = InvokeService(GetURL(), `PaymentReferenceMethods/queryreport?dateFrom=1991-01-01&dateTo=${dateToday}&postedBy=${systemUserId}&billingType=1`, "GET", "");
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
                        "</td><td class='text-capitalize'>" + jsonRecords[i].invoice_reference +
                        "</td><td class='text-capitalize'>" + DisplayBillingMethod(jsonRecords[i].payment_mode, paymentModeList) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_cash) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_noncash) +
                        "</td><td class='px-0 '>" +
                        "<div class='text-center'>" +
                        "<p class='text-danger btnDelete' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'" +
                        `onclick='CancelPaymentPerInvoice(${jsonRecords[i].payment_reference})'` +
                        ">" +
                        "<i  class='fas fa-trash mr-1'></i></p>" +
                        "</div>" +
                        "</td>" +
                        "</tr>";
                }
                var jsonSummary = oJsonData.summary;
                paymentBillingTable.innerHTML +=
                    "<tr class='border bg-primary' style='cursor:default'><td hidden>" +
                    "</td><td class='text-uppercase'>" + "" +
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
ClosePopupModal("btnCancelOne", "btnCloseConfirm1");
ClosePopupModal("btnConfirmSuccessOne", "btnCloseConfirm1");
ClosePopupModal("btnCancelTwo", "btnCloseConfimrTwo");
ClosePopupModal("btnCloseSuccessEntry", "btnCloseSuccessAlert");
ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");

function CancelPaymentPerInvoice(paymentReference) {
    GetDomElement("method").innerText = "cancel";
    GetDomElement("method").classList.add("text-danger");
    GetDomElement("confirmTwoMethod").innerText = "cancel";
    GetDomElement("confirmTwoMethod").classList.add("text-danger");

    GetDomElement("btnConfirmTwo").onclick = ()=>{
        this.diabled = true;
        var cancelpaymentreference = InvokeService(GetURL(), `PaymentReferenceMethods/cancelpayment/${paymentReference}`, "DELETE", "");

        if (cancelpaymentreference.code == 200) {
            this.diabled = false;
            var oData = JSON.parse(cancelpaymentreference.data);
            if (oData.code == 200) {
                ShowSuccessModal("btnConfirmTwo", "cancelled");
                GetDomElement('btnCloseConfimrTwo').click();
                PopulatePaymentPerInvoice();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", cancelpaymentreference.message, cancelpaymentreference.code);
        }
    }

}