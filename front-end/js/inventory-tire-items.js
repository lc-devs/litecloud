var engineadditives = document.getElementById("enginge-additives");
var brandinput = document.getElementById("brandname-inputlabel");
var itemdesinput = document.getElementById("vehicle-inputlabel");
var stuinput = document.getElementById("stu-input");
var partdescription = document.getElementById("partdescription-inputlabel");
var partnumber = document.getElementById("partnumber-inputlabel");

var rtuinput = document.getElementById("landline2-input");
var numberofrtu = document.getElementById("number-input");
var avecostinput = document.getElementById("avecost-input");
var murateinput = document.getElementById("murate-input");
var spriceinput = document.getElementById("sprice-input");
var alert = document.getElementById("alrt");
var sizeinput = document.getElementById("size-input");


//===================================================================
//Function: SavingInventoryvehicleparts
//Purpose: To save the vehicle parts modification
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================

function SavingInventoryvehicleparts(){
// this are the variables used for the inventory vehicle part items
brandnamevehicle = document.getElementById("brandname-inputlabel");
vehiclecategoryvehicle = document.getElementById("vehicle-inputlabel");
partdescriptionvehicle = document.getElementById("partdescription-inputlabel");
partnumbervehicle = document.getElementById("partnumber-inputlabel");
stockingunit = document.getElementById("stu-input");
retailunit = document.getElementById("landline2-input");
numberofrtu = document.getElementById("number-input");
weightedaverage = document.getElementById("avecost-input");
markuprate = document.getElementById("murate-input");
alertvehicle = document.getElementById("alrt");
inputs = [brandnamevehicle.value, vehiclecategoryvehicle.value, partdescriptionvehicle.value, partnumbervehicle.value, stockingunit.value, retailunit.value, numberofrtu.value,weightedaverage.value, markuprate.value]
 
 //Variable to keep track of Errors. Initialize to 0.
 var err = 0;
    
 //Start loop to validate.
 for (var i = 0; i < inputs.length; i++) {
     //Checks fields in the array making sure they are not empty.
     if(inputs[i] === "") {
         err++;
     }
 }
// conditions
if (brandnamevehicle.value == 0 ){
    alertvehicle.style.display="block";
    brandnamevehicle.style.borderColor="red";
}else{
    brandnamevehicle.style.borderColor="#ced4da";
}
if (vehiclecategoryvehicle.value == 0){
    alertvehicle.style.display="block";
    vehiclecategoryvehicle.style.borderColor="red";
}else{
    vehiclecategoryvehicle.style.borderColor="#ced4da";
}
if (partdescriptionvehicle.value == 0){
    alertvehicle.style.display="block";
    partdescriptionvehicle.style.borderColor="red";
}else{
    partdescriptionvehicle.style.borderColor="#ced4da";
}
if (partnumbervehicle.value == 0){
    alertvehicle.style.display="block";
    partnumbervehicle.style.borderColor="red";
}else{
    partnumbervehicle.style.borderColor="#ced4da";
}
if (stockingunit.value == ""){
    alertvehicle.style.display="block";
    stockingunit.style.borderColor="red";
}else{
    stockingunit.style.borderColor="#ced4da";
}
if (retailunit.value == ""){
    alertvehicle.style.display="block";
    retailunit.style.borderColor="red";
}else{
    retailunit.style.borderColor="#ced4da";
}
if (numberofrtu.value == ""){
    alertvehicle.style.display="block";
    numberofrtu.style.borderColor="red";
}else{
    numberofrtu.style.borderColor="#ced4da";
}
if (weightedaverage.value == ""){
    alertvehicle.style.display="block";
    weightedaverage.style.borderColor="red";
}else{
    weightedaverage.style.borderColor="#ced4da";
}
if (markuprate.value == ""){
    alertvehicle.style.display="block";
    markuprate.style.borderColor="red";
}else{
    markuprate.style.borderColor="#ced4da";
    
}
//Check that there are no errors.
if (err === 0) {
    alertvehicle.style.display = "none";
    document.getElementById("notice").style.display = "none";
    $('#firstconfirm').modal('show');
} else {
    alertvehicle.style.display = "block";
}

}
//===================================================================
//Function: brandname
//Purpose: To add new brandname
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================


function AddBrandname() {
    window.location.href=("brandname-management-page.html");
}
function size(){
    window.location.href=("size-management-page.html");
}

//===================================================================
//Function: partdescription
//Purpose: To add new partdescription
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================

function AddPartDescription(){
    window.location.href=("ratio-management-page.html");
}
//===================================================================
//Function: partnumber
//Purpose: To add new partnumber
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================

function AddPartNumber(){
    window.location.href=("thread-pattern-management.html");
}
function AddVehicleModel(){
    window.location.href=("size-management-page.html");
}
//===================================================================
//Function: SaveVehicleParts()
//Purpose: To save the newly battery items
//Author: Medy Villarias
//Parameter:
//  Name                  Comment
//  -----------            --------------------------------
//  response            table data for all country
//
//Result: save the model descriptiom,stocking unit,retail unit, numberrtu,weighted cost,mark up rate;
// 
//===================================================================
//saving inventory battery items
function SaveVehicleParts() {
var engineadditives = document.getElementById("enginge-additives").value;
console.log(engineadditives);
var brandinput = document.getElementById("brandname-inputlabel").value;
console.log(brandinput);
var itemdesinput = document.getElementById("vehicle-inputlabel").value;
var stuinput = document.getElementById("stu-input").value;
var partdescription = document.getElementById("partdescription-inputlabel").value;
var partnumber = document.getElementById("partnumber-inputlabel").value;
var rtuinput = document.getElementById("landline2-input").value;
var numberofrtu = document.getElementById("number-input").value;
var avecostinput = document.getElementById("avecost-input").value;
var murateinput = document.getElementById("murate-input").value;
var spriceinput = document.getElementById("sprice-input").value;
    objparam={
        "item_id": 0,
        "category_id": engineadditives,
        "brand_id": brandinput,
        "model_description": itemdesinput,
        "part_description": 0,
        "part_number": 0,
        "size": itemdesinput,
        "valve_type": 0,
        "ratio": partdescription,
        "thread_pattern": partnumber,
        "stocking_unit": stuinput,
        "retail_unit": rtuinput,
        "rtu_over_stu": numberofrtu,
        "wtd_ave_cost": avecostinput,
        "markup_rate": murateinput,
        "selling_price": spriceinput,
        "user_id": 0,
        "entry_date": "string"
      }

      paramBody=JSON.stringify(objparam);
      console.log(paramBody);
    var response = InvokeService("Inventory_InventoryItemsMethods" , "POST", paramBody);
      
    if (response.code == 200) {
      
        var data = JSON.parse(response.data);

        var vehicleparts = JSON.parse(data.jsonData);
     
       
    }

}
//===================================================================
//Function: GetSize
//Purpose: To get all the Size in the data table
//Author: Medy B. Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(brandname);
// 
//===================================================================
function GetSize(){
   
    var response = InvokeService("Inventory_ModelsMethods/getall?type=3", "GET", "");
    console.log(response);

    if (response.code == 200) {

        inputsize = document.getElementById("vehicle-inputlabel");
        inputsize.innerHTML = "";

        var data = JSON.parse(response.data);
    
        var sizes = JSON.parse(data.jsonData);
    
        sizes.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < sizes.length; i++) {
            PopulateDropdownList(inputsize, sizes[i].id, sizes[i].description, sizes[i].user_id, sizes[i].entry_date);
        }
        
    }
}
//===================================================================
//Function: GetBrandname
//Purpose: To get all Brand name in the data table
//Author: Medy Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(different brand name);
// 
//===================================================================
function GetBrandname(){
   
    var response = InvokeService("Inventory_BrandsMethods/getall", "GET", "");
    // console.log(response);

    if (response.code == 200) {
        brandname = document.getElementById("brandname-inputlabel");
        brandname.innerHTML = "";
        var data = JSON.parse(response.data);
    
        var brandnames = JSON.parse(data.jsonData);
    
        brandname.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < brandnames.length; i++) {
            PopulateDropdownList(brandname, brandnames[i].brand_id, brandnames[i].brand_name);
        }
        
    }
}

//===================================================================
//Function: GetVehicleModel
//Purpose: To get all Brand name in the data table
//Author: Medy Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(different brand name);
// 
//===================================================================
function GetVehicleModel(){
   
    var response = InvokeService("Inventory_ModelsMethods/getall?type=3", "GET", "");
    console.log(response);

    if (response.code == 200) {

        inputsize = document.getElementById("size-input");
        inputsize.innerHTML = "";

        var data = JSON.parse(response.data);
    
        var sizes = JSON.parse(data.jsonData);
    
        sizes.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < sizes.length; i++) {
            PopulateDropdownList(inputsize, sizes[i].id, sizes[i].description, sizes[i].user_id, sizes[i].entry_date);
        }
        
    }
}
//===================================================================
//Function: GetPartNumber
//Purpose: To get all the partnumber in the data table
//Author: Medy Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(different brand name);
// 
//===================================================================
function GetThreadpattern(){
   
    var response = InvokeService("Inventory_ModelsMethods/getall?type=4", "GET", "");
    console.log(response);

    if (response.code == 200) {

        inputpartnumber = document.getElementById("partnumber-inputlabel");
        inputpartnumber.innerHTML = "";

        var data = JSON.parse(response.data);
    
        var partnumber = JSON.parse(data.jsonData);
    
        partnumber.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < partnumber.length; i++) {
            PopulateDropdownList(inputpartnumber, partnumber[i].id, partnumber[i].description, partnumber[i].part_number_id, partnumber[i].entry_date);
        }
        
    }
}
//===================================================================
//Function: GetPartDescription
//Purpose: To get the partdescription in the data table
//Author: Medy Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(different brand name);
// 
//===================================================================
function GetRatio(){
   
    var response = InvokeService("Inventory_ModelsMethods/getall?type=2", "GET", "");
    console.log(response);

    if (response.code == 200) {

        inputpartdescription = document.getElementById("partdescription-inputlabel");
        inputpartdescription.innerHTML = "";

        var data = JSON.parse(response.data);
    
        var partdescription = JSON.parse(data.jsonData);
    
        partdescription.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < partdescription.length; i++) {
            PopulateDropdownList(inputpartdescription, partdescription[i].id, partdescription[i].description, partdescription[i].part_id, partdescription[i].entry_date);
        }
        
    }
}
//===================================================================
//Function: GetCategory
//Purpose: To get all category in the data table
//Author: Mark Anthony Alegre
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(category);
// 
//===================================================================
function GetCategory(){
   
    var response = InvokeService("Inventory_ModelsMethods/getall?type=1", "GET", "");
    console.log(response);

    if (response.code == 200) {
        category = document.getElementById("enginge-additives");
        category.innerHTML = "";
        var data = JSON.parse(response.data);
    
        var categories = JSON.parse(data.jsonData);
    
        category.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
    
        for (var i = 0; i < categories.length; i++) {
            PopulateDropdownList(category, categories[i].id, categories[i].description, categories[i].user_id, categories[i].entry_date);
        }
        
    }
}

document.getElementById("avecost-input").addEventListener("keyup", calculation);
        document.getElementById("murate-input").addEventListener("keyup", calculation);
        document.getElementById("sprice-input").addEventListener("keyup", calculation);
        document.getElementById("number-input").addEventListener("keyup", calculation);
        
        function calculation(event) {
          var avecostinput = parseFloat(document.getElementById("avecost-input").value);
          var murateinput = parseFloat(document.getElementById("murate-input").value)/100;
          var spriceinput = parseFloat(document.getElementById("sprice-input").value);
          var ratioinput = parseFloat(document.getElementById("number-input").value);
        
          var markupResult = spriceinput + avecostinput + avecostinput * murateinput * ratioinput;
          var priceResult = murateinput * avecostinput + avecostinput * ratioinput ;
        
          switch (event.target.getAttribute("id")) {
            case "murate-input":
              document.getElementById("sprice-input").value = priceResult.toFixed(1);
              break;
        
            case "sprice-input":
              document.getElementById("murate-input").value = markupResult.toFixed(2);
              break;
        
          }

        
        }