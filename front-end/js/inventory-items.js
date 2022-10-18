
//===================================================================
//Function: Redirection
//Purpose: To redict into another page
//Author: Mark Anthony Alegre
//Parameter:
//  Name                   Comment
//  -----------            --------------------------------
// 
//
//Result:
// 
//===================================================================
function RedirectToTubes(){
    window.location.href=("inventory-tube-items.html");
}
function RedirectToFlaps(){
    window.location.href=("inventory-flap-items.html");
}
function RedirectToBattery(){
    window.location.href=("inventory-battery-items.html");
}
function RedirectToVehicleParts(){
    window.location.href=("inventory-vehicle-part-items.html");
}
function RedirectToBrakeFluids(){
    window.location.href=("inventory-brake-fluids-items.html");
}
function RedirectToCoolant(){
    window.location.href=("inventory-coolant-items.html");
}
function RedirectToEngine(){
    window.location.href=("inventory-engine-additives-items.html");
}
function RedirectToOther(){
    window.location.href=("inventory-other-items.html");
}
function RedirectToTires(){
    window.location.href=("inventory-tire-items.html");
}

//POPULATING THE TABLE DATA...
    function PopulateInventoryItemsTable(){
    var modelDes=document.getElementById("searchcategory").value;

    var jsonData = InvokeService("Inventory_InventoryItemsMethods/model_description/"+ modelDes, "GET", "");
 
    var parseJsonData = JSON.parse(jsonData.data);

    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = document.getElementById('InventoryItemsDataTable');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0'>"+ oData[i].category_id+ "</td>" +
            "<td class='border text-center px-0'>"+ oData[i].model_description+"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].stocking_unit +"</td>" +
            "<td class='border text-center px-0'>"+ oData[i].retail_unit+"<input type='hidden' id='item_id' value= oData[i].item_id></td>" +
            "<td class='border text-center px-0'><button type='button' class='btn btn-outline-primary'  onclick='EditNonPersonData("+oData[i].nonperson_id+")' data-toggle='modal' data-target='#EditItemModal' onfocus='SaveNonPersonId_edit("+oData[i].nonperson_id+")'><i class='fa fa-pencil-square-o'></i></button><button type='button' class='btn btn-danger' onclick='SaveNonPersonId_deleteModal("+oData[i].nonperson_id+")' data-toggle='modal' data-target='#deleteModal'><i class='fa fa-trash' style='color: white;'></i>&nbsp;</button> </td>" +
            
            "</tr>";
        }

    } else if(parseJsonData.code == 404){
        
    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
    function DeleteInventoryItems(item_id){

    }
    }
    

    var newcategory = document.getElementById("category");
    var newitem = document.getElementById("item");
    var newquantity = document.getElementById("quantity");
    var newunit = document.getElementById("unit");
    
    //FUNCTION FOR CREATING NEW BRAND.
    function ValidateAddBrandInput(){
        var alert = document.getElementById("addbrandalert")
        var newcategoryvalue = document.getElementById("category").value;
        var newitemvalue = document.getElementById("item").value;
        var newquantityvalue = document.getElementById("quantity").value;
        var newunitvalue = document.getElementById("unit").value;

        if(newcategoryvalue == "" || newcategoryvalue == null){
    
            alert.style.display = "block";
            newcategory.style.borderColor = "red";
            $('#AddItemModal').modal('hide');
            
        }else{
    
            alert.style.display = "none";
            newcategory.style.borderColor = "#ced4da";
            $('#SaveModal').modal('show');
            $('#AddItemModal').modal('hide');
    
        }


        if(newitemvalue == "" || newitemvalue == null){
    
            alert.style.display = "block";
            newitem.style.borderColor = "red";
            $('#AddItemModal').modal('hide');
            
        }else{
    
            alert.style.display = "none";
            newitem.style.borderColor = "#ced4da";
            $('#SaveModal').modal('show');
            $('#AddItemModal').modal('hide');
    
        }
    
        if(newquantityvalue == "" || newquantityvalue == null){
    
            alert.style.display = "block";
            newquantity.style.borderColor = "red";
            $('#AddItemModal').modal('hide');
            
        }else{
    
            alert.style.display = "none";
            newquantity.style.borderColor = "#ced4da";
            $('#SaveModal').modal('show');
            $('#AddItemModal').modal('hide');
    
        }

        if(newunitvalue == "" || newunitvalue == null){
    
            alert.style.display = "block";
            newunit.style.borderColor = "red";
            $('#AddItemModal').modal('hide');
            
        }else{
    
            alert.style.display = "none";
            newunit.style.borderColor = "#ced4da";
            $('#SaveModal').modal('show');
            $('#AddItemModal').modal('hide');
    
        }
  
    }
    
   
    
    function populateBrandItemTable() {
        var brandSearchInput = document.getElementById("searchbar").value;
        var jsonData = InvokeService("Inventory_BrandsMethods/brand_name/"+ brandSearchInput, "GET", "");
        var parseJsonData = JSON.parse(jsonData.data);
        if (parseJsonData.code == 200) {
            var oData = JSON.parse(parseJsonData.jsonData);
            var divtbl = document.getElementById('brandTblItems');
            divtbl.innerHTML = "";
            for (var i = 0; i < oData.length; i++){
                divtbl.innerHTML +=
                "<tr>" +
                "<td>"+ oData[i].brand_name+"</td>" +
                "<td>"+
                "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#EditBrand"  + "\"" + "id=\"" + "editbrandbutton" + "\" onclick=\"DisplayCurrentBrandName('" + oData[i].brand_id + "', '"+ oData[i].brand_name  +"')\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" +
                "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-danger"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#DeleteModal"  + "\"" + "id=\"" + "deletebrandbutton" + "\"  onclick=\"DisplayCurrentBrandName('" + oData[i].brand_id + "', '"+ oData[i].brand_name  +"')\">" + "<i class=\"" + "fa fa-trash" + "\"" + "style=\"" + "margin:2px;" + "\"" + "\">" + "</i>" + "</button>" + "</td>";
                "</tr>";
            }
          
        } else if(parseJsonData.code == 404){
            
        } else {
            ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);
        }
    }
    
    function DisplayCurrentBrandName(id, name){
        var oldBrand = document.getElementById("oldBrandname");
        var brandID = document.getElementById("brandID");
    
        oldBrand.value = name;
        brandID.value = id;
    
    }
    
    function UpdateBrandName() {
    
     var oldBrandID = document.getElementById("brandID").value;
     var newBrand = document.getElementById("newBrandname").value;
     //declaring the value of the paramBody...
     var objParam={
        "brand_id": oldBrandID,
        "brand_name": newBrand,
        "user_id": 0,
        "entry_date": "string"
       }
    
       var paramBody = JSON.stringify(objParam);
    
       var response = InvokeService("Inventory_BrandsMethods", "PUT", paramBody);
     //getting the paramBody 
     
     if (response.code == 200) {
      
         var data = JSON.parse(response.data); //converting json string to json object(array)  
         JSON.parse(data.jsonData); //converting json string to json object(array)
         $('#ConfirmEditSuccessModal').modal('show');
         $('#ConfirmEditFailedModal').modal('hide');
    
         return populateBrandItemTable();
     }else{
    
        $('#ConfirmEditSuccessModal').modal('hide');
        $('#ConfirmEditFailedModal').modal('show');
    
    
     }
    
    }
    
    function addNewItem(){
        var newcategory = document.getElementById("category");
        var newitem = document.getElementById("item");
        var newquantity = document.getElementById("quantity");
        var newunit = document.getElementById("unit");
        
     //declaring the value of the paramBody...
     var objParam={
        "item_id": 1,
        "category_id": newcategory,
        "brand_id": 1,
        "model_description": newitem,
        "part_description": 1,
        "part_number": 1,
        "size": 1,
        "valve_type": 1,
        "ratio": 1,
        "thread_pattern": 1,
        "stocking_unit": newquantity,
        "retail_unit": newunit,
        "rtu_over_stu": 1,
        "wtd_ave_cost": 1,
        "markup_rate": 1,
        "selling_price": 1,
        "user_id": 0,
        "entry_date": ""
      
  }
       var paramBody = JSON.stringify(objParam);
    
       var response = InvokeService("Inventory_BrandsMethods", "POST", paramBody);
     //getting the paramBody 
     
     if (response.code == 200) {
    
         var data = JSON.parse(response.data); //converting json string to json object(array)  
         JSON.parse(data.jsonData); //converting json string to json object(array)
         $('#ConfirmSaveSuccessModal').modal('show');
         $('#ConfirmSaveFailedModal').modal('hide');
    
         return populateBrandItemTable();
    
     }else{
    
        $('#ConfirmSaveSuccessModal').modal('hide');
        $('#ConfirmSaveFailedModal').modal('show');
    
     }
    
    }
    
    function DeleteBrandName() {
    
        var oldBrandID = document.getElementById("brandID").value;
        //declaring the value of the paramBody...
          var response = InvokeService("Inventory_BrandsMethods/id?id=" + oldBrandID , "DELETE");
        //getting the paramBody 
    
        if (response.code == 200) {
            $('#ConfirmDeleteSuccessModal').modal('show');
            $('#ConfirmDeleteFailedModal').modal('hide');
       
            return populateBrandItemTable();
       
        }else{
       
           $('#ConfirmDeleteSuccessModal').modal('hide');
           $('#ConfirmDeleteFailedModal').modal('show');
       
        }
       
       
    }
    
    