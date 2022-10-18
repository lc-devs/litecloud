// document.writeln("<script src='js/address.js'></script >");
//container for filling up input

company=document.getElementById("company_input");
contactperson1=document.getElementById("contactperson_input1");
contactperson2=document.getElementById("contactperson_input2");
country=document.getElementById("country-input");
province=document.getElementById("province-input");
town=document.getElementById("town-input");
barangay=document.getElementById("barangay-input");
zipcode=document.getElementById("zipcode-input");
tin=document.getElementById("tin-input");
bldg=document.getElementById("bldg-input");
email1=document.getElementById("email-input1");
email2=document.getElementById("email-input2");
email3=document.getElementById("email-input3");
landline1=document.getElementById("landline1-input");
landline2=document.getElementById("landline2-input");
mobile1=document.getElementById("mobile1-input");
mobile2=document.getElementById("mobile2-input");
errorAlert=document.getElementById("alrt");

editcompany = document.getElementById("editcompany_input");
editcontactperson1 = document.getElementById("editcontactperson_input1");
editcontactperson2 = document.getElementById("editcontactperson_input2");
editcountry = document.getElementById("editcountry-input");
editprovince = document.getElementById("editprovince-input");
edittown = document.getElementById("edittown-input");
editbarangay = document.getElementById("editbarangay-input");
editzipcode = document.getElementById("editzipcode-input");
edittin = document.getElementById("edittin-input");
editbldg = document.getElementById("editbldg-input");
editemail1 = document.getElementById("editemail1-input");
editemail2 = document.getElementById("editemail2-input");
editemail3 = document.getElementById("editemail3-input");
editlandline1 = document.getElementById("editlandline1-input");
editlandline2 = document.getElementById("editlandline2-input");
editmobile1 = document.getElementById("editmobile1-input");
editmobile2 = document.getElementById("editmobile2-input");


 function CheckDetailEntries(){
    
  inputs=[company.value, contactperson1.value, country.text, province.text, town.text, barangay.text, bldg.value, landline1.value && mobile1.value ];
    //Check for any errors for a multiple fields...
  err=0;

    for (var i=0; i<inputs.length; i++){
        if(inputs[i]=="" || inputs[i]== "Please Select"){
            err++;
        }
    }
   
    if(company.value==""){
        company.style.borderColor="red";
    }else{
        company.style.borderColor="#ced4da";
    }
    if(contactperson1.value==""){
        contactperson1.style.borderColor="red";
    }else{
        contactperson1.style.borderColor="#ced4da";
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
    }else{
        bldg.style.borderColor="#ced4da";
    }
    if(email1.value==""){
        email1.style.borderColor="red";
    }else{
        email1.style.borderColor="#ced4da";
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
    $('#NonPersonConfirmation').modal("hide");
    errorAlert.style.display="block";
 
} else {
    //If errors, return false and alert user.
   
    $('#NonPersonConfirmation').modal("show");
    errorAlert.style.display="hide";
}

    
 }
 
function RedirectToEditNonPersonManagement(){
    window.location.href("edit-non-person.html")
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


function CheckEditDetailEntries(){
    //VARIABLES FOR ARRAY...that contains the inputs from the form.
    var editnonpersonfield=[editcompany.value, editcontactperson1.value, editcountry.text, editprovince.text, edittown.text, editbarangay.text, editbldg.value, editemail1.value, editlandline1.value || editmobile1.value ];
    //Variable to keep track of Errors. Initialize to 0.
    var err = 0;
    
    //Start loop to validate.
    for (var i = 0; i < editnonpersonfield.length; i++) {
        //Checks fields in the array making sure they are not empty.
        if(editnonpersonfield[i] === "") {
            err++;
        }
    }
    //Mark the fields who have no inputs.
    if (editcompany.value == ""){
        editcompany.style.borderColor="red";     
    }
    else {
        editcompany.style.borderColor="#ced4ac";
    }
    if(editcontactperson1.value==""){
        editcontactperson1.style.borderColor="red";
    }
    else {
        editcontactperson1.style.borderColor="#ced4ac";
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
        $('#editNonPersonModal').modal("hide");
        $('#confirmNonPerson').modal("show");
    } else {
        //If errors, return false and alert user.
        errorAlert.style.display="block";
        $('#confirmNonPerson').modal("hide");
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
//populate Country Dropdown list
function GetEditCountry(){
var response = InvokeService("Entities_CountryMethods/getall", "GET", "");
if (response.code == 200) {

    ecountry.innerHTML = "";
    var data = JSON.parse(response.data);

    var countries = JSON.parse(data.jsonData);

    ecountry.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";

    for (var i = 0; i < ecountries.length; i++) {
        PopulateDropdownList(ecountry, countries[i].country_id, countries[i].country_name);
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

function GetEditProvinces() {
    var country_id = ecountry.value;

    var response = InvokeService("Entities_ProvinceStateMethods/getall/" + country_id, "GET", "");

    if (response.code == 200) {
        eprovince = document.getElementById("province-input");
        eprovince.innerHTML = "";
        var data = JSON.parse(response.data);

        var provinces = JSON.parse(data.jsonData);
        eprovince.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
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

function GetEditTowns() {

    var province_id = eprovince.value;

    var response = InvokeService("Entities_TownMethods/getall/" + province_id, "GET", "");

    if (response.code == 200) {

        etown = document.getElementById("town-input");
        etown.innerHTML = "";
        var data = JSON.parse(response.data);

        var towns = JSON.parse(data.jsonData);

        etown.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < towns.length; i++) {
            PopulateDropdownList(etown, towns[i].town_id, towns[i].town_name);
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


function GetEditBarangays() {
    var town_id = etown.value;

    var response = InvokeService("Entities_BarangayMethods/getall/" + town_id, "GET", "");

    if (response.code == 200) {

        ebarangay = document.getElementById("barangay-input");
        ebarangay.innerHTML = "";
        var data = JSON.parse(response.data);

        var barangays = JSON.parse(data.jsonData);

        ebarangay.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < barangays.length; i++) {
            PopulateDropdownList(ebarangay, barangays[i].barangay_district_id, barangays[i].barangay_district_name);
        }

    }show
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
     paramBody=JSON.stringify(objParam);show
     console.log(response);
   
   if (response.code == 200) {

       var data = JSON.parse(response.data); //converting json string to json object(array)
       var nProvinces = JSON.parse(data.jsonData); //converting json string to json object(array)
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

       var data = JSON.parse(response.data); //converting json string to json object(array)
       var nTowns = JSON.parse(data.jsonData); //converting json string to json object(array)
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

function AddNewNonperson(){
    objParam={
        "nonperson_id": 0,
        "nonperson_name": company.value,
        "contact_person1": contactperson1.value,
        "contact_person2": contactperson2.value,
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
      console.log(paramBody)
              var response = InvokeService("Entities_NonPersonMethods", "POST", paramBody);
              console.log(response)
              if (response.code == 200) {

                var data = JSON.parse(response.data); //converting json string to json object(array)
                var postNonPerson = JSON.parse(data.jsonData); //converting json string to json object(array)
                
            }
}
function ValidateAddingPlaceInput(){
    newCountry=document.getElementById("add-country-input");
    newProvince=document.getElementById("add-province-input");
    newTown=document.getElementById("add-town-input");
    newBarangay=document.getElementById("add-barangay-input"); 

    if(newCountry.value==""){
        newCountry
    }
}

function PopulateNonPersonDataTable(){

    var searchCompany=document.getElementById("searchcompany");
    var response = InvokeService("Entities_NonPersonMethods/searchbyname/" + searchCompany.value, "GET", ""); 
    var parseJsonData = JSON.parse(response.data);

    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var tblbody = document.getElementById('tblbody');
        tblbody.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            tblbody.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0'>"+ oData[i].nonperson_name+ "</td>" +
            "<td class='border text-center px-0'>"+ oData[i].contact_person_id1 +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].landphone1 +"</td>" +
            "<td class='border text-center px-0'><button type='button' class='btn btn-outline-primary'  onclick='EditNonPersonData("+oData[i].nonperson_id+")' data-toggle='modal' data-target='#editNonPersonModal' onfocus='SaveNonPersonId_edit("+oData[i].nonperson_id+")'><i class='fa fa-pencil-square-o'></i></button><button type='button' class='btn btn-danger' onclick='SaveNonPersonId_deleteModal("+oData[i].nonperson_id+")' data-toggle='modal' data-target='#deleteModal'><i class='fa fa-trash' style='color: white;'></i>&nbsp;</button> </td>" +
            "</tr>";
        }

    } else if(parseJsonData.code == 404){
        var tblbody = document.getElementById('tblbody');
        tblbody.innerHTML="";
        tblbody.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0' colspan='5'>No Data Found.</td>" +
            "</tr>";
    } else {
        ShowErrorModalOnLoad(response.message, response.code);

    }
}
function SaveNonPersonId_deleteModal(nonperson_id){
    document.getElementById("nonPersonId_delete").value=nonperson_id;
    
}
function DeleteNonPersonDetails(){
        
    var nonPersonID=document.getElementById('nonPersonId_delete').value;
    var response = InvokeService("Entities_NonPersonMethods/id?id="+ nonPersonID, "DELETE", "");
   
    if(response.code==200){
        
        data=JSON.parse(response.data);
        deletePerson=JSON.parse(data.jsonData);
        $('#SuccessDelete').modal("show");
    }else{
        $('#FailureDelete').modal("show");
        btnSearch.click();
    }
}

function EditNonPersonData(nonperson_id){
    var response = InvokeService("Entities_NonPersonMethods/getdetails/" + nonperson_id, "GET", "");
    if(response.code=200){
        data=JSON.parse(response.data);
        editNonPersondata=JSON.parse(data.jsonData);

        for (var i=0; i<editNonPersondata.length; i++){

            document.getElementById("nonpersonID_edit").value=editNonPersondata[i].nonperson_id;
            editcompany.value=editNonPersondata[i].nonperson_name;
            editcontactperson1.value=editNonPersondata[i].contact_person_id1;
            editcontactperson2.value=editNonPersondata[i].contact_person_id2;
            editzipcode.value=editNonPersondata[i].zip_code;
            editemail1.value=editNonPersondata[i].email_address1;
            editemail2.value=editNonPersondata[i].email_address2;
            editemail3.value=editNonPersondata[i].email_address3;
            editlandline1.value=editNonPersondata[i].landphone1;
            editlandline2.value=editNonPersondata[i].landphone2;
            editmobile1.value=editNonPersondata[i].mobilephone1;
            editmobile2.value=editNonPersondata[i].mobilephone2;
            country_id=editNonPersondata[i].adrs_country;
            province_id=editNonPersondata[i].adrs_province;
            town_id=editNonPersondata[i].adrs_town;
            barangay_id=editNonPersondata[i].adrs_barangay;
            
            var barangayeditresponse=InvokeService("Entities_BarangayMethods/getone/"+ barangay_id, "GET", "");
           
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
                            editcountry = document.getElementById("editcountry-input");
                          
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
                    editprovince = document.getElementById("editprovince-input");
             
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

                    edittown = document.getElementById("edittown-input");
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
            
                    editbarangay = document.getElementById("editbarangay-input");
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
function SaveNonPersonId_edit(nonperson_id){
    nonPersonID=document.getElementById("nonpersonID").value;
    nonPersonID=nonperson_id;
}

function PutNonPersonData(){

    objParam={
            "nonperson_id": nonpersonID,
            "nonperson_name": editcompany.value,
            "contact_person1": editcontactperson1.value,
            "contact_person2": editcontactperson2.value,
            "tax_identification": edittin.value,
            "adrs_house_street": editbldg.value,
            "adrs_barangay": editbarangay.value,
            "adrs_town": edittown.value,
            "adrs_province": editprovince.value,
            "adrs_country": editcountry.value,
            "zip_code": editzipcode.value,
            "email_address1": editemail1.value,
            "email_address2": editemail2.value,
            "email_address3": editemail3.value,
            "landphone1": editlandline1.value,
            "landphone2": editlandline2.value,
            "mobilephone1": editmobile1.value,
            "mobilephone2": editmobile2.value
          }
          console.log(objParam);
            
              paramBody=JSON.stringify(objParam);

              var response = InvokeService("Entities_NonPersonMethods", "PUT", paramBody);

              if (response.code == 200) {
                $('#update_success').modal("show");
                var data = JSON.parse(response.data); //converting json string to json object(array)
                var saveNonPersons = JSON.parse(data.jsonData); //converting json string to json object(array)
                
            }
            else{
                $('#update_error').modal("show");
            }
        }

 //for going back to the previous page

 
 function GoToNonPersonPageForm(){
    window.location.href=("add-nonperson-customer-management.html")
}
}