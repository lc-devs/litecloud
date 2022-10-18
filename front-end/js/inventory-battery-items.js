var engineadditives = document.getElementById("engine-additives");
var brandinput = document.getElementById("brand-input");
var itemdesinput = document.getElementById("itemdes-input");
var stuinput = document.getElementById("stu-input");
var rtuinput = document.getElementById("rtu-input");
var ratioinput = document.getElementById("ratio-input");
var avecostinput = document.getElementById("avecost-input");
var murateinput = document.getElementById("murate-input");
var spriceinput = document.getElementById("sprice-input");
var alert = document.getElementById("alrt");
var sizeinput = document.getElementById("size-input");

//===================================================================
//Function: ValidateInput
//Purpose: To validate and secure information
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
//
//Result:
// serviceResponse (ServiceResponse Class)
//===================================================================

    function ValidateInput() {

 // this is the variables for inventory battery items
var engineadditives = document.getElementById("engine-additives");
var brandinput = document.getElementById("brand-input");
var itemdesinput = document.getElementById("itemdes-input");
var stuinput = document.getElementById("stu-input");
var rtuinput = document.getElementById("rtu-input");
var ratioinput = document.getElementById("ratio-input");
var avecostinput = document.getElementById("avecost-input");
var murateinput = document.getElementById("murate-input");
var spriceinput = document.getElementById("sprice-input");
var alert = document.getElementById("alrt");
var sizeinput = document.getElementById("size-input");
var engineadditivesvalues = engineadditives.options[engineadditives.selectedIndex].text;   
var brandinputvalues = brandinput.options[brandinput.selectedIndex].text;   
var sizeinputvalues = sizeinput.options[sizeinput.selectedIndex].text;
var inputs = [engineadditives.value,brandinput.value,sizeinput.value, itemdesinput.value, stuinput.value, rtuinput.value, ratioinput.value, avecostinput.value, murateinput.value];

        //Variable to keep track of Errors. Initialize to 0.
        var err = 0
    
        //Start loop to validate.
        for (var i = 0; i < inputs.length; i++) {
            //Checks fields in the array making sure they are not empty.
            if(inputs[i] === "") {
                err++;
            }
        }

        if(engineadditivesvalues == ""|| engineadditivesvalues == "Engine additives") {

            engineadditives.style.borderColor = "red";

        }else{

            engineadditives.style.borderColor = "#ced4da";

        }

        if(brandinputvalues == ""|| brandinputvalues == "Select from existing brands - with option to add brand") {

            brandinput.style.borderColor = "red";

        }else{

            brandinput.style.borderColor = "#ced4da";
            
        }
        
        if( sizeinputvalues == "Select from existing sizes - with option to add size") {

            sizeinput.style.borderColor = "red";

        }else{

            sizeinput.style.borderColor = "#ced4da";
            
        }
        if(itemdesinput.value == "") {

            itemdesinput.style.borderColor = "red";

        }else{

            itemdesinput.style.borderColor = "#ced4da";
            
        }

        if(stuinput.value == "") {

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
            
        }

        if(avecostinput.value == "") {

            avecostinput.style.borderColor = "red";

        }else{

            avecostinput.style.borderColor = "#ced4da";
            
        }

        if(murateinput.value == "") {

            murateinput.style.borderColor = "red";
            

        }else{

            murateinput.style.borderColor = "#ced4da";
            
        }

        
    
        //Check that there are no errors.
        if (err === 0) {
            alert.style.display = "none";
            document.getElementById("notice").style.display = "none";
            $('#firstconfirm').modal('show');
        } else {
            alert.style.display = "block";
        }

    }
//===================================================================
//Function: ValidateNumberInput
//Purpose: To validate and secure information
//Author: Medy B. Villarias
//Parameter:
//  Name             I/O         Comment
//  -----------      ----        --------------------------------
//  person_id         I           id of the person
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
//Function: AddBrandname
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

//===================================================================
//Function: AddSize
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

function AddSize(){
    window.location.href=("size-management-page.html");
}

//===================================================================
//Function: SaveBatteryItems()
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
function SaveBatteryItems() {
    var engineadditives = document.getElementById("engine-additives").value;
    console.log(engineadditives);
var brandinput = document.getElementById("brand-input").value;
var itemdesinput = document.getElementById("itemdes-input").value;
var stuinput = document.getElementById("stu-input").value;
var sizeinput = document.getElementById("size-input").value;
console.log(sizeinput);
var rtuinput = document.getElementById("rtu-input").value;
var ratioinput = document.getElementById("ratio-input").value;
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
        "size": sizeinput,
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
        "entry_date": "string"
      }

      paramBody=JSON.stringify(objparam);
      console.log(paramBody);
    var response = InvokeService("Inventory_InventoryItemsMethods" , "POST", paramBody);
      
    if (response.code == 200) {
      
        var data = JSON.parse(response.data);

        var battery = JSON.parse(data.jsonData);
     
       
    }

}


//===================================================================
//Function: GetCategory
//Purpose: To get all category in the data table
//Author: Medy B. Villarias
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
        category = document.getElementById("engine-additives");
        category.innerHTML = "";
        var data = JSON.parse(response.data);
    
        var categories = JSON.parse(data.jsonData);
    
        category.innerHTML += "<option value=\"" + "" + "\">" + "Please select" + "</option>";
        for (var i = 0; i < categories.length; i++) {
            PopulateDropdownList(category, categories[i].id, categories[i].description, categories[i].user_id, categories[i].entry_date);
        }
        
    }
}

//===================================================================
//Function: GetBrandname
//Purpose: To get all Brandname in the data table
//Author: Medy B. Villarias
//Parameter:
//  Name                    Comment
//  -----------            --------------------------------
// 
//
//Result:PopulateDropdownList(brandname);
// 
//===================================================================

function GetBrandname(){
   
    var response = InvokeService("Inventory_BrandsMethods/getall", "GET", "");
    // console.log(response);

    if (response.code == 200) {
        brandname = document.getElementById("brand-input");
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

document.getElementById("avecost-input").addEventListener("keyup", calculation);
        document.getElementById("murate-input").addEventListener("keyup", calculation);
        document.getElementById("sprice-input").addEventListener("keyup", calculation);
        document.getElementById("ratio-input").addEventListener("keyup", calculation);
        
        function calculation(event) {
          var avecostinput = parseFloat(document.getElementById("avecost-input").value);
          var murateinput = parseFloat(document.getElementById("murate-input").value)/100;
          var spriceinput = parseFloat(document.getElementById("sprice-input").value);
          var ratioinput = parseFloat(document.getElementById("ratio-input").value);
        
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
