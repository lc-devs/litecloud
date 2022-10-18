// //CONTAINER for Alert...
// errorAlert=document.getElementById("alertContent");
// errorAlert=document.getElementById('erroralert');
// // cloning and EDIT BUTTON...
// var brandbut=document.getElementById("editbrandbutton"); //id for input for edit buton
// var brandbutClone=brandbut.cloneNode(true);
// brandbutClone.removeAttribute("id");
// brandbutClone.class="btn btn-outline-primary";

var newbrand = document.getElementById("newbrandname");
var newBrand = document.getElementById("newBrandname")

//FUNCTION FOR CREATING NEW BRAND.
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

function populateBrandItemTable() {
    var brandSearchInput = document.getElementById("searchbar").value;
    var jsonData = InvokeService("Inventory_ModelsMethods/description/" + brandSearchInput + "?type=2", "GET", "");
    console.log(jsonData);
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = document.getElementById('brandTblItems');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr>" +
            "<td>"+ oData[i].description+"</td>" +
            "<td>"+
            "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#EditBrand"  + "\"" + "id=\"" + "editbrandbutton" + "\" onclick=\"DisplayCurrentBrandName('" + oData[i].id + "', '"+ oData[i].description  +"')\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" +
            "<button type=\"" + "button" + "\"" + "style=\"" + "margin:2px;" + "\"" + "class=\"" + "btn btn-danger"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#DeleteModal"  + "\"" + "id=\"" + "deletebrandbutton" + "\"  onclick=\"DisplayCurrentBrandName('" + oData[i].id + "', '"+ oData[i].description  +"')\">" + "<i class=\"" + "fa fa-trash" + "\"" + "style=\"" + "margin:2px;" + "\"" + "\">" + "</i>" + "</button>" + "</td>";
            "</tr>";
        }
      
    }else if(parseJsonData.code == 404){
        var divtbl = document.getElementById('brandTblItems');
        divtbl.innerHTML = "";
        divtbl.innerHTML +=
            "<tr> " +
            "<td class='border text-center px-0' colspan='2'>No Data Found.</td>" +
            "</tr>";
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
    "id": oldBrandID,
    "description": newBrand,
    "user_id": 0,
    "entry_date": "string"
  }

   var paramBody = JSON.stringify(objParam);

   var response = InvokeService("Inventory_ModelsMethods/id?type=2", "PUT", paramBody);
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

function addNewBrand(){
 var newbrandValue = document.getElementById("newbrandname").value;
    
 //declaring the value of the paramBody...
 var objParam={
    "id": 0,
    "description": newbrandValue,
    "user_id": 0,
    "entry_date": "string"
  }

   var paramBody = JSON.stringify(objParam);

   var response = InvokeService("Inventory_ModelsMethods?type=2", "POST", paramBody);
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
      var response = InvokeService("Inventory_ModelsMethods/id?id=" + oldBrandID + "&type=2" , "DELETE");
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

