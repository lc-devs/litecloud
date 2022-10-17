var key;
var customerId;

window.addEventListener('load', function () {
    
  queryId = window.location.search.substring(1);
  jsonQuery = parse_query_string(queryId);
  customerId = jsonQuery.id;

  sessionStorage.setItem('customerId', customerId);

  queryId = window.location.search.substring(1);
  jsonQuery = parse_query_string(queryId);
  key = jsonQuery.key;

  sessionStorage.setItem('authkey', key);

    customerId = this.sessionStorage.getItem('customerId');
    var oData= InvokeServices(GetURL(), `TwoFactorAuthentication/setup/${customerId}`, 'GET', '');
    
 
    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
       
        
        if (parsedData.code == 200) {
            
            var parsedData = JSON.parse(parsedData.jsonData);

            var base64 = parsedData.BarcodeImageUrl;
            GetDomElement('imageQR').setAttribute('src', base64);
            GetDomElement('setpCode').innerHTML = parsedData.SetupCode;
            
            
            
        } else {
        ShowErrorModalOnLoad(parsedData.message, parsedData.code);
            
        }
    } else {
        ShowErrorModalOnLoad(oData.message, oData.code);
    }
});


function parse_query_string(query) {
	var vars = query.split("&");
	var query_string = {};
	for (var i = 0; i < vars.length; i++) {
		var pair = vars[i].split("=");
		var key = decodeURIComponent(pair[0]);
		var value = decodeURIComponent(pair[1]);
		// If first entry with this name
		if (typeof query_string[key] === "undefined") {
			query_string[key] = decodeURIComponent(value);
			// If second entry with this name
		} else if (typeof query_string[key] === "string") {
			var arr = [query_string[key], decodeURIComponent(value)];
			query_string[key] = arr;
			// If third or later entry with this name
		} else {
			query_string[key].push(decodeURIComponent(value));
		}
	}
	return query_string;
}

GetDomElement('btnConfirm').addEventListener('click', function () {
    var code = GetDomElement('txtCode').value;
    if (!IsEmpty('txtCode')) {
        GetDomElement('txtCode').classList.remove('is-invalid');

        var oData= InvokeServices(GetURL(), `TwoFactorAuthentication/authenticate/${customerId}?securityCode=${code}`, 'POST', '');
        if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);

            if (parsedData.code == 200) {
                location.href = 'online-customer-side-login.html';
            } else {
                GetDomElement('divUnauthorizedAlert').classList.remove('d-none');
                GetDomElement('divOtherErrorAlert').classList.add('d-none');

            }
        } else {
            GetDomElement('divOtherErrorAlert').classList.remove('d-none');
            GetDomElement('divUnauthorizedAlert').classList.add('d-none');

        }
    } else {
        GetDomElement('txtCode').classList.add('is-invalid');
    }
});


function InvokeServices(serviceURL, controller, paramMethod, paramBody) {
    var url = serviceURL + controller;
    
    var serviceResponse;
    var xhttp = new XMLHttpRequest();
    try {
        serviceResponse = new ServiceResponse(0, "", null);
        xhttp.open(paramMethod, url, false);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.setRequestHeader("UserKey", key);
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
  