    //===================================================================
    //Function: ValidateAddInventoryAdjustmentInput
    //Purpose: To validate the input for the add-inventory-adjustment page before proceeding to the confirm modals.
    //          this serves as a form validation for the inputs of the add-inventory-adjustment page
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  none                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function ValidateAddInventoryAdjustmentInput() {

        var adjustmentinput = document.getElementById("adjustment-input");
        var desunitinput = document.getElementById("desunit-input");
        var sourceinput = document.getElementById("source-input");
        var inventoryitems = document.getElementById("inventory-items");
        var quantityinput = document.getElementById("quantity-input");
        var unitinput = document.getElementById("unit-input");
        var unitcostinput = document.getElementById("unit-cost-input");
        var totalcostinput = document.getElementById("total-cost-input");
        var alert = document.getElementById("alrt");
        var adjustmentinputvalues = adjustmentinput.options[adjustmentinput.selectedIndex].text;   
        var desunitinputvalues = desunitinput.options[desunitinput.selectedIndex].text; 
        var sourceinputvalues = sourceinput.options[sourceinput.selectedIndex].text;
        var inventoryitemsvalues = inventoryitems.options[inventoryitems.selectedIndex].text;   

        console.log(adjustmentinputvalues);
        console.log(desunitinputvalues);
        console.log(sourceinputvalues);
        console.log(inventoryitemsvalues);
        
    
        var inputs = [adjustmentinputvalues, desunitinputvalues, sourceinputvalues, inventoryitemsvalues, quantityinput.value, unitinput.value, unitcostinput.value, totalcostinput.value];
    
        //Variable to keep track of Errors. Initialize to 0.
        var err = 0
    
        //Start loop to validate.
        for (var i = 0; i < inputs.length; i++) {
            //Checks fields in the array making sure they are not empty.
            if(inputs[i] === "" || adjustmentinputvalues == "Adjustments"|| adjustmentinputvalues == "Please select an adjustment" ||
            desunitinputvalues == "Destination Unit" || desunitinputvalues == "Please select a Destination Unit" ||
            sourceinputvalues == "Source Unit" || sourceinputvalues == "Please select a Source Unit" ||
            inventoryitemsvalues == "Inventory Item" || inventoryitemsvalues == "Please select an item") {
                err++;
            }
        }

        if(adjustmentinputvalues == ""|| adjustmentinputvalues == "Adjustments"|| adjustmentinputvalues == "Please select an adjustment") {

            adjustmentinput.style.borderColor = "red";

        }else{

            adjustmentinput.style.borderColor = "#ced4da";

        }

        if(desunitinputvalues == ""|| desunitinputvalues == "Destination Unit" || desunitinputvalues == "Please select a Destination Unit") {

            desunitinput.style.borderColor = "red";

        }else{

            desunitinput.style.borderColor = "#ced4da";
            
        }

        if(sourceinputvalues == ""|| sourceinputvalues == "Source Unit" || sourceinputvalues == "Please select a Source Unit") {

            sourceinput.style.borderColor = "red";

        }else{

            sourceinput.style.borderColor = "#ced4da";

        }

        if(inventoryitemsvalues == ""|| inventoryitemsvalues == "Inventory Item" || inventoryitemsvalues == "Please select an item" ) {

            inventoryitems.style.borderColor = "red";

        }else{

            inventoryitems.style.borderColor = "#ced4da";
            
        }if(quantityinput.value == "") {

            quantityinput.style.borderColor = "red";
            
        }else{

            quantityinput.style.borderColor = "#ced4da";
                
        }

        if(unitinput.value == "") {

            unitinput.style.borderColor = "red";

        }else{

            unitinput.style.borderColor = "#ced4da";
            
        }

        if(unitcostinput.value == "") {

            unitcostinput.style.borderColor = "red";

        }else{

            unitcostinput.style.borderColor = "#ced4da";
            
        }

        if(totalcostinput.value == "") {

            totalcostinput.style.borderColor = "red";

        }else{

            totalcostinput.style.borderColor = "#ced4da";
            
        }    
        //Check that there are no errors.
        if (err === 0) {
            alert.style.display = "none";
            document.getElementById("notice").style.display = "none";
            $('#SaveModal').modal('show');
            $('#AddNewAdjustment').modal('hide');
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
    //Function: ValidateAdjustmentDescription
    //Purpose: Use to validate the adjustment description content in the pop up modal located in the inventory adjustment templates page.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function ValidateAdjustmentDescription() {
        var adjustmentDescription = document.getElementById("adjustment-description");
        var alert = document.getElementById("alrt");
        

        if(adjustmentDescription.value == "") {

            adjustmentDescription.style.borderColor = "red";
            alert.style.display = "block"; 

        }else{

            adjustmentDescription.style.borderColor = "#ced4da";
            alert.style.display = "none";
            $('#SaveModal').modal('show');
            $('#AddNewAdjustmentTemplate').modal('hide');

        }

    }
    
    //===================================================================
    //Function: populateAdjustmentsTemplateTable
    //Purpose: this function is use to display the adjustment template data you have searched in the searchbar to the table
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateAdjustmentsTemplateTable() {
        var descriptionSearchInput = document.getElementById("searchbar").value;
        var jsonData = InvokeService("Inventory_AdjustmentTemplatesMethods/description/" + descriptionSearchInput , "GET", "");
        var parseJsonData = JSON.parse(jsonData.data);
        if (parseJsonData.code == 200) {
            var oData = JSON.parse(parseJsonData.jsonData);
            var divtbl = document.getElementById('tableForPopulation');
            var BoolForAddtoQuantity, BoolForRequireBoth;
            divtbl.innerHTML = "";
            for (var i = 0; i < oData.length; i++){
                if(oData[i].add_to_quantity == 0 ){
                    BoolForAddtoQuantity = "No";
                }else{
                    BoolForAddtoQuantity = "Yes";
                }
                if(oData[i].require_destination_and_source == 0){
                    BoolForRequireBoth = "No";
                }else{
                    BoolForRequireBoth = "Yes";
                }
                divtbl.innerHTML +=
                "<tr>" +
                "<td>"+ oData[i].description+"</td>" +
                "<td>"+ BoolForAddtoQuantity+"</td>" +
                "<td>"+ BoolForRequireBoth+"</td>" +
                "<td>"+ oData[i].user_id+"</td>" +
                "<td>"+ oData[i].entry_date+"</td>" +
                "<td>"+
                "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#EditAdjustmentTemplate"  + "\"" + "id=\"" + "editbrandbutton" + "\" onclick=\"DisplayCurrentAdjustmentTemplate('" + oData[i].template_id + "', '"+ oData[i].description  +"', '" + oData[i].add_to_quantity + "', '" + oData[i].require_destination_and_source + "' )\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" +
                "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-danger"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#DeleteModal"  + "\"" + "id=\"" + "deletebrandbutton" +
                 "\"  onclick=\"DisplayCurrentAdjustmentTemplateForDelete('" + oData[i].template_id + "')\">" + "<i class=\"" + "fa fa-trash" + "\"" + "style=\"" + "margin:2px;" + "\"" + "\">" + "</i>" + "</button>" + "</td>";
                "</tr>";
            }
        
        } else if(parseJsonData.code == 404){
            
        } else {
            ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);
        }
    }                            

    //===================================================================
    //Function: addNewAdjustmentTemplate
    //Purpose: this function is use to add new adjustment templates
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function addNewAdjustmentTemplate(){
        var newAdjustmentDescription = document.getElementById("adjustment-description").value;
        var CheckBoxState = document.getElementById("addbox");
        var RadioButton1State = document.getElementById("radio1");
        let addtoQuantityValue = 0, requirebothValue = 0;

        if(RadioButton1State.checked == false ){

            requirebothValue = 1;

        }else{

            requirebothValue = 0;

        }
        
        if(CheckBoxState.checked == false){

            addtoQuantityValue = 0;

        }else{

            addtoQuantityValue = 1;

        }
        
           
        //declaring the value of the paramBody...
        var objParam={
            "template_id": 0,
            "description": newAdjustmentDescription,
            "add_to_quantity": addtoQuantityValue,
            "require_destination_and_source": requirebothValue,
            "user_id": 0,
            "entry_date": ""
          }
       
          var paramBody = JSON.stringify(objParam);
       
          var response = InvokeService("Inventory_AdjustmentTemplatesMethods", "POST", paramBody);
        //getting the paramBody 
        
        if (response.code == 200) {
       
            var data = JSON.parse(response.data); //converting json string to json object(array)  
            JSON.parse(data.jsonData); //converting json string to json object(array)
            $('#ConfirmSaveSuccessModal').modal('show');
            $('#ConfirmSaveFailedModal').modal('hide');

            return populateAdjustmentsTable();
       
        }else{
       
           $('#ConfirmSaveSuccessModal').modal('hide');
           $('#ConfirmSaveFailedModal').modal('show');
       
        }
       
    }

    //===================================================================
    //Function: DisplayCurrentAdjustmentTemplateForDelete
    //Purpose: this function is use to store the id of the certain adjustment you want to delete inside a hidden html input. The stored id will be used in the invoking of the service URL.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  id                    id of the adjustment template you want to delete
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function DisplayCurrentAdjustmentTemplateForDelete(id){
        
        var tempID = document.getElementById("tempID");
        tempID.value = id;

    }

    //===================================================================
    //Function: DisplayCurrentAdjustmentTemplate
    //Purpose: this function is use when modifying adjustment template data. This function will display the data of a certain adjustment template you want to modify.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  id                    id of the adjustment template
    //  description           description of the adjustment template
    //  add2quant             a boolean data. 0 is No and 1 is Yes. This determines if the user want the item to be added to quantity or not.
    //  requireboth           a boolean data. 0 is No and 1 is Yes. This determines if the user want the item to require both destination and source unit or not.
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function DisplayCurrentAdjustmentTemplate(id, description, add2quant, requireboth){
        var currentAdjustmentDesc = document.getElementById("current-adjustment-description");
        var currentCheckboxState = document.getElementById("currentbox");
        var currentRadio1State = document.getElementById("currentRadio1");
        var currentRadio2State = document.getElementById("currentRadio2");
        var tempID = document.getElementById("tempID");
    
        tempID.value = id;
        currentAdjustmentDesc.value = description;

        if(add2quant == 0){
            currentCheckboxState.checked = false;
        }else{
            currentCheckboxState.checked = true;
        }

        if(requireboth == 0) {
            currentRadio1State.checked = true;
        }else{
            currentRadio2State.checked = true;
        }

    }
    
    //===================================================================
    //Function: updateAdjustmentTemplate
    //Purpose: the purpose of this function is use to update the database if you modify parts of the adjustment template data. The stored id from the function: DisplayCurrentAdjustmentTemplateForDelete(id) will be used in the invoking of the Service URL. 
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function updateAdjustmentTemplate(){
        var currentTempID = document.getElementById("tempID").value;
        var newAdjustmentDescription = document.getElementById("current-adjustment-description").value;
        var CheckBoxState = document.getElementById("currentbox");
        var RadioButton1State = document.getElementById("currentRadio1");
        let addtoQuantityValue = 0, requirebothValue = 0;

        if(RadioButton1State.checked == false ){

            requirebothValue = 1;

        }else{

            requirebothValue = 0;

        }
        
        if(CheckBoxState.checked == false){

            addtoQuantityValue = 0;

        }else{

            addtoQuantityValue = 1;

        }           
        //declaring the value of the paramBody...
        var objParam={
            "template_id": currentTempID,
            "description": newAdjustmentDescription,
            "add_to_quantity": addtoQuantityValue,
            "require_destination_and_source": requirebothValue,
            "user_id": 0,
            "entry_date": ""
          }
       
          var paramBody = JSON.stringify(objParam);
       
          var response = InvokeService("Inventory_AdjustmentTemplatesMethods/id", "PUT", paramBody);
        //getting the paramBody 
        
        if (response.code == 200) {
       
            var data = JSON.parse(response.data); //converting json string to json object(array)  
            JSON.parse(data.jsonData); //converting json string to json object(array)
            $('#ConfirmEditSuccessModal').modal('show');
            $('#ConfirmEditFailedModal').modal('hide');

            return populateAdjustmentsTemplateTable();
       
        }else{
       
           $('#ConfirmEditSuccessModal').modal('hide');
           $('#ConfirmEditFailedModal').modal('show');
       
        }
       
    }

    //===================================================================
    //Function: deleteAdjustmentTemplate
    //Purpose: this function is use to delete an adjustment template. Same with the function: updateAdjustmentTemplate(), the stored data from the function: DisplayCurrentAdjustmentTemplateForDelete(id) will be used in the invoking of the Service URL
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function deleteAdjustmentTemplate() {
        var currentTempID = document.getElementById("tempID").value;
        
        //declaring the value of the paramBody...
        var response = InvokeService("Inventory_AdjustmentTemplatesMethods/id?id=" + currentTempID , "DELETE");
        //getting the paramBody 
        // console.log(response);
        if (response.code == 200) {
            $('#ConfirmDeleteSuccessModal').modal('show');
            $('#ConfirmDeleteFailedModal').modal('hide');
       
            return populateAdjustmentsTemplateTable();
       
        }else{
       
           $('#ConfirmDeleteSuccessModal').modal('hide');
           $('#ConfirmDeleteFailedModal').modal('show');
       
        }
       
       
    }

    // inventory adjustment page part

    //===================================================================
    //Function: populateDropdownAdjustments
    //Purpose: this function is use to populate all the adjustment templates inside a dropdown
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateDropdownAdjustments(){
        var adjustmentResponse = InvokeService("Inventory_AdjustmentTemplatesMethods/getall", "GET", "");;
        var adjustment, data, adjustments;
        if (adjustmentResponse.code == 200) {
            adjustment = document.getElementById("adjustment-input");
            adjustment.innerHTML = "";
            var data = JSON.parse(adjustmentResponse.data);
            var adjustments = JSON.parse(data.jsonData);
        
            adjustment.innerHTML += "<option value=\"" + "" + "\" >" + "Please select an adjustment" + "</option>";
        
            for (var i = 0; i < adjustments.length; i++) {
                PopulateDropdownList(adjustment, adjustments[i].template_id, adjustments[i].description);
            }
            
        }
    }

    //===================================================================
    //Function: populateDropdownInventoryItem
    //Purpose: this function is use to populate all the inventory items inside a dropdown
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateDropdownInventoryItem(){
        var inventoryItemResponse = InvokeService("Inventory_InventoryItemsMethods/getall", "GET", "");;
        var item, data, items;
        if (inventoryItemResponse.code == 200) {
            item = document.getElementById("inventory-items");
            item.innerHTML = "";
            var data = JSON.parse(inventoryItemResponse.data);
            var items = JSON.parse(data.jsonData);
        
            item.innerHTML += "<option value=\"" + "" + "\" >" + "Please select an item" + "</option>";
        
            for (var i = 0; i < items.length; i++) {
                PopulateDropdownList(item, items[i].item_id, items[i].model_description);
            }
            
        }
    }

    //===================================================================
    //Function: populateDropdownDestinationUnit
    //Purpose: this function is use to populate all the destination unit inside a dropdown
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateDropdownDestinationUnit(){
        var unitResponse = InvokeService("Inventory_InventoryUnitLocation/getall", "GET", "");;
        var desunit, data, desunits;
        if (unitResponse.code == 200) {
            desunit = document.getElementById("desunit-input");
            desunit.innerHTML = "";
            var data = JSON.parse(unitResponse.data);
            var desunits = JSON.parse(data.jsonData);
        
            desunit.innerHTML += "<option value=\"" + "" + "\" >" + "Please select a Destination Unit" + "</option>";
        
            for (var i = 0; i < desunits.length; i++) {
                PopulateDropdownList(desunit, desunits[i].unit_location_id, desunits[i].description);
            }
            
        }
    }

    //===================================================================
    //Function: populateDropdownSourceUnit
    //Purpose: this function is use to populate all the source unit inside a dropdown
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateDropdownSourceUnit(){
        var unitResponse = InvokeService("Inventory_InventoryUnitLocation/getall", "GET", "");;
        var srcunit, data, units;
        if (unitResponse.code == 200) {
            srcunit = document.getElementById("source-input");
            srcunit.innerHTML = "";
            var data = JSON.parse(unitResponse.data);
            var units = JSON.parse(data.jsonData);
        
            srcunit.innerHTML += "<option value=\"" + "" + "\" >" + "Please select a Source Unit" + "</option>";
        
            for (var i = 0; i < units.length; i++) {
                PopulateDropdownList(srcunit, units[i].unit_location_id, units[i].description);
            }
            
        }
    }

    //===================================================================
    //Function: addNewAdjustments
    //Purpose: this function is use to create new invnetory adjustment
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function addNewAdjustments(){

        var adjustmenttemplateinput = document.getElementById('adjustment-input');
        var adjustmenttemplateIDvalue = adjustmenttemplateinput.options[adjustmenttemplateinput.selectedIndex].value;
        // var desunitinput = document.getElementById('desunit-input');
        // var desunitinputIDvalue = desunitinput.options[desunitinput.selectedIndex].value;
        // var sourceinput = document.getElementById('source-input');
        // var sourceinputIDvalue = sourceinput.options[sourceinput.selectedIndex].value;
        var inventoryitemsinput = document.getElementById('inventory-items');
        var inventoryitemsinputIDvalue = inventoryitemsinput.options[inventoryitemsinput.selectedIndex].value;
        var quantityinput = document.getElementById("quantity-input").value;
        var remarksinput = document.getElementById("remarks-input").value;    
        console.log()

        //declaring the value of the paramBody...
        var objParam={
            "adjustment_id": 0,
            "adjustment_date": "",
            "template_id": adjustmenttemplateIDvalue,
            "destination_id": 15,
            "source_id": 9,
            "item_id": inventoryitemsinputIDvalue,
            "quantity": quantityinput,
            "remarks": remarksinput,
            "user_id": 0,
            "entry_date": ""
          }
       
          var paramBody = JSON.stringify(objParam);
       
          var response = InvokeService("Inventory_ItemsAdjustmentsMethods", "POST", paramBody);
        //getting the paramBody 
        
        if (response.code == 200) {
       
            $('#ConfirmSaveSuccessModal').modal('show');
            $('#ConfirmSaveFailedModal').modal('hide');
       
        }else{
       
           $('#ConfirmSaveSuccessModal').modal('hide');
           $('#ConfirmSaveFailedModal').modal('show');
       
        }
       
    }

    //===================================================================
    //Function: populateAdjustmentsTable
    //Purpose: this function uses time span to search for adjustments. It requires you to fill the date from(beginning date) and date to(ending date). All the adjustments created between that timespan will then be displayed in the table
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateAdjustmentsTable() {
        var date_from = document.getElementById("date_from").value;
        var date_to = document.getElementById("date_to").value;
        var date_fromReformed = date_from + ":00";
        var date_toReformed = date_to + ":00";
        var jsonData = InvokeService("Inventory_ItemsAdjustmentsMethods/getall?date_from=" + date_fromReformed + "&date_to=" + date_toReformed , "GET", "");
        var parseJsonData = JSON.parse(jsonData.data);
        if (parseJsonData.code == 200) {
            var oData = JSON.parse(parseJsonData.jsonData);
            var divtbl = document.getElementById('adjustmentTblItems');
            divtbl.innerHTML = "";
            for (var i = 0; i < oData.length; i++){
                divtbl.innerHTML +=
                "<tr>" +
                "<td>"+ oData[i].adjustment_date+"</td>" +
                "<td style=\"" + "cursor: pointer" + "\">"+"<button type=\"" + "button" + "\"" + "style=\"" + "background: none!important;border: none;padding: 0!important;color:#069;" + "\"" + "onmouseover=\"StoreTempID('" + oData[i].template_id + "')\"" + "onclick=\"populateTemplateInformation();\">" + oData[i].template_id+"</td>" +
                "<td style=\"" + "cursor: pointer" + "\">"+"<button type=\"" + "button" + "\"" + "style=\"" + "background: none!important;border: none;padding: 0!important;color:#069;" + "\"" + "onmouseover=\"StoreUnitID('" + oData[i].destination_id + "')\"" + "onclick=\"populateUnitInformation();\">" + oData[i].destination_id+"</td>" +
                "<td style=\"" + "cursor: pointer" + "\">"+"<button type=\"" + "button" + "\"" + "style=\"" + "background: none!important;border: none;padding: 0!important;color:#069;" + "\"" + "onmouseover=\"StoreUnitID('" + oData[i].source_id + "')\"" + "onclick=\"populateUnitInformation();\">" + oData[i].source_id+"</td>" +
                "<td style=\"" + "cursor: pointer" + "\">"+"<button type=\"" + "button" + "\"" + "style=\"" + "background: none!important;border: none;padding: 0!important;color:#069;" + "\"" + "onmouseover=\"StoreItemID('" + oData[i].item_id + "')\"" + "onclick=\"populateItemInformation();\">" + oData[i].item_id+"</td>" ;
            }
        
        }
        
    }

    //===================================================================
    //Function: StoreTempID(id)
    //Purpose: this function is use to store the adjustment template id inside a hidden html input.
    //         This is different from the function: DisplayCurrentAdjustmentTemplateForDelete(id) because this stores the template id value which was populated in the table 
    //         in the inventory adjustment page. This is used when invoking the service URL once the user click the template id(the displayed template id is in a link button form) 
    //         because upon clicking the template id, all the template id information will be displayed in modal form.    
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  id                    id of the adjustment template 
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function StoreTempID(id) {
        var TempID = document.getElementById("tempidhidden");
        TempID.value = id;
    }

    //===================================================================
    //Function: populateTemplateInformation
    //Purpose: this function is use display the adjustment template information inside a modal. Once the adjustment template id link button is clicked the modal will pop up with all
    //         the information about the adjustment template. In the process of invoking the service, this function will use the id stored by the function: StoreTempID(id) 
    //         to invoke the service URL.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateTemplateInformation(){
        var tempID = document.getElementById("tempidhidden").value;
        var Response = InvokeService("Inventory_AdjustmentTemplatesMethods/getone/" + tempID, "GET", "");
        if (Response.code == 200) {
            var TemplateID = document.getElementById("TemplateID");
            var Description = document.getElementById("Description");    
            var Add2Quant = document.getElementById("Add2Quant");
            var RequireBoth = document.getElementById("RequireBoth");    
            var UserID = document.getElementById("UserID");
            var EntryDate = document.getElementById("EntryDate");    
            var data = JSON.parse(Response.data);
            var parsedData = JSON.parse(data.jsonData);
            var boolForAdd2Quant, boolForRequireBoth;
            if(parsedData[0].add_to_quantity == 0 ){
                boolForAdd2Quant = "No";
            }else{
                boolForAdd2Quant = "Yes";
            }
            if(parsedData[0].require_destination_and_source == 0 ){
                boolForRequireBoth = "No";
            }else{
                boolForRequireBoth = "Yes";
            }
            TemplateID.innerText = parsedData[0].template_id;
            Description.innerText = parsedData[0].description;
            UserID.innerText = parsedData[0].user_id;
            EntryDate.innerText = parsedData[0].entry_date;
            Add2Quant.innerText = boolForAdd2Quant;
            RequireBoth.innerText = boolForRequireBoth;     
            
            $('#TempInfo').modal('show');
        }

    }

    //===================================================================
    //Function: StoreUnitID(unit_id)
    //Purpose: this function is use to store the unit id (either destination unit or source unit) inside a hidden html input.
    //         This stores the unit id value which was populated in the table in the inventory adjustment page. This is used when invoking the service URL once the user click 
    //         the unit id(the displayed unit id is in a link button form) because upon clicking the unit id, all the unit id information will be displayed in modal form.    
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  unit_id               id of the unit
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function StoreUnitID(unit_id) {
        var UnitID = document.getElementById("unitidhidden");
        UnitID.value = unit_id;
    }

    //===================================================================
    //Function: populateUnitInformation
    //Purpose: this function is use display the unit information(either destination unit or source unit) inside a modal. Once the unit id link button(either destination or source)
    //         is clicked the modal will pop up with all the information about the unit(either destination or source). In the process of invoking the service, this function will use the id 
    //         stored by the function: StoreUnitID(unit_id) to invoke the service URL.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateUnitInformation(){
        var desunitID = document.getElementById("unitidhidden").value;
        var Response = InvokeService("Inventory_InventoryUnitsItems/getone/" + desunitID, "GET", "");
        if (Response.code == 200) {
            var UnitItemID = document.getElementById("UnitItemID");
            var ItemID = document.getElementById("ItemID");    
            var UnitID = document.getElementById("UnitID");
            var StartingPeriod = document.getElementById("StartingPeriod");    
            var LastEntry = document.getElementById("LastEntry");
            var StartingQuantity = document.getElementById("StartingQuantity");   
            var QuantityIN = document.getElementById("QuantityIN");
            var QuantityOUT = document.getElementById("QuantityOUT");    
            var EndingQuantity = document.getElementById("EndingQuantity");
            var StartingCost = document.getElementById("StartingCost");    
            var CostIN = document.getElementById("CostIN");
            var CostOUT = document.getElementById("CostOUT");
            var EndingCost = document.getElementById("EndingCost");
            var UnitCost = document.getElementById("UnitCost");    
            var LastHighestIN = document.getElementById("LastHighestIN");
            var UserIDForDesUnit = document.getElementById("UserIDForDesUnit");    
            var EntryDateForDesUnit = document.getElementById("EntryDateForDesUnit");

            var data = JSON.parse(Response.data);
            var parsedData = JSON.parse(data.jsonData);
            
            UnitItemID.innerText = parsedData[0].unit_item_id;
            ItemID.innerText = parsedData[0].item_id;
            UnitID.innerText = parsedData[0].unit_id;
            StartingPeriod.innerText = parsedData[0].starting_period;
            LastEntry.innerText = parsedData[0].last_entry;
            StartingQuantity.innerText = parsedData[0].starting_quantity;  
            QuantityIN.innerText = parsedData[0].quantity_in;
            QuantityOUT.innerText = parsedData[0].quantity_out;
            EndingQuantity.innerText = parsedData[0].ending_quantity;
            StartingCost.innerText = parsedData[0].starting_cost;
            CostIN.innerText = parsedData[0].cost_in;
            CostOUT.innerText = parsedData[0].cost_out;
            EndingCost.innerText = parsedData[0].ending_cost;
            UnitCost.innerText = parsedData[0].unit_cost;
            LastHighestIN.innerText = parsedData[0].last_highest_in_unit_cost;
            UserIDForDesUnit.innerText = parsedData[0].user_id;
            EntryDateForDesUnit.innerText = parsedData[0].entry_date;  
            
            $('#UnitInfo').modal('show');
        }

    }

    //===================================================================
    //Function: StoreItemID(item_id)
    //Purpose: this function is use to store the item id inside a hidden html input.
    //         This stores the item id value which was populated in the table in the inventory adjustment page. This is used when invoking the service URL once the user click 
    //         the item id(the displayed item id is in a link button form) because upon clicking the item id, all the item id information will be displayed in modal form.    
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  item_id               id of the item
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function StoreItemID(item_id) {
        var ItemID = document.getElementById("itemidhidden");
        ItemID.value = item_id;
    }

    //===================================================================
    //Function: populateItemInformation
    //Purpose: this function is use to display the item information inside a modal. Once the item id link button is clicked the modal will pop up with all the information about the item. 
    //         In the process of invoking the service, this function will use the id stored by the function: StoreItemID(item_id) to invoke the service URL.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                  Comment
    //  -----------           --------------------------------
    //  None                  None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateItemInformation(){
        var itemID = document.getElementById("itemidhidden").value;
        var Response = InvokeService("Inventory_InventoryItemsMethods/getone/" + itemID, "GET", "");
        if (Response.code == 200) {
            var BrandID = document.getElementById("BrandID");
            var CategoryID = document.getElementById("CategoryID");    
            var EntryDateForItem = document.getElementById("EntryDateForItem");
            var ItemIDForItem = document.getElementById("ItemIDForItem");    
            var MarkUpRate = document.getElementById("MarkUpRate");
            var ModelDescription = document.getElementById("ModelDescription");   
            var PartDescription = document.getElementById("PartDescription");
            var PartNumber = document.getElementById("PartNumber");    
            var Ratio = document.getElementById("Ratio");
            var RetailUnit = document.getElementById("RetailUnit");    
            var RTUoverSTU = document.getElementById("RTUoverSTU");
            var SellingPrice = document.getElementById("SellingPrice");
            var Size = document.getElementById("Size");
            var StockingUnit = document.getElementById("StockingUnit");    
            var ThreadPattern = document.getElementById("ThreadPattern");
            var UserIDForItem = document.getElementById("UserIDForItem");    
            var ValveType = document.getElementById("ValveType");
            var WTDAveCost = document.getElementById("WTDAveCost");

            var data = JSON.parse(Response.data);
            var parsedData = JSON.parse(data.jsonData);
            console.log(parsedData);
            
            BrandID.innerText =parsedData[0].brand_id;
            CategoryID.innerText =parsedData[0].category_id;
            EntryDateForItem.innerText =parsedData[0].entry_date;
            ItemIDForItem.innerText =parsedData[0].item_id;
            MarkUpRate.innerText =parsedData[0].markup_rate;
            ModelDescription.innerText =parsedData[0].model_description;
            PartDescription.innerText =parsedData[0].part_description;
            PartNumber.innerText =parsedData[0].part_number;
            Ratio.innerText =parsedData[0].ratio;
            RetailUnit.innerText =parsedData[0].retail_unit;
            RTUoverSTU.innerText =parsedData[0].rtu_over_stu;
            SellingPrice.innerText =parsedData[0].selling_price;
            Size.innerText =parsedData[0].size;
            StockingUnit.innerText =parsedData[0].stocking_unit;
            ThreadPattern.innerText =parsedData[0].thread_pattern;
            UserIDForItem.innerText =parsedData[0].user_id;
            ValveType.innerText =parsedData[0].valve_type;
            WTDAveCost.innerText =parsedData[0].wtd_ave_cost;
            
            $('#ItemInfo').modal('show');
        }

    }


