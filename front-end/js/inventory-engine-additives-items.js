

//===================================================================
//Function: ValidateInput
//Purpose: Use to validate the inputs in the inventory-engine-additives-items page form. This serves as the function for form validation.
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
    var brandinput = document.getElementById("brand-input");
    var itemdesinput = document.getElementById("itemdes-input");
    var stuinput = document.getElementById("stu-input");
    var rtuinput = document.getElementById("rtu-input");
    var ratioinput = document.getElementById("ratio-input");
    var avecostinput = document.getElementById("avecost-input");
    var murateinput = document.getElementById("murate-input");
    var spriceinput = document.getElementById("sprice-input");
    var alert = document.getElementById("alrt");
    var categoryinputvalues = categoryinput.options[categoryinput.selectedIndex].text;   
    var brandinputvalues = brandinput.options[brandinput.selectedIndex].text;   

    var inputs = [categoryinputvalues,brandinputvalues, itemdesinput.value, stuinput.value, rtuinput.value, ratioinput.value,spriceinput.value];

    //Variable to keep track of Errors. Initialize to 0.
    var err = 0

    //Start loop to validate.
    for (var i = 0; i < inputs.length; i++) {
        //Checks fields in the array making sure they are not empty.
        if(inputs[i] === "" || categoryinputvalues == "Categories" || brandinputvalues == "Brand Name" || categoryinputvalues == "Please select a category" || brandinputvalues == "Please select a brand" ) {
            err++;
        }
    }
    
    if(categoryinputvalues == "" || categoryinputvalues == "Categories" || categoryinputvalues == "Please select a category") {

        categoryinput.style.borderColor = "red";

    }else{

        categoryinput.style.borderColor = "#ced4da";

    }

    if(brandinputvalues == "" || brandinputvalues == "Brand Name" || brandinputvalues == "Please select a brand" ) {

        brandinput.style.borderColor = "red";

    }else{

        brandinput.style.borderColor = "#ced4da";
        
    }

    if(itemdesinput.value == "") {

        itemdesinput.style.borderColor = "red";

    }else{

        itemdesinput.style.borderColor = "#ced4da";
        
    }if(stuinput.value == "") {

        stuinput.style.borderColor = "red";
        
    }else{

        stuinput.style.borderColor = "#ced4da";
            
        }

    if(rtuinput.value == "") {

        rtuinput.style.borderColor = "red";

    }else{

        rtuinput.style.borderColor = "#ced4da";
        
    }

    if(ratioinput.value == "") {

        ratioinput.style.borderColor = "red";

    }else{

        ratioinput.style.borderColor = "#ced4da";
        
    }if(avecostinput.value == "") {

        avecostinput.style.borderColor = "red";

    }else{

        avecostinput.style.borderColor = "#ced4da";
        
    }if(murateinput.value == "") {

        murateinput.style.borderColor = "red";

    }else{

        murateinput.style.borderColor = "#ced4da";
        
    }if(spriceinput.value == "") {

        spriceinput.style.borderColor = "red";

    }else{

        spriceinput.style.borderColor = "#ced4da";
        
    }

    //Check that there are no errors.
    if (err === 0) {
        alert.style.display = "none";
        document.getElementById("notice").style.display = "none";
        $('#SaveModal').modal('show');
    } else {
        alert.style.display = "block";
    }

}

//===================================================================
//Function: ValidateNumberInput
//Purpose: Use to validate the number input. This function will not allow negative numbers to be entered in some number input.
//Author: James Carl Sitsit
//Parameter:
//  Name                Comment
//  -----------         --------------------------------
//  event               It will detect the pressing of "-"
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function ValidateNumberInput(event) {
    if (event.key == "-") {
        event.preventDefault();
        document.getElementById("notice").style.display = "block";
        this.parentElement.style.borderColor = "red";
        return false;
    }
}

//===================================================================
//Function: DisableSelPrice
//Purpose: Function for disabling the Selling Price input if the weighted Average Cost input and Mark Up Rate input are filled.
//         This is implemented to avoid manipulation of the selling price.
//Author: James Carl Sitsit
//Parameter:
//  Name                Comment
//  -----------         --------------------------------
//  event               It will detect the pressing of "-"
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================
function DisableSelPrice() {

    var murateinput = document.getElementById("murate-input");

    if(murateinput.value != "") {

        document.getElementById("sprice-input").disabled = true;

    }else if(murateinput.value == 0){

        document.getElementById("sprice-input").disabled = false;
 
    }else{

        document.getElementById("sprice-input").disabled = false;
 
    }

}

//populate Categories Dropdown list
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


//populate Brand Dropdown list
function populateBrand(){
    var brandResponse = InvokeService("BrandsMethods/getall", "GET", "");
    var brand, data, brands;
    if (brandResponse.code == 200) {
        var brand = document.getElementById("brand-input");
        brand.innerHTML = "";
        var data = JSON.parse(brandResponse.data);
    
        var brands = JSON.parse(data.jsonData);
    
        brand.innerHTML += "<option value=\"" + "" + "\">" + "Please select a brand" + "</option>";
    
        for (var i = 0; i < brands.length; i++) {
            PopulateDropdownList(brand, brands[i].brand_id, brands[i].brand_name);
        }
        
    }
}


function addNewItems(){

    var brandinput = document.getElementById('brand-input');
    var brandIDvalue = brandinput.options[brandinput.selectedIndex].value;
    var categoryinput = document.getElementById('category-input');
    var categoryIDvalue = categoryinput.options[categoryinput.selectedIndex].value;
    var itemdesinput = document.getElementById("itemdes-input").value;
    var stuinput = document.getElementById("stu-input").value;
    var rtuinput = document.getElementById("rtu-input").value;
    var ratioinput = document.getElementById("ratio-input").value;
    var avecostinput = document.getElementById("avecost-input").value;
    var murateinput = document.getElementById("murate-input").value;
    var spriceinput = document.getElementById("sprice-input").value;         

    //declaring the value of the paramBody...
    var objParam={
        
            "item_id": 0,
            "category_id": categoryIDvalue,
            "brand_id": brandIDvalue,
            "model_description": itemdesinput,
            "part_description": 0,
            "part_number": 0,
            "size": 0,
            "valve_type": 0,
            "ratio": 0,
            "thread_pattern": 0,
            "stocking_unit": stuinput,
            "retail_unit": rtuinput,
            "rtu_over_stu": ratioinput,
            "wtd_ave_cost": avecostinput,
            "markup_rate": murateinput,
            "selling_price": spriceinput,
            "user_id": 0,
            "entry_date": ""
          
      }
   
      var paramBody = JSON.stringify(objParam);
      var response = InvokeService("Inventory_InventoryItemsMethods", "POST", paramBody);
    //getting the paramBody 
    if (response.code == 200) {
   
        $('#ConfirmSaveSuccessModal').modal('show');
        $('#ConfirmSaveFailedModal').modal('hide');
   
    }else{
   
       $('#ConfirmSaveSuccessModal').modal('hide');
       $('#ConfirmSaveFailedModal').modal('show');
   
    }
   
}

