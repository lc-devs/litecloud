document.writeln("<script src='js/address.js'></script >");

//container for filling up input
company=document.getElementById("location_input");
unit=document.getElementById("unit-incharge");
warehousechecked=document.getElementById("warehouse-checked");
storechecked=document.getElementById("store-checked");
var country=document.getElementById("country-input");
var province=document.getElementById("province-input");
var town=document.getElementById("town-input");
var barangay=document.getElementById("barangay-input");
var bldg=document.getElementById("bldg-input");
var email1=document.getElementById("email-input");
var email2=document.getElementById("email-input1");
var email3=document.getElementById("email-input2");
var landline1=document.getElementById("landline1-input");
var landline2=document.getElementById("landline2-input");
var mobile1=document.getElementById("mobile1-input");
var mobile2=document.getElementById("mobile2-input");
var errorAlert=document.getElementById("alrt");

//modal response for any error or success

 function CheckDetailEntries(isNew){
  
inputs=[company.value, unit.value, warehousechecked.checked || storechecked.checked,  country.options[country.selectedIndex].text, province.options[province.selectedIndex].text, town.options[town.selectedIndex].text, barangay.options[barangay.selectedIndex].text, bldg.value, email1.value, landline1.value || mobile1.value];
 //Checking if there is error...
 err=0;

 //Looping for error...
 for (var i = 0; i < inputs.length; i++) {
    //Checks fields in the array making sure they are not empty.
    if(inputs[i] === "" || inputs[i]== false) {
        err++;
    }
}
 if(company.value==""){
     company.style.borderColor="red";
 }else{
     company.style.borderColor="#ced4ac";
 }
 if(unit.value==""){
     unit.style.borderColor="red";
 }else{
     unit.style.borderColor="#ced4ac";
 }

 if(warehousechecked.checked==false && storechecked.checked==false){

     document.getElementById("notice").style.display="block";
    
 }
 else{
     document.getElementById("notice").style.display="hide";
     document.getElementById("notice").style.display="none";
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
    errorAlert.style.display="none";
    // $('#firstconfirm').modal("show");
    if(isNew == 'true'){
        AddNewUnitLocation();
    }else{
        UpdateUnitLocation();
    }

} else {
    //If errors, return false and alert user.
    errorAlert.style.display="block";
    // $('#firstconfirm').modal("show");
}
 }

 //Go to Warehouse/store page...

 function GoToWarehouseStorePage(){
     window.location.href=("warehouse-stores.html");
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
    
    FetchCountry("country-input"); 
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

    FetchProvinces(country.value, "province-input");

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

    FetchTowns(province.value, "town-input");

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

    FetchBarangays(town.value, "barangay-input");

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

    FetchCountry("ecountry-input");

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

    FetchProvinces(editcountry.value,"eprovince-input" );

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

    FetchTowns(editprovince.value, "etown-input");

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

    FetchBarangays(edittown.value, "ebarangay-input");
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

    var isSuccess = ExecuteSaveNewCountry(newCountry);
   
    if (isSuccess) {       
       
        $('#countrysuccessconfirm').modal("show");
       GetCountry();
        
    }
    else{
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

    var isSuccess = ExecuteSaveNewProvince(countryId, newProvince);     
    
    if (isSuccess) {
        $('#provincesuccessconfirm').modal("show");
       GetProvinces();
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
    newTown = document.getElementById("add-town-input").value;
    provinceId = document.getElementById("province-input").value;
    
    var isSuccess = ExecuteSaveNewTown(provinceId, newTown);
    
    if (isSuccess) {
        $('#townsuccessconfirm').modal("show");
        GetTowns();
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
    newBarangay = document.getElementById("add-barangay-input").value;
    townId = document.getElementById("town-input").value;
    provinceId = document.getElementById("province-input").value;



    var isSuccess = ExecuteSaveNewBarangay(provinceId, townId, newBarangay);

    if (isSuccess) {
        
         $('#barangaysuccessconfirm').modal("show");
         GetBarangays();

   }else{
        $('#barangaysavefailure').modal("show");
    }
}

 function AddNewUnitLocation(){
    objParam={
        "unit_location_id": 0,
        "description": company.value,
        "person_incharge": unit.value,
        "warehouse": warehousechecked.checked == true? 1:0,
        "bldg_street_address": barangay.value,
        "barangay_id": barangay.value,
        "town_id": town.value,
        "province_id": province.value,
        "country_id": country.value,
        "email_address": email1.value,
        "landline_nos1": landline1.value,
        "landline_nos2":landline2.value,
        "mobile_nos1": mobile1.value,
        "mobile_nos2": mobile2.value,
        "user_id": 0,
        "entry_date": "string"
      }
      paramBody=JSON.stringify(objParam);

      var response= InvokeService("Inventory_InventoryUnitLocation", "POST", paramBody);

      if(response.code==200){

          data = JSON.parse(response.data);          

          if(data.code == 200){
            ShowModal("Unit Location is saved", true, "warehouse-stores.html");
           
          }else{
            //failednotice
            ShowModal("The system encounter an error. Please try again later.", false, "");
          }
      }
 }

 function UpdateUnitLocation(){

    var id = document.getElementById("unit_location_id").value;

    objParam={
        "unit_location_id": id,
        "description": company.value,
        "person_incharge": unit.value,
        "warehouse": warehousechecked.checked == true? 1:0,
        "bldg_street_address": barangay.value,
        "barangay_id": barangay.value,
        "town_id": town.value,
        "province_id": province.value,
        "country_id": country.value,
        "email_address": email1.value,
        "landline_nos1": landline1.value,
        "landline_nos2":landline2.value,
        "mobile_nos1": mobile1.value,
        "mobile_nos2": mobile2.value,
        "user_id": 0,
        "entry_date": "string"
      }
      paramBody=JSON.stringify(objParam);

      var response= InvokeService("Inventory_InventoryUnitLocation/id", "PUT", paramBody);

      if(response.code==200){

          data = JSON.parse(response.data);          

          if(data.code == 200){
            ShowModal("Unit Location is saved", true, "warehouse-stores.html");
           
          }else{
            //failednotice
            ShowModal("The system encounter an error. Please try again later.", false, "");
          }
      }
 }
 

function GetPersons() {
    var response = InvokeService("Entities_PersonMethods/searchall/", "GET", "");

    if (response.code == 200) {

        unit = document.getElementById("unit-incharge");
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
  }

  function GetWareHouse() {
    var response = InvokeService("Inventory_InventoryUnitLocation/getall", "GET", "");

    if (response.code == 200) {

        tblWarehouses = document.getElementById("tblWarehouses");
        tblWarehouses.innerHTML = "";
        var data = JSON.parse(response.data);

        if(data.code == 200){

            var Warehouses = JSON.parse(data.jsonData);
            tblWarehouses.innerHTML = "";

            var htmlData = "";
            for (var i = 0; i < Warehouses.length; i++) {
                htmlData += "<tr>";
                htmlData += "<td> "+ Warehouses[i].description +"</td>";
                if(Warehouses[i].warehouse == 1){
                    htmlData += "<td> "+ '<button type="button" class="btn btn-success" disabled> <i class="fa fa-check"></i></button>' +"</td>";
                    htmlData += "<td></td>";
                }else{
                    htmlData += "<td></td>";
                    htmlData += "<td> "+ '<button type="button" class="btn btn-success" disabled> <i class="fa fa-check"></i></button>' +"</td>";
                    
                }
                htmlData += "<td>";
                htmlData += '<div class="table-data-feature">';
                                                                          
                htmlData += '<button type="button" class="btn btn-outline-primary" onclick="window.location.href=\'edit-warehouses-stores-customer-management.html?id=' + Warehouses[i].unit_location_id + '\'"><i class="fa fa-pencil-square-o"></i></button>';
                htmlData += '&nbsp;<button type="button" class="btn btn-danger" data-toggle="modal" data-target="#staticModal" data-toggle="tooltip" data-placement="top" title="" data-original-title="Delete" onclick="SetIDToDelete(' + Warehouses[i].unit_location_id + ')"><i class="fa fa-trash" style="color: white;"></i>&nbsp;</button>';
                
                htmlData += '</div>';
                htmlData += "</td>";
                
                htmlData += "</tr>";

                tblWarehouses.innerHTML = htmlData;
               
            }
        }else{
            tblWarehouses.innerHTML = "<tr><td colspan=5 class='text-left'>"+ data.message +"</td></tr>";
            
        }
      
    }
    else{
        tblWarehouses.innerHTML = "<tr><td colspan=5 class='text-left'>"+ "The system encounter an error while retrieving data." +"</td></tr>";
        
    }
  }
  function DeleteUnitLocation(){

    var unit_location_id = document.getElementById("unit_location_id");
 
    var response = InvokeService("Inventory_InventoryUnitLocation/id?id=" + unit_location_id.value, "DELETE", "");

    if (response.code == 200) {
        var data = JSON.parse(response.data);

        if(data.code == 200){
            ShowModal("Unit Location is deleted.", true, "warehouse-stores.html");
           
        }else{
          //failednotice
          ShowModal("The system encounter an error. Please try again later.", false,"");
        }
    }else{
        //failednotice
        ShowModal("The system encounter an error. Please try again later.", false,"");
      }


  }

  function SetIDToDelete(location_id){
    var unit_location_id = document.getElementById("unit_location_id");
    unit_location_id.value = location_id;
  }

