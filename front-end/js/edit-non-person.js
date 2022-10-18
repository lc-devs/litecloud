//container for filling up input

company=document.getElementById("company_input");
contactperson=document.getElementById("contactperson_input");
country=document.getElementById("country-input");
province=document.getElementById("province-input");
town=document.getElementById("town-input");
barangay=document.getElementById("barangay-input");
zipcode=document.getElementById("zipcode-input");
tin=document.getElementById("tin-input");
bldg=document.getElementById("bldg-input");
email=document.getElementById("email-input");
landline1=document.getElementById("landline1-input");
mobile1=document.getElementById("mobile1-input");
inputs1=[company.value, contactperson.value, country.text, province.text, town.text, barangay.text, bldg.value];



 function CheckDetailEntries(){
    //Check for any errors for a multiple fields...
  err=0;
    //
    for (var i=0; i=inputs1.length; i++){
        if(inputs1[i]==""){
            err++
        }
    }
    if(company.value==""){
        company.style.borderColor="red";
    }else{
        company.style.borderColor="#ced4da";
    }
    if(contactperson.value==""){
        company.style.borderColor="red";
    }else{
        company.style.borderColor="#ced4da";
    }
    if(country.text==""){
        country.style.borderColor="red";
    }else{
        country.style.borderColor="#ced4da";
    }
    if(province.text==""){
        province.style.borderColor="red";
    }else{
        province.style.borderColor="#ced4da";
    }
    if(town.text==""){
        town.style.borderColor="red";
    }else{
        town.style.borderColor="#ced4da";
    }
    if(barangay.text==""){
        barangay.style.borderColor="red";
    }else{
        barangay.style.borderColor="#ced4da";
    }
    if(bldg.value==""){
        bldg.style.borderColor="red";
    }else{
        bldg.style.borderColor="#ced4da";
    }
    if(email.value==""){
        email.style.borderColor="red";
    }else{
        email.style.borderColor="#ced4da";
    }
    //for (var i=0; i)
 }

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
//populate Country Dropdown list
var response = InvokeService("Entities_CountryMethods/getall", "GET", "");
if (response.code == 200) {
    country = document.getElementById("country-input");
    country.innerHTML = "";
    var data = JSON.parse(response.data);

    var countries = JSON.parse(data.jsonData);

    country.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";

    for (var i = 0; i < countries.length; i++) {
        PopulateDropdownList(country, countries[i].country_id, countries[i].country_name);
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
function GetProvinces() {

    country = document.getElementById("country-input");

    var country_id = country.value;

    var response = InvokeService("Entities_ProvinceStateMethods/getall/" + country_id, "GET", "");

    if (response.code == 200) {
        province = document.getElementById("province-input");
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

function GetTowns() {

    province = document.getElementById("province-input");


    var province_id = province.value;

    var response = InvokeService("Entities_TownMethods/getall/" + province_id, "GET", "");

    if (response.code == 200) {

        town = document.getElementById("town-input");
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


function GetBarangays() {

    town = document.getElementById("town-input");


    var town_id = town.value;

    var response = InvokeService("Entities_BarangayMethods/getall/" + town_id, "GET", "");

    if (response.code == 200) {

        barangay = document.getElementById("barangay-input");
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
//Function: SaveEditCountry
//Purpose: To save the edited country from the input field
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  country_id            id of the town
//
//Result:
// 
//===================================================================


function SaveEditCountry(){
    //declaring the value of the paramBody...
    var paramBody={
        "country_id": 0,
        "country_name": "string"
      }
      //value of the input field of Add province...
      newCountry=document.getElementById("add-country-input").value;

      var response = InvokeService("Entities_CountryMethods", "POST", paramBody);
      paramBody.country_name=newCountry;  //newcounry value is the value for country_name...

    //getting the paramBody     
    
    if (response.code == 200) {
     
        country = document.getElementById("country-input");

        var data = JSON.parse(response.data); //converting json string to json object(array)
        console.log(data.jsonData);

        var nCountries = JSON.parse(data.jsonData); //converting json string to json object(array)
        
        country.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        
       
        
    }
}
//===================================================================
//Function: SaveEditProvince
//Purpose: To save the edited province from the input field
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  province_id           id of the province
//
//Result:
// 
//===================================================================


function SaveEditProvince(){

    //declaring the value of the paramBody...

    var paramBody={
        "province_state_id": 0,
        "province_state_name": "string"
      }
      //value of the input field of Add province...
      newProvince=document.getElementById("add-province-input").value;

      var response = InvokeService("Entities_ProvinceMethods", "POST", paramBody);
      paramBody.province_state_name=newProvince; //newProvince value is the value for province_state_name

    //getting the paramBody
    
    if (response.code == 200) {
     
        province = document.getElementById("province-input");
        province.innerHTML = "";

        var data = JSON.parse(response.data); //converting json string to json object(array)
        console.log(response.data);

        var newprovinces = JSON.parse(data.jsonData); //converting json string to json object(array)
        
        province.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>"; 
    }
}

//===================================================================
//Function: SaveEditTown
//Purpose: To save the edited town from the input field
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  town_id           id of the town
//
//Result:
// 
//===================================================================

// Populate NEW Town Inputs...

function SaveEditTown(){
    //declaring the value of the paramBody...
    var paramBody={
        "town_id": 0,
        "town_name": "string"
      }
      //getting the value of the add Town input field...

      newTown=document.getElementById("add-town-input").value;

      var response = InvokeService("Entities_TownMethods", "POST", paramBody);
      paramBody.town_name=newTown;

    //getting the paramBody
    
    if (response.code == 200) {
     
        town = document.getElementById("town-input");
        town.innerHTML = "";

        var data = JSON.parse(response.data); //converting json string to json object(array)
        console.log(response.data);

        var newtowns = JSON.parse(data.jsonData); //converting json string to json object(array)
        
        town.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    }
}

//===================================================================
//Function: SaveEditBarangay
//Purpose: To save the edited barangay from the input field
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  province_id           id of the province
//
//Result:PopulateDropdownList(barangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
// 
//===================================================================

function SaveEditBarangay(){
    
    var paramBody={
        "barangay_district_id": 0,
        "barangay_district_name": "string"
      }
        newBarangayDistrict=document.getElementById("add-barangay-input").value;
        paramBody.barangay_district_name=newBarangayDistrict;

        var response = InvokeService("Entities_TownMethods", "POST", paramBody);

    //getting the paramBody
    
    if (response.code == 200) {
     
        town = document.getElementById("town-input");
        town.innerHTML = "";

        var data = JSON.parse(response.data); //converting json string to json object(array)
        console.log(response.data);

        var newtowns = JSON.parse(data.jsonData); //converting json string to json object(array)
        
        barangay.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        
       
        
    }
}
 //for going back to the previous page

 function GoBackToNonPersonPage(){
     window.location.href=("nonperson.html")
 }
 function GoToNonPersonPageForm(){
    window.location.href=("add-nonperson-customer-management.html")
}
 
 function search_customer() {
     var input, filter, table, tr, td, i, txtValue;
     input = document.getElementById("searchbar");
     filter = input.value.toUpperCase();
     table = document.getElementById("searchpersons");
     tr = table.getElementsByTagName("tr");
     for (i = 0; i < tr.length; i++) {
       td = tr[i].getElementsByTagName("td")[0];
       if (td) {
         txtValue = td.textContent || td.innerText;
         if (txtValue.toUpperCase().indexOf(filter) > -1) {
           tr[i].style.display = "";
         } else {
           tr[i].style.display = "none";
         }
       }       
     }
    }


// Going to Edit Non-Person Management

function RedirectToEditNonPersonManagement(){
    window.location.href=("edit-non-person-page.html");
}