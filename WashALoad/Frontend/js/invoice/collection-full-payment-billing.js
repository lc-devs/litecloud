var proofOfPayment = GetDomElement("paymentProof");
var encodedImage;
var paymentModeList = [];
var filteredPaymentList = [];
var requiredPaymentProof = this.document.querySelectorAll('.payment_proof');
var requiredFlag = 0;
var floatFlag = 0;
var paymentBillingTable = GetDomElement("tblBody");
var systemUserId = sessionStorage.getItem("userId");
var dateToday = new Date().toISOString().split('T')[0];

window.addEventListener('load', function () {
    GetAllPaymentMethods();
    FilterPaymentMethods(paymentModeList, filteredPaymentList);
    PopulateFullPaymentBilling();
   if(filteredPaymentList.length < 1){
        PopulateFilteredPaymentMethods("selModeOfPayment", paymentModeList)
   }else{
        PopulateFilteredPaymentMethods("selModeOfPayment", filteredPaymentList)
   }
    
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
    floatFlag = this.options[selectedI].dataset.float;
});

GetDomElement('btnSubmitNew').addEventListener('click', function () {
    GetDomElement('divProofOfPaymentTooLarge').classList.add('d-none');
    var billingReference = parseInt(ReturnInputValue("txtBillingReference"));
    proofOfPayment.style.border = "none";
    this.dataset.target = "";
    var oRequiredFeilds = document.querySelectorAll('.input-required');
    var bValidForm = CheckValidForm(oRequiredFeilds, "divAddNewInvalidForm");
    var amount = GetDomElement("txtAmount").value;
    if (bValidForm) {
        if (!isNaN(amount)) {
            GetDomElement('divAddNewInvalidAmountErrorAlert').classList.add('d-none');
            GetDomElement("txtAmount").classList.remove('is-invalid');

            /* POST HERE */
            if (proofOfPayment.files.length > 0) {
                if (proofOfPayment.files[0].size > 1048576) {
                    GetDomElement('divProofOfPaymentTooLarge').classList.remove('d-none');
                    proofOfPayment.style.border = "1px solid red";
                    return;
                }
            }
            GetDomElement('divProofOfPaymentTooLarge').classList.add('d-none');
            proofOfPayment.style.border = "none";
            this.dataset.target = "#btnConfirm1";
            GetDomElement("method").innerText = "add";
            GetDomElement("method").classList.add("text-success");
            GetDomElement("confirmTwoMethod").innerText = "add";
            GetDomElement("confirmTwoMethod").classList.add("text-success");
            EncodeBASE64("paymentProof", "imageBase64");
            GetDomElement("btnConfirmTwo").onclick = ()=>{
                // Validate billing reference no.

                var searchBilling = InvokeService(GetURL(), `BillingMethods/billingcustomerdetails/${billingReference}`, "GET", "");
                if (searchBilling.code == 200) {
                    var oData = JSON.parse(searchBilling.data);
                    if (oData.code == 200) {
                        oJsonResponse = JSON.parse(oData.jsonData);
                        var oCollectionFull = {
                            "payment_reference": 0,
                            "payment_date": ReturnInputValue("txtPaymentDate"),
                            "customer_id": oJsonResponse.customer_id,
                            "amount_paid": parseFloat(ReturnInputValue("txtAmount")),
                            "payment_mode": ReturnInputValue("selModeOfPayment"),
                            "float_payment": parseInt(floatFlag),
                            "posted_by": systemUserId,
                            "posting_datetime": "2022-04-18",
                            "payment_image": ReturnInputValue("imageBase64"),
                            "image_entry_id": 0,
                            "paymentReferenceDetails": [{
                                "payment_reference": 0,
                                "invoice_reference": 0
                            }]
                        }

                        var strCollectionFull = JSON.stringify(oCollectionFull);
                        var submitNewCollection = InvokeService(GetURL(), `PaymentReferenceMethods/fullpaymentbybilling/${billingReference}`, "POST", strCollectionFull);
                        if (submitNewCollection.code == 200) {
                            var oData = JSON.parse(submitNewCollection.data);
                            if (oData.code == 200) {
                                ShowSuccessModal("btnConfirmTwo", "added");
                                GetDomElement('btnCloseConfimrTwo').click();
                                GetDomElement('btnCloseAddNewModel').click();
                                PopulateFullPaymentBilling();
                            } else {
                                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                            }

                        } else {
                            ShowErrorModal("btnConfirmTwo", submitNewCollection.message, submitNewCollection.code);
                        }
                    } else if (oData.code == 404) {
                        ShowErrorModal("btnConfirmTwo", "Invalid Billing Reference", 404);
                    } else {
                        ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
                    }

                } else {
                    ShowErrorModal("btnConfirmTwo", searchBilling.message, searchBilling.code);
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
        /* INPUT FILE BORDER RED */
        if (requiredFlag = 1) {
            if (proofOfPayment.files.length == 0) {
                proofOfPayment.style.border = "1px solid red";
            } else {
                proofOfPayment.style.border = "none";
            }
        }

    }

});

GetDomElement("searchBillingReference").addEventListener('click', function () {
    PopulateBillingReference();
});

GetDomElement("txtBillingReference").addEventListener('keypress', function (e) {
    if (e.key == "Enter") {
        PopulateBillingReference();
    }
});

GetDomElement("btnAddNewCollection").addEventListener('click', function () {
    GetDomElement("billingNotFound").classList.add('d-none');
    GetDomElement("billingNotFound").classList.remove('d-sm-flex');
    GetDomElement("txtPaymentDate").value = "";
    GetDomElement("txtPaymentDate").classList.remove('is-invalid');
    GetDomElement("txtBillingReference").value = "";
    GetDomElement("txtBillingReference").classList.remove('is-invalid');
    GetDomElement("txtAmount").value = "";
    GetDomElement("txtAmount").classList.remove('is-invalid');
    GetDomElement("selModeOfPayment").selectedIndex = 0;
    GetDomElement("selModeOfPayment").classList.remove('is-invalid');
    GetDomElement("divAddNewInvalidForm").classList.add('d-none');
    GetDomElement("divAddNewInvalidAmountErrorAlert").classList.add('d-none');
    GetDomElement('divProofOfPaymentTooLarge').classList.add('d-none');
    GetDomElement("accountDetails").innerText = "";
    proofOfPayment.style.border = "none";
    proofOfPayment.value = null;
});

function PopulateFullPaymentBilling() {
    GetDomElement("emptyTable").classList.add('d-none');
    var oPaymentBilling = InvokeService(GetURL(), `PaymentReferenceMethods/queryreport?dateFrom=1991-01-01&dateTo=${dateToday}&postedBy=${systemUserId}&billingType=0`, "GET", "");
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
                        "</td><td class='text-uppercase'>" + ProofOfPaymentPopup(jsonRecords[i].payment_reference, jsonRecords[i].payment_mode,"proofPlaceholder", "paymentReferenceNo") +
                        "</td><td class='text-capitalize'>" + truncateName(jsonRecords[i].customer_name, 30) +
                        "</td><td class='text-capitalize'>" + jsonRecords[i].billing_reference +
                        "</td><td class='text-capitalize'>" + DisplayBillingMethod(jsonRecords[i].payment_mode, paymentModeList) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_cash) +
                        "</td><td class='text-capitalize'>" + reformatAmount(jsonRecords[i].paid_noncash) +
                        "</td><td class='px-0 '>" +
                        "<div class='text-center'>" +
                        "<p class='text-danger btnDelete' data-toggle='modal' data-target='#btnConfirm1' style='cursor:pointer'" +
                        `onclick='CancelPayment(${jsonRecords[i].payment_reference})'` +
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


function CancelPayment(paymentReference) {
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
                PopulateFullPaymentBilling();
            } else {
                ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
            }

        } else {
            ShowErrorModal("btnConfirmTwo", cancelpaymentreference.message, cancelpaymentreference.code);
        }
    }

}


function PopulateBillingReference() {
    var txtBillingReference = parseInt(ReturnInputValue("txtBillingReference"));
    var searchBilling = InvokeService(GetURL(), `BillingMethods/billingcustomerdetails/${txtBillingReference}`, "GET", "");
    if (searchBilling.code == 200) {
        var oData = JSON.parse(searchBilling.data);
        if (oData.code == 200) {
            var jsonData = JSON.parse(oData.jsonData);
            GetDomElement("billingNotFound").classList.add('d-none');
            GetDomElement("billingNotFound").classList.remove('d-sm-flex');
            GetDomElement("customerID").value = jsonData.customer_id;
            var customerDetails = `<p>${jsonData.customer_name} <br>${jsonData.address} </p>`;
            GetDomElement("accountDetails").innerHTML = customerDetails;
        } else if (oData.code == 404) {
            GetDomElement("customerID").value = "";
            GetDomElement("accountDetails").innerHTML = "";
            GetDomElement("billingNotFound").classList.remove('d-none');
            GetDomElement("billingNotFound").classList.add('d-sm-flex');
        } else {
            ShowErrorModal("btnConfirmTwo", oData.message, oData.code);
        }

    } else {
        ShowErrorModal("btnConfirmTwo", searchBilling.message, searchBilling.code);
    }
}
