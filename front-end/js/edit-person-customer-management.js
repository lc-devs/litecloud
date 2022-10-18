
//container for filling up input from add person customer management page...
var lastname=document.getElementById("lastname-input");
var firstname=document.getElementById("firstname-input");
var middlename=document.getElementById("middlename-input");
var dateofbirth=document.getElementById("dateofbirth-input");
var gender=document.getElementById("gender-input");
var status=document.getElementById("status-input");
var country=document.getElementById("country-input");
var province=document.getElementById("province-input");
var town=document.getElementById("town-input");
var barangay=document.getElementById("barangay-input");
var zipcode=document.getElementById("zipcode-input");
var tin=document.getElementById("tin-input");
var bldg=document.getElementById("bldg-input");
var email1=document.getElementById("email1-input");
var email2=document.getElementById("email2-input");
var email3=document.getElementById("email3-input");
var landline1=document.getElementById("landline1-input");
var landline2=document.getElementById("landline2-input");
var mobile1=document.getElementById("mobile1-input");
var mobile2=document.getElementById("mobile2-input");
var gender=document.getElementById("sex-input");   
var errorAlert=document.getElementById("alrt");


 

//===================================================================
//Function: CheckDetailEntries
//Purpose: To validate the details inputs by the user
//Author: Marlo Ellica
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  Inputs         I           inputs from input field
//
//Result:
// success or error alert
//===================================================================
function CheckDetailEntries(){
//VARIABLES FOR ARRAY...that contains the inputs from the form.
var Inputs=[country.text, province.text, town.text, barangay.text, bldg.value, email1.value, landline1.value || mobile1.value ]
//Variable to keep track of Errors. Initialize to 0.
var err = 0;

//Start loop to validate.
for (var i = 0; i < Inputs.length; i++) {
    //Checks fields in the array making sure they are not empty.
    if(Inputs[i] === "") {
        err++;
    }
}
//Mark the fields who have no inputs.
if (lastname.value == ""){
    lastname.style.borderColor="red";     
}
else {
    lastname.style.borderColor="#ced4ac";
}
if(firstname.value==""){
    firstname.style.borderColor="red";
}
else {
    firstname.style.borderColor="#ced4ac";
}
if(middlename.value=="" || middlename=="surname"){
    middlename.style.borderColor="red";
}
else{
    middlename.style.borderColor="#ced4ac";
}
if(gender.options[gender.selectedIndex].text=="Gender" || gender.text=="" || gender.value==0){
    gender.style.borderColor="red";
}
else{
    gender.style.borderColor="#ced4ac";
}
if(country.options[country.selectedIndex].text=="Please select" || country.options[country.selectedIndex].text=="Country"){
    country.style.borderColor="red";
}
else{
    country.style.borderColor="#ced4ac";
}
if(province.options[province.selectedIndex].text=="Please select" || province.options[province.selectedIndex].text== "Province/State"){
    province.style.borderColor="red";
}
else{
    province.style.borderColor="#ced4ac";
}
if(town.options[town.selectedIndex].text=="Please select" || town.options[town.selectedIndex].text=="Town/Municipality"){
    town.style.borderColor="red";
}
else{
    town.style.borderColor="#ced4ac";
}
if(barangay.options[barangay.selectedIndex].text=="Please select" || barangay.options[barangay.selectedIndex].text=="Barangay/District"){
    barangay.style.borderColor="red";
}
else{
    barangay.style.borderColor="#ced4ac";
}
if(bldg.value==""){
    bldg.style.borderColor="red";
}
else{
    bldg.style.borderColor="#ced4ac";
}
if(email1.value==""){
    email1.style.borderColor="red"
}
else{
    email1.style.borderColor="#ced4ac";
}
if(landline1.value.length==0 && mobile1.value.length==0){
    landline1.style.borderColor="red";
    mobile1.style.borderColor="red";
}
else if(landline1.value.length>=1 || mobile1.value.length>=1){
    landline1.style.borderColor="#ced4ac";
    mobile1.style.borderColor="#ced4ac";
}

//Check that there are no errors.
if (err == 0) {
    countryAlert=country.style.borderColor="red";
    if(countryAlert==true)
{
    errorAlert.style.display="block";
    $('#firstconfirm').modal("none");
}   else {
    errorAlert.style.display="hide";
    errorAlert.style.display="none";
    $('#firstconfirm').modal("show");

} 
   
    $("#firstconfirm").modal("show");
} else {
    //If errors, return false and alert user.
    errorAlert.style.display="block";
    $('#firstconfirm').modal("hide");
}
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
// isulod na og function para imo na tawagon after save... gets?? yes sir
function GetCountry(){
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

function AddCurrentCountryDetails(){
    var editCountry=document.getElementById("edit-country-input").value;
    country=document.getElementById("country-input");
    var currentDetails=document.getElementById("current-country-input");
    currentDetails.value=country.options[country.selectedIndex].text;
}
//===================================================================
//Function: SaveNewCountry
//Purpose: To save the new country from the input field
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
     //value of the input field of Add province...
    var editCountry=document.getElementById("edit-country-input").value;
    country=document.getElementById("country-input");
    
    //declaring the value of the paramBody...
    var objParam={
        "country_id": country.value,
        "country_name": editCountry
      }

      var paramBody = JSON.stringify(objParam);

      var response = InvokeService("Entities_CountryMethods", "PUT", paramBody);

    if (response.code == 200) {

        var data = JSON.parse(response.data); //converting json string to json object(array)  
        var eCountry = JSON.parse(data.jsonData); //converting json string to json object(array)

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
    }


}

function AddCurrentProvinceDetails(){

    province=document.getElementById("province-input");
    var currentProvinceDetails=document.getElementById("current-province-input");
    currentProvinceDetails.value=province.options[province.selectedIndex].text;
}
//===================================================================
//Function: SaveNewProvince
//Purpose: Save new Province in the dropdown or in the table
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

    var countryId=document.getElementById("country-input").value;
    editProvince=document.getElementById("edit-province-input");
    province=document.getElementById("province-input");

    var objParam={
        "province_state_id": province.value,
        "province_state_name": editProvince.value,
        "country": {
          "country_id": countryId,
          "country_name": country.text
        }
      }
      console.log(objParam);
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_ProvinceStateMethods", "PUT", paramBody);
      
      console.log(response);
    
    if (response.code == 200) {
        var data = JSON.parse(response.data); //converting json string to json object(array)
        var eProvinces = JSON.parse(data.jsonData); //converting json string to json object(array)
        
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
}

function AddCurrentTownDetails(){

    town=document.getElementById("town-input");
    var currentTownDetails=document.getElementById("current-town-input");
    currentTownDetails.value=town.options[town.selectedIndex].text;
}
//===================================================================
//Function: SaveNewTown
//Purpose: Save new Town in the dropdown or in the table
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
    province=document.getElementById("province-input");
    town=document.getElementById("town-input");
    editTown=document.getElementById("edit-town-input").value;
    provinceId=document.getElementById("province-input").value;
    
    var objParam={
        "town_id": town.value,
        "town_name": editTown,
        "province_state_id": province.value
      }
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_TownMethods", "PUT", paramBody);

    
    if (response.code == 200) {

        var data = JSON.parse(response.data); //converting json string to json object(array)
        var eTowns = JSON.parse(data.jsonData); //converting json string to json object(array)
        
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
     
}
function AddCurrentBarangayDetails(){

    barangay=document.getElementById("barangay-input");
    var currentBarangayDetails=document.getElementById("current-barangay-input");
    currentBarangayDetails.value=barangay.options[barangay.selectedIndex].text;
}
//===================================================================
//Function: SaveNewBarangay
//Purpose: Save new Barangay to the dropdown or to the table
//Author: Marlo Ellica
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
//  province_id           id of the province
//
//Result:PopulateDropdownList(barangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
// 
//===================================================================
// Populate NEW Barangay Inputs...
function SaveEditBarangay(){
    province=document.getElementById("province-input");
    town=document.getElementById("town-input")
    editBarangay=document.getElementById("edit-barangay-input");
    provinceId=document.getElementById("province-input");

    var objParam={
        "barangay_district_id": barangay.value,
        "barangay_district_name": editBarangay.value,
        "town": {
          "town_id": town.value,
          "town_name": "",
          "province_state_id": province.value
        }
      }
      console.log(objParam);
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_BarangayMethods", "PUT", paramBody);
      
      console.log(response);

    if (response.code == 200) {

        var data = JSON.parse(response.data); //converting json string to json object(array)
        var nBarangays = JSON.parse(data.jsonData); //converting json string to json object(array)
        
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
}


function DeleteCountry(){
    var response = InvokeService("Entities_CountryMethods", "DELETE", "id?" + country_id);
}

function GoToPersonPage(){
    window.location.href=("person-customer-management.html")
}
function GoToPersonPageForm(){
    window.location.href=("add-person-customer-management.html")
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
