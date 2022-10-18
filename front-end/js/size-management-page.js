// //CONTAINER for Alert...
// errorAlert=document.getElementById("alertContent");
// errorAlert=document.getElementById('erroralert');
// // cloning and EDIT BUTTON...
// var brandbut=document.getElementById("editbrandbutton"); //id for input for edit buton
// var brandbutClone=brandbut.cloneNode(true);
// brandbutClone.removeAttribute("id");
// brandbutClone.class="btn btn-outline-primary";

//FUNCTION FOR CREATING NEW BRAND.
function ValidateDateInput(event){
    //INPUT VALUE.
    newBrand=document.getElementById("newbrandname").value; //input for new brandname ID
    if(newBrand=="" || newBrand==null || newBrand==undefined){

    }
}

function EditBrand(){
    oldBrand=document.getElementById("oldBrandname");
    newBrand=document.getElementById("newBrandname").value;
    //Validation of INPUT data...
    if(oldBrand==newBrand || newBrand=="" || newBrand==null){
        errorAlert.style.display="block";
        document.getElementById("alertContent").innerHTML="Error For Data Input. Please Check The Information Below."
    }
    else {
        
    }
}


function populateBrandItemTable() {
    var jsonData = InvokeService("BrandsMethods/getall", "GET", "");
    var parseJsonData = JSON.parse(jsonData.data);
    if (parseJsonData.code == 200) {
        var oData = JSON.parse(parseJsonData.jsonData);
        var divtbl = document.getElementById('brandTblItems');
        divtbl.innerHTML = "";
        for (var i = 0; i < oData.length; i++){
            divtbl.innerHTML +=
            "<tr>" +
            "<td>"+ oData[i].brand_name+"</td>" +
            "<td>"+"<button type=\"" + "button" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#mediumModal"  + "\"" + "id=\"" + "editbrandbutton" + "\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" + "</td>";
            "</tr>";
        }
           
    } else if(parseJsonData.code == 404){
        
    } else {
        ShowErrorModalOnLoad(parseJsonData.message, parseJsonData.code);

    }
}

// function populateCurrentBrandNameForEditing() {

//     var jsonData = InvokeService("BrandsMethods/getone", "GET", "");
//     var parseJsonData = JSON.parse(jsonData.data);
//     if (parseJsonData.code == 200) {
//         var oData = JSON.parse(parseJsonData.jsonData);
//         var currentBrandName = document.getElementById('currentbrandname');
//         currentBrandName.innerHTML = "";
//         for (var i = 1; i < oData.length; i++){
//             currentBrandName.innerHTML +="<td>"+"<button type=\"" + "button" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#EditItemModal"  + "\"" + "id=\"" + "editbrandbutton" + "\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" + "</td>";
            
//             "<tr> " +
//             "<td>"+ oData[i].brand_name +"</td>" +
//             "<td>"+"<button type=\"" + "button" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#NewModal"  + "\"" + "id=\"" + "editbrandbutton" + "\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" + "</td>";
//             "</tr>";
//         }

//     }
// }"<td>"+"<button type=\"" + "button" + "\"" + "class=\"" + "btn btn-outline-primary"  + "\"" + "data-toggle=\"" + "modal"  + "\"" + "data-target=\"" + "#EditItemModal"  + "\"" + "id=\"" + "editbrandbutton" + "\">" + "<i class=\"" + "fa fa-pencil-square-o" + "\">" + "</i>" + "</button>" + "</td>";
            


function addNewBrand(){

 //value of the input field of Add province...
 var newBrand = document.getElementById("newbrandname").value;
    
 //declaring the value of the paramBody...
 var objParam={
    "brand_id": 0,
    "brand_name": newBrand,
    "user_id": 0,
    "entry_date": "string"
   }

   console.log(objParam);

   var paramBody = JSON.stringify(objParam);

   var response = InvokeService("BrandsMethods", "POST", paramBody);
 //getting the paramBody 
 
 console.log(response);
 
 if (response.code == 200) {
  
     brand = document.getElementById("newbrandname");

     var data = JSON.parse(response.data); //converting json string to json object(array)  
     var nCountries = JSON.parse(data.jsonData); //converting json string to json object(array)
     
     return populateBrandItemTable();
 }

}

function editNewBrand(){

    //value of the input field of Add province...
    var newBrand = document.getElementById("newbrandname").value;
    // const index = oData.map(object => object.brand_name).indexOf('Condden');
    // console.log(index);
       
    //declaring the value of the paramBody...
    var objParam={
       "brand_id": 0,
       "brand_name": newBrand,
       "user_id": 0,
       "entry_date": ""
      }
   
      console.log(objParam);
   
      var paramBody = JSON.stringify(objParam);
   
      var response = InvokeService("BrandsMethods", "POST", paramBody);
    //getting the paramBody 
    
    console.log(response);
    
    if (response.code == 200) {
     
        brand = document.getElementById("newbrandname");
   
        var data = JSON.parse(response.data); //converting json string to json object(array)  
        var nCountries = JSON.parse(data.jsonData); //converting json string to json object(array)
        
        return populateBrandItemTable();
    }
   
   }

