
//container for filling up input from add person customer management page...
var lastname=document.getElementById("lastname-input");
var firstname=document.getElementById("firstname-input");
var middlename=document.getElementById("middlename-input");
var dateofbirth=document.getElementById("dateofbirth-input");
var gender=document.getElementById("gender-input");
var civilstatus=document.getElementById("status-input");
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

//container for filling up input from add person customer management page...
var editlastname=document.getElementById("elastname-input");
var editfirstname=document.getElementById("efirstname-input");
var editmiddlename=document.getElementById("emiddlename-input");
var editdateofbirth=document.getElementById("edateofbirth-input");
var editgender=document.getElementById("egender-input");
var editstatus=document.getElementById("estatus-input");
var editcountry=document.getElementById("ecountry-input");
var editprovince=document.getElementById("eprovince-input");
var edittown=document.getElementById("etown-input");
var editbarangay=document.getElementById("ebarangay-input");
var editzipcode=document.getElementById("ezipcode-input");
var edittin=document.getElementById("etin-input");
var editbldg=document.getElementById("ebldg-input");
var editemail1=document.getElementById("eemail1-input");
var editemail2=document.getElementById("eemail2-input");
var editemail3=document.getElementById("eemail3-input");
var editlandline1=document.getElementById("elandline1-input");
var editlandline2=document.getElementById("elandline2-input");
var editmobile1=document.getElementById("emobile1-input");
var editmobile2=document.getElementById("emobile2-input");
var editgender=document.getElementById("esex-input");   
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
var Inputs=[lastname.value, firstname.value, middlename.value, gender.text, country.text, province.text, town.text, barangay.text, bldg.value, email1.value, landline1.value || mobile1.value ]
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
if(country.options[country.selectedIndex].text=="Please select" || country.options[country.selectedIndex].text=="Country"|| country.text=="Country"){
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
    errorAlert.style.display="hide";
    errorAlert.style.display="none";
    $('#firstconfirmPerson').modal("show");

} else {
    //If errors, return false and alert user.
    errorAlert.style.display="block";
    $('#firstconfirmPerson').modal("");
}
}

function CheckEditDetailEntries(){
    //VARIABLES FOR ARRAY...that contains the inputs from the form.
    var Inputs=[editlastname.value, editfirstname.value, editmiddlename.value, editcountry.text, editprovince.text, edittown.text, editbarangay.text, editbldg.value, editemail1.value, editlandline1.value || editmobile1.value ]
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
    if (editlastname.value == ""){
        editlastname.style.borderColor="red";     
    }
    else {
        editlastname.style.borderColor="#ced4ac";
    }
    if(editfirstname.value==""){
        editfirstname.style.borderColor="red";
    }
    else {
        editfirstname.style.borderColor="#ced4ac";
    }
    if(editmiddlename.value=="" || editmiddlename=="surname"){
        editmiddlename.style.borderColor="red";
    }
    else{
        editmiddlename.style.borderColor="#ced4ac";
    }
    if(editgender.options[editgender.selectedIndex].text=="Gender" || editgender.text=="" || editgender.value==0){
        editgender.style.borderColor="red";
    }
    else{
        editgender.style.borderColor="#ced4ac";
    }
    if(editcountry.options[editcountry.selectedIndex].text=="Please select" || editcountry.options[editcountry.selectedIndex].text=="Country" || editcountry.text=="Country" || editcountry.text=="Please select"){
        editcountry.style.borderColor="red";
    }
    else if(editcountry.text=="Philippines"){
        editcountry.style.borderColor="#ced4ac";
    }
    else{
        editcountry.style.borderColor="#ced4ac";
    }
    if(editprovince.options[editprovince.selectedIndex].text=="Please select" || editprovince.options[editprovince.selectedIndex].text== "Province/State"){
        editprovince.style.borderColor="red";
    }
    else{
        editprovince.style.borderColor="#ced4ac";
    }
    if(edittown.options[edittown.selectedIndex].text=="Please select" || edittown.options[edittown.selectedIndex].text=="Town/Municipality"){
        edittown.style.borderColor="red";
    }
    else{
        edittown.style.borderColor="#ced4ac";
    }
    if(editbarangay.options[editbarangay.selectedIndex].text=="Please select" || editbarangay.options[editbarangay.selectedIndex].text=="Barangay/District"){
        editbarangay.style.borderColor="red";
    }
    else{
        editbarangay.style.borderColor="#ced4ac";
    }
    if(editbldg.value==""){
        editbldg.style.borderColor="red";
    }
    else{
        editbldg.style.borderColor="#ced4ac";
    }
    if(editemail1.value==""){
        editemail1.style.borderColor="red"
    }
    else{
        editemail1.style.borderColor="#ced4ac";
    }
    if(editlandline1.value.length==0 && editmobile1.value.length==0){
        editlandline1.style.borderColor="red";
        editmobile1.style.borderColor="red";
    }
    else if(editlandline1.value.length>=1 || editmobile1.value.length>=1){
        editlandline1.style.borderColor="#ced4ac";
        editmobile1.style.borderColor="#ced4ac";
    }
    
    //Check that there are no errors.
    
    if (err == 0) {
        errorAlert.style.display="hide";
        $('#editPersonModal').modal("hide");
        $('#confirmPerson').modal("show");
    } else {
        //If errors, return false and alert user.
        errorAlert.style.display="block";
        $('#editPersonModal').modal("show");
        $('#confirmPerson').modal("hide");
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
function ValidateSaveCountry(){
    newCountry=document.getElementById("add-country-input");
    if(newCountry.value==""){
        newCountry.style.borderColor="red";
    }
    else{
        $('#confirmcountry').modal("show");
        $('#country_value_input').modal("hide");
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
function GetEditCountry(){
    var response = InvokeService("Entities_CountryMethods/getall", "GET", "");
    if (response.code == 200) {
        editcountry = document.getElementById("ecountry-input");
        editcountry.innerHTML = "";
        var data = JSON.parse(response.data);
    
        var countries = JSON.parse(data.jsonData);
    
        editcountry.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < countries.length; i++) {
            PopulateDropdownList(editcountry, countries[i].country_id, countries[i].country_name);
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

function GetEditProvince(){
    editcountry = document.getElementById("ecountry-input");

    var country_id = editcountry.value;

    var response = InvokeService("Entities_ProvinceStateMethods/getall/" + country_id, "GET", "");

    if (response.code == 200) {
        editprovince = document.getElementById("eprovince-input");
        editprovince.innerHTML = "";
        var data = JSON.parse(response.data);

        var eprovinces = JSON.parse(data.jsonData);
        editprovince.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < provinces.length; i++) {
            PopulateDropdownList(editprovince, eprovinces[i].province_state_id, eprovinces[i].province_state_name);
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

function GetEditTown(){

    editprovince = document.getElementById("eprovince-input");


    var province_id = editprovince.value;

    var response = InvokeService("Entities_TownMethods/getall/" + province_id, "GET", "");

    if (response.code == 200) {

        edittown = document.getElementById("etown-input");
        edittown.innerHTML = "";
        var data = JSON.parse(response.data);

        var etowns = JSON.parse(data.jsonData);
  
        edittown.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < towns.length; i++) {
            PopulateDropdownList(edittown, etowns[i].town_id, etowns[i].town_name);
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

function GetEditBarangay(){


    edittown = document.getElementById("etown-input");


    var town_id = edittown.value;

    var response = InvokeService("Entities_BarangayMethods/getall/" + town_id, "GET", "");

    if (response.code == 200) {

        editbarangay = document.getElementById("ebarangay-input");
        editbarangay.innerHTML = "";
        var data = JSON.parse(response.data);

        var barangays = JSON.parse(data.jsonData);

        barangays.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < barangays.length; i++) {
            PopulateDropdownList(editbarangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
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
//  country_id            id of the town
//
//Result:
// 
//===================================================================


function SaveNewCountry(){
    
     //value of the input field of Add province...
    var newCountry=document.getElementById("add-country-input").value;
    
    //declaring the value of the paramBody...
    var objParam={
        "country_id": 0,
        "country_name": newCountry
      }

      var paramBody = JSON.stringify(objParam);

      var response = InvokeService("Entities_CountryMethods", "POST", paramBody);

    if (response.code == 200) {
        
        var data = JSON.parse(response.data); //converting json string to json object(array)  
        var nCountries = JSON.parse(data.jsonData); //converting json string to json object(array)
        $('#countrysuccessconfirm').modal("show");
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
    }else{
        $('#countrysavefailure').modal("show");
    } 
    

    


}

function ValidateSaveProvince(){
    newProvince=document.getElementById("add-province-input");
    if(newProvince.value==""){
        newProvince.style.borderColor="red";
    }
    else{
        $('#confirmprovince').modal("show");
        $('#province_value_input').modal("hide");
    }
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


function SaveNewProvince(){

    var countryId=document.getElementById("country-input").value;
   
    newProvince=document.getElementById("add-province-input").value;

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
        $('#provincesuccessconfirm').modal("show");
        var data = JSON.parse(response.data); //converting json string to json object(array)
        var nProvinces = JSON.parse(data.jsonData); //converting json string to json object(array)
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
    }else{
        $('#provincesavefailure').modal("show");
    }
}

function ValidateSaveTown(){
    newTown=document.getElementById("add-town-input");
    if(newTown.value==""){
        newTown.style.borderColor="red";
    }
    else{
        $('#confirmtown').modal("show");
        $('#town_value_input').modal("hide");
    }
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

function SaveNewTown(){
    newTown=document.getElementById("add-town-input").value;
    provinceId=document.getElementById("province-input").value;
    
    var objParam={
        "town_id": 0,
        "town_name": newTown,
        "province_state_id": provinceId
      }
      paramBody=JSON.stringify(objParam);

      var response = InvokeService("Entities_TownMethods", "POST", paramBody);

    
    if (response.code == 200) {
        $('#townsuccessconfirm').modal("show");
        var data = JSON.parse(response.data); //converting json string to json object(array)
        var nTowns = JSON.parse(data.jsonData); //converting json string to json object(array)
        province = document.getElementById("province-input");


        var province_id = province.value;
    
        var response = InvokeService("Entities_TownMethods/getall/" + province_id, "GET", "");
     
    
            town = document.getElementById("town-input");
            town.innerHTML = "";
            var data = JSON.parse(response.data);
    
            var towns = JSON.parse(data.jsonData);
      
            town.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
            for (var i = 0; i < towns.length; i++) {
                PopulateDropdownList(town, towns[i].town_id, towns[i].town_name);
            
    
        }
    }else{
        $('#townsavefailure').modal("show");
    }
     
}

function ValidateSaveBarangay(){
    newBarangay=document.getElementById("add-barangay-input");
    if(newBarangay.value==""){
        newBarangay.style.borderColor="red";
    }
    else{
        $('#confirmbarangay').modal("show");
        $('#barangay_value_input').modal("hide");
    }
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
function SaveNewBarangay(){
    newBarangay=document.getElementById("add-barangay-input").value;
    townId=document.getElementById("town-input").value;
    provinceId=document.getElementById("province-input").value;;

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
        
        var data = JSON.parse(response.data); //converting json string to json object(array)
        var nBarangays = JSON.parse(data.jsonData); //converting json string to json object(array)
        $('#barangaysuccessconfirm').modal("show");

        var response = InvokeService("Entities_BarangayMethods/getall/" + townId, "GET", "");

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
    }else{
        $('#barangaysavefailure').modal("show");
    }
}

function SavePersonData(){
    objParam={
        "person_id": 0,
        "lastname": lastname.value,
        "firstname": firstname.value,
        "middlename": middlename.value,
        "sex": gender.value,
        "birthdate": dateofbirth.value,
        "civilstatus": civilstatus.value,
        "tax_identification": tin.value,
        "adrs_house_street": bldg.value,
        "adrs_barangay": barangay.value,
        "adrs_town": town.value,
        "adrs_province": province.value,
        "adrs_country": country.value,
        "zip_code": zipcode.value,
        "email_address1": email1.value,
        "email_address2": email2.value,
        "email_address3": email3.value,
        "landphone1": landline1.value,
        "landphone2": landline2.value,
        "mobilephone1": mobile1.value,
        "mobilephone2": mobile2.value
      }
            
              paramBody=JSON.stringify(objParam);

              var response = InvokeService("Entities_PersonMethods", "POST", paramBody);
              console.log(response)

              if (response.code == 200) {
                
                var data = JSON.parse(response.data);
                console.log(data) //converting json string to json object(array)
                 //converting json string to json object(array)
                
                if(data.code==200){
                    $('#personsavesuccess').modal("show");
                   var savePersons = JSON.parse(data.jsonData);

                   var click_YES=document.getElementById("YES_button");
                   click_YES.onclick= function RedirectToPersonPage(){
                    window.location.href=("person-customer-management.html");
                   }
                }
                else{
                    $('#personsavefailure').modal("show");
                }
               
                
            }
            else{
                $('#personsavefailure').modal("show");
            }
}
function PopulatePersonDataTable(){
    var searchname=document.getElementById("searchname").value;

    var jsonData = InvokeService("Entities_PersonMethods/searchbyname/" + searchname, "GET", "");
 
    var parseJsonData = JSON.parse(jsonData.data);

    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = document.getElementById('tblPersons');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0'>"+ oData[i].lastname + "</td>" +
            "<td class='border text-center px-0'>"+ oData[i].firstname +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].middlename +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].birthdate +"</td>" +
            "<td class='border text-center px-0'><button type='button' class='btn btn-outline-primary'  onclick='EditPersonData("+oData[i].person_id+")' data-toggle='modal' data-target='#editPersonModal'><i class='fa fa-pencil-square-o'></i></button><button type='button' class='btn btn-danger' data-toggle='modal' data-target='#deleteModal' onclick='SetDeletePersonId("+oData[i].person_id+")'><i class='fa fa-trash' style='color: white;'></i>&nbsp;</button> </td>" +
            "</tr>";
        }

    } else if(parseJsonData.code == 404){
        var divtbl = document.getElementById('tblPersons');
        divtbl.innerHTML="";
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0' colspan='5'>No Data Found.</td>" +
            "</tr>";
    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}
function SetDeletePersonId(person_id){
    hiddenPersonId=document.getElementById("delete_person_id");
    hiddenPersonId.value=person_id;
    
}
function DeletePersonDetails(){
    person_Id=hiddenPersonId.value;
    var response = InvokeService("Entities_PersonMethods/id?id="+ person_Id, "DELETE", "");
    var personParse=(response.data);
    if(personParse.code==200){
        btnSearch=document.getElementById("btnSearch");
        
        $('#deletesuccess').modal("show");
    }else{
        $('#deletefailure').modal("show");
        btnSearch.click();
    }
}

function EditPersonData(person_id){
    
    var response = InvokeService("Entities_PersonMethods/getdetails/" + person_id, "GET", "");
    if(response.code=200){
        data=JSON.parse(response.data);
        editPersondata=JSON.parse(data.jsonData);
        for (var i=0; editPersondata.length; i++){
            document.getElementById("person_id").value=editPersondata[i].person_id;
            editlastname.value=editPersondata[i].lastname;
            editfirstname.value=editPersondata[i].firstname;
            editmiddlename.value=editPersondata[i].middlename;
            editdateofbirth.value=editPersondata[i].birthdate;
            editgender.value=editPersondata[i].sex;
            editstatus.value=editPersondata[i].civilstatus;
            editzipcode.value=editPersondata[i].zip_code;
            editbldg.value=editPersondata[i].adrs_house_street;
            editemail1.value=editPersondata[i].email_address1;
            editemail2.value=editPersondata[i].email_address2;
            editemail3.value=editPersondata[i].email_address3;
            editlandline1.value=editPersondata[i].landphone1;
            editlandline2.value=editPersondata[i].landphone2;
            editmobile1.value=editPersondata[i].mobilephone1;
            editmobile2.value=editPersondata[i].mobilephone2;
            country_id=editPersondata[i].adrs_country;
            province_id=editPersondata[i].adrs_province;
            town_id=editPersondata[i].adrs_town;
            barangay_id=editPersondata[i].adrs_barangay;

            var barangayeditresponse=InvokeService("Entities_BarangayMethods/getone/"+ town_id, "GET", "");
            if(barangayeditresponse.code==200){

                data=JSON.parse(barangayeditresponse.data);
                barangayresponse=JSON.parse(data.jsonData);
              
                for (var i = 0; i < barangayresponse.length; i++) {
                    editbarangay.options[editbarangay.selectedIndex].text=barangayresponse[i].barangay_district_name;
                }
               
            }
            var towneditresponse=InvokeService("Entities_TownMethods/getone/"+ town_id, "GET", "");
            if(towneditresponse.code==200){
                data=JSON.parse(towneditresponse.data);
                townresponse=JSON.parse(data.jsonData);
                for (var i = 0; i < townresponse.length; i++) {
                    edittown.options[edittown.selectedIndex].text=townresponse[i].town_name;
                }
             
            }
            var provinceeditresponse=InvokeService("Entities_ProvinceStateMethods/getone/"+ province_id, "GET", "");
            if(provinceeditresponse.code=200){
                data=JSON.parse(provinceeditresponse.data);
                provinceresponse=JSON.parse(data.jsonData);

                for (var i = 0; i < provinceresponse.length; i++) {
                    editprovince.options[editprovince.selectedIndex].text=provinceresponse[i].province_state_name;
                }
                
            }
            var countryeditresponse = InvokeService("Entities_CountryMethods/getone/"+country_id, "GET", "");
          
            if (countryeditresponse.code == 200) {
               
                var data = JSON.parse(countryeditresponse.data);
            
                var editcountries = JSON.parse(data.jsonData);
            
                for (var i = 0; i < editcountries.length; i++) {

                    editcountry.options[editcountry.selectedIndex].text=editcountries[i].country_name;
                  
                }
                editcountry.onfocus= function GetEditCountry(){
                    var response = InvokeService("Entities_CountryMethods/getall", "GET", "");
                        if (response.code == 200) {
                            editcountry = document.getElementById("ecountry-input");
                            editcountry.innerHTML = "";
                            var data = JSON.parse(response.data);
                        
                            var countries = JSON.parse(data.jsonData);
                            
                            editcountry.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
                        
                            for (var i = 0; i < countries.length; i++) {
                                PopulateDropdownList(editcountry, countries[i].country_id, countries[i].country_name);
                               
                            }
                            
                        }
                }
            }
            //-------------------------
            editcountry.onchange=function GetEditProvince(){

                var response = InvokeService("Entities_ProvinceStateMethods/getall/" + editcountry.value, "GET", "");
            
                if (response.code == 200) {
                    editprovince = document.getElementById("eprovince-input");
                    editprovince.innerHTML = "";
                    var data = JSON.parse(response.data);
            
                    var eprovinces = JSON.parse(data.jsonData);
                    editprovince.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
                    for (var i = 0; i < eprovinces.length; i++) {
                        PopulateDropdownList(editprovince, eprovinces[i].province_state_id, eprovinces[i].province_state_name);
                    }
            
                }
            }
            
            editprovince.onchange=function GetEditTown(){
                var response = InvokeService("Entities_TownMethods/getall/" + editprovince.value, "GET", "");

                if (response.code == 200) {

                    edittown = document.getElementById("etown-input");
                    edittown.innerHTML = "";
                    var data = JSON.parse(response.data);

                    var etowns = JSON.parse(data.jsonData);
            
                    edittown.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
                    for (var i = 0; i < etowns.length; i++) {
                        PopulateDropdownList(edittown, etowns[i].town_id, etowns[i].town_name);
                    }

                }
            }
            edittown.onchange=function GetEditBarangay(){
                var response = InvokeService("Entities_BarangayMethods/getall/" + edittown.value, "GET", "");

                if (response.code == 200) {
            
                    editbarangay = document.getElementById("ebarangay-input");
                    editbarangay.innerHTML = "";
                    var data = JSON.parse(response.data);
            
                    var ebarangays = JSON.parse(data.jsonData);
            
                    editbarangay.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
                    for (var i = 0; i < ebarangays.length; i++) {
                        PopulateDropdownList(editbarangay, ebarangays[i].barangay_district_id, ebarangays[i].barangay_district_name);
                    }
            
                }
            }
        }
    }

}

function PutPersonData(){
    objParam={
        "person_id": document.getElementById("person_id").value,
        "lastname": editlastname.value,
        "firstname": editfirstname.value,
        "middlename": editmiddlename.value,
        "sex": editgender.value,
        "birthdate": editdateofbirth.value,
        "civilstatus": editstatus.value,
        "tax_identification": edittin.value,
        "adrs_house_street": editbldg.value,
        "adrs_barangay": editbarangay.options[editbarangay.selectedIndex].value,
        "adrs_town": edittown.options[edittown.selectedIndex].value,
        "adrs_province": editprovince.options[editprovince.selectedIndex].value,
        "adrs_country": editcountry.options[editcountry.selectedIndex].value,
        "zip_code": editzipcode.value,
        "email_address1": editemail1.value,
        "email_address2": editemail2.value,
        "email_address3": editemail3.value,
        "landphone1": editlandline1.value,
        "landphone2": editlandline2.value,
        "mobilephone1": editmobile1.value,
        "mobilephone2": editmobile2.value
      }
      console.log(objParam)
            
              paramBody=JSON.stringify(objParam);

              var response = InvokeService("Entities_PersonMethods", "PUT", paramBody);
                console.log(response)

              if (response.code == 200) {
                $('#personsaveeditsuccess').modal("show");
                var data = JSON.parse(response.data); //converting json string to json object(array)
                var savePersons = JSON.parse(data.jsonData); //converting json string to json object(array)
                window.location.href=('person-customer-management.html')
                
            }
            else{
                $('#personsaveeditfailure').modal("show");
            }
}




function GoToPersonPage(){
    window.location.href=("person-customer-management.html")
}
function GoToPersonPageForm(){
    window.location.href=("add-person-customer-management.html")
}
function GoToEditPersonPageForm(){
    window.location.href=("edit-person-customer-management.html")
}