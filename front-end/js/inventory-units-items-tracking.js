
//===================================================================
//Function: GoBackToUnitPage
//Purpose: Use as a function for redirecting to Inventory-units-items-tracking page
//Author: James Carl Sitsit
//Parameter:
//  Name                Comment
//  -----------         --------------------------------
//  None                None
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function GoBackToUnitPage(){
    window.location.href=("Inventory-units-items-tracking.html")
}

//===================================================================
//Function: GoToUnitPageForm
//Purpose: Use as a function for redirecting to add-inventory-units-items-tracking page
//Author: James Carl Sitsit
//Parameter:
//  Name                Comment
//  -----------         --------------------------------
//  None                None
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function GoToUnitPageForm(){
    window.location.href=("add-inventory-units-items-tracking.html")
}
 
//===================================================================
//Function: search_records
//Purpose: Use as a searching algorithm in the table. This makes the records in the table searchable.
//Author: James Carl Sitsit
//Parameter:
//  Name                Comment
//  -----------         --------------------------------
//  None                None
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function search_records() {
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

//===================================================================
//Function: ValidateInput
//Purpose: Use to validate the inputs in the inventory-units-items-tracking page form. This serves as the function for form validation.
//Author: James Carl Sitsit
//Parameter:
//  Name                 Comment
//  -----------          --------------------------------
//  None                 None
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function ValidateInput() {

    var categoryinput = document.getElementById("category-input");
    var inventoryiteminput = document.getElementById("inventoryitem-input");
    var unitinput = document.getElementById("unit-input");
    var startingperiodinput = document.getElementById("startingperiod-input");
    var lastentryinput = document.getElementById("lastentry-input");
    var startingqntinput = document.getElementById("startingqnt-input");
    var startingqntininput = document.getElementById("startingqntin-input");
    var startingqntoutinput = document.getElementById("startingqntout-input");
    var endingqntinput = document.getElementById("endingqnt-input");
    var startingcostinput = document.getElementById("startingcost-input");
    var startingcostininput = document.getElementById("startingcostin-input");
    var startingcostoutinput = document.getElementById("startingcostout-input");
    var endingcostinput = document.getElementById("endingcost-input");
    var unitcostinput = document.getElementById("unitcost-input");
    var lasthighinunitcostinput = document.getElementById("lasthighinunitcost-input");
    var alert = document.getElementById("alrt");
    var categoryinputvalues = categoryinput.options[categoryinput.selectedIndex].text;   
    var inventoryiteminputvalues = inventoryiteminput.options[inventoryiteminput.selectedIndex].text;  
    var unitinputvalues = unitinput.options[unitinput.selectedIndex].text;

    var inputs = [categoryinputvalues, inventoryiteminputvalues, unitinputvalues, startingperiodinput.value, lastentryinput.value, startingqntinput.value, startingqntininput.value, startingqntoutinput.value,
                endingqntinput.value, startingcostinput.value, startingcostininput.value, startingcostoutinput.value, endingcostinput.value, unitcostinput.value, lasthighinunitcostinput.value];

    //Variable to keep track of Errors. Initialize to 0.
    var err = 0

    //Start loop to validate.
    for (var i = 0; i < inputs.length; i++) {
        //Checks fields in the array making sure they are not empty.
        if(inputs[i] === "" || categoryinputvalues == "Category" || inventoryiteminputvalues == "Brand Name" ) {
            err++;
        }
    }

    if(categoryinputvalues == ""|| categoryinputvalues == "Category" || categoryinputvalues == "Please select a category" ) {

        categoryinput.style.borderColor = "red";

    }else{

        categoryinput.style.borderColor = "#ced4da";

    }

    if(inventoryiteminputvalues == ""|| inventoryiteminputvalues == "Inventory Item" || inventoryiteminputvalues == "Please select an item") {

        inventoryiteminput.style.borderColor = "red";

    }else{

        inventoryiteminput.style.borderColor = "#ced4da";
        
    }

    if(unitinputvalues == ""|| unitinputvalues == "Unit Location" || unitinputvalues == "Please select a unit") {

        unitinput.style.borderColor = "red";

    }else{

        unitinput.style.borderColor = "#ced4da";
        
    }

    if(startingperiodinput.value == "") {

        startingperiodinput.style.borderColor = "red";

    }else{

        startingperiodinput.style.borderColor = "#ced4da";
        
    }
    
    if(!lastentryinput.value) {

        lastentryinput.style.borderColor = "red";
        
    }else{

        lastentryinput.style.borderColor = "#ced4da";
            
    }

    if(startingqntinput.value == "") {

        startingqntinput.style.borderColor = "red";

    }else{

        startingqntinput.style.borderColor = "#ced4da";
        
    }

    if(startingqntininput.value == "") {

        startingqntininput.style.borderColor = "red";

    }else{

        startingqntininput.style.borderColor = "#ced4da";
        
    }

    if(startingqntoutinput.value == "") {

        startingqntoutinput.style.borderColor = "red";

    }else{

        startingqntoutinput.style.borderColor = "#ced4da";
        
    }
    
    if(endingqntinput.value == "") {

        endingqntinput.style.borderColor = "red";

    }else{

        endingqntinput.style.borderColor = "#ced4da";
        
    }
    
    if(startingcostinput.value == "") {

        startingcostinput.style.borderColor = "red";

    }else{

        startingcostinput.style.borderColor = "#ced4da";
        
    }

    if(startingcostininput.value == "") {

        startingcostininput.style.borderColor = "red";
        
    }else{

        startingcostininput.style.borderColor = "#ced4da";
            
    }

    if(startingcostoutinput.value == "") {

        startingcostoutinput.style.borderColor = "red";

    }else{

        startingcostoutinput.style.borderColor = "#ced4da";
        
    }

    if(endingcostinput.value == "") {

        endingcostinput.style.borderColor = "red";

    }else{

        endingcostinput.style.borderColor = "#ced4da";
        
    }

    if(unitcostinput.value == "") {

        unitcostinput.style.borderColor = "red";

    }else{

        unitcostinput.style.borderColor = "#ced4da";
        
    }
    
    if(lasthighinunitcostinput.value == "") {

        lasthighinunitcostinput.style.borderColor = "red";

    }else{

        lasthighinunitcostinput.style.borderColor = "#ced4da";
        
    }
    

    //Check that there are no errors.
    if (err === 0) {
        alert.style.display = "none";
        document.getElementById("notice").style.display = "none";
        $('#SaveModal').modal('show');
        $('#AddNewItem').modal('hide');
    } else {
        alert.style.display = "block";
    }

}


function populateCategories(){
    var categoriesResponse = InvokeService("Inventory_ModelsMethods/getall?type=1", "GET", "");
    var category, data, categories;
    if (categoriesResponse.code == 200) {
        var category = document.getElementById("category-input");
        category.innerHTML = "";
        var data = JSON.parse(categoriesResponse.data);
        var categories = JSON.parse(data.jsonData);
    
        category.innerHTML += "<option value=\"" + "" + "\">" + "Please select a category" + "</option>";
    
        for (var i = 0; i < categories.length; i++) {
            PopulateDropdownList(category, categories[i].id, categories[i].description);
        }
        
    }
}

function populateItemsByCategory() {

    category = document.getElementById("category-input");

    var category_id = category.value;

    var response = InvokeService("Inventory_InventoryItemsMethods/getall/" + category_id , "GET", "");

    if (response.code == 200) {
        var inventoryitem = document.getElementById("inventoryitem-input");
        inventoryitem.innerHTML = "";
        var data = JSON.parse(response.data);

        var inventoryitems = JSON.parse(data.jsonData);
        inventoryitem.innerHTML += "<option value=\"" + "" + "\">" + "Please select an item" + "</option>";
        for (var i = 0; i < inventoryitems.length; i++) {
            PopulateDropdownList(inventoryitem, inventoryitems[i].item_id, inventoryitems[i].model_description);
        }

    }

}

function populateUnitLocation(){
    var unitLocationResponse = InvokeService("Inventory_InventoryUnitLocation/getall", "GET", "");
    var unit, data, units;
    if (unitLocationResponse.code == 200) {
        var unit = document.getElementById("unit-input");
        unit.innerHTML = "";
        var data = JSON.parse(unitLocationResponse.data);
        var units = JSON.parse(data.jsonData);
    
        unit.innerHTML += "<option value=\"" + "" + "\">" + "Please select a unit" + "</option>";
    
        for (var i = 0; i < units.length; i++) {
            PopulateDropdownList(unit, units[i].unit_location_id, units[i].description);
        }
        
    }
}

function setLastEntry(){

    var date = new Date();
    var currentDate = date.toISOString().slice(0,10);
    var currentTime = date.getHours() + ':' + date.getMinutes();
    var lastentryinput = document.getElementById("lastentry-input");  
    var lastentrytime = currentDate + "T" + currentTime;
    lastentryinput.value = lastentrytime;
    

}



function addNewItems(){


    var inventoryiteminput = document.getElementById('inventoryitem-input');
    var inventoryiteminputIDvalue = inventoryiteminput.options[inventoryiteminput.selectedIndex].value;
    var unitinput = document.getElementById('unit-input');
    var unitinputIDvalue = unitinput.options[unitinput.selectedIndex].value;
    var startingperiodinput = document.getElementById("startingperiod-input").value;
    var lastentryinput = document.getElementById("lastentry-input").value;    
    var startingqntinput = document.getElementById("startingqnt-input").value;
    var startingqntininput = document.getElementById("startingqntin-input").value;
    var startingqntoutinput = document.getElementById("startingqntout-input").value;    
    var endingqntinput = document.getElementById("endingqnt-input").value;
    var startingcostinput = document.getElementById("startingcost-input").value;    
    var startingcostininput = document.getElementById("startingcostin-input").value;
    var endingcostinput = document.getElementById("endingcost-input").value;    
    var startingcostoutinput = document.getElementById("startingcostout-input").value;
    var unitcostinput = document.getElementById("unitcost-input").value;
    var lasthighinunitcostinput = document.getElementById("lasthighinunitcost-input").value;
     

    //declaring the value of the paramBody...
    var objParam={
        "unit_item_id": 0,
        "item_id": 11,
        "unit_id": 25,
        "starting_period": startingperiodinput,
        "last_entry": lastentryinput,
        "starting_quantity": startingqntinput,
        "quantity_in": startingqntininput,
        "quantity_out": startingqntoutinput,
        "ending_quantity": endingqntinput,
        "starting_cost": startingcostinput,
        "cost_in": startingcostininput,
        "cost_out": startingcostoutinput,
        "ending_cost": endingcostinput,
        "unit_cost": unitcostinput,
        "last_highest_in_unit_cost": lasthighinunitcostinput,
        "user_id": 0,
        "entry_date": ""

      }
   
      var paramBody = JSON.stringify(objParam);
   
      var response = InvokeService("Inventory_InventoryUnitsItems", "POST", paramBody);
    //getting the paramBody 
    
    if (response.code == 200) {
   
        $('#ConfirmSaveSuccessModal').modal('show');
        $('#ConfirmSaveFailedModal').modal('hide');
   
    }else{
   
       $('#ConfirmSaveSuccessModal').modal('hide');
       $('#ConfirmSaveFailedModal').modal('show');
   
    }
   
}