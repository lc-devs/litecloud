
var locations = [];
var customerId = [];
function populateTableQuery(parsedData) {
    var divtbl = GetDomElement('tblQuery');
    divtbl.innerHTML = "";
    
    for (var i = 0; i < parsedData.length; i++) {
        
        locations.push({
            'lat': parsedData[i].latitude,
            'lng': parsedData[i].longitude,
            'id' : parsedData[i].customer_id
        })
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0 text-warning' >" + `${parsedData[i].booking_reference == '' ? "<label class='text-danger'>N/A</label>" : parsedData[i].booking_reference}` + "</td>" +
            "<td class='border text-center px-0 text-info text-capitalize'>" + parsedData[i].customer_name + "</td>" +
            "<td class='border text-center px-0'>" + getType(parsedData[i].customer_type) +"</td>" +
           // "<td class='border text-center'><i class='fas fa-trash text-danger fa-lg deleteItems' style='cursor: pointer!important;' ></i></td>" +
            "</tr>";

        // var soReference = document.querySelectorAll('.so-reference')
        // for (var i = 0; i < soReference.length; i++){
            
        //     soReference[i].addEventListener('click', function () {

        //     });
        // }
        

    }
}

function getType(type) {
    if (type == 'Industrial') {
        return `<div class='d-flex justify-content-between'>
        <button class='btn btn-secondary mx-auto' onclick='pickupData(this, 1)'>Industrial</button>
        <button class='btn btn-light mr-auto text-light' disabled>NonIndustrial</button>
       </div>`
    } else if (type == 'Non-Idustrial') {
        return `<div class='d-flex justify-content-between'>
        <button class='btn btn-light mx-auto text-light' disabled>Industrial</button>
        <button class='btn btn-dark mr-auto' onclick='pickupData(this, 0)'>NonIndustrial</button>
       </div>`
        
    } else if (type == 'Industrial/Non-Idustrial') {
        return `<div class='d-flex justify-content-between'>
        <button class='btn btn-secondary mx-auto' onclick='pickupData(this, 1)'>Industrial</button>
        <button class='btn btn-dark mr-auto' onclick='pickupData(this, 0)'>NonIndustrial</button>
       </div>`
    }
}
function pickupData(data, type) {
    sessionStorage.setItem('bookingReference', data.parentNode.parentNode.parentNode.cells[0].innerHTML);
    if (type == 1) {
        location.href = 'online-industrial-pickup-with-booking.html';
    } else {
        location.href = 'online-non-industrial-pickup-with-booking.html';
    }
}



window.addEventListener('load', function () {
    var dateNow = new Date();
    var txtFrom = dateNow.toISOString().slice(0, 10);
    var txtTo = dateNow.toISOString().slice(0, 10);
    
    var pickUpby = sessionStorage.getItem('userId');

    var date1 = dateNow.toLocaleDateString('en-GB');
    var year = date1.slice(6,10);
    var month = date1.slice(3,5);
    var day = date1.slice(0, 2);
    var txtdate = year + '-' + month + '-' + day;
    

    // var oData= InvokeService(GetURL(), `PickupIndustrialMethods?dateFrom=${txtFrom}&dateTo=${txtTo}&customerID=0&pickupBy=${pickUpby}&isAllEntries=false}&isForPickupOnly=true`, 'GET', '');

    //     if (oData.code == 200) {
    //         var parsedData = JSON.parse(oData.data);
    //         if (parsedData.code == 200) {
    //             var parsedData = JSON.parse(parsedData.jsonData);
    //             // var parsedData = JSON.parse(parsedData);
    //             populateTableQuery(parsedData)
    //         } else {
                
    //             ShowErrorModalOnLoad('No Data Found for Today', parsedData.code);

    //         }
    //     } else {
    //         ShowErrorModalOnLoad(oData.message, oData.code);

    // }
    
    var oData= InvokeService(GetURL(), `BookingsForPickupMethods/bookings/dates/${txtdate}`, 'GET', '');
    
    if (oData.code == 200) {
            var parsedData = JSON.parse(oData.data);
            if (parsedData.code == 200) {
                GetDomElement('btnViewMap').classList.remove('d-none');

                var parsedData = JSON.parse(parsedData.jsonData);

               
                populateTableQuery(parsedData);
                if (locations.length > 0) {
                    for (var i = 0; i < locations.length; i++){
                        addMarker(locations[i]);
                    }

                }

            } else if(parsedData.code == 401) {
                
                ShowErrorModalOnLoad('', parsedData.code);

            } else {
                GetDomElement('btnViewMap').classList.add('d-none');
                GetDomElement('tblHeader').innerHTML = 'No Data Found for Today';
                GetDomElement('tblHeader').classList.add('bg-transparent');
            }
        } else {
            ShowErrorModalOnLoad(oData.message, oData.code);

        }
});



ClosePopupModal("btnOKSODetails", "btnCloseSODetails");
ClosePopupModal("btnOKMap", "btnCloseMap");
// ClosePopupModal("btnCloseFailedAlert", "btnCloseFailedEntry");


let map;
let markers = [];

function initMap() {
  

  map = new google.maps.Map(document.getElementById("map"), {
    zoom: 12,
    center: { lat: 10.380051465330913, lng:123.74672781399734 },
    mapTypeId: "roadmap",
  });
  // This event listener will call addMarker() when the map is clicked.
  
  // add event listeners for the buttons

//   addMarker(cebu);
    
}

// Adds a marker to the map and push to the array.
function addMarker(position) {
    
    
    const marker = new google.maps.Marker({
        position,
        map,
    });
   
    markers.push(marker);

    marker.addListener("click", () => {

        var infowindow = new google.maps.InfoWindow({
            content: getInfo(position.id),
        });
        
        infowindow.open({
            anchor: marker,
            map,
            shouldFocus: false,
        });
      });
    
    // new google.maps.event.trigger( marker, 'click' );
}
function getInfo(id) {
    var content = '';
    var oData = InvokeService(GetURL(), `CustomerMethods/search/customer/id/${id}`, 'GET', '');
    if (oData.code == 200) {
        var parsedData = JSON.parse(oData.data);
        if (parsedData.code == 200) {
            var parsedData = JSON.parse(parsedData.jsonData);
            content = `
            <h5>${parsedData[0].customer_name} </h5>
            <label>${parsedData[0].cellular_number}</label><br>
            <label>${parsedData[0].email_address}</label><br>
            <label class='text-capitalize'>${parsedData[0].street_building_address}, ${parsedData[0].barangay_address}, ${parsedData[0].town_address}, ${parsedData[0].province}</label>
            `
        } else {
            // ShowErrorModalOnLoad(parsedData.message, parsedData.code);
            
        }
    } else {
        ShowErrorModalOnLoad(oData.message, oData.code);
        
    }
    return content;
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
