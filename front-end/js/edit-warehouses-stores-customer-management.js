const params = new Proxy(new URLSearchParams(window.location.search), {
	get: (searchParams, prop) => searchParams.get(prop),
  });
  // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
  let id = params.id;



//container for filling up input
document.getElementById("unit_location_id").value = id;
description = document.getElementById("location_input");
unit=document.getElementById("unit-incharge");
warehousechecked=document.getElementById("warehouse-checked");
storechecked=document.getElementById("store-checked");
var country=document.getElementById("country-input");
var province=document.getElementById("province-input");
var town=document.getElementById("town-input");
var barangay=document.getElementById("barangay-input");
// var zipcode=document.getElementById("zipcode-input");
// var tin=document.getElementById("tin-input");
var bldg=document.getElementById("bldg-input");
var email1=document.getElementById("email-input");
// var email2=document.getElementById("email-input1");
// var email3=document.getElementById("email-input2");
var landline1=document.getElementById("landline1-input");
var landline2=document.getElementById("landline2-input");
var mobile1=document.getElementById("mobile1-input");
var mobile2=document.getElementById("mobile2-input");
var errorAlert=document.getElementById("alrt");


//Get Person
var response = InvokeService("Entities_PersonMethods/searchall/", "GET", "");

    if (response.code == 200) {

        unit.innerHTML = "";
        var data = JSON.parse(response.data);

        if(data.code == 200){

            var person = JSON.parse(data.jsonData);
  
            unit.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
            for (var i = 0; i < person.length; i++) {
                PopulateDropdownList(unit, person[i].person_id, person[i].firstname + " " +  person[i].lastname);
            }
        }else{
            PopulateDropdownList(unit, 0, data.message);
        }

       
    }
//

//Fetch Country
FetchCountry("country-input");
//----------------------------

//Populate unit location details
response = InvokeService("Inventory_InventoryUnitLocation/getone/" + id, "GET", "");

if (response.code == 200) {

	var data = JSON.parse(response.data);

	if(data.code == 200){

		var oUnitLocation = JSON.parse(data.jsonData);

		description.value = oUnitLocation[0].description;
		$("#unit-incharge").val(oUnitLocation[0].person_incharge).change();
		oUnitLocation[0].warehouse == 1? warehousechecked.checked = true:storechecked.checked = true;
		$("#country-input").val(oUnitLocation[0].country_id).change();
		SaveNewProvince();
		$("#province-input").val(oUnitLocation[0].province_id).change();
		GetTowns();
		$("#town-input").val(oUnitLocation[0].town_id).change();
		GetBarangays();
		$("#barangay-input").val(oUnitLocation[0].barangay_id).change();
		bldg.value = oUnitLocation[0].bldg_street_address;
		email1.value = oUnitLocation[0].email_address;
		landline1.value = oUnitLocation[0].landline_nos1;
		landline2.value = oUnitLocation[0].landline_nos2;
		mobile1.value = oUnitLocation[0].mobile_nos1;
		mobile2.value = oUnitLocation[0].mobile_nos2;

		
	}else{
		ShowModal("The system encounter an error. Please try again later.", false, "warehouse-stores.html");
	}

	
}
