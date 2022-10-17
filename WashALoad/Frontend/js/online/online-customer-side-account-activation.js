var customerId;
var key;

GetDomElement('btnActivate').addEventListener('click', function () {
    var amount = document.querySelectorAll('.amount');
    var requiredAll = document.querySelectorAll('.input-required');
    var BValidForm = CheckValidForm(requiredAll, 'divRequireAlert');
    if (BValidForm) {
        if (GetDomElement('txtPassword1').value != GetDomElement('txtPassword2').value) {
            GetDomElement('divPasswordNotMatched').classList.remove('d-none');
          GetDomElement('divAddress').classList.add('d-none');
          GetDomElement('divUnauthorizedAlert').classList.add('d-none');
              GetDomElement('divOtherErrorAlert').classList.add('d-none');
        } else if (GetDomElement('latitude').innerHTML == '0' || GetDomElement('longitude').innerHTML == '0') {
            GetDomElement('divPasswordNotMatched').classList.add('d-none');
          GetDomElement('divAddress').classList.remove('d-none');
          GetDomElement('divUnauthorizedAlert').classList.add('d-none');
              GetDomElement('divOtherErrorAlert').classList.add('d-none');
        } else {
            

          var lati = GetDomElement('latitude').innerHTML;
          var longi = GetDomElement('longitude').innerHTML;
          var pass = GetDomElement('txtPassword1').value;
          
          var oData= InvokeServices(GetURL(), `CustomerMethods/onlineactivation/customer/id/${customerId}?lat=${lati}&lng=${longi}&password=${pass}`, 'PUT', '');
          
          if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {

              location.href = `google-authenticator.html?id=${customerId}&key=${key}`;
            } else if(parsedData.code == 401){
              GetDomElement('divUnauthorizedAlert').classList.remove('d-none');
              GetDomElement('divOtherErrorAlert').classList.add('d-none');
              GetDomElement('divPasswordNotMatched').classList.add('d-none');
          GetDomElement('divAddress').classList.add('d-none');
            }
            
        } else {
          GetDomElement('divOtherErrorAlert').classList.remove('d-none');
            GetDomElement('divUnauthorizedAlert').classList.add('d-none');
            GetDomElement('divPasswordNotMatched').classList.add('d-none');
          GetDomElement('divAddress').classList.add('d-none');
      
        }
         
        }
        
    } 

});

window.addEventListener('load', function () {

  queryId = window.location.search.substring(1);
  jsonQuery = parse_query_string(queryId);
  customerId = jsonQuery.id;

  sessionStorage.setItem('customerId', customerId);

  queryId = window.location.search.substring(1);
  jsonQuery = parse_query_string(queryId);
  key = jsonQuery.key;

  sessionStorage.setItem('authkey', key);

  
  var oData= InvokeServices(GetURL(), `CustomerMethods/search/customer/id/${customerId}`, 'GET', '');
  if (oData.code == 200) {
      var parsedData = JSON.parse(oData.data);
      if (parsedData.code == 200) {
          
          var parsedDatas = JSON.parse(parsedData.jsonData);
          GetDomElement('name').innerHTML = parsedDatas[0].customer_name;
          GetDomElement('email').innerHTML = parsedDatas[0].email_address;
          GetDomElement('phoneNo').innerHTML = parsedDatas[0].cellular_number;
          GetDomElement('address').innerHTML = capitalize(parsedDatas[0].street_building_address) + " " +
                                                    capitalize(parsedDatas[0].barangay_address) + ", " +
                                                    capitalize(parsedDatas[0].town_address) + ", " +
                                                    capitalize(parsedDatas[0].province);
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

let map;
let markers = [];

function initMap() {
  const cebu = { lat: 10.380051465330913, lng:123.74672781399734 };

  map = new google.maps.Map(document.getElementById("map"), {
    zoom: 12,
    center: cebu,
    mapTypeId: "roadmap",
  });
  // This event listener will call addMarker() when the map is clicked.
  map.addListener("click", (event) => {
    addMarker(event.latLng);
  });
  // add event listeners for the buttons

  addMarker(cebu);
}

// Adds a marker to the map and push to the array.
function addMarker(position) {
    // console.log(position.toJSON());
    GetDomElement('latitude').innerHTML = position.toJSON().lat;
    GetDomElement('longitude').innerHTML = position.toJSON().lng;

    deleteMarkers();
    const marker = new google.maps.Marker({
        position,
        map,
    });

    markers.push(marker);
        
}

// Sets the map on all markers in the array.
function setMapOnAll(map) {
  for (let i = 0; i < markers.length; i++) {
    markers[i].setMap(map);
    }
    
}

// Removes the markers from the map, but keeps them in the array.
function hideMarkers() {
  setMapOnAll(null);
}

// Shows any markers currently in the array.
function showMarkers() {
  setMapOnAll(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
  hideMarkers();
  markers = [];
}

window.initMap = initMap;



