
    var newbrand = document.getElementById("newbrandname");
    var newBrand = document.getElementById("newBrandname")

    
    //===================================================================
    //Function: ValidateAddBrandInput
    //Purpose: Use to validate the input in the add new brand modal located in the brandname-management-page.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function ValidateAddBrandInput(){
        var alert = document.getElementById("addbrandalert")
        var newbrandValue = document.getElementById("newbrandname").value;

        if(newbrandValue == "" || newbrandValue == null){

            alert.style.display = "block";
            newbrand.style.borderColor = "red";
            $('#SaveModal').modal('hide');
            
        }else{

            alert.style.display = "none";
            newbrand.style.borderColor = "#ced4da";
            $('#SaveModal').modal('show');
            $('#AddNewBrand').modal('hide');

        }

    }

    //===================================================================
    //Function: ValidateEditBrandInput
    //Purpose: Use to validate the input in the edit brand modal located in the brandname-management-page.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function ValidateEditBrandInput(){
        var alert = document.getElementById("editbrandalert")
        var newBrandValue = document.getElementById("newBrandname").value;

        if(newBrandValue == "" || newBrandValue == null){

            alert.style.display = "block";
            newBrand.style.borderColor = "red";
            $('#EditModal').modal('hide');
            
        }else{

            alert.style.display = "none";
            newBrand.style.borderColor = "#ced4da";
            $('#EditModal').modal('show');
            $('#EditBrand').modal('hide');

        }

    }

    //===================================================================
    //Function: populateBrandItemTable
    //Purpose: Use to populate all the brand names upon search in the table
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function populateBrandItemTable() {
        var brandSearchInput = document.getElementById("searchbar").value;
        var jsonData = InvokeService("BrandsMethods/brand_name/"+ brandSearchInput, "GET", "");
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

    //===================================================================
    //Function: DisplayCurrentBrandName
    //Purpose: Use to display the current information of the brand you are about to edit.
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  id                  id of the brand name(not displayed but hiddenly stored. The stored id will be used in invoking the update and delete service URL)
    //  name                name of the brand (displayed)
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function DisplayCurrentBrandName(id, name){
        var oldBrand = document.getElementById("oldBrandname");
        var brandID = document.getElementById("brandID");

        oldBrand.value = name;
        brandID.value = id;

    }

    //===================================================================
    //Function: UpdateBrandName
    //Purpose: Used in modifying brandnames. The stored brand id from the function: DisplayCurrentBrandName(id, name) will be useful when invoking the update service URL
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
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

    var response = InvokeService("BrandsMethods", "PUT", paramBody);
    //getting the paramBody 
    
    if (response.code == 200) {

        $('#ConfirmEditSuccessModal').modal('show');
        $('#ConfirmEditFailedModal').modal('hide');

        return populateBrandItemTable();
    }else{

        $('#ConfirmEditSuccessModal').modal('hide');
        $('#ConfirmEditFailedModal').modal('show');


    }

    }

    //===================================================================
    //Function: addNewBrand
    //Purpose: Used in adding new brand
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function addNewBrand(){
    var newbrandValue = document.getElementById("newbrandname").value;
        
    //declaring the value of the paramBody...
    var objParam={
        "brand_id": 0,
        "brand_name": newbrandValue,
        "user_id": 0,
        "entry_date": "string"
    }

    var paramBody = JSON.stringify(objParam);

    var response = InvokeService("BrandsMethods", "POST", paramBody);
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

    //===================================================================
    //Function: DeleteBrandName
    //Purpose: Used in deleting brandnames. The stored brand id from the function: DisplayCurrentBrandName(id, name) will be useful when invoking the delete service URL
    //Author: James Carl Sitsit
    //Parameter:
    //  Name                Comment
    //  -----------         --------------------------------
    //  None                None
    //
    //Result:
    // serviceResponse (ServiceResponse Class)
    //===================================================================
    function DeleteBrandName() {

        var oldBrandID = document.getElementById("brandID").value;
        //declaring the value of the paramBody...
        var response = InvokeService("BrandsMethods/id?id=" + oldBrandID , "DELETE");
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

