//===================================================================
//Function: GetCountry
//Purpose: To get all provinces in the data table
//Author: Marlo Ellica
//Parameter:
//  Name                  Comment
//  -----------            --------------------------------
//  response            table data for all country
//
//Result: PopulateDropdownList(province, provinces[i].province_state_id, provinces[i].province_state_name);
// 
//===================================================================

function FetchCountry(dropDownCountry){
    var response = InvokeService("Entities_CountryMethods/getall", "GET", "");
    if (response.code == 200) {

        country = document.getElementById(dropDownCountry);
        country.innerHTML = "";

        var data = JSON.parse(response.data);
    
        var countries = JSON.parse(data.jsonData);
    
        country.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < countries.length; i++) {
            PopulateDropdownList(country, countries[i].country_id, countries[i].country_name);
        }
        
    }
  
}

//===================================================================
//Function: GetProvinces
//Purpose: To get all provinces in the data table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  country_id             id of the country
//
//Result: PopulateDropdownList(province, provinces[i].province_state_id, provinces[i].province_state_name);
// 
//===================================================================
function FetchProvinces(country_id, dropDownProvince) {

    var response = InvokeService("Entities_ProvinceStateMethods/getall/" + country_id, "GET", "");

    if (response.code == 200) {
        province = document.getElementById(dropDownProvince);
        province.innerHTML = "";
        var data = JSON.parse(response.data);

        var provinces = JSON.parse(data.jsonData);
        province.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < provinces.length; i++) {
            PopulateDropdownList(province, provinces[i].province_state_id, provinces[i].province_state_name);
        }

    }

}
//===================================================================
//Function: GetTown
//Purpose: To get all town in the data table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  province_id            id of the province
//
//Result: PopulateDropdownList(town, towns[i].town_id, towns[i].town_name);
// 
//===================================================================

function FetchTowns(province_id, dropDownTown) {

    var response = InvokeService("Entities_TownMethods/getall/" + province_id, "GET", "");

    if (response.code == 200) {

        town = document.getElementById(dropDownTown);
        town.innerHTML = "";
        var data = JSON.parse(response.data);

        var towns = JSON.parse(data.jsonData);
  
        town.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < towns.length; i++) {
            PopulateDropdownList(town, towns[i].town_id, towns[i].town_name);
        }

    }
}
//===================================================================
//Function: GetBarangay
//Purpose: To get all barangay in the data table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  town_id            id of the town
//
//Result:PopulateDropdownList(barangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
// 
//===================================================================


function FetchBarangays(town_id, dropDownBarangay) {

    var response = InvokeService("Entities_BarangayMethods/getall/" + town_id, "GET", "");

    if (response.code == 200) {

        barangay = document.getElementById(dropDownBarangay);
        barangay.innerHTML = "";
        var data = JSON.parse(response.data);

        var barangays = JSON.parse(data.jsonData);

        barangays.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < barangays.length; i++) {
            PopulateDropdownList(barangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
        }

    }
}

//===================================================================
//Function: SaveNewCountry
//Purpose: To save the new country from the input field
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  newCountry            new Country string
//
//Result:true/false
// 
//===================================================================


function ExecuteSaveNewCountry(newCountry){

	var isSuccess = false;
   
   //declaring the value of the paramBody...
   var objParam={
	   "country_id": 0,
	   "country_name": newCountry
	 }

	 var paramBody = JSON.stringify(objParam);

	 var response = InvokeService("Entities_CountryMethods", "POST", paramBody);

   if (response.code == 200) {	   
		isSuccess = true;
		
   }   
   return isSuccess;   
}

//===================================================================
//Function: SaveNewProvince
//Purpose: Save new Province in the dropdown or in the table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  countryId         	  id of the country
//  newProvince           province name string
//Result:true/false
// 
//===================================================================


function ExecuteSaveNewProvince(countryId, newProvince){

	var isSuccess = false;


    var objParam={
        "province_state_id": 0,
        "province_state_name": newProvince,
        "country": {
          "country_id": countryId,
          "country_name": "string"
        }
      }
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_ProvinceStateMethods", "POST", paramBody);
      
    
    if (response.code == 200) {
        isSuccess = true;
    }
	
	return isSuccess;
}
//===================================================================
//Function: SaveNewTown
//Purpose: Save new Town in the dropdown or in the table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  provinceId           id of the province
//  newTown		         name of the town string
//
//Result: true/false
// 
//===================================================================

// Populate NEW Town Inputs...

function ExecuteSaveNewTown(provinceId, newTown){

	var isSuccess = false;
    
    var objParam={
        "town_id": 0,
        "town_name": newTown,
        "province_state_id": provinceId
      }
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_TownMethods", "POST", paramBody);

    
    if (response.code == 200) {
       isSuccess = true;
    }
	return isSuccess;
     
}

//===================================================================
//Function: SaveNewBarangay
//Purpose: Save new Barangay to the dropdown or to the table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  province_id           	id of the province
//  townId           		id of the town
//  newBarangay           	name of the barangay string
//
//Result: true/false;
// 
//===================================================================

// Populate NEW Barangay Inputs...
function ExecuteSaveNewBarangay(provinceId, townId, newBarangay){

	var isSuccess = false;

    var objParam={
        "barangay_district_id": 0,
        "barangay_district_name": newBarangay,
        "town": {
          "town_id": townId,
          "town_name": "string",
          "province_state_id": provinceId
        }
      }
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_BarangayMethods", "POST", paramBody);
    
    if (response.code == 200) {
        
       isSuccess = true;

    }
	return isSuccess;
}